#if UNIT_ZENJECT
#nullable enable
namespace UniT.Data.DI
{
    using UniT.Data.Converters.DI;
    using UniT.Data.Serializers.DI;
    using UniT.Data.Storages.DI;
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
            container.BindAssetStorages();
            container.BindFileStorages();
            container.BindInterfacesTo<DataManager>().AsSingle();
        }
    }
}
#endif