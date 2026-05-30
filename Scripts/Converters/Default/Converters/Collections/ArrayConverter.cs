#nullable enable
namespace UniT.Data.Converters.Default
{
    using System;
    using System.Linq;
    using UniT.Extensions;
    using UnityEngine.Scripting;

    public sealed class ArrayConverter : Converter
    {
        private readonly string separator;

        [Preserve]
        public ArrayConverter(SeparatorConfig config)
        {
            this.separator = config.CollectionSeparator;
        }

        protected override bool CanConvert(Type type) => type.IsArray;

        protected override object? GetDefaultValue(Type type)
        {
            return Array.CreateInstance(type.GetElementType()!, 0);
        }

        protected override object ConvertFromString(Type type, string str)
        {
            var elementType      = type.GetElementType()!;
            var elementConverter = this.Manager.GetConverter(elementType);
            var elements         = str.Split(this.separator);
            var array            = Array.CreateInstance(elementType, elements.Length);
            for (var i = 0; i < elements.Length; ++i)
            {
                array.SetValue(elementConverter.ConvertFromString(elementType, elements[i]), i);
            }
            return array;
        }

        protected override string ConvertToString(Type type, object obj)
        {
            var elementType      = type.GetElementType()!;
            var elementConverter = this.Manager.GetConverter(elementType);
            return ((Array)obj).Cast<object>()
                .Select(static (element, state) => state.elementConverter.ConvertToString(state.elementType, element), (elementConverter, elementType))
                .Join(this.separator);
        }
    }
}