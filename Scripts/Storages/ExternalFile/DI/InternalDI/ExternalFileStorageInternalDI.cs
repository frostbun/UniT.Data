#nullable enable
namespace UniT.Data.Storages.ExternalFile.DI
{
    using UniT.DI;

    public static class ExternalFileStorageInternalDI
    {
        public static void AddExternalFileStorage<T>(this DependencyContainer container) where T : IExternalFileVersionProvider
        {
            container.AddInterfaces<T>();
            container.AddInterfacesAndSelf<ExternalFileStorage>();
        }
    }
}