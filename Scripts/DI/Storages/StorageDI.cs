#if UNIT_DI
#nullable enable
namespace UniT.Data.Storage.DI
{
    using UniT.DI;
    using UniT.ResourceManagement.DI;

    public static class StorageDI
    {
        public static void AddDataStorages(this DependencyContainer container)
        {
            container.AddAssetsManager();

            container.AddInterfacesAndSelf<AssetBinaryDataStorage>();
            container.AddInterfacesAndSelf<AssetTextDataStorage>();
            container.AddInterfacesAndSelf<AssetBlobDataStorage>();

            container.AddInterfacesAndSelf<PlayerPrefsDataStorage>();
        }

        public static void AddExternalDataStorages(this DependencyContainer container)
        {
            container.AddInterfacesAndSelf<ExternalBinaryDataStorage>();
            container.AddInterfacesAndSelf<ExternalTextDataStorage>();
        }
    }
}
#endif