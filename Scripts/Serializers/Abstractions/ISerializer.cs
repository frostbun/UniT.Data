#nullable enable
namespace UniT.Data.Serializers
{
    using System;

    public interface ISerializer
    {
        public Type RawDataType { get; }

        public bool CanSerialize(Type type);

        public object Deserialize(Type type, object rawData);

        public object Serialize(object data);
    }
}