#if UNIT_CSV
#nullable enable
namespace UniT.Data.Serializers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using UniT.Extensions;

    [AttributeUsage(AttributeTargets.Field)]
    public sealed class CsvIgnoreAttribute : Attribute
    {
    }

    public static class CsvIgnoreAttributeExtensions
    {
        private static bool IsCsvIgnored(this FieldInfo field)
        {
            return field.GetCustomAttribute<CsvIgnoreAttribute>() is not null;
        }

        public static (List<FieldInfo> NormalFields, List<FieldInfo> NestedFields) GetCsvFields(this Type type)
        {
            return type.GetAllFields()
                .Where(field => !field.IsCsvIgnored())
                .Split(field => !typeof(ICsvData).IsAssignableFrom(field.FieldType));
        }
    }
}
#endif