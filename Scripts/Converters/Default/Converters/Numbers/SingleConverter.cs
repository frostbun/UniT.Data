#nullable enable
namespace UniT.Data.Converters.Default
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

        protected override object ConvertFromString(Type type, string str)
        {
            return float.Parse(str, this.formatProvider);
        }

        protected override string ConvertToString(Type type, object obj)
        {
            return ((float)obj).ToString(this.formatProvider);
        }
    }
}