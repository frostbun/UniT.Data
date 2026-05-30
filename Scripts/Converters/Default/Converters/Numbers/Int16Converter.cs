#nullable enable
namespace UniT.Data.Converters.Default
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

        protected override object ConvertFromString(Type type, string str)
        {
            return short.Parse(str, this.formatProvider);
        }

        protected override string ConvertToString(Type type, object obj)
        {
            return ((short)obj).ToString(this.formatProvider);
        }
    }
}