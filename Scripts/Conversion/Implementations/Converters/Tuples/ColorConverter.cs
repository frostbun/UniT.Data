#nullable enable
namespace UniT.Data.Conversion
{
    using System;
    using UnityEngine;
    using UnityEngine.Scripting;

    /// <summary>
    ///     Depends on <see cref="TupleConverter"/>
    /// </summary>
    public sealed class ColorConverter : Converter<Color>
    {
        [Preserve]
        public ColorConverter()
        {
        }

        private static readonly Type TupleType = typeof((float, float, float, float));

        protected override object ConvertFromString(string str, Type type)
        {
            var tuple = ((float, float, float, float))this.Manager.ConvertFromString(str, TupleType);
            return new Color(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4);
        }

        protected override string ConvertToString(object obj, Type type)
        {
            var color = (Color)obj;
            return this.Manager.ConvertToString((color.r, color.g, color.b, color.a), TupleType);
        }
    }
}