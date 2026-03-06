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
            using var stream = this.externalFileVersionManager.GetFile(key);
            if (stream is null)
            {
                this.logger.Warning($"{key} not found, fallback to local asset");
                return this.assetTextDataStorage.Read(key);
            }
            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }

        #if UNIT_UNITASK
        public override async UniTask<string?> ReadAsync(string key, IProgress<float>? progress, CancellationToken cancellationToken)
        {
            var             subProgresses = progress.CreateSubProgresses(2).ToArray();
            await using var stream        = await this.externalFileVersionManager.GetFileAsync(key, subProgresses[0], cancellationToken);
            if (stream is null)
            {
                this.logger.Warning($"{key} not found, fallback to local asset");
                return await this.assetTextDataStorage.ReadAsync(key, subProgresses[1], cancellationToken);
            }
            using var reader = new StreamReader(stream);
            return await reader.ReadToEndAsync();
        }
        #else
        public override IEnumerator ReadAsync(string key, Action<string?> callback, IProgress<float>? progress)
        {
            var subProgresses = progress.CreateSubProgresses(2).ToArray();
            var s             = default(Stream);
            yield return this.externalFileVersionManager.GetFileAsync(key, result => s = result, subProgresses[0]);
            using var stream = s;
            if (stream is null)
            {
                this.logger.Warning($"{key} not found, fallback to local asset");
                yield return this.assetTextDataStorage.ReadAsync(key, callback, subProgresses[1]);
                yield break;
            }
            using var reader = new StreamReader(stream);
            yield return reader.ReadToEndAsync().ToCoroutine(callback);
        }
        #endif
    }
}