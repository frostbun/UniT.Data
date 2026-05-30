#nullable enable
namespace UniT.Data.Converters.Default
{
    using System;
    using UnityEngine.Scripting;

    public sealed class StringConverter : Converter<string>
    {
        [Preserve]
        public StringConverter()
        {
        }

        protected override object ConvertFromString(Type type, string str)
        {
            return str;
        }

        protected override string ConvertToString(Type type, object obj)
        {
            return (string)obj;
        }
    }
}