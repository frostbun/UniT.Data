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

    public interface IDataStorage
    {
        public Type RawDataType { get; }

        public bool CanStore(Type type);

        public object? Read(string key);

        #if UNIT_UNITASK
        public UniTask<object?> ReadAsync(string key, IProgress<float>? progress = null, CancellationToken cancellationToken = default);
        #else
        public IEnumerator ReadAsync(string key, Action<object?> callback, IProgress<float>? progress = null);
        #endif
    }
}