#nullable enable
namespace UniT.Data.Converters.Default
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine.Scripting;

    /// <summary>
    ///     Depends on <see cref="ArrayConverter" />
    /// </summary>
    public sealed class AbstractCollectionConverter : Converter
    {
        [Preserve]
        public AbstractCollectionConverter()
        {
        }

        protected override bool CanConvert(Type type)
        {
            if (!type.IsGenericType) return false;
            type = type.GetGenericTypeDefinition();
            return type == typeof(IReadOnlyList<>) || type == typeof(IReadOnlyCollection<>) || type == typeof(IList<>) || type == typeof(ICollection<>) || type == typeof(IEnumerable<>);
        }

        protected override object? GetDefaultValue(Type type)
        {
            return this.Manager.GetDefaultValue(MakeArrayType(type));
        }

        protected override object ConvertFromString(Type type, string str)
        {
            return this.Manager.ConvertFromString(MakeArrayType(type), str);
        }

        protected override string ConvertToString(Type type, object obj)
        {
            return this.Manager.ConvertToString(MakeArrayType(type), obj as Array ?? ((IEnumerable)obj).Cast<object>().ToArray());
        }

        private static Type MakeArrayType(Type type)
        {
            return type.GetGenericArguments()[0].MakeArrayType();
        }
    }
}