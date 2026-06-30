#nullable enable
namespace UniT.Data.Converters.Default
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UniT.Extensions;
    using UnityEngine;
    using UnityEngine.Scripting;

    public sealed class AbstractTupleConverter : Converter
    {
        private static readonly IReadOnlyDictionary<Type, Type> SupportedTypes = new Dictionary<Type, Type>
        {
            { typeof(Vector2), typeof(float) },
            { typeof(Vector3), typeof(float) },
            { typeof(Vector4), typeof(float) },
            { typeof(Quaternion), typeof(float) },
            { typeof(Vector2Int), typeof(int) },
            { typeof(Vector3Int), typeof(int) },
            { typeof(Color), typeof(float) },
            { typeof(Color32), typeof(byte) },
        };

        private readonly string separator;

        [Preserve]
        public AbstractTupleConverter(SeparatorConfig config)
        {
            this.separator = config.TupleSeparator;
        }

        protected override bool CanConvert(Type type) => SupportedTypes.ContainsKey(type);

        protected override object ConvertFromString(Type type, string str)
        {
            var items     = str.Split(this.separator);
            var itemType  = SupportedTypes[type];
            var converter = this.Manager.GetConverter(itemType);
            return Activator.CreateInstance(
                type,
                items.Select(
                    static (item, state) => state.converter.ConvertFromString(state.itemType, item),
                    (itemType, converter)
                ).ToArray()
            );
        }

        protected override string ConvertToString(Type type, object obj)
        {
            var items = obj switch
            {
                Vector2 vector2       => new object[] { vector2.x, vector2.y },
                Vector3 vector3       => new object[] { vector3.x, vector3.y, vector3.z },
                Vector4 vector4       => new object[] { vector4.x, vector4.y, vector4.z, vector4.w },
                Quaternion quaternion => new object[] { quaternion.x, quaternion.y, quaternion.z, quaternion.w },
                Vector2Int vector2Int => new object[] { vector2Int.x, vector2Int.y },
                Vector3Int vector3Int => new object[] { vector3Int.x, vector3Int.y, vector3Int.z },
                Color color           => new object[] { color.r, color.g, color.b, color.a },
                Color32 color32       => new object[] { color32.r, color32.g, color32.b, color32.a },
                _                     => throw new NotSupportedException($"Unsupported type: {obj.GetType().Name}"),
            };
            var itemType  = SupportedTypes[type];
            var converter = this.Manager.GetConverter(itemType);
            return items.Select(item => converter.ConvertToString(itemType, item)).Join(this.separator);
        }
    }
}