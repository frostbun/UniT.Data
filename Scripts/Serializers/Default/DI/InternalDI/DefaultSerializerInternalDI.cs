#nullable enable
namespace UniT.Data.Serializers.Default.DI
{
    using UniT.DI;

    public static class DefaultSerializerInternalDI
    {
        public static void AddDefaultSerializer(this DependencyContainer container)
        {
            container.AddInterfaces<DefaultSerializer>();
        }
    }
}