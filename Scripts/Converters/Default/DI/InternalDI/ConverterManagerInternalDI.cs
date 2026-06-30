#nullable enable
namespace UniT.Data.Converters.Default.DI
{
    using System;
    using System.Globalization;
    using UniT.DI;
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
                formatProvider: CultureInfo.InvariantCulture
            );
        }

        public static void AddDefaultConverters(this DependencyContainer container, SeparatorConfig separatorConfig, IFormatProvider formatProvider)
        {
            container.Add(separatorConfig);
            container.Add(formatProvider);
            container.AddInterfaces<JsonConverter>();

            #region Primitives

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

            container.AddInterfaces<BooleanConverter>();
            container.AddInterfaces<CharConverter>();
            container.AddInterfaces<StringConverter>();

            container.AddInterfaces<DateTimeConverter>();
            container.AddInterfaces<DateTimeOffsetConverter>();
            container.AddInterfaces<TimeSpanConverter>();

            container.AddInterfaces<UriConverter>();
            container.AddInterfaces<GuidConverter>();

            #endregion

            #region Others

            container.AddInterfaces<EnumConverter>();
            container.AddInterfaces<NullableConverter>();
            container.AddInterfaces<TupleConverter>();
            container.AddInterfaces<AbstractTupleConverter>();

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