#if UNIT_VCONTAINER
#nullable enable
namespace UniT.Data.Conversion.DI
{
    using System;
    using System.Globalization;
    using VContainer;
    #if UNIT_JSON
    using Newtonsoft.Json;
    using JsonConverter = JsonConverter;
    #endif

    public static class ConverterManagerVContainer
    {
        public static void RegisterConverterManager(this IContainerBuilder builder)
        {
            if (builder.Exists(typeof(IConverterManager), true)) return;

            #region Configs

            if (!builder.Exists(typeof(IFormatProvider), true))
            {
                builder.RegisterInstance<IFormatProvider>(CultureInfo.InvariantCulture);
            }
            if (!builder.Exists(typeof(SeparatorConfig)))
            {
                builder.RegisterInstance(new SeparatorConfig());
            }

            #endregion

            #region Converters

            #if UNIT_JSON
            if (!builder.Exists(typeof(JsonSerializerSettings)))
            {
                builder.RegisterInstance(DefaultJsonSerializerSettings.Value);
            }
            builder.Register<JsonConverter>(Lifetime.Singleton).AsImplementedInterfaces();
            #endif

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
            builder.Register<Vector2Converter>(Lifetime.Singleton).AsImplementedInterfaces();    // Depend on TupleConverter
            builder.Register<Vector3Converter>(Lifetime.Singleton).AsImplementedInterfaces();    // Depend on TupleConverter
            builder.Register<Vector4Converter>(Lifetime.Singleton).AsImplementedInterfaces();    // Depend on TupleConverter
            builder.Register<Vector2IntConverter>(Lifetime.Singleton).AsImplementedInterfaces(); // Depend on TupleConverter
            builder.Register<Vector3IntConverter>(Lifetime.Singleton).AsImplementedInterfaces(); // Depend on TupleConverter
            builder.Register<ColorConverter>(Lifetime.Singleton).AsImplementedInterfaces();      // Depend on TupleConverter
            builder.Register<Color32Converter>(Lifetime.Singleton).AsImplementedInterfaces();    // Depend on TupleConverter

            #endregion

            #region Collections

            builder.Register<ArrayConverter>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<CollectionConverter>(Lifetime.Singleton).AsImplementedInterfaces();         // Depend on ArrayConverter
            builder.Register<AbstractCollectionConverter>(Lifetime.Singleton).AsImplementedInterfaces(); // Depend on ArrayConverter
            builder.Register<DictionaryConverter>(Lifetime.Singleton).AsImplementedInterfaces();         // Depend on ArrayConverter
            builder.Register<ReadOnlyDictionaryConverter>(Lifetime.Singleton).AsImplementedInterfaces(); // Depend on DictionaryConverter
            builder.Register<AbstractDictionaryConverter>(Lifetime.Singleton).AsImplementedInterfaces(); // Depend on DictionaryConverter

            #endregion

            #endregion

            builder.Register<ConverterManager>(Lifetime.Singleton).AsImplementedInterfaces();
        }
    }
}
#endif