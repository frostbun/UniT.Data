#nullable enable
namespace UniT.Data.Storages.External
{
    using System;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Threading;
    using Cysharp.Threading.Tasks;
    using UniT.Extensions;
    using UniT.Logging;
    using UnityEngine;
    using ILogger = UniT.Logging.ILogger;

    public abstract class ExternalFileVersionManager : IExternalFileVersionManager
    {
        private static readonly string PersistentDataPath = Application.persistentDataPath;
        private static readonly string TemporaryCachePath = Application.temporaryCachePath;

        private readonly ILogger logger;

        protected ExternalFileVersionManager(ILoggerManager loggerManager)
        {
            this.logger = loggerManager.GetLogger(this);
        }

        private string ZipFilePath      => Path.Combine(PersistentDataPath, this.Version);
        private string ExtractDirectory => Path.Combine(TemporaryCachePath, this.Version);

        private string Version
        {
            get => this.version;
            set
            {
                PlayerPrefs.SetString(nameof(ExternalFileVersionManager), this.version = value);
                PlayerPrefs.Save();
            }
        }

        private string version = PlayerPrefs.GetString(nameof(ExternalFileVersionManager));

        private bool  validating;
        private bool? validateResult;

        async UniTask<string?> IExternalFileVersionManager.GetFilePathAsync(string name, IProgress<float>? progress, CancellationToken cancellationToken)
        {
            if (this.validating) await UniTask.WaitUntil(this, @this => !@this.validating, cancellationToken: cancellationToken);
            if (this.validateResult is not null) return GetFilePath();
            this.validating = true;
            try
            {
                var subProgresses = progress.CreateSubProgresses(2).ToArray();
                try
                {
                    this.Version = await this.FetchVersionAsync(
                        progress: subProgresses[0],
                        cancellationToken: cancellationToken
                    );
                }
                catch (Exception e) when (e is not OperationCanceledException)
                {
                    this.logger.Exception(e);
                    this.logger.Error("Failed to fetch version");
                }
                if (this.Version.IsNullOrWhiteSpace())
                {
                    this.validateResult = false;
                    return GetFilePath();
                }
                if (File.Exists(this.ZipFilePath) && await this.ValidateAndExtractAsync(cancellationToken))
                {
                    this.validateResult = true;
                    return GetFilePath();
                }
                try
                {
                    await this.DownloadZipFileAsync(
                        version: this.Version,
                        savePath: this.ZipFilePath,
                        progress: subProgresses[1],
                        cancellationToken: cancellationToken
                    );
                }
                catch (Exception e) when (e is not OperationCanceledException)
                {
                    this.logger.Exception(e);
                    this.logger.Error("Failed to download zip file");
                }
                this.validateResult = await this.ValidateAndExtractAsync(cancellationToken);
                return GetFilePath();
            }
            finally
            {
                this.validating = false;
            }

            string? GetFilePath()
            {
                if (this.validateResult is not true)
                {
                    this.logger.Warning("Failed to validate zip file. Returning `null`");
                    return null;
                }
                var path = Path.Combine(this.ExtractDirectory, name);
                return File.Exists(path) ? path : null;
            }
        }

        private UniTask<bool> ValidateAndExtractAsync(CancellationToken cancellationToken)
        {
            return Application.platform is RuntimePlatform.WebGLPlayer
                ? UniTask.FromResult(ValidateAndExtract())
                : UniTask.RunOnThreadPool(ValidateAndExtract, cancellationToken: cancellationToken);

            bool ValidateAndExtract()
            {
                this.logger.Debug($"Validating {this.Version}");

                if (!File.Exists(this.ZipFilePath))
                {
                    this.logger.Error($"Zip file not found: {this.ZipFilePath}");
                    return false;
                }

                var hash = ComputeHash(this.ZipFilePath);
                if (!string.Equals(hash, this.Version, StringComparison.OrdinalIgnoreCase))
                {
                    this.logger.Error($"Hash mismatch. Expected: {this.Version}, Got: {hash}");
                    File.Delete(this.ZipFilePath);
                    return false;
                }

                this.logger.Debug($"Extracting {this.ZipFilePath} to {this.ExtractDirectory}");
                ZipFile.ExtractToDirectory(this.ZipFilePath, this.ExtractDirectory, true);

                this.logger.Debug("Validated");
                return true;

                static string ComputeHash(string filePath)
                {
                    using var sha256  = SHA256.Create();
                    using var zipFile = File.OpenRead(filePath);
                    return BitConverter.ToString(sha256.ComputeHash(zipFile)).Replace("-", string.Empty);
                }
            }
        }

        protected abstract UniTask<string> FetchVersionAsync(IProgress<float>? progress, CancellationToken cancellationToken);

        protected abstract UniTask DownloadZipFileAsync(string version, string savePath, IProgress<float>? progress, CancellationToken cancellationToken);
    }
}