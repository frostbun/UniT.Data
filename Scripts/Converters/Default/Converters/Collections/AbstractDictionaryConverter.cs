#nullable enable
namespace UniT.Data.Converters.Default
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine.Scripting;

    /// <summary>
    ///     Depends on <see cref="DictionaryConverter" />
    /// </summary>
    public sealed class AbstractDictionaryConverter : Converter
    {
        private static readonly IReadOnlyCollection<Type> SupportedTypes = new HashSet<Type>
        {
            typeof(IDictionary<,>),
            typeof(IReadOnlyDictionary<,>),
        };

        [Preserve]
        public AbstractDictionaryConverter()
        {
        }

        protected override bool CanConvert(Type type) => type.IsGenericType && SupportedTypes.Contains(type.GetGenericTypeDefinition());

        protected override object? GetDefaultValue(Type type)
        {
            return this.Manager.GetDefaultValue(MakeDictionaryType(type));
        }

        protected override object ConvertFromString(Type type, string str)
        {
            return this.Manager.ConvertFromString(MakeDictionaryType(type), str);
        }

        protected override string ConvertToString(Type type, object obj)
        {
            return this.Manager.ConvertToString(MakeDictionaryType(type), obj);
        }

        private static Type MakeDictionaryType(Type type)
        {
            return typeof(Dictionary<,>).MakeGenericType(type.GetGenericArguments());
        }
    }
}