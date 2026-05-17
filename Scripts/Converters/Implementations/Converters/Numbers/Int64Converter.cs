#nullable enable
namespace UniT.Data.Converters
{
    using System;
    using UnityEngine.Scripting;

    public sealed class Int64Converter : Converter<long>
    {
        private readonly IFormatProvider formatProvider;

        [Preserve]
        public Int64Converter(IFormatProvider formatProvider)
        {
            this.formatProvider = formatProvider;
        }

        protected override object ConvertFromString(string str, Type type)
        {
            return long.Parse(str, this.formatProvider);
        }

        protected override string ConvertToString(object obj, Type type)
        {
            return ((long)obj).ToString(this.formatProvider);
        }
    }
}