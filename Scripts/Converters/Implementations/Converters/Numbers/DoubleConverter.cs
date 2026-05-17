#nullable enable
namespace UniT.Data.Converters
{
    using System;
    using UnityEngine.Scripting;

    public sealed class DoubleConverter : Converter<double>
    {
        private readonly IFormatProvider formatProvider;

        [Preserve]
        public DoubleConverter(IFormatProvider formatProvider)
        {
            this.formatProvider = formatProvider;
        }

        protected override object ConvertFromString(string str, Type type)
        {
            return double.Parse(str, this.formatProvider);
        }

        protected override string ConvertToString(object obj, Type type)
        {
            return ((double)obj).ToString(this.formatProvider);
        }
    }
}