#nullable enable
namespace UniT.Data.Storages.Asset.DI
{
    using Zenject;

    public static class AssetStorageZenject
    {
        public static void BindAssetStorage(this DiContainer container)
        {
            container.BindInterfacesAndSelfTo<AssetStorage>().AsSingle();
        }
    }
}