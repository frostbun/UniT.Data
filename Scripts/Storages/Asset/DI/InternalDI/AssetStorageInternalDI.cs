#nullable enable
namespace UniT.Data.Storages.Asset.DI
{
    using UniT.DI;

    public static class AssetStorageInternalDI
    {
        public static void AddAssetStorage(this DependencyContainer container)
        {
            container.AddInterfacesAndSelf<AssetStorage>();
        }
    }
}