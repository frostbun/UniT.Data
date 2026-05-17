#nullable enable
namespace UniT.Data.Converters
{
    using System;
    using UnityEngine.Scripting;

    public sealed class CharConverter : Converter<char>
    {
        [Preserve]
        public CharConverter()
        {
        }

        protected override object ConvertFromString(string str, Type type)
        {
            return char.Parse(str);
        }

        protected override string ConvertToString(object obj, Type type)
        {
            return obj.ToString();
        }
    }
}