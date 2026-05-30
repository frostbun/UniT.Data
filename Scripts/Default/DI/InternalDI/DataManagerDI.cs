#nullable enable
namespace UniT.Data.Default.DI
{
    using UniT.DI;

    public static class DataManagerDI
    {
        public static void AddDataManager(this DependencyContainer container)
        {
            container.AddInterfaces<DataManager>();
        }
    }
}