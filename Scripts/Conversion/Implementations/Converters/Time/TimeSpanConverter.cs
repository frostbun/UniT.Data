#nullable enable
namespace UniT.Data.Conversion
{
    using System;
    using UnityEngine.Scripting;

    public sealed class TimeSpanConverter : Converter<TimeSpan>
    {
        [Preserve]
        public TimeSpanConverter()
        {
        }

        protected override object ConvertFromString(string str, Type type)
        {
            return TimeSpan.Parse(str);
        }

        protected override string ConvertToString(object obj, Type type)
        {
            return obj.ToString();
        }
    }
}