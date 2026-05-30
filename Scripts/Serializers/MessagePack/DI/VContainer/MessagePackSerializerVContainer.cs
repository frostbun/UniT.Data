#nullable enable
namespace UniT.Data.Serializers.MessagePack.DI
{
    using global::MessagePack;
    using VContainer;
    using MessagePackSerializer = MessagePackSerializer;

    public static class MessagePackSerializerVContainer
    {
        public static void RegisterMessagePackSerializer(this IContainerBuilder builder)
        {
            builder.RegisterMessagePackSerializer(MessagePackSerializerOptions.Standard);
        }

        public static void RegisterMessagePackSerializer(this IContainerBuilder builder, MessagePackSerializerOptions options)
        {
            builder.RegisterInstance(options);
            builder.Register<MessagePackSerializer>(Lifetime.Singleton).AsImplementedInterfaces();
        }
    }
}