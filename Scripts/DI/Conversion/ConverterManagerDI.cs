#if UNIT_DI
#nullable enable
namespace UniT.Data.Conversion.DI
{
    using System;
    using System.Globalization;
    using UniT.DI;
    #if UNIT_JSON
    using Newtonsoft.Json;
    using JsonConverter = JsonConverter;
    #endif

    public static class ConverterManagerDI
    {
        public static void AddConverterManager(this DependencyContainer container)
        {
            if (container.Contains<IConverterManager>()) return;

            #region Configs

            if (!container.Contains<IFormatProvider>())
            {
                container.Add<IFormatProvider>(CultureInfo.InvariantCulture);
            }
            if (!container.Contains<SeparatorConfig>())
            {
                container.Add(new SeparatorConfig());
            }

            #endregion

            #region Converters

            #if UNIT_JSON
            if (!container.Contains<JsonSerializerSettings>())
            {
                container.Add(DefaultJsonSerializerSettings.Value);
            }
            container.AddInterfaces<JsonConverter>();
            #endif

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
            container.AddInterfaces<Vector2Converter>();    // Depend on TupleConverter
            container.AddInterfaces<Vector3Converter>();    // Depend on TupleConverter
            container.AddInterfaces<Vector4Converter>();    // Depend on TupleConverter
            container.AddInterfaces<Vector2IntConverter>(); // Depend on TupleConverter
            container.AddInterfaces<Vector3IntConverter>(); // Depend on TupleConverter
            container.AddInterfaces<ColorConverter>();      // Depend on TupleConverter
            container.AddInterfaces<Color32Converter>();    // Depend on TupleConverter

            #endregion

            #region Collections

            container.AddInterfaces<ArrayConverter>();
            container.AddInterfaces<CollectionConverter>();         // Depend on ArrayConverter
            container.AddInterfaces<AbstractCollectionConverter>(); // Depend on ArrayConverter
            container.AddInterfaces<DictionaryConverter>();         // Depend on ArrayConverter
            container.AddInterfaces<ReadOnlyDictionaryConverter>(); // Depend on DictionaryConverter
            container.AddInterfaces<AbstractDictionaryConverter>(); // Depend on DictionaryConverter

            #endregion

            #endregion

            container.AddInterfaces<ConverterManager>();
        }
    }
}
#endif