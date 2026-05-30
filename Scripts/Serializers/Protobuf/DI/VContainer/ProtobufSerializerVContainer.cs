#nullable enable
namespace UniT.Data.Serializers.Protobuf.DI
{
    using VContainer;

    public static class ProtobufSerializerVContainer
    {
        public static void RegisterProtobufSerializer(this IContainerBuilder builder)
        {
            builder.Register<ProtobufSerializer>(Lifetime.Singleton).AsImplementedInterfaces();
        }
    }
}