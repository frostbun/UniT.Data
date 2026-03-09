#nullable enable
namespace UniT.Data.Storage
{
    using System;
    using System.IO;
    using System.Linq;
    using UniT.Extensions;
    using UniT.Logging;
    using UnityEngine.Scripting;
    #if UNIT_UNITASK
    using System.Threading;
    using Cysharp.Threading.Tasks;
    #else
    using System.Collections;
    #endif

    public sealed class ExternalBinaryDataStorage : DataStorage<byte[]>
    {
        private readonly IExternalFileVersionManager externalFileVersionManager;
        private readonly AssetBinaryDataStorage      assetBinaryDataStorage;
        private readonly ILogger                     logger;

        [Preserve]
        public ExternalBinaryDataStorage(IExternalFileVersionManager externalFileVersionManager, AssetBinaryDataStorage assetBinaryDataStorage, ILoggerManager loggerManager)
        {
            this.externalFileVersionManager = externalFileVersionManager;
            this.assetBinaryDataStorage     = assetBinaryDataStorage;
            this.logger                     = loggerManager.GetLogger(this);
        }

        public override byte[]? Read(string key)
        {
            var path = this.externalFileVersionManager.GetFilePath(key);
            if (path is null)
            {
                this.logger.Warning($"{key} not found, fallback to local asset");
                return this.assetBinaryDataStorage.Read(key);
            }
            return File.ReadAllBytes(path);
        }

        #if UNIT_UNITASK
        public override async UniTask<byte[]?> ReadAsync(string key, IProgress<float>? progress, CancellationToken cancellationToken)
        {
            var subProgresses = progress.CreateSubProgresses(2).ToArray();
            var path          = await this.externalFileVersionManager.GetFilePathAsync(key, subProgresses[0], cancellationToken);
            if (path is null)
            {
                this.logger.Warning($"{key} not found, fallback to local asset");
                return await this.assetBinaryDataStorage.ReadAsync(key, subProgresses[1], cancellationToken);
            }
            return await File.ReadAllBytesAsync(path, cancellationToken);
        }
        #else
        public override IEnumerator ReadAsync(string key, Action<byte[]?> callback, IProgress<float>? progress)
        {
            var subProgresses = progress.CreateSubProgresses(2).ToArray();
            var path          = default(string);
            yield return this.externalFileVersionManager.GetFilePathAsync(key, result => path = result, subProgresses[0]);
            if (path is null)
            {
                this.logger.Warning($"{key} not found, fallback to local asset");
                yield return this.assetBinaryDataStorage.ReadAsync(key, callback, subProgresses[1]);
                yield break;
            }
            yield return File.ReadAllBytesAsync(path).ToCoroutine(callback);
        }
        #endif
    }
}