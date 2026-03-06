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
            using var stream = this.externalFileVersionManager.GetFile(key);
            if (stream is null)
            {
                this.logger.Warning($"{key} not found, fallback to local asset");
                return this.assetBinaryDataStorage.Read(key);
            }
            using var memoryStream = new MemoryStream();
            stream.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }

        #if UNIT_UNITASK
        public override async UniTask<byte[]?> ReadAsync(string key, IProgress<float>? progress, CancellationToken cancellationToken)
        {
            var             subProgresses = progress.CreateSubProgresses(2).ToArray();
            await using var stream        = await this.externalFileVersionManager.GetFileAsync(key, subProgresses[0], cancellationToken);
            if (stream is null)
            {
                this.logger.Warning($"{key} not found, fallback to local asset");
                return await this.assetBinaryDataStorage.ReadAsync(key, subProgresses[1], cancellationToken);
            }
            using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream, cancellationToken);
            return memoryStream.ToArray();
        }
        #else
        public override IEnumerator ReadAsync(string key, Action<byte[]?> callback, IProgress<float>? progress)
        {
            var subProgresses = progress.CreateSubProgresses(2).ToArray();
            var s             = default(Stream);
            yield return this.externalFileVersionManager.GetFileAsync(key, result => s = result, subProgresses[0]);
            using var stream = s;
            if (stream is null)
            {
                this.logger.Warning($"{key} not found, fallback to local asset");
                yield return this.assetBinaryDataStorage.ReadAsync(key, callback, subProgresses[1]);
                yield break;
            }
            using var memoryStream = new MemoryStream();
            yield return stream.CopyToAsync(memoryStream).ToCoroutine();
            callback(memoryStream.ToArray());
        }
        #endif
    }
}