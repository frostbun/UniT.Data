#nullable enable
namespace UniT.Data.Storages.File.DI
{
    using UniT.DI;

    public static class FileStoragesInternalDI
    {
        public static void AddFileStorages(this DependencyContainer container)
        {
            container.AddInterfaces<FileBinaryStorage>();
            container.AddInterfaces<FileTextStorage>();
        }
    }
}