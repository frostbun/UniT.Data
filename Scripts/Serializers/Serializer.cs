#nullable enable
namespace UniT.Data.Serializers
{
    using System;

    public abstract class Serializer<TRawData, TData> : ISerializer where TRawData : notnull where TData : notnull
    {
        Type ISerializer.RawDataType => typeof(TRawData);

        bool ISerializer.CanSerialize(Type type) => this.CanSerialize(type);

        object ISerializer.Deserialize(Type type, object rawData) => this.Deserialize(type, (TRawData)rawData);

        object ISerializer.Serialize(Type type, object data) => this.Serialize(type, (TData)data);

        protected virtual bool CanSerialize(Type type) => typeof(TData).IsAssignableFrom(type);

        public abstract TData Deserialize(Type type, TRawData rawData);

        public abstract TRawData Serialize(Type type, TData data);
    }
}