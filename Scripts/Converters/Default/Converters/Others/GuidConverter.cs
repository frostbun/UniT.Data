#nullable enable
namespace UniT.Data.Converters.Default
{
    using System;
    using UnityEngine.Scripting;

    public sealed class GuidConverter : Converter<Guid>
    {
        [Preserve]
        public GuidConverter()
        {
        }

        protected override object ConvertFromString(Type type, string str)
        {
            return Guid.Parse(str);
        }

        protected override string ConvertToString(Type type, object obj)
        {
            return obj.ToString();
        }
    }
}