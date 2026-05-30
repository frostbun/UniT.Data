#nullable enable
namespace UniT.Data.Converters.Default
{
    using System;
    using UnityEngine;
    using UnityEngine.Scripting;

    /// <summary>
    ///     Depends on <see cref="TupleConverter" />
    /// </summary>
    public sealed class Vector3IntConverter : Converter<Vector3Int>
    {
        [Preserve]
        public Vector3IntConverter()
        {
        }

        private static readonly Type TupleType = typeof((int, int, int));

        protected override object ConvertFromString(Type type, string str)
        {
            var tuple = ((int, int, int))this.Manager.ConvertFromString(TupleType, str);
            return new Vector3Int(tuple.Item1, tuple.Item2, tuple.Item3);
        }

        protected override string ConvertToString(Type type, object obj)
        {
            var vector = (Vector3Int)obj;
            return this.Manager.ConvertToString(TupleType, (vector.x, vector.y, vector.z));
        }
    }
}