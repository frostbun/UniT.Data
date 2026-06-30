#nullable enable
namespace UniT.Data.Storages.PlayerPrefs.DI
{
    using Zenject;

    public static class PlayerPrefsStorageZenject
    {
        public static void BindPlayerPrefsStorage(this DiContainer container)
        {
            container.BindInterfacesAndSelfTo<PlayerPrefsStorage>().AsSingle();
        }
    }
}