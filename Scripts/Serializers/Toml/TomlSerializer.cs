#nullable enable
namespace UniT.Data.Serializers.Toml
{
    using System;
    using Tomlyn;
    using UnityEngine.Scripting;

    public sealed class TomlSerializer : Serializer<string, object>
    {
        private readonly TomlSerializerOptions options;

        [Preserve]
        public TomlSerializer(TomlSerializerOptions options)
        {
            this.options = options;
        }

        public override object Deserialize(Type type, string rawData)
        {
            return Tomlyn.TomlSerializer.Deserialize(rawData, type, this.options)!;
        }

        public override string Serialize(Type type, object data)
        {
            return Tomlyn.TomlSerializer.Serialize(data, this.options);
        }
    }
}