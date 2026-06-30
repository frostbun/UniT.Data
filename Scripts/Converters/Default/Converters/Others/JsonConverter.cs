#nullable enable
namespace UniT.Data.Converters.Default
{
    using System;
    using UnityEngine;
    using UnityEngine.Scripting;

    public sealed class JsonConverter : Converter
    {
        [Preserve]
        public JsonConverter()
        {
        }

        protected override bool CanConvert(Type type) => true;

        protected override object ConvertFromString(Type type, string str)
        {
            return JsonUtility.FromJson(str, type);
        }

        protected override string ConvertToString(Type type, object obj)
        {
            return JsonUtility.ToJson(obj);
        }
    }
}