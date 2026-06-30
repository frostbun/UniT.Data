#nullable enable
namespace UniT.Data.Serializers
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Runtime.CompilerServices;

    public abstract class Serializer<TRawData, TData> : ISerializer where TRawData : notnull where TData : notnull
    {
        Type ISerializer.RawDataType => typeof(TRawData);

        bool ISerializer.CanSerialize(Type type) => this.CanSerialize(type);

        object ISerializer.Deserialize(Type type, object rawData) => this.Deserialize(type, (TRawData)rawData);

        object ISerializer.Serialize(Type type, object data) => this.Serialize(type, (TData)data);

        protected virtual bool CanSerialize(Type type) => typeof(TData).IsAssignableFrom(type);

        [Pure]
        public abstract TData Deserialize(Type type, TRawData rawData);

        [Pure]
        public abstract TRawData Serialize(Type type, TData data);

        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual T Deserialize<T>(TRawData rawData) where T : TData => (T)this.Deserialize(typeof(T), rawData);

        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual TRawData Serialize<T>(T data) where T : TData => this.Serialize(typeof(T), data);
    }
}