#nullable enable
namespace UniT.Data.Conversion
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UniT.Extensions;
    using UnityEngine.Scripting;

    /// <summary>
    ///     Depends on <see cref="ArrayConverter"/>
    /// </summary>
    public sealed class DictionaryConverter : Converter
    {
        private readonly string separator;

        [Preserve]
        public DictionaryConverter(SeparatorConfig config)
        {
            this.separator = config.KeyValueSeparator;
        }

        private static readonly Type ArrayType = typeof(string[]);

        protected override bool CanConvert(Type type) => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>);

        protected override object? GetDefaultValue(Type type)
        {
            return Activator.CreateInstance(type, 0);
        }

        protected override object ConvertFromString(string str, Type type)
        {
            var keyType        = type.GetGenericArguments()[0];
            var valueType      = type.GetGenericArguments()[1];
            var keyConverter   = this.Manager.GetConverter(keyType);
            var valueConverter = this.Manager.GetConverter(valueType);
            var dictionary     = (IDictionary)Activator.CreateInstance(type);
            foreach (var item in (string[])this.Manager.ConvertFromString(str, ArrayType))
            {
                var kv = item.Split(this.separator);
                dictionary.Add(
                    keyConverter.ConvertFromString(kv[0], keyType),
                    valueConverter.ConvertFromString(kv[1], valueType)
                );
            }
            return dictionary;
        }

        protected override string ConvertToString(object obj, Type type)
        {
            var keyType        = type.GetGenericArguments()[0];
            var valueType      = type.GetGenericArguments()[1];
            var keyConverter   = this.Manager.GetConverter(keyType);
            var valueConverter = this.Manager.GetConverter(valueType);
            return this.Manager.ConvertToString(
                ((IDictionary)obj).Cast<DictionaryEntry>()
                .Select(
                    (kv, state) => state.separator.Wrap(
                        state.keyConverter.ConvertToString(kv.Key, state.keyType),
                        state.valueConverter.ConvertToString(kv.Value, state.valueType)
                    ),
                    (this.separator, keyConverter, keyType, valueConverter, valueType)
                )
                .ToArray(),
                ArrayType
            );
        }
    }
}