#nullable enable
namespace UniT.Data.Converters.Default
{
    using System;
    using UnityEngine;
    using UnityEngine.Scripting;

    /// <summary>
    ///     Depends on <see cref="TupleConverter" />
    /// </summary>
    public sealed class ColorConverter : Converter<Color>
    {
        [Preserve]
        public ColorConverter()
        {
        }

        private static readonly Type TupleType = typeof((float, float, float, float));

        protected override object ConvertFromString(Type type, string str)
        {
            var tuple = ((float, float, float, float))this.Manager.ConvertFromString(TupleType, str);
            return new Color(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4);
        }

        protected override string ConvertToString(Type type, object obj)
        {
            var color = (Color)obj;
            return this.Manager.ConvertToString(TupleType, (color.r, color.g, color.b, color.a));
        }
    }
}