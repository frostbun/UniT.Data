#nullable enable
namespace UniT.Data.Converters
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

        protected override object ConvertFromString(string str, Type type)
        {
            return ushort.Parse(str, this.formatProvider);
        }

        protected override string ConvertToString(object obj, Type type)
        {
            return ((ushort)obj).ToString(this.formatProvider);
        }
    }
}