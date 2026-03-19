#if UNIT_ZENJECT
#nullable enable
namespace UniT.Data.Storage.DI
{
    using UniT.ResourceManagement.DI;
    using Zenject;

    public static class StorageZenject
    {
        public static void BindAssetDataStorages(this DiContainer container)
        {
            container.BindAssetsManager();

            container.BindInterfacesAndSelfTo<AssetBinaryDataStorage>().AsSingle();
            container.BindInterfacesAndSelfTo<AssetTextDataStorage>().AsSingle();
            container.BindInterfacesAndSelfTo<AssetBlobDataStorage>().AsSingle();
        }

        public static void BindExternalDataStorages(this DiContainer container)
        {
            container.BindInterfacesAndSelfTo<ExternalBinaryDataStorage>().AsSingle();
            container.BindInterfacesAndSelfTo<ExternalTextDataStorage>().AsSingle();
        }

        public static void BindFileDataStorages(this DiContainer container)
        {
            container.BindInterfacesAndSelfTo<FileBinaryDataStorage>().AsSingle();
            container.BindInterfacesAndSelfTo<FileTextDataStorage>().AsSingle();
        }

        public static void BindPlayerPrefsDataStorages(this DiContainer container)
        {
            container.BindInterfacesAndSelfTo<PlayerPrefsBinaryDataStorage>().AsSingle();
            container.BindInterfacesAndSelfTo<PlayerPrefsTextDataStorage>().AsSingle();
        }
    }
}
#endif