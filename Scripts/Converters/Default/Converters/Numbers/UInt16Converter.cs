#nullable enable
namespace UniT.Data.Converters.Default
{
    using System;
    using UnityEngine.Scripting;

    public sealed class UInt16Converter : Converter<ushort>
    {
        private readonly IFormatProvider formatProvider;

        [Preserve]
        public UInt16Converter(IFormatProvider formatProvider)
        {
            this.formatProvider = formatProvider;
        }

        protected override object ConvertFromString(Type type, string str)
        {
            return ushort.Parse(str, this.formatProvider);
        }

        protected override string ConvertToString(Type type, object obj)
        {
            return ((ushort)obj).ToString(this.formatProvider);
        }
    }
}