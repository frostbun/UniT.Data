#nullable enable
namespace UniT.Data.Storages.PlayerPrefs.DI
{
    using UniT.DI;

    public static class PlayerPrefsStoragesInternalDI
    {
        public static void AddPlayerPrefsStorages(this DependencyContainer container)
        {
            container.AddInterfaces<PlayerPrefsBinaryStorage>();
            container.AddInterfaces<PlayerPrefsTextStorage>();
        }
    }
}