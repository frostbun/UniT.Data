#nullable enable
namespace UniT.Data.Serializers.Protobuf.DI
{
    using UniT.DI;

    public static class ProtobufSerializerInternalDI
    {
        public static void AddProtobufSerializer(this DependencyContainer container)
        {
            container.AddInterfaces<ProtobufSerializer>();
        }
    }
}