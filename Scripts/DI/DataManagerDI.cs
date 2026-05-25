#if UNIT_DI
#nullable enable
namespace UniT.Data.DI
{
    using UniT.Data.Converters.DI;
    using UniT.Data.Serializers.DI;
    using UniT.Data.Storages.DI;
    using UniT.DI;
    using UniT.Logging.DI;

    public static class DataManagerDI
    {
        public static void AddDataManager(this DependencyContainer container)
        {
            if (container.Contains<IDataManager>()) return;
            container.AddLoggerManager();
            container.AddConverterManager();
            container.AddSerializers();
            container.AddAssetStorages();
            #if !UNITY_WEBGL
            container.AddFileStorages();
            #else
            container.AddPlayerPrefsStorages();
            #endif
            container.AddInterfaces<DataManager>();
        }
    }
}
#endif