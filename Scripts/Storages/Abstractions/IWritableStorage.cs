#nullable enable
namespace UniT.Data.Storages
{
    using System;
    using System.Threading;
    using Cysharp.Threading.Tasks;

    public interface IWritableStorage : IReadableStorage
    {
        public UniTask WriteAsync(string key, object value, IProgress<float>? progress = null, CancellationToken cancellationToken = default);
    }
}