#nullable enable
namespace UniT.Data.Converters
{
    using System;
    using UnityEngine.Scripting;

    public sealed class DateTimeOffsetConverter : Converter<DateTimeOffset>
    {
        private readonly IFormatProvider formatProvider;

        [Preserve]
        public DateTimeOffsetConverter(IFormatProvider formatProvider)
        {
            this.formatProvider = formatProvider;
        }

        protected override object ConvertFromString(string str, Type type)
        {
            return DateTimeOffset.Parse(str, this.formatProvider);
        }

        protected override string ConvertToString(object obj, Type type)
        {
            return ((DateTimeOffset)obj).ToString(this.formatProvider);
        }
    }
}