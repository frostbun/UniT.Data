#if UNIT_DI
#nullable enable
namespace UniT.Data.Storage.DI
{
    using UniT.DI;
    using UniT.ResourceManagement.DI;

    public static class StorageDI
    {
        public static void AddAssetDataStorages(this DependencyContainer container)
        {
            container.AddAssetsManager();

            container.AddInterfacesAndSelf<AssetBinaryDataStorage>();
            container.AddInterfacesAndSelf<AssetTextDataStorage>();
            container.AddInterfacesAndSelf<AssetBlobDataStorage>();
        }

        public static void AddExternalDataStorages(this DependencyContainer container)
        {
            container.AddInterfacesAndSelf<ExternalBinaryDataStorage>();
            container.AddInterfacesAndSelf<ExternalTextDataStorage>();
        }

        public static void AddFileDataStorages(this DependencyContainer container)
        {
            container.AddInterfacesAndSelf<FileBinaryDataStorage>();
            container.AddInterfacesAndSelf<FileTextDataStorage>();
        }

        public static void AddPlayerPrefsDataStorages(this DependencyContainer container)
        {
            container.AddInterfacesAndSelf<PlayerPrefsBinaryDataStorage>();
            container.AddInterfacesAndSelf<PlayerPrefsTextDataStorage>();
        }
    }
}
#endif