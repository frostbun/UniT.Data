#nullable enable
namespace UniT.Data.Storages.File.DI
{
    using VContainer;

    public static class FileStoragesVContainer
    {
        public static void RegisterFileStorages(this IContainerBuilder builder)
        {
            builder.Register<FileBinaryStorage>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<FileTextStorage>(Lifetime.Singleton).AsImplementedInterfaces();
        }
    }
}