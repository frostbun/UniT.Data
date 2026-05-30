#nullable enable
namespace UniT.Data.Serializers.Toml.DI
{
    using Tomlyn;
    using Zenject;
    using TomlSerializer = TomlSerializer;

    public static class TomlSerializerZenject
    {
        public static void BindTomlSerializer(this DiContainer container)
        {
            container.BindTomlSerializer(new());
        }

        public static void BindTomlSerializer(this DiContainer container, TomlSerializerOptions options)
        {
            container.BindInstance(options);
            container.BindInterfacesTo<TomlSerializer>().AsSingle();
        }
    }
}