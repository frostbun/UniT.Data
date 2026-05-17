#nullable enable
namespace UniT.Data.Converters
{
    using System;
    using UnityEngine.Scripting;

    public sealed class UInt32Converter : Converter<uint>
    {
        private readonly IFormatProvider formatProvider;

        [Preserve]
        public UInt32Converter(IFormatProvider formatProvider)
        {
            this.formatProvider = formatProvider;
        }

        protected override object ConvertFromString(string str, Type type)
        {
            return uint.Parse(str, this.formatProvider);
        }

        protected override string ConvertToString(object obj, Type type)
        {
            return ((uint)obj).ToString(this.formatProvider);
        }
    }
}