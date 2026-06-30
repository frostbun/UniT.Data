#nullable enable
namespace UniT.Data.Storages.PlayerPrefs.DI
{
    using UniT.DI;

    public static class PlayerPrefsStorageInternalDI
    {
        public static void AddPlayerPrefsStorage(this DependencyContainer container)
        {
            container.AddInterfacesAndSelf<PlayerPrefsStorage>();
        }
    }
}