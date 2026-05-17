#if UNIT_JSON
#nullable enable
namespace UniT.Data.Serializers
{
    using System;
    using Newtonsoft.Json;
    using UnityEngine.Scripting;

    public sealed class JsonSerializer : Serializer<string, object>
    {
        private readonly JsonSerializerSettings settings;

        [Preserve]
        public JsonSerializer(JsonSerializerSettings settings)
        {
            this.settings = settings;
        }

        public override object Deserialize(Type type, string rawData)
        {
            return JsonConvert.DeserializeObject(rawData, type, this.settings)!;
        }

        public override string Serialize(object data)
        {
            return JsonConvert.SerializeObject(data, this.settings);
        }
    }
}
#endif