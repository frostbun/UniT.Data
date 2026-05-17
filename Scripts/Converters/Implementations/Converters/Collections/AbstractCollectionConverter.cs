#nullable enable
namespace UniT.Data.Converters
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
        private static readonly IReadOnlyCollection<Type> SupportedTypes = new HashSet<Type>
        {
            typeof(ICollection<>),
            typeof(IList<>),
            typeof(IReadOnlyCollection<>),
            typeof(IReadOnlyList<>),
        };

        [Preserve]
        public AbstractCollectionConverter()
        {
        }

        protected override bool CanConvert(Type type) => type.IsGenericType && SupportedTypes.Contains(type.GetGenericTypeDefinition());

        protected override object? GetDefaultValue(Type type)
        {
            return this.Manager.GetDefaultValue(MakeArrayType(type));
        }

        protected override object ConvertFromString(string str, Type type)
        {
            return this.Manager.ConvertFromString(str, MakeArrayType(type));
        }

        protected override string ConvertToString(object obj, Type type)
        {
            return this.Manager.ConvertToString(obj as Array ?? ((IEnumerable)obj).Cast<object>().ToArray(), MakeArrayType(type));
        }

        private static Type MakeArrayType(Type type)
        {
            return type.GetGenericArguments()[0].MakeArrayType();
        }
    }
}