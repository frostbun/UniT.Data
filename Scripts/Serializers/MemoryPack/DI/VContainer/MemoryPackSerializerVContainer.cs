#nullable enable
namespace UniT.Data.Serializers.MemoryPack.DI
{
    using global::MemoryPack;
    using VContainer;
    using MemoryPackSerializer = MemoryPackSerializer;

    public static class MemoryPackSerializerVContainer
    {
        public static void RegisterMemoryPackSerializer(this IContainerBuilder builder)
        {
            builder.RegisterMemoryPackSerializer(MemoryPackSerializerOptions.Default);
        }

        public static void RegisterMemoryPackSerializer(this IContainerBuilder builder, MemoryPackSerializerOptions options)
        {
            builder.RegisterInstance(options);
            builder.Register<MemoryPackSerializer>(Lifetime.Singleton).AsImplementedInterfaces();
        }
    }
}