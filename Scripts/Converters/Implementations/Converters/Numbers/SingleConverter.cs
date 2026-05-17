#nullable enable
namespace UniT.Data.Converters
{
    using System;
    using UnityEngine.Scripting;

    public sealed class SingleConverter : Converter<float>
    {
        private readonly IFormatProvider formatProvider;

        [Preserve]
        public SingleConverter(IFormatProvider formatProvider)
        {
            this.formatProvider = formatProvider;
        }

        protected override object ConvertFromString(string str, Type type)
        {
            return float.Parse(str, this.formatProvider);
        }

        protected override string ConvertToString(object obj, Type type)
        {
            return ((float)obj).ToString(this.formatProvider);
        }
    }
}