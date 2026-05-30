#nullable enable
namespace UniT.Data.Converters.Default
{
    using System;
    using UnityEngine.Scripting;

    public sealed class EnumConverter : Converter
    {
        [Preserve]
        public EnumConverter()
        {
        }

        protected override bool CanConvert(Type type) => type.IsEnum;

        protected override object ConvertFromString(Type type, string str)
        {
            return Enum.Parse(type, str);
        }

        protected override string ConvertToString(Type type, object obj)
        {
            return obj.ToString();
        }
    }
}