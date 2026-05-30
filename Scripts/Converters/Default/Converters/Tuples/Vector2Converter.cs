#nullable enable
namespace UniT.Data.Converters.Default
{
    using System;
    using UnityEngine;
    using UnityEngine.Scripting;

    /// <summary>
    ///     Depends on <see cref="TupleConverter" />
    /// </summary>
    public sealed class Vector2Converter : Converter<Vector2>
    {
        [Preserve]
        public Vector2Converter()
        {
        }

        private static readonly Type TupleType = typeof((float, float));

        protected override object ConvertFromString(Type type, string str)
        {
            var tuple = ((float, float))this.Manager.ConvertFromString(TupleType, str);
            return new Vector2(tuple.Item1, tuple.Item2);
        }

        protected override string ConvertToString(Type type, object obj)
        {
            var vector = (Vector2)obj;
            return this.Manager.ConvertToString(TupleType, (vector.x, vector.y));
        }
    }
}