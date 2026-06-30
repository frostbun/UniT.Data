#nullable enable
namespace UniT.Data.Serializers.Yaml
{
    using System;
    using SharpYaml;
    using UnityEngine.Scripting;
    using BaseSerializer = SharpYaml.YamlSerializer;

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
            return BaseSerializer.Deserialize(rawData, type, this.options)!;
        }

        public override string Serialize(Type type, object data)
        {
            return BaseSerializer.Serialize(data, type, this.options);
        }

        public override T Deserialize<T>(string rawData)
        {
            return BaseSerializer.Deserialize<T>(rawData, this.options)!;
        }

        public override string Serialize<T>(T data)
        {
            return BaseSerializer.Serialize(data, this.options);
        }
    }
}