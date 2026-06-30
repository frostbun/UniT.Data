#nullable enable
namespace UniT.Data.Storages.ExternalFile
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Threading;
    using Cysharp.Threading.Tasks;
    using UniT.Extensions;
    using UniT.Logging;
    using UniT.ResourceManagement;
    using UnityEngine;
    using UnityEngine.Scripting;
    using ILogger = UniT.Logging.ILogger;

    public sealed class ExternalFileStorage : IReadableStorage
    {
        private readonly IExternalFileVersionProvider provider;
        private readonly IExternalAssetManager        externalAssetManager;
        private readonly ILogger                      logger;

        [Preserve]
        public ExternalFileStorage(IExternalFileVersionProvider provider, IExternalAssetManager externalAssetManager, ILoggerManager loggerManager)
        {
            this.provider             = provider;
            this.externalAssetManager = externalAssetManager;
            this.logger               = loggerManager.GetLogger(this);
        }

        bool IStorage.CanStore(Type type) => type == typeof(byte[]) || type == typeof(string);

        public UniTask<bool> ContainsAsync(string key, IProgress<float>? progress, CancellationToken cancellationToken)
        {
            return this.GetFilePathAsync(key, progress, cancellationToken).ContinueWith(Item.IsNotNull);
        }

        public async UniTask<object> ReadAsync(string key, Type type, IProgress<float>? progress, CancellationToken cancellationToken)
        {
            var path = await this.GetFilePathAsync(key, progress, cancellationToken)
                ?? throw new KeyNotFoundException($"{key} not found");
            if (type == typeof(byte[]))
            {
                return await File.ReadAllBytesAsync(path, cancellationToken).AsUniTask();
            }
            if (type == typeof(string))
            {
                return await File.ReadAllTextAsync(path, cancellationToken).AsUniTask();
            }
            throw new NotSupportedException($"Unsupported type: {type.Name}");
        }

        #region Private

        private static readonly string PersistentDataPath = Application.persistentDataPath;
        private static readonly string TemporaryCachePath = Application.temporaryCachePath;

        private static string version = PlayerPrefs.GetString(nameof(ExternalFileStorage));

        private static string Version
        {
            get => version;
            set
            {
                PlayerPrefs.SetString(nameof(ExternalFileStorage), version = value);
                PlayerPrefs.Save();
            }
        }

        private static string ZipFilePath      => Path.Combine(PersistentDataPath, Version);
        private static string ExtractDirectory => Path.Combine(TemporaryCachePath, Version);

        private bool  validating;
        private bool? validateResult;

        private async UniTask<string?> GetFilePathAsync(string name, IProgress<float>? progress, CancellationToken cancellationToken)
        {
            if (this.validating) await UniTask.WaitUntil(this, @this => !@this.validating, cancellationToken: cancellationToken);
            if (this.validateResult is not null) return this.GetFilePath(name);
            this.validating = true;
            try
            {
                var subProgresses = progress.CreateSubProgresses(2).ToArray();
                if (await this.FetchVersionAsync(subProgresses[0], cancellationToken) is not var (version, downloadUrl) || version.IsNullOrWhiteSpace())
                {
                    if (Version.IsNullOrWhiteSpace())
                    {
                        this.logger.Warning("No version available");
                        this.validateResult = false;
                        return null;
                    }
                    this.logger.Debug($"Using cached version: {Version}");
                    this.validateResult = await this.ValidateAndExtractAsync(cancellationToken);
                    return this.GetFilePath(name);
                }
                Version = version;
                if (File.Exists(ZipFilePath) && await this.ValidateAndExtractAsync(cancellationToken))
                {
                    this.logger.Debug("Skipping download");
                    this.validateResult = true;
                    return this.GetFilePath(name);
                }
                if (!await this.DownloadZipFileAsync(downloadUrl, subProgresses[1], cancellationToken))
                {
                    this.validateResult = false;
                    return null;
                }
                this.validateResult = await this.ValidateAndExtractAsync(cancellationToken);
                return this.GetFilePath(name);
            }
            finally
            {
                this.validating = false;
            }
        }

        private async UniTask<(string Version, string DownloadUrl)?> FetchVersionAsync(IProgress<float>? progress, CancellationToken cancellationToken)
        {
            try
            {
                this.logger.Debug("Fetching version");
                var result = await this.provider.FetchVersionAsync(progress, cancellationToken);
                this.logger.Debug($"Got: {result}");
                return result;
            }
            catch (Exception e) when (e is not OperationCanceledException)
            {
                this.logger.Error("Failed to fetch version");
                this.logger.Exception(e);
                return null;
            }
        }

        private async UniTask<bool> DownloadZipFileAsync(string downloadUrl, IProgress<float>? progress, CancellationToken cancellationToken)
        {
            try
            {
                this.logger.Debug("Downloading zip file");
                await this.externalAssetManager.DownloadFileAsync(
                    url: downloadUrl,
                    savePath: ZipFilePath,
                    progress: progress,
                    cancellationToken: cancellationToken
                );
                this.logger.Debug("Downloaded zip file");
                return true;
            }
            catch (Exception e) when (e is not OperationCanceledException)
            {
                this.logger.Error("Failed to download zip file");
                this.logger.Exception(e);
                return false;
            }
        }

        private UniTask<bool> ValidateAndExtractAsync(CancellationToken cancellationToken)
        {
            #if !UNITY_WEBGL
            return UniTask.RunOnThreadPool(ValidateAndExtract, cancellationToken: cancellationToken);
            #else
            return UniTask.FromResult(ValidateAndExtract());
            #endif

            bool ValidateAndExtract()
            {
                try
                {
                    this.logger.Debug($"Validating {version}");

                    if (!File.Exists(ZipFilePath))
                    {
                        this.logger.Error($"Zip file not found: {ZipFilePath}");
                        return false;
                    }

                    var hash = ComputeHash(ZipFilePath);
                    if (!string.Equals(hash, version, StringComparison.OrdinalIgnoreCase))
                    {
                        this.logger.Error($"Hash mismatch. Expected: {version}, Got: {hash}");
                        File.Delete(ZipFilePath);
                        return false;
                    }

                    this.logger.Debug($"Extracting {ZipFilePath} to {ExtractDirectory}");
                    ZipFile.ExtractToDirectory(ZipFilePath, ExtractDirectory, true);

                    this.logger.Debug("Validated");
                    return true;
                }
                catch (Exception e)
                {
                    this.logger.Error("Failed to validate or extract zip file");
                    this.logger.Exception(e);
                    return false;
                }

                static string ComputeHash(string filePath)
                {
                    using var sha256  = SHA256.Create();
                    using var zipFile = File.OpenRead(filePath);
                    return BitConverter.ToString(sha256.ComputeHash(zipFile)).Replace("-", string.Empty);
                }
            }
        }

        private string? GetFilePath(string name)
        {
            if (this.validateResult is not true) return null;
            var path = Path.Combine(ExtractDirectory, name);
            return File.Exists(path) ? path : null;
        }

        #endregion
    }
}