#nullable enable
namespace UniT.Data.Storages
{
    using System;
    using System.IO;
    using UniT.Extensions;
    using UnityEngine.Scripting;
    #if UNIT_UNITASK
    using System.Threading;
    using Cysharp.Threading.Tasks;
    #else
    using System.Collections;
    #endif

    public class ExternalBinaryStorage : IStorage
    {
        private readonly IExternalFileVersionManager externalFileVersionManager;

        [Preserve]
        public ExternalBinaryStorage(IExternalFileVersionManager externalFileVersionManager)
        {
            this.externalFileVersionManager = externalFileVersionManager;
        }

        protected virtual bool CanStore(Type type) => !typeof(IWritableData).IsAssignableFrom(type);

        Type IStorage.RawDataType => typeof(byte[]);

        bool IStorage.CanStore(Type type) => this.CanStore(type);

        bool IStorage.Contains(string key)
        {
            return this.externalFileVersionManager.GetFilePath(key) is { };
        }

        object IStorage.Read(string key)
        {
            var path = this.externalFileVersionManager.GetFilePath(key)
                ?? throw new ArgumentOutOfRangeException(nameof(key), key, $"{key} not found");
            return File.ReadAllText(path);
        }

        #if UNIT_UNITASK
        UniTask<bool> IStorage.ContainsAsync(string key, IProgress<float>? progress, CancellationToken cancellationToken)
        {
            return this.externalFileVersionManager.GetFilePathAsync(key, progress, cancellationToken).ContinueWith(Item.IsNotNull);
        }

        async UniTask<object> IStorage.ReadAsync(string key, IProgress<float>? progress, CancellationToken cancellationToken)
        {
            var path = await this.externalFileVersionManager.GetFilePathAsync(key, progress, cancellationToken)
                ?? throw new ArgumentOutOfRangeException(nameof(key), key, $"{key} not found");
            return await File.ReadAllTextAsync(path, cancellationToken);
        }
        #else
        IEnumerator IStorage.ContainsAsync(string key, Action<bool> callback, IProgress<float>? progress)
        {
            return this.externalFileVersionManager.GetFilePathAsync(key, path => callback(path is { }), progress);
        }

        IEnumerator IStorage.ReadAsync(string key, Action<object> callback, IProgress<float>? progress)
        {
            var path = default(string)!;
            yield return this.externalFileVersionManager.GetFilePathAsync(
                key,
                result => path = result
                    ?? throw new ArgumentOutOfRangeException(nameof(key), key, $"{key} not found"),
                progress
            );
            yield return File.ReadAllTextAsync(path).ToCoroutine(callback);
        }
        #endif
    }
}