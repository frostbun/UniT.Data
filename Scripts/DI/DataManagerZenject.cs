#if UNIT_ZENJECT
#nullable enable
namespace UniT.Data.DI
{
    using UniT.Data.Conversion.DI;
    using UniT.Data.Serialization.DI;
    using UniT.Data.Storage.DI;
    using UniT.Logging.DI;
    using Zenject;

    public static class DataManagerZenject
    {
        public static void BindDataManager(this DiContainer container)
        {
            if (container.HasBinding<IDataManager>()) return;
            container.BindLoggerManager();
            container.BindConverterManager();
            container.BindSerializers();
            container.BindAssetDataStorages();
            container.BindFileDataStorages();
            container.BindInterfacesTo<DataManager>().AsSingle();
        }
    }
}
#endif