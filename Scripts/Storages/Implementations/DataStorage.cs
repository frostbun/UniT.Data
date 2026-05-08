#nullable enable
namespace UniT.Data.Storage
{
    using System;
    #if UNIT_UNITASK
    using System.Threading;
    using Cysharp.Threading.Tasks;
    #else
    using System.Collections;
    #endif

    public abstract class DataStorage<TRawData> : IDataStorage
    {
        Type IDataStorage.RawDataType => typeof(TRawData);

        bool IDataStorage.CanStore(Type type) => this.CanStore(type);

        protected virtual bool CanStore(Type type) => !typeof(IWritableData).IsAssignableFrom(type);

        #region Sync

        object? IDataStorage.Read(string key) => this.Read(key);

        public abstract TRawData? Read(string key);

        #endregion

        #region Async

        #if UNIT_UNITASK
        UniTask<object?> IDataStorage.ReadAsync(string key, IProgress<float>? progress, CancellationToken cancellationToken) => this.ReadAsync(key, progress, cancellationToken).ContinueWith(rawData => (object?)rawData);

        public abstract UniTask<TRawData?> ReadAsync(string key, IProgress<float>? progress, CancellationToken cancellationToken);
        #else
        IEnumerator IDataStorage.ReadAsync(string key, Action<object?> callback, IProgress<float>? progress) => this.ReadAsync(key, rawData => callback(rawData), progress);

        public abstract IEnumerator ReadAsync(string key, Action<TRawData?> callback, IProgress<float>? progress);
        #endif

        #endregion
    }
}