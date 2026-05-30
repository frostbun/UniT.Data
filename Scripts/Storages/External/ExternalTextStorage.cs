#nullable enable
namespace UniT.Data.Storages.External
{
    using System;
    using System.IO;
    using System.Threading;
    using Cysharp.Threading.Tasks;
    using UniT.Extensions;
    using UnityEngine.Scripting;

    public class ExternalTextStorage : Storage<string>, IReadableStorage
    {
        private readonly IExternalFileVersionManager externalFileVersionManager;

        [Preserve]
        public ExternalTextStorage(IExternalFileVersionManager externalFileVersionManager)
        {
            this.externalFileVersionManager = externalFileVersionManager;
        }

        protected override bool CanStore(Type type) => !typeof(IWritableData).IsAssignableFrom(type);

        UniTask<bool> IReadableStorage.ContainsAsync(string key, IProgress<float>? progress, CancellationToken cancellationToken)
        {
            return this.externalFileVersionManager.GetFilePathAsync(key, progress, cancellationToken).ContinueWith(Item.IsNotNull);
        }

        async UniTask<object> IReadableStorage.ReadAsync(string key, IProgress<float>? progress, CancellationToken cancellationToken)
        {
            var path = await this.externalFileVersionManager.GetFilePathAsync(key, progress, cancellationToken)
                ?? throw new ArgumentOutOfRangeException(nameof(key), key, $"{key} not found");
            return await File.ReadAllTextAsync(path, cancellationToken);
        }
    }
}