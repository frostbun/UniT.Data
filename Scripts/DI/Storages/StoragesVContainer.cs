#if UNIT_VCONTAINER
#nullable enable
namespace UniT.Data.Storages.DI
{
    using UniT.ResourceManagement.DI;
    using VContainer;

    public static class StoragesVContainer
    {
        public static void RegisterAssetStorages(this IContainerBuilder builder)
        {
            builder.RegisterAssetsManager();

            builder.Register<AssetBinaryStorage>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.Register<AssetTextStorage>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.Register<AssetBlobStorage>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
        }

        public static void RegisterExternalStorages(this IContainerBuilder builder)
        {
            builder.Register<ExternalBinaryStorage>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.Register<ExternalTextStorage>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
        }

        public static void RegisterFileStorages(this IContainerBuilder builder)
        {
            builder.Register<FileBinaryStorage>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.Register<FileTextStorage>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
        }

        public static void RegisterPlayerPrefsStorages(this IContainerBuilder builder)
        {
            builder.Register<PlayerPrefsBinaryStorage>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.Register<PlayerPrefsTextStorage>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
        }
    }
}
#endif