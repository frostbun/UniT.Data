#nullable enable
namespace UniT.Data.Serializers.Toml
{
    using System;
    using Tomlyn;
    using UnityEngine.Scripting;
    using BaseSerializer = Tomlyn.TomlSerializer;

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