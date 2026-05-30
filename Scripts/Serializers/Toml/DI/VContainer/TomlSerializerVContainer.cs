#nullable enable
namespace UniT.Data.Serializers.Toml.DI
{
    using Tomlyn;
    using VContainer;
    using TomlSerializer = TomlSerializer;

    public static class TomlSerializerVContainer
    {
        public static void RegisterTomlSerializer(this IContainerBuilder builder)
        {
            builder.RegisterTomlSerializer(new());
        }

        public static void RegisterTomlSerializer(this IContainerBuilder builder, TomlSerializerOptions options)
        {
            builder.RegisterInstance(new TomlSerializerOptions());
            builder.Register<TomlSerializer>(Lifetime.Singleton).AsImplementedInterfaces();
        }
    }
}