#nullable enable
namespace UniT.Data.Converters.Default
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UniT.Extensions;
    using UnityEngine.Scripting;

    /// <summary>
    ///     Depends on <see cref="ArrayConverter" />
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

        protected override object ConvertFromString(Type type, string str)
        {
            var keyType        = type.GetGenericArguments()[0];
            var valueType      = type.GetGenericArguments()[1];
            var keyConverter   = this.Manager.GetConverter(keyType);
            var valueConverter = this.Manager.GetConverter(valueType);
            var dictionary     = (IDictionary)Activator.CreateInstance(type);
            foreach (var item in (string[])this.Manager.ConvertFromString(ArrayType, str))
            {
                var kv = item.Split(this.separator);
                dictionary.Add(
                    keyConverter.ConvertFromString(keyType, kv[0]),
                    valueConverter.ConvertFromString(valueType, kv[1])
                );
            }
            return dictionary;
        }

        protected override string ConvertToString(Type type, object obj)
        {
            var keyType        = type.GetGenericArguments()[0];
            var valueType      = type.GetGenericArguments()[1];
            var keyConverter   = this.Manager.GetConverter(keyType);
            var valueConverter = this.Manager.GetConverter(valueType);
            var dictionary     = (IDictionary)obj;
            return this.Manager.ConvertToString(
                ArrayType,
                IterTools.Zip(
                    dictionary.Keys.Cast<object>(),
                    dictionary.Values.Cast<object>()
                ).Select(
                    static (key, value, state) => state.separator.Wrap(
                        state.keyConverter.ConvertToString(state.keyType, key),
                        state.valueConverter.ConvertToString(state.valueType, value)
                    ),
                    (this.separator, keyType, valueType, keyConverter, valueConverter)
                ).ToArray()
            );
        }
    }
}