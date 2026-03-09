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

    public sealed class ExternalTextDataStorage : DataStorage<string>
    {
        private readonly IExternalFileVersionManager externalFileVersionManager;
        private readonly AssetTextDataStorage        assetTextDataStorage;
        private readonly ILogger                     logger;

        [Preserve]
        public ExternalTextDataStorage(IExternalFileVersionManager externalFileVersionManager, AssetTextDataStorage assetTextDataStorage, ILoggerManager loggerManager)
        {
            this.externalFileVersionManager = externalFileVersionManager;
            this.assetTextDataStorage       = assetTextDataStorage;
            this.logger                     = loggerManager.GetLogger(this);
        }

        public override string? Read(string key)
        {
            var path = this.externalFileVersionManager.GetFilePath(key);
            if (path is null)
            {
                this.logger.Warning($"{key} not found, fallback to local asset");
                return this.assetTextDataStorage.Read(key);
            }
            return File.ReadAllText(path);
        }

        #if UNIT_UNITASK
        public override async UniTask<string?> ReadAsync(string key, IProgress<float>? progress, CancellationToken cancellationToken)
        {
            var subProgresses = progress.CreateSubProgresses(2).ToArray();
            var path          = await this.externalFileVersionManager.GetFilePathAsync(key, subProgresses[0], cancellationToken);
            if (path is null)
            {
                this.logger.Warning($"{key} not found, fallback to local asset");
                return await this.assetTextDataStorage.ReadAsync(key, subProgresses[1], cancellationToken);
            }
            return await File.ReadAllTextAsync(path, cancellationToken);
        }
        #else
        public override IEnumerator ReadAsync(string key, Action<string?> callback, IProgress<float>? progress)
        {
            var subProgresses = progress.CreateSubProgresses(2).ToArray();
            var path          = default(string);
            yield return this.externalFileVersionManager.GetFilePathAsync(key, result => path = result, subProgresses[0]);
            if (path is null)
            {
                this.logger.Warning($"{key} not found, fallback to local asset");
                yield return this.assetTextDataStorage.ReadAsync(key, callback, subProgresses[1]);
                yield break;
            }
            yield return File.ReadAllTextAsync(path).ToCoroutine(callback);
        }
        #endif
    }
}