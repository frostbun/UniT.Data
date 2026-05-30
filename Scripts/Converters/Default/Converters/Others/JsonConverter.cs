#nullable enable
namespace UniT.Data.Converters.Default
{
    using System;
    using Newtonsoft.Json;
    using UnityEngine.Scripting;

    public sealed class JsonConverter : Converter<object>
    {
        private readonly JsonSerializerSettings settings;

        [Preserve]
        public JsonConverter(JsonSerializerSettings settings)
        {
            this.settings = settings;
        }

        protected override object ConvertFromString(Type type, string str)
        {
            return JsonConvert.DeserializeObject(str, type, this.settings)!;
        }

        protected override string ConvertToString(Type type, object obj)
        {
            return JsonConvert.SerializeObject(obj, this.settings);
        }
    }
}