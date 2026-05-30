#nullable enable
namespace UniT.Data.Serializers.Toml.DI
{
    using Tomlyn;
    using UniT.DI;
    using TomlSerializer = TomlSerializer;

    public static class TomlSerializerInternalDI
    {
        public static void AddTomlSerializer(this DependencyContainer container)
        {
            container.AddTomlSerializer(new());
        }

        public static void AddTomlSerializer(this DependencyContainer container, TomlSerializerOptions options)
        {
            container.Add(options);
            container.AddInterfaces<TomlSerializer>();
        }
    }
}