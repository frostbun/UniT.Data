#nullable enable
namespace UniT.Data.Converters.Default
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

        protected override object ConvertFromString(Type type, string str)
        {
            return Activator.CreateInstance(type, this.Manager.ConvertFromString(MakeDictionaryType(type), str));
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