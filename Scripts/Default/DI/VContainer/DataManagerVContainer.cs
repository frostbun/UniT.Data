#nullable enable
namespace UniT.Data.Default.DI
{
    using VContainer;

    public static class DataManagerVContainer
    {
        public static void RegisterDataManager(this IContainerBuilder builder)
        {
            builder.Register<DataManager>(Lifetime.Singleton).AsImplementedInterfaces();
        }
    }
}