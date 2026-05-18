#nullable enable
namespace UniT.Data.Storages
{
    using System;
    using System.Threading;
    using Cysharp.Threading.Tasks;

    public interface IFlushableStorage : IWritableStorage
    {
        public UniTask FlushAsync(IProgress<float>? progress = null, CancellationToken cancellationToken = default);
    }
}