#nullable enable
namespace UniT.Data.Converters.Default
{
    using System;
    using UnityEngine;
    using UnityEngine.Scripting;

    /// <summary>
    ///     Depends on <see cref="TupleConverter" />
    /// </summary>
    public sealed class Vector4Converter : Converter<Vector4>
    {
        [Preserve]
        public Vector4Converter()
        {
        }

        private static readonly Type TupleType = typeof((float, float, float, float));

        protected override object ConvertFromString(Type type, string str)
        {
            var tuple = ((float, float, float, float))this.Manager.ConvertFromString(TupleType, str);
            return new Vector4(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4);
        }

        protected override string ConvertToString(Type type, object obj)
        {
            var vector = (Vector4)obj;
            return this.Manager.ConvertToString(TupleType, (vector.x, vector.y, vector.z, vector.w));
        }
    }
}