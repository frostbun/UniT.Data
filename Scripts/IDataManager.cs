#nullable enable
namespace UniT.Data
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using Cysharp.Threading.Tasks;
    using UniT.Extensions;

    public interface IDataManager
    {
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
    }
}