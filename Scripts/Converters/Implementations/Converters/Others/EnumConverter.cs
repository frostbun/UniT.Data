#nullable enable
namespace UniT.Data.Converters
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

        protected override object ConvertFromString(string str, Type type)
        {
            return Enum.Parse(type, str);
        }

        protected override string ConvertToString(object obj, Type type)
        {
            return obj.ToString();
        }
    }
}