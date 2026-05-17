#nullable enable
namespace UniT.Data.Converters
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using UnityEngine.Scripting;

    /// <summary>
    ///     Depends on <see cref="DictionaryConverter" />
    /// </summary>
    public sealed class ReadOnlyDictionaryConverter : Converter
    {
        [Preserve]
        public ReadOnlyDictionaryConverter()
        {
        }

        protected override bool CanConvert(Type type) => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ReadOnlyDictionary<,>);

        protected override object? GetDefaultValue(Type type)
        {
            return Activator.CreateInstance(type, this.Manager.GetDefaultValue(MakeDictionaryType(type)));
        }

        protected override object ConvertFromString(string str, Type type)
        {
            return Activator.CreateInstance(type, this.Manager.ConvertFromString(str, MakeDictionaryType(type)));
        }

        protected override string ConvertToString(object obj, Type type)
        {
            return this.Manager.ConvertToString(obj, MakeDictionaryType(type));
        }

        private static Type MakeDictionaryType(Type type)
        {
            return typeof(Dictionary<,>).MakeGenericType(type.GetGenericArguments());
        }
    }
}