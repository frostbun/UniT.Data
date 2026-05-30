#nullable enable
namespace UniT.Data.Storages.External.DI
{
    using UniT.DI;

    public static class ExternalStoragesInternalDI
    {
        public static void AddExternalStorages(this DependencyContainer container)
        {
            container.AddInterfaces<ExternalBinaryStorage>();
            container.AddInterfaces<ExternalTextStorage>();
        }
    }
}