#nullable enable
namespace UniT.Data.Storages.ExternalFile.DI
{
    using VContainer;

    public static class ExternalFileStorageVContainer
    {
        public static void RegisterExternalFileStorage<T>(this IContainerBuilder builder) where T : IExternalFileVersionProvider
        {
            builder.Register<T>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<ExternalFileStorage>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
        }
    }
}