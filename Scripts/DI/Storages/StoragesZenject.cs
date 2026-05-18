#if UNIT_ZENJECT
#nullable enable
namespace UniT.Data.Storages.DI
{
    using UniT.ResourceManagement.DI;
    using Zenject;

    public static class StoragesZenject
    {
        public static void BindAssetStorages(this DiContainer container)
        {
            container.BindAssetsManager();

            container.BindInterfacesAndSelfTo<AssetBinaryStorage>().AsSingle();
            container.BindInterfacesAndSelfTo<AssetTextStorage>().AsSingle();
            container.BindInterfacesAndSelfTo<AssetObjectStorage>().AsSingle();
        }

        public static void BindExternalStorages(this DiContainer container)
        {
            container.BindInterfacesAndSelfTo<ExternalBinaryStorage>().AsSingle();
            container.BindInterfacesAndSelfTo<ExternalTextStorage>().AsSingle();
        }

        public static void BindFileStorages(this DiContainer container)
        {
            container.BindInterfacesAndSelfTo<FileBinaryStorage>().AsSingle();
            container.BindInterfacesAndSelfTo<FileTextStorage>().AsSingle();
        }

        public static void BindPlayerPrefsStorages(this DiContainer container)
        {
            container.BindInterfacesAndSelfTo<PlayerPrefsBinaryStorage>().AsSingle();
            container.BindInterfacesAndSelfTo<PlayerPrefsTextStorage>().AsSingle();
        }
    }
}
#endif