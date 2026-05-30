#nullable enable
namespace UniT.Data.Serializers.Default
{
    using System;
    using UniT.Data.Converters;
    using UnityEngine.Scripting;

    public sealed class DefaultSerializer : Serializer<string, object>
    {
        private readonly IConverterManager converterManager;

        [Preserve]
        public DefaultSerializer(IConverterManager converterManager)
        {
            this.converterManager = converterManager;
        }

        public override object Deserialize(Type type, string rawData)
        {
            return this.converterManager.ConvertFromString(type, rawData);
        }

        public override string Serialize(Type type, object data)
        {
            return this.converterManager.ConvertToString(type, data);
        }
    }
}