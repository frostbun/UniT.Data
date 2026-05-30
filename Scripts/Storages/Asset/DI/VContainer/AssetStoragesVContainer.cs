#nullable enable
namespace UniT.Data.Storages.Asset.DI
{
    using VContainer;

    public static class AssetStoragesVContainer
    {
        public static void RegisterAssetStorages(this IContainerBuilder builder)
        {
            builder.Register<AssetBinaryStorage>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<AssetTextStorage>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<AssetObjectStorage>(Lifetime.Singleton).AsImplementedInterfaces();
        }
    }
}