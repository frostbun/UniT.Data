#nullable enable
namespace UniT.Data.Storages
{
    using System;
    using System.Threading;
    using Cysharp.Threading.Tasks;

    public interface IWritableStorage : IStorage
    {
        public UniTask WriteAsync(string key, object value, Type type, IProgress<float>? progress = null, CancellationToken cancellationToken = default);

        public void Flush();
    }
}