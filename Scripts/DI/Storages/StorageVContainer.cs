#if UNIT_VCONTAINER
#nullable enable
namespace UniT.Data.Storage.DI
{
    using UniT.ResourceManagement.DI;
    using VContainer;

    public static class StorageVContainer
    {
        public static void RegisterDataStorages(this IContainerBuilder builder)
        {
            builder.RegisterAssetsManager();

            builder.Register<AssetBinaryDataStorage>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.Register<AssetTextDataStorage>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.Register<AssetBlobDataStorage>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();

            builder.Register<PlayerPrefsDataStorage>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
        }

        public static void RegisterExternalDataStorages(this IContainerBuilder builder)
        {
            builder.Register<ExternalBinaryDataStorage>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.Register<ExternalTextDataStorage>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
        }
    }
}
#endif