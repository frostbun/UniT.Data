#nullable enable
namespace UniT.Data.Serializers.Yaml.DI
{
    using SharpYaml;
    using Zenject;
    using YamlSerializer = YamlSerializer;

    public static class YamlSerializerZenject
    {
        public static void BindYamlSerializer(this DiContainer container)
        {
            container.BindYamlSerializer(new());
        }

        public static void BindYamlSerializer(this DiContainer container, YamlSerializerOptions options)
        {
            container.BindInstance(options);
            container.BindInterfacesAndSelfTo<YamlSerializer>().AsSingle();
        }
    }
}