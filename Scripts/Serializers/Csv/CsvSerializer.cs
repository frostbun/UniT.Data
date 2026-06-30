#nullable enable
namespace UniT.Data.Serializers.Csv
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using CsvHelper;
    using CsvHelper.Configuration;
    using UniT.Data.Converters;
    using UniT.Extensions;
    using UnityEngine.Scripting;

    public sealed class CsvSerializer : Serializer<string, ICsvData>
    {
        private readonly CsvConfiguration  configuration;
        private readonly IConverterManager converterManager;

        [Preserve]
        public CsvSerializer(CsvConfiguration configuration, IConverterManager converterManager)
        {
            this.configuration    = configuration;
            this.converterManager = converterManager;
        }

        public override ICsvData Deserialize(Type type, string rawData)
        {
            using var reader       = new CsvReader(new StringReader(rawData), this.configuration);
            var       deserializer = new Deserializer(type, reader, this.converterManager);

            deserializer.Reset();
            if (!reader.Read()) return deserializer.Data;

            reader.ReadHeader();
            deserializer.Initialize();
            while (reader.Read()) deserializer.Deserialize();
            return deserializer.Data;
        }

        public override string Serialize(Type type, ICsvData data)
        {
            using var stringWriter = new StringWriter();
            using var writer       = new CsvWriter(stringWriter, this.configuration);
            var       serializer   = new Serializer(writer, this.converterManager);

            if (!serializer.Reset(data)) return string.Empty;

            foreach (var header in serializer.GetHeaders())
            {
                writer.WriteField(header);
            }
            writer.NextRecord();

            serializer.Serialize();
            writer.NextRecord();

            while (serializer.MoveNext())
            {
                serializer.Serialize();
                writer.NextRecord();
            }

            return stringWriter.ToString();
        }

        private sealed class Deserializer
        {
            #region Constructor

            private readonly Func<object>      constructor;
            private readonly CsvReader         reader;
            private readonly IConverterManager converterManager;

            public Deserializer(Type type, CsvReader reader, IConverterManager converterManager)
            {
                this.constructor      = type.GetEmptyConstructor();
                this.reader           = reader;
                this.converterManager = converterManager;
            }

            #endregion

            public ICsvData Data { get; private set; } = null!;

            private bool initialized;

            private Func<object>                                                      rowConstructor = null!;
            private FieldInfo                                                         keyField       = null!;
            private IReadOnlyList<(FieldInfo Field, int Index, IConverter Converter)> normalFields   = null!;
            private IReadOnlyList<(FieldInfo Field, Deserializer Deserializer)>       nestedFields   = null!;

            public void Reset()
            {
                this.Data = (ICsvData)this.constructor();
            }

            public void Initialize()
            {
                if (this.initialized) return;

                var rowType = this.Data.RowType;
                var (prefix, key)                = rowType.GetCsvRow();
                var (normalFields, nestedFields) = rowType.GetCsvFields();

                this.rowConstructor = rowType.GetEmptyConstructor();

                this.keyField = normalFields.FirstOrDefault(field => key.IsNullOrWhiteSpace() || field.GetCsvColumn(prefix) == key)
                    ?? throw new InvalidOperationException($"{rowType.Name} has no field {key}");

                this.normalFields = normalFields
                    .Select(static (field, state) =>
                    {
                        var column    = field.GetCsvColumn(state.prefix);
                        var index     = state.@this.reader.GetFieldIndex(column);
                        var converter = state.@this.converterManager.GetConverter(field.FieldType);

                        if (index < 0 && !field.IsCsvOptional()) throw new InvalidOperationException($"Column {column} not found for {state.rowType.Name}. If this is intentional, add [CsvIgnore] or [CsvOptional] attribute to the field.");

                        return (field, index, converter);
                    }, (@this: this, rowType, prefix))
                    .WhereSecond(static index => index >= 0)
                    .ToArray();

                this.nestedFields = nestedFields
                    .Select(static (field, @this) => (field, new Deserializer(field.FieldType, @this.reader, @this.converterManager)), this)
                    .ToArray();

                this.initialized = true;
            }

            public void Deserialize()
            {
                var keyValue = default(object);
                var row      = this.rowConstructor();

                foreach (var (field, index, converter) in this.normalFields)
                {
                    var str = this.reader[index];
                    if (str.IsNullOrWhiteSpace())
                    {
                        field.SetValue(row, converter.GetDefaultValue(field.FieldType));
                        continue;
                    }
                    var value = converter.ConvertFromString(field.FieldType, str);
                    field.SetValue(row, value);

                    if (field != this.keyField) continue;

                    keyValue = value;

                    foreach (var (nestedField, nestedDeserializer) in this.nestedFields)
                    {
                        nestedDeserializer.Reset();
                        nestedDeserializer.Initialize();
                        nestedField.SetValue(row, nestedDeserializer.Data);
                    }
                }

                if (keyValue is not null)
                {
                    this.Data.Add(keyValue, row);
                }

                foreach (var (_, nestedDeserializer) in this.nestedFields)
                {
                    nestedDeserializer.Deserialize();
                }
            }
        }

        private sealed class Serializer
        {
            #region Constructor

            private readonly CsvWriter         writer;
            private readonly IConverterManager converterManager;

            public Serializer(CsvWriter writer, IConverterManager converterManager)
            {
                this.writer           = writer;
                this.converterManager = converterManager;
            }

            #endregion

            private ICsvData    data   = null!;
            private IEnumerator values = null!;

            private bool initialized;
            private bool started;
            private bool hasValue;

            private IReadOnlyList<(FieldInfo Field, IConverter Converter)>  normalFields = null!;
            private IReadOnlyList<(FieldInfo Field, Serializer Serializer)> nestedFields = null!;

            public bool Reset(ICsvData data)
            {
                this.data     = data;
                this.values   = data.GetValues();
                this.started  = false;
                this.hasValue = false;

                this.Initialize();

                return this.MoveNext();
            }

            private void Initialize()
            {
                if (this.initialized) return;

                var (normalFields, nestedFields) = this.data.RowType.GetCsvFields();

                this.normalFields = normalFields
                    .Select(static (field, converterManager) => (field, converterManager.GetConverter(field.FieldType)), this.converterManager)
                    .ToArray();

                this.nestedFields = nestedFields
                    .Select(static (field, @this) => (field, new Serializer(@this.writer, @this.converterManager)), this)
                    .ToArray();

                this.initialized = true;
            }

            public IEnumerable<string> GetHeaders()
            {
                return this.normalFields
                    .SelectFirsts((field, prefix) => field.GetCsvColumn(prefix), this.data.RowType.GetCsvRow().Prefix)
                    .Concat(this.nestedFields.SelectSeconds().SelectMany(nestedSerializer => nestedSerializer.GetHeaders()));
            }

            public bool MoveNext()
            {
                if (
                    this.started
                    && this.nestedFields.SelectSeconds().Aggregate(false, (hasNestedValue, nestedSerializer) => nestedSerializer.MoveNext() || hasNestedValue)
                )
                    return true;

                this.hasValue = this.values.MoveNext();
                if (!this.hasValue)
                {
                    (this.values as IDisposable)?.Dispose();
                    return false;
                }
                this.started = true;

                var row = this.values.Current;
                foreach (var (nestedField, nestedSerializer) in this.nestedFields)
                {
                    nestedSerializer.Reset((ICsvData)nestedField.GetValue(row));
                }
                return true;
            }

            public void Serialize()
            {
                if (this.hasValue)
                {
                    var row = this.values.Current;
                    foreach (var (field, converter) in this.normalFields)
                    {
                        var value = field.GetValue(row);
                        if (value is null)
                        {
                            this.writer.WriteField(string.Empty);
                            continue;
                        }
                        this.writer.WriteField(converter.ConvertToString(field.FieldType, value));
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

                foreach (var (_, nestedSerializer) in this.nestedFields)
                {
                    nestedSerializer.Serialize();
                }
            }
        }
    }
}