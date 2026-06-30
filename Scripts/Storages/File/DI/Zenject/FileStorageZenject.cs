#nullable enable
namespace UniT.Data.Storages.File.DI
{
    using Zenject;

    public static class FileStorageZenject
    {
        public static void BindFileStorage(this DiContainer container)
        {
            container.BindInterfacesAndSelfTo<FileStorage>().AsSingle();
        }
    }
}