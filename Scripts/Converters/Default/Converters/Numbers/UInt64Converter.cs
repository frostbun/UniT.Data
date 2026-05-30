#nullable enable
namespace UniT.Data.Converters.Default
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

        protected override object ConvertFromString(Type type, string str)
        {
            return ulong.Parse(str, this.formatProvider);
        }

        protected override string ConvertToString(Type type, object obj)
        {
            return ((ulong)obj).ToString(this.formatProvider);
        }
    }
}