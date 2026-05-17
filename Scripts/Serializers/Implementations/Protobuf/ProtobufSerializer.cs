#if UNIT_PROTOBUF
#nullable enable
namespace UniT.Data.Serializers
{
    using System;
    using Google.Protobuf;
    using UnityEngine.Scripting;

    public sealed class ProtobufSerializer : Serializer<byte[], IMessage>
    {
        [Preserve]
        public ProtobufSerializer()
        {
        }

        public override IMessage Deserialize(Type type, byte[] rawData)
        {
            var data = (IMessage)Activator.CreateInstance(type)!;
            data.MergeFrom(rawData);
            return data;
        }

        public override byte[] Serialize(IMessage data)
        {
            return data.ToByteArray();
        }
    }
}
#endif