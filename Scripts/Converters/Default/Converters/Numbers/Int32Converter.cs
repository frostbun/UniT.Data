#nullable enable
namespace UniT.Data.Converters.Default
{
    using System;
    using UnityEngine.Scripting;

    public sealed class Int32Converter : Converter<int>
    {
        private readonly IFormatProvider formatProvider;

        [Preserve]
        public Int32Converter(IFormatProvider formatProvider)
        {
            this.formatProvider = formatProvider;
        }

        protected override object ConvertFromString(Type type, string str)
        {
            return int.Parse(str, this.formatProvider);
        }

        protected override string ConvertToString(Type type, object obj)
        {
            return ((int)obj).ToString(this.formatProvider);
        }
    }
}