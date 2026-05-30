#nullable enable
namespace UniT.Data.Converters.Default
{
    using System;
    using UnityEngine.Scripting;

    public sealed class BooleanConverter : Converter<bool>
    {
        [Preserve]
        public BooleanConverter()
        {
        }

        protected override object ConvertFromString(Type type, string str)
        {
            return bool.Parse(str);
        }

        protected override string ConvertToString(Type type, object obj)
        {
            return obj.ToString();
        }
    }
}