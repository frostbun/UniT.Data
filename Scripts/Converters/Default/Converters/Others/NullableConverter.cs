#nullable enable
namespace UniT.Data.Converters.Default
{
    using System;
    using UnityEngine.Scripting;

    public sealed class NullableConverter : Converter
    {
        [Preserve]
        public NullableConverter()
        {
        }

        protected override bool CanConvert(Type type) => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);

        protected override object ConvertFromString(Type type, string str)
        {
            return this.Manager.ConvertFromString(Nullable.GetUnderlyingType(type)!, str);
        }

        protected override string ConvertToString(Type type, object obj)
        {
            return obj.ToString();
        }
    }
}