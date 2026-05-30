#nullable enable
namespace UniT.Data.Converters.Default
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using UnityEngine.Scripting;

    /// <summary>
    ///     Depends on <see cref="ArrayConverter" />
    /// </summary>
    public sealed class CollectionConverter : Converter
    {
        private static readonly IReadOnlyCollection<Type> SupportedTypes = new HashSet<Type>
        {
            typeof(List<>),
            typeof(ReadOnlyCollection<>),
            typeof(HashSet<>),
            typeof(Stack<>),
            typeof(Queue<>),
        };

        [Preserve]
        public CollectionConverter()
        {
        }

        protected override bool CanConvert(Type type) => type.IsGenericType && SupportedTypes.Contains(type.GetGenericTypeDefinition());

        protected override object? GetDefaultValue(Type type)
        {
            return Activator.CreateInstance(type, this.Manager.GetDefaultValue(MakeArrayType(type)));
        }

        protected override object ConvertFromString(Type type, string str)
        {
            return Activator.CreateInstance(type, this.Manager.ConvertFromString(MakeArrayType(type), str));
        }

        protected override string ConvertToString(Type type, object obj)
        {
            return this.Manager.ConvertToString(MakeArrayType(type), ((IEnumerable)obj).Cast<object>().ToArray());
        }

        private static Type MakeArrayType(Type type)
        {
            return type.GetGenericArguments()[0].MakeArrayType();
        }
    }
}