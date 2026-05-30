#nullable enable
namespace UniT.Data.Storages.Asset.DI
{
    using UniT.DI;

    public static class AssetStoragesInternalDI
    {
        public static void AddAssetStorages(this DependencyContainer container)
        {
            container.AddInterfaces<AssetBinaryStorage>();
            container.AddInterfaces<AssetTextStorage>();
            container.AddInterfaces<AssetObjectStorage>();
        }
    }
}