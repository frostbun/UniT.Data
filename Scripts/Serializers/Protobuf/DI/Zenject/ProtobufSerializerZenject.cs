#nullable enable
namespace UniT.Data.Serializers.Protobuf.DI
{
    using Zenject;

    public static class ProtobufSerializerZenject
    {
        public static void BindProtobufSerializer(this DiContainer container)
        {
            container.BindInterfacesTo<ProtobufSerializer>().AsSingle();
        }
    }
}