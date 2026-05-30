#nullable enable
namespace UniT.Data.Converters.Default
{
    using System;
    using UnityEngine.Scripting;

    public sealed class UriConverter : Converter<Uri>
    {
        [Preserve]
        public UriConverter()
        {
        }

        protected override object ConvertFromString(Type type, string str)
        {
            return new Uri(str);
        }

        protected override string ConvertToString(Type type, object obj)
        {
            return obj.ToString();
        }
    }
}