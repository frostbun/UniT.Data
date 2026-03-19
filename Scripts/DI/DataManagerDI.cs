#if UNIT_DI
#nullable enable
namespace UniT.Data.DI
{
    using UniT.Data.Conversion.DI;
    using UniT.Data.Serialization.DI;
    using UniT.Data.Storage.DI;
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
            container.AddAssetDataStorages();
            container.AddFileDataStorages();
            container.AddInterfaces<DataManager>();
        }
    }
}
#endif