#nullable enable
namespace UniT.Data.Storages.File.DI
{
    using Zenject;

    public static class AssetStoragesZenject
    {
        public static void BindFileStorages(this DiContainer container)
        {
            container.BindInterfacesTo<FileBinaryStorage>().AsSingle();
            container.BindInterfacesTo<FileTextStorage>().AsSingle();
        }
    }
}