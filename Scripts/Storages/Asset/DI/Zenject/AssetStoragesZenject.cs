#nullable enable
namespace UniT.Data.Storages.Asset.DI
{
    using Zenject;

    public static class AssetStoragesZenject
    {
        public static void BindAssetStorages(this DiContainer container)
        {
            container.BindInterfacesTo<AssetBinaryStorage>().AsSingle();
            container.BindInterfacesTo<AssetTextStorage>().AsSingle();
            container.BindInterfacesTo<AssetObjectStorage>().AsSingle();
        }
    }
}