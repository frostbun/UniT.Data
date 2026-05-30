#nullable enable
namespace UniT.Data.Serializers.Unity
{
    using System;
    using UnityEngine.Scripting;
    using Object = UnityEngine.Object;

    public sealed class UnitySerializer : Serializer<Object, Object>
    {
        [Preserve]
        public UnitySerializer()
        {
        }

        public override Object Deserialize(Type type, Object rawData) => rawData;

        public override Object Serialize(Type type, Object data) => data;
    }
}