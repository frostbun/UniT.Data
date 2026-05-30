#nullable enable
namespace UniT.Data.Converters.Default
{
    using System;
    using UnityEngine.Scripting;

    public sealed class CharConverter : Converter<char>
    {
        [Preserve]
        public CharConverter()
        {
        }

        protected override object ConvertFromString(Type type, string str)
        {
            return char.Parse(str);
        }

        protected override string ConvertToString(Type type, object obj)
        {
            return obj.ToString();
        }
    }
}