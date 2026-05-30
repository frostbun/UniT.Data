#nullable enable
namespace UniT.Data.Storages
{
    using System;

    public abstract class Storage<TRawData> : IStorage where TRawData : notnull
    {
        Type IStorage.RawDataType => typeof(TRawData);

        bool IStorage.CanStore(Type type) => this.CanStore(type);

        protected abstract bool CanStore(Type type);
    }
}