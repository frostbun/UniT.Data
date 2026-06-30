#nullable enable
namespace UniT.Data.Storages.Asset.DI
{
    using VContainer;

    public static class AssetStorageVContainer
    {
        public static void RegisterAssetStorage(this IContainerBuilder builder)
        {
            builder.Register<AssetStorage>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
        }
    }
}