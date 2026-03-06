#if UNIT_ZENJECT
#nullable enable
namespace UniT.Data.Storage.DI
{
    using UniT.ResourceManagement.DI;
    using Zenject;

    public static class StorageZenject
    {
        public static void BindDataStorages(this DiContainer container)
        {
            container.BindAssetsManager();

            container.BindInterfacesAndSelfTo<AssetBinaryDataStorage>().AsSingle();
            container.BindInterfacesAndSelfTo<AssetTextDataStorage>().AsSingle();
            container.BindInterfacesAndSelfTo<AssetBlobDataStorage>().AsSingle();

            container.BindInterfacesAndSelfTo<PlayerPrefsDataStorage>().AsSingle();
        }

        public static void BindExternalDataStorages(this DiContainer container)
        {
            container.BindInterfacesAndSelfTo<ExternalBinaryDataStorage>().AsSingle();
            container.BindInterfacesAndSelfTo<ExternalTextDataStorage>().AsSingle();
        }
    }
}
#endif