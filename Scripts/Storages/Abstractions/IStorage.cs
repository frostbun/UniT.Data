#nullable enable
namespace UniT.Data.Storages
{
    using System;
    #if UNIT_UNITASK
    using System.Threading;
    using Cysharp.Threading.Tasks;
    #else
    using System.Collections;
    #endif

    public interface IStorage
    {
        public Type RawDataType { get; }

        public bool CanStore(Type type);

        public bool Contains(string key);

        public object Read(string key);

        #if UNIT_UNITASK
        public UniTask<bool> ContainsAsync(string key, IProgress<float>? progress = null, CancellationToken cancellationToken = default);

        public UniTask<object> ReadAsync(string key, IProgress<float>? progress = null, CancellationToken cancellationToken = default);
        #else
        public IEnumerator ContainsAsync(string key, Action<bool> callback, IProgress<float>? progress = null);

        public IEnumerator ReadAsync(string key, Action<object> callback, IProgress<float>? progress = null);
        #endif
    }
}