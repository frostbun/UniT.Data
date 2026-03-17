#if UNIT_CSV
#nullable enable
namespace UniT.Data.Serialization
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using CsvHelper;
    using CsvHelper.Configuration;
    using UniT.Data.Conversion;
    using UniT.Extensions;
    using UnityEngine.Scripting;

    public sealed class CsvSerializer : Serializer<string, ICsvData>
    {
        private readonly IConverterManager converterManager;
        private readonly CsvConfiguration  configuration;

        [Preserve]
        public CsvSerializer(IConverterManager converterManager, CsvConfiguration configuration)
        {
            this.converterManager = converterManager;
            this.configuration    = configuration;
        }

        public override ICsvData Deserialize(Type type, string rawData)
        {
            var       data   = (ICsvData)type.GetEmptyConstructor()();
            using var reader = new CsvReader(new StringReader(rawData), this.configuration);
            if (!reader.Read()) return data;
            reader.ReadHeader();
            var populator = new Populator(this.converterManager, data, reader);
            while (reader.Read()) populator.Populate();
            return data;
        }

        public override string Serialize(ICsvData data)
        {
            using var stringWriter = new StringWriter();
            using var writer       = new CsvWriter(stringWriter, this.configuration);
            var       serializer   = new Serializer(this.converterManager, data, writer);

            var hasValue = serializer.MoveNext();
            if (!hasValue) return string.Empty;

            foreach (var header in serializer.GetHeaders())
            {
                writer.WriteField(header);
            }
            writer.NextRecord();

            while (hasValue)
            {
                serializer.Serialize();
                writer.NextRecord();
                hasValue = serializer.MoveNext();
            }

            return stringWriter.ToString();
        }

        private sealed class Populator
        {
            #region Constructor

            private readonly IConverterManager                                                 converterManager;
            private readonly ICsvData                                                          data;
            private readonly CsvReader                                                         reader;
            private readonly Func<object>                                                      rowConstructor;
            private readonly FieldInfo                                                         keyField;
            private readonly IReadOnlyDictionary<FieldInfo, (int Index, IConverter Converter)> normalFields;
            private readonly IReadOnlyList<FieldInfo>                                          nestedFields;

            private readonly Dictionary<FieldInfo, Populator> nestedPopulators = new Dictionary<FieldInfo, Populator>();

            public Populator(IConverterManager converterManager, ICsvData data, CsvReader reader)
            {
                this.converterManager = converterManager;
                this.data             = data;
                this.reader           = reader;

                var rowType = data.RowType;
                this.rowConstructor              = rowType.GetEmptyConstructor();
                var (prefix, key)                = rowType.GetCsvRow();
                var (normalFields, nestedFields) = rowType.GetCsvFields();
                this.keyField                    = normalFields.FirstOrDefault(field => key.IsNullOrWhiteSpace() || field.GetCsvColumn(prefix) == key) ?? throw new InvalidOperationException($"{rowType.Name} has no field {key}");
                this.normalFields = normalFields
                    .Select((field, state) =>
                    {
                        var column = field.GetCsvColumn(state.prefix);
                        var index  = state.reader.GetFieldIndex(column);
                        return (field, column, index);
                    }, (prefix, reader))
                    .Where((field, column, index, rowType) =>
                    {
                        if (index >= 0) return true;
                        if (field.IsCsvOptional()) return false;
                        throw new InvalidOperationException($"Column {column} not found in {rowType.Name}. If this is intentional, add [CsvIgnore] or [CsvOptional] attribute to the field.");
                    }, rowType)
                    .ToDictionary(
                        (field, _, _) => field,
                        (field, _, index) => (index, this.converterManager.GetConverter(field.FieldType))
                    );
                this.nestedFields = nestedFields;
            }

            #endregion

            public void Populate()
            {
                var keyValue = default(object);
                var row      = this.rowConstructor();

                foreach (var (field, (index, converter)) in this.normalFields)
                {
                    var str = this.reader[index];
                    if (str.IsNullOrWhiteSpace())
                    {
                        field.SetValue(row, converter.GetDefaultValue(field.FieldType));
                        continue;
                    }
                    var value = converter.ConvertFromString(str, field.FieldType);
                    field.SetValue(row, value);
                    if (field == this.keyField) keyValue = value;
                }

                if (keyValue is { })
                {
                    this.data.Add(keyValue, row);
                    this.nestedPopulators.Clear();
                }

                foreach (var field in this.nestedFields)
                {
                    this.nestedPopulators.GetOrAdd(field, state =>
                    {
                        var nestedData      = (ICsvData)state.field.FieldType.GetEmptyConstructor()();
                        var nestedPopulator = new Populator(state.@this.converterManager, nestedData, state.@this.reader);
                        state.field.SetValue(state.row, nestedData);
                        return nestedPopulator;
                    }, (@this: this, field, row)).Populate();
                }
            }
        }

        private sealed class Serializer
        {
            #region Constructor

            private readonly IConverterManager                          converterManager;
            private readonly IEnumerator                                data;
            private readonly CsvWriter                                  writer;
            private readonly IReadOnlyList<string>                      headers;
            private readonly IReadOnlyDictionary<FieldInfo, IConverter> normalFields;
            private readonly IReadOnlyList<FieldInfo>                   nestedFields;

            private readonly Dictionary<FieldInfo, Serializer>            nestedSerializers = new Dictionary<FieldInfo, Serializer>();
            private readonly Dictionary<FieldInfo, IReadOnlyList<string>> nestedHeaders     = new Dictionary<FieldInfo, IReadOnlyList<string>>();

            public Serializer(IConverterManager converterManager, ICsvData data, CsvWriter writer)
            {
                this.converterManager = converterManager;
                this.data             = data.GetValues();
                this.writer           = writer;
                var rowType = data.RowType;
                var (prefix, _)                  = rowType.GetCsvRow();
                var (normalFields, nestedFields) = rowType.GetCsvFields();
                this.headers                     = normalFields.Select((field, prefix) => field.GetCsvColumn(prefix), prefix).ToArray();
                this.normalFields = normalFields.ToDictionary(
                    field => field,
                    field => this.converterManager.GetConverter(field.FieldType)
                );
                this.nestedFields = nestedFields;
            }

            #endregion

            public IEnumerable<string> GetHeaders()
            {
                return this.headers.Concat(this.nestedFields.SelectMany(field => this.nestedHeaders[field]));
            }

            private bool hasValue;

            public bool MoveNext()
            {
                if (
                    this.nestedSerializers.Count > 0
                    && this.nestedFields.Aggregate(false, (hasNestedValue, field) => hasNestedValue || this.nestedSerializers[field].MoveNext())
                ) return true;

                this.hasValue = this.data.MoveNext();
                if (!this.hasValue)
                {
                    (this.data as IDisposable)?.Dispose();
                    return false;
                }

                var row = this.data.Current;
                foreach (var field in this.nestedFields)
                {
                    var serializer = this.nestedSerializers[field] = new Serializer(this.converterManager, (ICsvData)field.GetValue(row), this.writer);
                    serializer.MoveNext();
                    this.nestedHeaders.TryAdd(field, serializer => serializer.GetHeaders().ToArray(), serializer);
                }
                return true;
            }

            public void Serialize()
            {
                if (this.hasValue)
                {
                    var row = this.data.Current;
                    foreach (var (field, converter) in this.normalFields)
                    {
                        var value = field.GetValue(row);
                        if (value is null)
                        {
                            this.writer.WriteField(string.Empty);
                            continue;
                        }
                        this.writer.WriteField(converter.ConvertToString(value, field.FieldType));
                    }
                    this.hasValue = false;
                }
                else
                {
                    foreach (var _ in this.normalFields)
                    {
                        this.writer.WriteField(string.Empty);
                    }
                }

                foreach (var field in this.nestedFields)
                {
                    this.nestedSerializers[field].Serialize();
                }
            }
        }
    }
}
#endif