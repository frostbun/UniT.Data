#nullable enable
namespace UniT.Data.Converters
{
    using System;
    using UnityEngine.Scripting;

    public sealed class Int16Converter : Converter<short>
    {
        private readonly IFormatProvider formatProvider;

        [Preserve]
        public Int16Converter(IFormatProvider formatProvider)
        {
            this.formatProvider = formatProvider;
        }

        protected override object ConvertFromString(string str, Type type)
        {
            return short.Parse(str, this.formatProvider);
        }

        protected override string ConvertToString(object obj, Type type)
        {
            return ((short)obj).ToString(this.formatProvider);
        }
    }
}