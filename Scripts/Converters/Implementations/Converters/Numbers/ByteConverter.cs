#nullable enable
namespace UniT.Data.Converters
{
    using System;
    using UnityEngine.Scripting;

    public sealed class ByteConverter : Converter<byte>
    {
        private readonly IFormatProvider formatProvider;

        [Preserve]
        public ByteConverter(IFormatProvider formatProvider)
        {
            this.formatProvider = formatProvider;
        }

        protected override object ConvertFromString(string str, Type type)
        {
            return byte.Parse(str, this.formatProvider);
        }

        protected override string ConvertToString(object obj, Type type)
        {
            return ((byte)obj).ToString(this.formatProvider);
        }
    }
}