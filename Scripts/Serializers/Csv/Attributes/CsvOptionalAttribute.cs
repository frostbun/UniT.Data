#nullable enable
namespace UniT.Data.Serializers.Csv
{
    using System;
    using System.Reflection;

    [AttributeUsage(AttributeTargets.Field)]
    public sealed class CsvOptionalAttribute : Attribute
    {
    }

    public static class CsvOptionalAttributeExtensions
    {
        public static bool IsCsvOptional(this FieldInfo field)
        {
            return field.GetCustomAttribute<CsvOptionalAttribute>() is not null;
        }
    }
}