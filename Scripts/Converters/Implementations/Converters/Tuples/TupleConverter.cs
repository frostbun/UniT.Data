#nullable enable
namespace UniT.Data.Converters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using UniT.Extensions;
    using UnityEngine.Scripting;

    public sealed class TupleConverter : Converter
    {
        private readonly string separator;

        [Preserve]
        public TupleConverter(SeparatorConfig config)
        {
            this.separator = config.TupleSeparator;
        }

        protected override bool CanConvert(Type type) => type.GetInterfaces().Contains(typeof(ITuple));

        protected override object ConvertFromString(string str, Type type)
        {
            var items     = str.Split(this.separator);
            var itemTypes = type.GetGenericArguments();
            return Activator.CreateInstance(type, IterTools.Zip(items, itemTypes, this.Manager.ConvertFromString).ToArray());
        }

        protected override string ConvertToString(object obj, Type type)
        {
            var tuple     = (ITuple)obj;
            var itemTypes = type.GetGenericArguments();
            return IterTools.Zip(ToEnumerable(tuple), itemTypes, this.Manager.ConvertToString).Join(this.separator);

            static IEnumerable<object> ToEnumerable(ITuple tuple)
            {
                for (var i = 0; i < tuple.Length; ++i) yield return tuple[i];
            }
        }
    }
}