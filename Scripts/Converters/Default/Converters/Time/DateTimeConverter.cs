#nullable enable
namespace UniT.Data.Converters.Default
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

        protected override object ConvertFromString(Type type, string str)
        {
            return DateTime.Parse(str, this.formatProvider);
        }

        protected override string ConvertToString(Type type, object obj)
        {
            return ((DateTime)obj).ToString(this.formatProvider);
        }
    }
}