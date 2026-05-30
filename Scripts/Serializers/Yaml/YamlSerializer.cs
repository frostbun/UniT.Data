#nullable enable
namespace UniT.Data.Serializers.Yaml
{
    using System;
    using SharpYaml;
    using UnityEngine.Scripting;

    public sealed class YamlSerializer : Serializer<string, object>
    {
        private readonly YamlSerializerOptions options;

        [Preserve]
        public YamlSerializer(YamlSerializerOptions options)
        {
            this.options = options;
        }

        public override object Deserialize(Type type, string rawData)
        {
            return SharpYaml.YamlSerializer.Deserialize(rawData, type, this.options)!;
        }

        public override string Serialize(Type type, object data)
        {
            return SharpYaml.YamlSerializer.Serialize(data, this.options);
        }
    }
}