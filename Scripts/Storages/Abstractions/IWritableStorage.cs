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

    public interface IWritableStorage : IStorage
    {
        public void Write(string key, object value);

        #if UNIT_UNITASK
        public UniTask WriteAsync(string key, object value, IProgress<float>? progress = null, CancellationToken cancellationToken = default);
        #else
        public IEnumerator WriteAsync(string key, object value, Action? callback = null, IProgress<float>? progress = null);
        #endif
    }
}