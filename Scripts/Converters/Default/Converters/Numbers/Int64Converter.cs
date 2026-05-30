#nullable enable
namespace UniT.Data.Converters.Default
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

        protected override object ConvertFromString(Type type, string str)
        {
            return long.Parse(str, this.formatProvider);
        }

        protected override string ConvertToString(Type type, object obj)
        {
            return ((long)obj).ToString(this.formatProvider);
        }
    }
}