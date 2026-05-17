#nullable enable
namespace UniT.Data.Converters
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

        protected override object ConvertFromString(string str, Type type)
        {
            return this.Manager.ConvertFromString(str, Nullable.GetUnderlyingType(type)!);
        }

        protected override string ConvertToString(object obj, Type type)
        {
            return obj.ToString();
        }
    }
}