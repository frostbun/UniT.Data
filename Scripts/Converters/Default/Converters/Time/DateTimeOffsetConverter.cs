#nullable enable
namespace UniT.Data.Converters.Default
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

        protected override object ConvertFromString(Type type, string str)
        {
            return DateTimeOffset.Parse(str, this.formatProvider);
        }

        protected override string ConvertToString(Type type, object obj)
        {
            return ((DateTimeOffset)obj).ToString(this.formatProvider);
        }
    }
}