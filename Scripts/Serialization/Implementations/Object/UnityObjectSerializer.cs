#nullable enable
namespace UniT.Data.Serialization
{
    using System;
    using UnityEngine.Scripting;
    using Object = UnityEngine.Object;

    public sealed class UnityObjectSerializer : Serializer<Object, Object>
    {
        [Preserve]
        public UnityObjectSerializer()
        {
        }

        public override Object Deserialize(Type type, Object rawData) => rawData;

        public override Object Serialize(Object data) => data;
    }
}