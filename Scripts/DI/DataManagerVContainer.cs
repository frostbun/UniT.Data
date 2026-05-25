#if UNIT_VCONTAINER
#nullable enable
namespace UniT.Data.DI
{
    using UniT.Data.Converters.DI;
    using UniT.Data.Serializers.DI;
    using UniT.Data.Storages.DI;
    using UniT.Logging.DI;
    using VContainer;

    public static class DataManagerVContainer
    {
        public static void RegisterDataManager(this IContainerBuilder builder)
        {
            if (builder.Exists(typeof(IDataManager), true)) return;
            builder.RegisterLoggerManager();
            builder.RegisterConverterManager();
            builder.RegisterSerializers();
            builder.RegisterAssetStorages();
            #if !UNITY_WEBGL
            builder.RegisterFileStorages();
            #else
            builder.RegisterPlayerPrefsStorages();
            #endif
            builder.Register<DataManager>(Lifetime.Singleton).AsImplementedInterfaces();
        }
    }
}
#endif