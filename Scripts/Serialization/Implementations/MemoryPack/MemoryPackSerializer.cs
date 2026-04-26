#if UNIT_MEMORYPACK
#nullable enable
namespace UniT.Data.Serialization
{
    using System;
    using System.Reflection;
    using MemoryPack;
    using UnityEngine.Scripting;

    public sealed class MemoryPackSerializer : Serializer<byte[], object>
    {
        private readonly MemoryPackSerializerOptions options;

        [Preserve]
        public MemoryPackSerializer(MemoryPackSerializerOptions options)
        {
            this.options = options;
        }

        protected override bool CanSerialize(Type type) => base.CanSerialize(type) && type.GetCustomAttribute<MemoryPackableAttribute>() is { };

        public override object Deserialize(Type type, byte[] rawData)
        {
            return MemoryPack.MemoryPackSerializer.Deserialize(type, rawData, this.options)!;
        }

        public override byte[] Serialize(object data)
        {
            return MemoryPack.MemoryPackSerializer.Serialize(data.GetType(), data, this.options);
        }
    }
}
#endif