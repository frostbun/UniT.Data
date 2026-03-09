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

    public interface IExternalFileVersionManager
    {
        public string? GetFilePath(string name);

        #if UNIT_UNITASK
        public UniTask<string?> GetFilePathAsync(string name, IProgress<float>? progress = null, CancellationToken cancellationToken = default);
        #else
        public IEnumerator GetFilePathAsync(string name, Action<string?> callback, IProgress<float>? progress = null);
        #endif
    }
}