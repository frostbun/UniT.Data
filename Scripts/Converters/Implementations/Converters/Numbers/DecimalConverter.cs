#nullable enable
namespace UniT.Data.Converters
{
    using System;
    using UnityEngine.Scripting;

    public sealed class DecimalConverter : Converter<decimal>
    {
        private readonly IFormatProvider formatProvider;

        [Preserve]
        public DecimalConverter(IFormatProvider formatProvider)
        {
            this.formatProvider = formatProvider;
        }

        protected override object ConvertFromString(string str, Type type)
        {
            return decimal.Parse(str, this.formatProvider);
        }

        protected override string ConvertToString(object obj, Type type)
        {
            return ((decimal)obj).ToString(this.formatProvider);
        }
    }
}