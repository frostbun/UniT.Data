#nullable enable
namespace UniT.Data.Serializers.MessagePack.DI
{
    using global::MessagePack;
    using UniT.DI;
    using MessagePackSerializer = MessagePackSerializer;

    public static class MessagePackSerializerInternalDI
    {
        public static void AddMessagePackSerializer(this DependencyContainer container)
        {
            container.AddMessagePackSerializer(MessagePackSerializerOptions.Standard);
        }

        public static void AddMessagePackSerializer(this DependencyContainer container, MessagePackSerializerOptions options)
        {
            container.Add(options);
            container.AddInterfaces<MessagePackSerializer>();
        }
    }
}