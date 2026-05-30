#nullable enable
namespace UniT.Data.Converters.Default
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using UniT.Extensions;
    using UnityEngine.Scripting;

    public sealed class TupleConverter : Converter<ITuple>
    {
        private readonly string separator;

        [Preserve]
        public TupleConverter(SeparatorConfig config)
        {
            this.separator = config.TupleSeparator;
        }

        protected override object ConvertFromString(Type type, string str)
        {
            var items     = str.Split(this.separator);
            var itemTypes = type.GetGenericArguments();
            return Activator.CreateInstance(type, IterTools.Zip(itemTypes, items, this.Manager.ConvertFromString).ToArray());
        }

        protected override string ConvertToString(Type type, object obj)
        {
            var tuple     = (ITuple)obj;
            var itemTypes = type.GetGenericArguments();
            return IterTools.Zip(itemTypes, ToEnumerable(tuple), this.Manager.ConvertToString).Join(this.separator);

            static IEnumerable<object> ToEnumerable(ITuple tuple)
            {
                for (var i = 0; i < tuple.Length; ++i) yield return tuple[i];
            }
        }
    }
}