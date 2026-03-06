#nullable enable
namespace UniT.Data.Storage
{
    using System;
    using System.IO;
    #if UNIT_UNITASK
    using System.Threading;
    using Cysharp.Threading.Tasks;
    #else
    using System.Collections;
    #endif

    public interface IExternalFileVersionManager
    {
        public Stream? GetFile(string name);

        #if UNIT_UNITASK
        public UniTask<Stream?> GetFileAsync(string name, IProgress<float>? progress = null, CancellationToken cancellationToken = default);
        #else
        public IEnumerator GetFileAsync(string name, Action<Stream?> callback, IProgress<float>? progress = null);
        #endif
    }
}