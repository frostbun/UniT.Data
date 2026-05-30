#nullable enable
namespace UniT.Data.Converters.Default.DI
{
    using System;
    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using UniT.Extensions;
    using VContainer;
    using JsonConverter = JsonConverter;

    public static class ConverterManagerVContainer
    {
        public static void RegisterConverterManager(this IContainerBuilder builder, bool registerDefaultConverters = true)
        {
            if (registerDefaultConverters)
            {
                builder.RegisterDefaultConverters();
            }
            builder.Register<ConverterManager>(Lifetime.Singleton).AsImplementedInterfaces();
        }

        public static void RegisterDefaultConverters(this IContainerBuilder builder)
        {
            builder.RegisterDefaultConverters(
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

        public static void RegisterDefaultConverters(this IContainerBuilder builder, SeparatorConfig separatorConfig, IFormatProvider formatProvider, JsonSerializerSettings jsonSerializerSettings)
        {
            builder.RegisterInstance(separatorConfig);
            builder.RegisterInstance(formatProvider);
            builder.RegisterInstance(jsonSerializerSettings);
            builder.Register<JsonConverter>(Lifetime.Singleton).AsImplementedInterfaces();

            #region Numbers

            builder.Register<ByteConverter>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<Int16Converter>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<Int32Converter>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<Int64Converter>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<UInt16Converter>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<UInt32Converter>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<UInt64Converter>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<SingleConverter>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<DoubleConverter>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<DecimalConverter>(Lifetime.Singleton).AsImplementedInterfaces();

            #endregion

            #region Time

            builder.Register<DateTimeConverter>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<DateTimeOffsetConverter>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<TimeSpanConverter>(Lifetime.Singleton).AsImplementedInterfaces();

            #endregion

            #region Others

            builder.Register<BooleanConverter>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<CharConverter>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<StringConverter>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<EnumConverter>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<NullableConverter>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<UriConverter>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<GuidConverter>(Lifetime.Singleton).AsImplementedInterfaces();

            #endregion

            #region Tuples

            builder.Register<TupleConverter>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<Vector2Converter>(Lifetime.Singleton).AsImplementedInterfaces();    // Depends on TupleConverter
            builder.Register<Vector3Converter>(Lifetime.Singleton).AsImplementedInterfaces();    // Depends on TupleConverter
            builder.Register<Vector4Converter>(Lifetime.Singleton).AsImplementedInterfaces();    // Depends on TupleConverter
            builder.Register<Vector2IntConverter>(Lifetime.Singleton).AsImplementedInterfaces(); // Depends on TupleConverter
            builder.Register<Vector3IntConverter>(Lifetime.Singleton).AsImplementedInterfaces(); // Depends on TupleConverter
            builder.Register<ColorConverter>(Lifetime.Singleton).AsImplementedInterfaces();      // Depends on TupleConverter
            builder.Register<Color32Converter>(Lifetime.Singleton).AsImplementedInterfaces();    // Depends on TupleConverter

            #endregion

            #region Collections

            builder.Register<ArrayConverter>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<CollectionConverter>(Lifetime.Singleton).AsImplementedInterfaces();         // Depends on ArrayConverter
            builder.Register<AbstractCollectionConverter>(Lifetime.Singleton).AsImplementedInterfaces(); // Depends on ArrayConverter
            builder.Register<DictionaryConverter>(Lifetime.Singleton).AsImplementedInterfaces();         // Depends on ArrayConverter
            builder.Register<ReadOnlyDictionaryConverter>(Lifetime.Singleton).AsImplementedInterfaces(); // Depends on DictionaryConverter
            builder.Register<AbstractDictionaryConverter>(Lifetime.Singleton).AsImplementedInterfaces(); // Depends on DictionaryConverter

            #endregion
        }
    }
}