#nullable enable
namespace UniT.Data.Serializers.MemoryPack.DI
{
    using global::MemoryPack;
    using UniT.DI;
    using MemoryPackSerializer = MemoryPackSerializer;

    public static class MemoryPackSerializerInternalDI
    {
        public static void AddMemoryPackSerializer(this DependencyContainer container)
        {
            container.AddMemoryPackSerializer(MemoryPackSerializerOptions.Default);
        }

        public static void AddMemoryPackSerializer(this DependencyContainer container, MemoryPackSerializerOptions options)
        {
            container.Add(options);
            container.AddInterfacesAndSelf<MemoryPackSerializer>();
        }
    }
}