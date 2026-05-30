#nullable enable
namespace UniT.Data.Serializers.Yaml.DI
{
    using SharpYaml;
    using VContainer;
    using YamlSerializer = YamlSerializer;

    public static class YamlSerializerVContainer
    {
        public static void RegisterYamlSerializer(this IContainerBuilder builder)
        {
            builder.RegisterYamlSerializer(new());
        }

        public static void RegisterYamlSerializer(this IContainerBuilder builder, YamlSerializerOptions options)
        {
            builder.RegisterInstance(options);
            builder.Register<YamlSerializer>(Lifetime.Singleton).AsImplementedInterfaces();
        }
    }
}