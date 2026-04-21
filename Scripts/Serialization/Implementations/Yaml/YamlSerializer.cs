#if UNIT_YAML
#nullable enable
namespace UniT.Data.Serialization
{
    using System;
    using SharpYaml;
    using UnityEngine.Scripting;

    public sealed class YamlSerializer : Serializer<string, IYamlData>
    {
        private readonly YamlSerializerOptions options;

        [Preserve]
        public YamlSerializer(YamlSerializerOptions options)
        {
            this.options = options;
        }

        public override IYamlData Deserialize(Type type, string rawData)
        {
            return (IYamlData)SharpYaml.YamlSerializer.Deserialize(rawData, type, this.options)!;
        }

        public override string Serialize(IYamlData data)
        {
            return SharpYaml.YamlSerializer.Serialize(data, this.options);
        }
    }
}
#endif