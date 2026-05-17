#nullable enable
namespace UniT.Data.Converters
{
    using System;
    using UnityEngine.Scripting;

    public sealed class DateTimeConverter : Converter<DateTime>
    {
        private readonly IFormatProvider formatProvider;

        [Preserve]
        public DateTimeConverter(IFormatProvider formatProvider)
        {
            this.formatProvider = formatProvider;
        }

        protected override object ConvertFromString(string str, Type type)
        {
            return DateTime.Parse(str, this.formatProvider);
        }

        protected override string ConvertToString(object obj, Type type)
        {
            return ((DateTime)obj).ToString(this.formatProvider);
        }
    }
}