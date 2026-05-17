#nullable enable
namespace UniT.Data
{
    using System;
    using System.Runtime.CompilerServices;
    using UniT.Extensions;
    #if UNIT_UNITASK
    using System.Threading;
    using Cysharp.Threading.Tasks;
    #else
    using System.Collections;
    #endif

    public interface IDataManager
    {
        #region Sync

        public object Load(string key, Type type);

        public void Save(string key, object data);

        public void Flush();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Load<T>(string key) => (T)this.Load(key, typeof(T));

        #region Implicit Key

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public object Load(Type type) => this.Load(type.GetKey(), type);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Save(object data) => this.Save(data.GetType().GetKey(), data);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Load<T>() => (T)this.Load(typeof(T).GetKey(), typeof(T));

        #endregion

        #endregion

        #region Async

        #if UNIT_UNITASK
        public UniTask<object> LoadAsync(string key, Type type, IProgress<float>? progress = null, CancellationToken cancellationToken = default);

        public UniTask SaveAsync(string key, object data, IProgress<float>? progress = null, CancellationToken cancellationToken = default);

        public UniTask FlushAsync(IProgress<float>? progress = null, CancellationToken cancellationToken = default);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public UniTask<T> LoadAsync<T>(string key, IProgress<float>? progress = null, CancellationToken cancellationToken = default) => this.LoadAsync(key, typeof(T), progress, cancellationToken).ContinueWith(data => (T)data);

        #region Implicit Key

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public UniTask<object> LoadAsync(Type type, IProgress<float>? progress = null, CancellationToken cancellationToken = default) => this.LoadAsync(type.GetKey(), type, progress, cancellationToken);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public UniTask SaveAsync(object data, IProgress<float>? progress = null, CancellationToken cancellationToken = default) => this.SaveAsync(data.GetType().GetKey(), data, progress, cancellationToken);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public UniTask<T> LoadAsync<T>(IProgress<float>? progress = null, CancellationToken cancellationToken = default) => this.LoadAsync(typeof(T).GetKey(), typeof(T), progress, cancellationToken).ContinueWith(data => (T)data);

        #endregion

        #else
        public IEnumerator LoadAsync(string key, Type type, Action<object> callback, IProgress<float>? progress = null);

        public IEnumerator SaveAsync(string key, object data, Action? callback = null, IProgress<float>? progress = null);

        public IEnumerator FlushAsync(Action? callback = null, IProgress<float>? progress = null);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IEnumerator LoadAsync<T>(string key, Action<T> callback, IProgress<float>? progress = null) => this.LoadAsync(key, typeof(T), data => callback((T)data), progress);

        #region Implicit Key

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IEnumerator LoadAsync(Type type, Action<object> callback, IProgress<float>? progress = null) => this.LoadAsync(type.GetKey(), type, callback, progress);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IEnumerator SaveAsync(object data, Action? callback = null, IProgress<float>? progress = null) => this.SaveAsync(data.GetType().GetKey(), data, callback, progress);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IEnumerator LoadAsync<T>(Action<T> callback, IProgress<float>? progress = null) => this.LoadAsync(typeof(T).GetKey(), typeof(T), data => callback((T)data), progress);

        #endregion

        #endif

        #endregion
    }
}