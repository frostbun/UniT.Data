#nullable enable
namespace UniT.Data.Storages.External.DI
{
    using VContainer;

    public static class ExternalStoragesVContainer
    {
        public static void RegisterExternalStorages(this IContainerBuilder builder)
        {
            builder.Register<ExternalBinaryStorage>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<ExternalTextStorage>(Lifetime.Singleton).AsImplementedInterfaces();
        }
    }
}