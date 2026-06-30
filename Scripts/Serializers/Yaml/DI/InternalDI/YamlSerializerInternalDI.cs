#nullable enable
namespace UniT.Data.Serializers.Yaml.DI
{
    using SharpYaml;
    using UniT.DI;
    using YamlSerializer = YamlSerializer;

    public static class YamlSerializerInternalDI
    {
        public static void AddYamlSerializer(this DependencyContainer container)
        {
            container.AddYamlSerializer(new());
        }

        public static void AddYamlSerializer(this DependencyContainer container, YamlSerializerOptions options)
        {
            container.Add(options);
            container.AddInterfacesAndSelf<YamlSerializer>();
        }
    }
}