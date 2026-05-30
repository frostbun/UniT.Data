#nullable enable
namespace UniT.Data.Converters.Default.DI
{
    using System;
    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using UniT.Extensions;
    using Zenject;
    using JsonConverter = JsonConverter;

    public static class ConverterManagerZenject
    {
        public static void BindConverterManager(this DiContainer container, bool bindDefaultConverters = true)
        {
            if (bindDefaultConverters)
            {
                container.BindDefaultConverters();
            }
            container.BindInterfacesTo<ConverterManager>().AsSingle();
        }

        public static void BindDefaultConverters(this DiContainer container)
        {
            container.BindDefaultConverters(
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

        public static void BindDefaultConverters(this DiContainer container, SeparatorConfig separatorConfig, IFormatProvider formatProvider, JsonSerializerSettings jsonSerializerSettings)
        {
            container.BindInstance(separatorConfig);
            container.BindInstance(formatProvider);
            container.BindInstance(jsonSerializerSettings);
            container.BindInterfacesTo<JsonConverter>().AsSingle();

            #region Numbers

            container.BindInterfacesTo<ByteConverter>().AsSingle();
            container.BindInterfacesTo<Int16Converter>().AsSingle();
            container.BindInterfacesTo<Int32Converter>().AsSingle();
            container.BindInterfacesTo<Int64Converter>().AsSingle();
            container.BindInterfacesTo<UInt16Converter>().AsSingle();
            container.BindInterfacesTo<UInt32Converter>().AsSingle();
            container.BindInterfacesTo<UInt64Converter>().AsSingle();
            container.BindInterfacesTo<SingleConverter>().AsSingle();
            container.BindInterfacesTo<DoubleConverter>().AsSingle();
            container.BindInterfacesTo<DecimalConverter>().AsSingle();

            #endregion

            #region Time

            container.BindInterfacesTo<DateTimeConverter>().AsSingle();
            container.BindInterfacesTo<DateTimeOffsetConverter>().AsSingle();
            container.BindInterfacesTo<TimeSpanConverter>().AsSingle();

            #endregion

            #region Others

            container.BindInterfacesTo<BooleanConverter>().AsSingle();
            container.BindInterfacesTo<CharConverter>().AsSingle();
            container.BindInterfacesTo<StringConverter>().AsSingle();
            container.BindInterfacesTo<EnumConverter>().AsSingle();
            container.BindInterfacesTo<NullableConverter>().AsSingle();
            container.BindInterfacesTo<UriConverter>().AsSingle();
            container.BindInterfacesTo<GuidConverter>().AsSingle();

            #endregion

            #region Tuples

            container.BindInterfacesTo<TupleConverter>().AsSingle();
            container.BindInterfacesTo<Vector2Converter>().AsSingle();    // Depends on TupleConverter
            container.BindInterfacesTo<Vector3Converter>().AsSingle();    // Depends on TupleConverter
            container.BindInterfacesTo<Vector4Converter>().AsSingle();    // Depends on TupleConverter
            container.BindInterfacesTo<Vector2IntConverter>().AsSingle(); // Depends on TupleConverter
            container.BindInterfacesTo<Vector3IntConverter>().AsSingle(); // Depends on TupleConverter
            container.BindInterfacesTo<ColorConverter>().AsSingle();      // Depends on TupleConverter
            container.BindInterfacesTo<Color32Converter>().AsSingle();    // Depends on TupleConverter

            #endregion

            #region Collections

            container.BindInterfacesTo<ArrayConverter>().AsSingle();
            container.BindInterfacesTo<CollectionConverter>().AsSingle();         // Depends on ArrayConverter
            container.BindInterfacesTo<AbstractCollectionConverter>().AsSingle(); // Depends on ArrayConverter
            container.BindInterfacesTo<DictionaryConverter>().AsSingle();         // Depends on ArrayConverter
            container.BindInterfacesTo<ReadOnlyDictionaryConverter>().AsSingle(); // Depends on DictionaryConverter
            container.BindInterfacesTo<AbstractDictionaryConverter>().AsSingle(); // Depends on DictionaryConverter

            #endregion
        }
    }
}