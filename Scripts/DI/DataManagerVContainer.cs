#if UNIT_VCONTAINER
#nullable enable
namespace UniT.Data.DI
{
    using UniT.Data.Conversion.DI;
    using UniT.Data.Serialization.DI;
    using UniT.Data.Storage.DI;
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
            builder.RegisterAssetDataStorages();
            builder.RegisterFileDataStorages();
            builder.Register<DataManager>(Lifetime.Singleton).AsImplementedInterfaces();
        }
    }
}
#endif