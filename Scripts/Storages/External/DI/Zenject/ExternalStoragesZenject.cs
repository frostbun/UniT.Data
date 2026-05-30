#nullable enable
namespace UniT.Data.Storages.External.DI
{
    using Zenject;

    public static class ExternalStoragesZenject
    {
        public static void BindExternalStorages(this DiContainer container)
        {
            container.BindInterfacesTo<ExternalBinaryStorage>().AsSingle();
            container.BindInterfacesTo<ExternalTextStorage>().AsSingle();
        }
    }
}