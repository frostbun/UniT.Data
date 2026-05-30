#nullable enable
namespace UniT.Data.Storages.PlayerPrefs.DI
{
    using Zenject;

    public static class PlayerPrefsStoragesZenject
    {
        public static void BindPlayerPrefsStorages(this DiContainer container)
        {
            container.BindInterfacesTo<PlayerPrefsBinaryStorage>().AsSingle();
            container.BindInterfacesTo<PlayerPrefsTextStorage>().AsSingle();
        }
    }
}