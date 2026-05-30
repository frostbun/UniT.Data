#nullable enable
namespace UniT.Data.Storages.PlayerPrefs.DI
{
    using VContainer;

    public static class PlayerPrefsStoragesVContainer
    {
        public static void RegisterPlayerPrefsStorages(this IContainerBuilder builder)
        {
            builder.Register<PlayerPrefsBinaryStorage>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<PlayerPrefsTextStorage>(Lifetime.Singleton).AsImplementedInterfaces();
        }
    }
}