#nullable enable
namespace UniT.Data.Converters
{
    using System;
    using UnityEngine.Scripting;

    public sealed class UInt64Converter : Converter<ulong>
    {
        private readonly IFormatProvider formatProvider;

        [Preserve]
        public UInt64Converter(IFormatProvider formatProvider)
        {
            this.formatProvider = formatProvider;
        }

        protected override object ConvertFromString(string str, Type type)
        {
            return ulong.Parse(str, this.formatProvider);
        }

        protected override string ConvertToString(object obj, Type type)
        {
            return ((ulong)obj).ToString(this.formatProvider);
        }
    }
}