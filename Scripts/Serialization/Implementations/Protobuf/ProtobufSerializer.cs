#if UNIT_PROTOBUF
#nullable enable
namespace UniT.Data.Serialization
{
    using System;
    using Google.Protobuf;
    using UnityEngine.Scripting;

    public sealed class ProtobufSerializer : Serializer<byte[], object>
    {
        [Preserve]
        public ProtobufSerializer()
        {
        }

        protected override bool CanSerialize(Type type) => base.CanSerialize(type) && typeof(IMessage).IsAssignableFrom(type);

        public override object Deserialize(Type type, byte[] rawData)
        {
            var data = (IMessage)Activator.CreateInstance(type)!;
            data.MergeFrom(rawData);
            return data;
        }

        public override byte[] Serialize(object data)
        {
            return ((IMessage)data).ToByteArray();
        }
    }
}
#endif