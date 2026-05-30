#nullable enable
namespace UniT.Data.Converters.Default.DI
{
    using System;
    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using UniT.DI;
    using UniT.Extensions;
    using JsonConverter = JsonConverter;

    public static class ConverterManagerInternalDI
    {
        public static void AddConverterManager(this DependencyContainer container, bool addDefaultConverters = true)
        {
            if (addDefaultConverters)
            {
                container.AddDefaultConverters();
            }
            container.AddInterfaces<ConverterManager>();
        }

        public static void AddDefaultConverters(this DependencyContainer container)
        {
            container.AddDefaultConverters(
                separatorConfig: new(),
                formatProvider: CultureInfo.InvariantCulture,
                jsonSerializerSettings: new()
                {
                    Culture                = CultureInfo.InvariantCulture,
                    TypeNameHandling       = TypeNameHandling.Auto,
                    ReferenceLoopHandling  = ReferenceLoopHandling.Ignore,
                    ObjectCreationHandling = ObjectCreationHandling.Replace,
                    ContractResolver       = new WritablePropertyOnlyContractResolver(),
                    Converters = new Newtonsoft.Json.JsonConverter[]
                    {
                        new StringEnumConverter(),
                    },
                }
            );
        }

        public static void AddDefaultConverters(this DependencyContainer container, SeparatorConfig separatorConfig, IFormatProvider formatProvider, JsonSerializerSettings jsonSerializerSettings)
        {
            container.Add(separatorConfig);
            container.Add(formatProvider);
            container.Add(jsonSerializerSettings);
            container.AddInterfaces<JsonConverter>();

            #region Numbers

            container.AddInterfaces<ByteConverter>();
            container.AddInterfaces<Int16Converter>();
            container.AddInterfaces<Int32Converter>();
            container.AddInterfaces<Int64Converter>();
            container.AddInterfaces<UInt16Converter>();
            container.AddInterfaces<UInt32Converter>();
            container.AddInterfaces<UInt64Converter>();
            container.AddInterfaces<SingleConverter>();
            container.AddInterfaces<DoubleConverter>();
            container.AddInterfaces<DecimalConverter>();

            #endregion

            #region Time

            container.AddInterfaces<DateTimeConverter>();
            container.AddInterfaces<DateTimeOffsetConverter>();
            container.AddInterfaces<TimeSpanConverter>();

            #endregion

            #region Others

            container.AddInterfaces<BooleanConverter>();
            container.AddInterfaces<CharConverter>();
            container.AddInterfaces<StringConverter>();
            container.AddInterfaces<EnumConverter>();
            container.AddInterfaces<NullableConverter>();
            container.AddInterfaces<UriConverter>();
            container.AddInterfaces<GuidConverter>();

            #endregion

            #region Tuples

            container.AddInterfaces<TupleConverter>();
            container.AddInterfaces<Vector2Converter>();    // Depends on TupleConverter
            container.AddInterfaces<Vector3Converter>();    // Depends on TupleConverter
            container.AddInterfaces<Vector4Converter>();    // Depends on TupleConverter
            container.AddInterfaces<Vector2IntConverter>(); // Depends on TupleConverter
            container.AddInterfaces<Vector3IntConverter>(); // Depends on TupleConverter
            container.AddInterfaces<ColorConverter>();      // Depends on TupleConverter
            container.AddInterfaces<Color32Converter>();    // Depends on TupleConverter

            #endregion

            #region Collections

            container.AddInterfaces<ArrayConverter>();
            container.AddInterfaces<CollectionConverter>();         // Depends on ArrayConverter
            container.AddInterfaces<AbstractCollectionConverter>(); // Depends on ArrayConverter
            container.AddInterfaces<DictionaryConverter>();         // Depends on ArrayConverter
            container.AddInterfaces<ReadOnlyDictionaryConverter>(); // Depends on DictionaryConverter
            container.AddInterfaces<AbstractDictionaryConverter>(); // Depends on DictionaryConverter

            #endregion
        }
    }
}