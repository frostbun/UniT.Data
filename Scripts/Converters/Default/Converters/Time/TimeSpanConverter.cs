#nullable enable
namespace UniT.Data.Converters.Default
{
    using System;
    using UnityEngine.Scripting;

    public sealed class TimeSpanConverter : Converter<TimeSpan>
    {
        [Preserve]
        public TimeSpanConverter()
        {
        }

        protected override object ConvertFromString(Type type, string str)
        {
            return TimeSpan.Parse(str);
        }

        protected override string ConvertToString(Type type, object obj)
        {
            return obj.ToString();
        }
    }
}