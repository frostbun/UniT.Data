#nullable enable
namespace UniT.Data.Converters
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

        protected override object ConvertFromString(string str, Type type)
        {
            return int.Parse(str, this.formatProvider);
        }

        protected override string ConvertToString(object obj, Type type)
        {
            return ((int)obj).ToString(this.formatProvider);
        }
    }
}