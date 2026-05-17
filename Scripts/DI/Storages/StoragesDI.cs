#if UNIT_DI
#nullable enable
namespace UniT.Data.Storages.DI
{
    using UniT.DI;
    using UniT.ResourceManagement.DI;

    public static class StoragesDI
    {
        public static void AddAssetStorages(this DependencyContainer container)
        {
            container.AddAssetsManager();

            container.AddInterfacesAndSelf<AssetBinaryStorage>();
            container.AddInterfacesAndSelf<AssetTextStorage>();
            container.AddInterfacesAndSelf<AssetBlobStorage>();
        }

        public static void AddExternalStorages(this DependencyContainer container)
        {
            container.AddInterfacesAndSelf<ExternalBinaryStorage>();
            container.AddInterfacesAndSelf<ExternalTextStorage>();
        }

        public static void AddFileStorages(this DependencyContainer container)
        {
            container.AddInterfacesAndSelf<FileBinaryStorage>();
            container.AddInterfacesAndSelf<FileTextStorage>();
        }

        public static void AddPlayerPrefsStorages(this DependencyContainer container)
        {
            container.AddInterfacesAndSelf<PlayerPrefsBinaryStorage>();
            container.AddInterfacesAndSelf<PlayerPrefsTextStorage>();
        }
    }
}
#endif