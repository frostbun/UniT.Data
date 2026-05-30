#nullable enable
namespace UniT.Data.Serializers.MemoryPack
{
    using System;
    using System.Reflection;
    using global::MemoryPack;
    using UnityEngine.Scripting;
    using BaseSerializer = global::MemoryPack.MemoryPackSerializer;

    public sealed class MemoryPackSerializer : Serializer<byte[], object>
    {
        private readonly MemoryPackSerializerOptions options;

        [Preserve]
        public MemoryPackSerializer(MemoryPackSerializerOptions options)
        {
            this.options = options;
        }

        protected override bool CanSerialize(Type type) => base.CanSerialize(type) && type.GetCustomAttribute<MemoryPackableAttribute>() is not null;

        public override object Deserialize(Type type, byte[] rawData)
        {
            return BaseSerializer.Deserialize(type, rawData, this.options)!;
        }

        public override byte[] Serialize(Type type, object data)
        {
            return BaseSerializer.Serialize(type, data, this.options);
        }
    }
}