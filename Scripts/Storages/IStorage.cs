#nullable enable
namespace UniT.Data.Storages
{
    using System;

    public interface IStorage
    {
        public bool CanStore(Type type);
    }
}