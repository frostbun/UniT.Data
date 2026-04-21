#if UNIT_TOML
#nullable enable
namespace UniT.Data.Serialization
{
    using System;
    using Tomlyn;
    using UnityEngine.Scripting;

    public sealed class TomlSerializer : Serializer<string, ITomlData>
    {
        private readonly TomlSerializerOptions options;

        [Preserve]
        public TomlSerializer(TomlSerializerOptions options)
        {
            this.options = options;
        }

        public override ITomlData Deserialize(Type type, string rawData)
        {
            return (ITomlData)Tomlyn.TomlSerializer.Deserialize(rawData, type, this.options)!;
        }

        public override string Serialize(ITomlData data)
        {
            return Tomlyn.TomlSerializer.Serialize(data, this.options);
        }
    }
}
#endif