#nullable enable
namespace UniT.Data.Storages
{
    using System;
    using System.Threading;
    using Cysharp.Threading.Tasks;

    public interface IReadableStorage : IStorage
    {
        public UniTask<bool> ContainsAsync(string key, IProgress<float>? progress = null, CancellationToken cancellationToken = default);

        public UniTask<object> ReadAsync(string key, Type type, IProgress<float>? progress = null, CancellationToken cancellationToken = default);
    }
}