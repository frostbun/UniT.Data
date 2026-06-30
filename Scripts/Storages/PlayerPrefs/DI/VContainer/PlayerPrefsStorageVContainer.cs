#nullable enable
namespace UniT.Data.Storages.PlayerPrefs.DI
{
    using VContainer;

    public static class PlayerPrefsStorageVContainer
    {
        public static void RegisterPlayerPrefsStorage(this IContainerBuilder builder)
        {
            builder.Register<PlayerPrefsStorage>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
        }
    }
}