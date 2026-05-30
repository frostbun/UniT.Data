#nullable enable
namespace UniT.Data.Converters.Default
{
    using System;
    using UnityEngine;
    using UnityEngine.Scripting;

    /// <summary>
    ///     Depends on <see cref="TupleConverter" />
    /// </summary>
    public sealed class Vector2IntConverter : Converter<Vector2Int>
    {
        [Preserve]
        public Vector2IntConverter()
        {
        }

        private static readonly Type TupleType = typeof((int, int));

        protected override object ConvertFromString(Type type, string str)
        {
            var tuple = ((int, int))this.Manager.ConvertFromString(TupleType, str);
            return new Vector2Int(tuple.Item1, tuple.Item2);
        }

        protected override string ConvertToString(Type type, object obj)
        {
            var vector = (Vector2Int)obj;
            return this.Manager.ConvertToString(TupleType, (vector.x, vector.y));
        }
    }
}