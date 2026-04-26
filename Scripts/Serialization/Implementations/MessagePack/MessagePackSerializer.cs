#if UNIT_MESSAGEPACK
#nullable enable
namespace UniT.Data.Serialization
{
    using System;
    using System.Reflection;
    using MessagePack;
    using UnityEngine.Scripting;

    public sealed class MessagePackSerializer : Serializer<byte[], object>
    {
        private readonly MessagePackSerializerOptions options;

        [Preserve]
        public MessagePackSerializer(MessagePackSerializerOptions options)
        {
            this.options = options;
        }

        protected override bool CanSerialize(Type type) => base.CanSerialize(type) && type.GetCustomAttribute<MessagePackObjectAttribute>() is { };

        public override object Deserialize(Type type, byte[] rawData)
        {
            return MessagePack.MessagePackSerializer.Deserialize(type, rawData, this.options)!;
        }

        public override byte[] Serialize(object data)
        {
            return MessagePack.MessagePackSerializer.Serialize(data.GetType(), data, this.options);
        }
    }
}
#endif