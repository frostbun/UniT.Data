#nullable enable
namespace UniT.Data.Storages
{
    using System;
    using System.Threading;
    using Cysharp.Threading.Tasks;

    public interface IExternalFileVersionManager
    {
        public UniTask<string?> GetFilePathAsync(string name, IProgress<float>? progress = null, CancellationToken cancellationToken = default);
    }
}