#nullable enable
namespace UniT.Data.Conversion
{
    using System;
    using UnityEngine;
    using UnityEngine.Scripting;

    /// <summary>
    ///     Depends on <see cref="TupleConverter"/>
    /// </summary>
    public sealed class Color32Converter : Converter<Color32>
    {
        [Preserve]
        public Color32Converter()
        {
        }

        private static readonly Type TupleType = typeof((byte, byte, byte, byte));

        protected override object ConvertFromString(string str, Type type)
        {
            var tuple = ((byte, byte, byte, byte))this.Manager.ConvertFromString(str, TupleType);
            return new Color32(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4);
        }

        protected override string ConvertToString(object obj, Type type)
        {
            var color = (Color32)obj;
            return this.Manager.ConvertToString((color.r, color.g, color.b, color.a), TupleType);
        }
    }
}