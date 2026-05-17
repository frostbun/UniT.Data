#nullable enable
namespace UniT.Data.Converters
{
    using System;
    using UnityEngine.Scripting;

    public sealed class BooleanConverter : Converter<bool>
    {
        [Preserve]
        public BooleanConverter()
        {
        }

        protected override object ConvertFromString(string str, Type type)
        {
            return bool.Parse(str);
        }

        protected override string ConvertToString(object obj, Type type)
        {
            return obj.ToString();
        }
    }
}