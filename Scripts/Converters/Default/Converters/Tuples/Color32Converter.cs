#nullable enable
namespace UniT.Data.Converters.Default
{
    using System;
    using UnityEngine;
    using UnityEngine.Scripting;

    /// <summary>
    ///     Depends on <see cref="TupleConverter" />
    /// </summary>
    public sealed class Color32Converter : Converter<Color32>
    {
        [Preserve]
        public Color32Converter()
        {
        }

        private static readonly Type TupleType = typeof((byte, byte, byte, byte));

        protected override object ConvertFromString(Type type, string str)
        {
            var tuple = ((byte, byte, byte, byte))this.Manager.ConvertFromString(TupleType, str);
            return new Color32(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4);
        }

        protected override string ConvertToString(Type type, object obj)
        {
            var color = (Color32)obj;
            return this.Manager.ConvertToString(TupleType, (color.r, color.g, color.b, color.a));
        }
    }
}