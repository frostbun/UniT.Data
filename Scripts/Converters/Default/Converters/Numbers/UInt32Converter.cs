#nullable enable
namespace UniT.Data.Converters.Default
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

        protected override object ConvertFromString(Type type, string str)
        {
            return uint.Parse(str, this.formatProvider);
        }

        protected override string ConvertToString(Type type, object obj)
        {
            return ((uint)obj).ToString(this.formatProvider);
        }
    }
}