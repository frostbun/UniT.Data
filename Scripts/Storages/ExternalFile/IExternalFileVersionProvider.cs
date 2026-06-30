#nullable enable
namespace UniT.Data.Storages.ExternalFile
{
    using System;
    using System.Threading;
    using Cysharp.Threading.Tasks;

    public interface IExternalFileVersionProvider
    {
        public UniTask<(string Version, string DownloadUrl)> FetchVersionAsync(IProgress<float>? progress = null, CancellationToken cancellationToken = default);
    }
}