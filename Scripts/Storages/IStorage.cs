#nullable enable
namespace UniT.Data.Storages
{
    using System;

    public interface IStorage
    {
        public Type RawDataType { get; }

        public bool CanStore(Type type);
    }
}