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

    public abstract class EditorWritableDataStorage<TRawData> : DataStorage<TRawData>, IWritableDataStorage
    {
        #region Sync

        void IWritableDataStorage.Write(string key, object value) => this.Write(key, (TRawData)value);

        void IWritableDataStorage.Flush() => this.Flush();

        public abstract void Write(string key, TRawData value);

        public abstract void Flush();

        #endregion

        #region Async

        #if UNIT_UNITASK
        UniTask IWritableDataStorage.WriteAsync(string key, object value, IProgress<float>? progress, CancellationToken cancellationToken) => this.WriteAsync(key, (TRawData)value, progress, cancellationToken);

        UniTask IWritableDataStorage.FlushAsync(IProgress<float>? progress, CancellationToken cancellationToken) => this.FlushAsync(progress, cancellationToken);

        public abstract UniTask WriteAsync(string key, TRawData value, IProgress<float>? progress, CancellationToken cancellationToken);

        public abstract UniTask FlushAsync(IProgress<float>? progress, CancellationToken cancellationToken);
        #else
        IEnumerator IWritableDataStorage.WriteAsync(string key, object value, Action? callback, IProgress<float>? progress) => this.WriteAsync(key, (TRawData)value, callback, progress);

        IEnumerator IWritableDataStorage.FlushAsync(Action? callback, IProgress<float>? progress) => this.FlushAsync(callback, progress);

        public abstract IEnumerator WriteAsync(string key, TRawData value, Action? callback, IProgress<float>? progress);

        public abstract IEnumerator FlushAsync(Action? callback, IProgress<float>? progress);
        #endif

        #endregion
    }
}