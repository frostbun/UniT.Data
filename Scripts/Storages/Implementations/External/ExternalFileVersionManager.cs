#nullable enable
namespace UniT.Data.Storage
{
    using System;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Security.Cryptography;
    using UniT.Extensions;
    using UniT.Logging;
    using UnityEngine;
    using ILogger = UniT.Logging.ILogger;
    #if UNIT_UNITASK
    using System.Threading;
    using Cysharp.Threading.Tasks;
    #else
    using System.Collections;
    #endif

    public abstract class ExternalFileVersionManager : IExternalFileVersionManager
    {
        private static readonly string PERSISTENT_DATA_PATH = Application.persistentDataPath;
        private static readonly string TEMPORARY_CACHE_PATH = Application.temporaryCachePath;

        private readonly ILogger logger;

        protected ExternalFileVersionManager(ILoggerManager loggerManager)
        {
            this.logger = loggerManager.GetLogger(this);
        }

        private string ZipFilePath      => Path.Combine(PERSISTENT_DATA_PATH, this.Version);
        private string ExtractDirectory => Path.Combine(TEMPORARY_CACHE_PATH, this.Version);

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

        private bool validating;
        private bool validated;

        #region Sync

        string? IExternalFileVersionManager.GetFilePath(string name)
        {
            this.logger.Warning("`GetFilePath` only use cached files. Use `GetFilePathAsync` to download new files from remote.");
            if (this.Version.IsNullOrWhiteSpace())
            {
                this.logger.Error("Version not set");
                return null;
            }
            this.ValidateAndExtract();
            if (!this.validated)
            {
                this.logger.Error("Failed to validate zip file. Returning `null`");
                return null;
            }
            return this.GetFilePath(name);
        }

        private void ValidateAndExtract()
        {
            if (this.validated) return;
            this.logger.Debug($"Validating {this.Version}");

            if (!File.Exists(this.ZipFilePath))
            {
                this.logger.Error($"Zip file not found: {this.ZipFilePath}");
            }

            var hash = ComputeHash(this.ZipFilePath);
            if (!string.Equals(hash, this.Version, StringComparison.OrdinalIgnoreCase))
            {
                this.logger.Error($"Hash mismatch. Expected: {this.Version}, Got: {hash}");
                File.Delete(this.ZipFilePath);
                return;
            }

            this.logger.Debug($"Extracting {this.ZipFilePath} to {this.ExtractDirectory}");
            ZipFile.ExtractToDirectory(this.ZipFilePath, this.ExtractDirectory, true);

            this.logger.Debug("Validated");
            this.validated = true;

            static string ComputeHash(string filePath)
            {
                using var sha256  = SHA256.Create();
                using var zipFile = File.OpenRead(filePath);
                return BitConverter.ToString(sha256.ComputeHash(zipFile)).Replace("-", "");
            }
        }

        private string? GetFilePath(string name)
        {
            var path = Path.Combine(this.ExtractDirectory, name);
            return File.Exists(path) ? path : null;
        }

        #endregion

        #region Async

        #if UNIT_UNITASK
        async UniTask<string?> IExternalFileVersionManager.GetFilePathAsync(string name, IProgress<float>? progress, CancellationToken cancellationToken)
        {
            if (this.validated) return this.GetFilePath(name);
            if (this.validating) await UniTask.WaitUntil(this, @this => !@this.validating, cancellationToken: cancellationToken);
            this.validating = true;
            try
            {
                var subProgresses = progress.CreateSubProgresses(2).ToArray();
                if (!this.validated)
                {
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
                    if (File.Exists(this.ZipFilePath))
                    {
                        #if !UNITY_WEBGL
                        await UniTask.RunOnThreadPool(this.ValidateAndExtract, cancellationToken: cancellationToken);
                        #else
                        this.ValidateAndExtract();
                        #endif
                    }
                }
                if (!this.validated && !this.Version.IsNullOrWhiteSpace())
                {
                    try
                    {
                        await this.DownloadZipFileAsync(
                            version: this.Version,
                            savePath: this.ZipFilePath,
                            progress: subProgresses[1],
                            cancellationToken: cancellationToken
                        );
                        #if !UNITY_WEBGL
                        await UniTask.RunOnThreadPool(this.ValidateAndExtract, cancellationToken: cancellationToken);
                        #else
                        this.ValidateAndExtract();
                        #endif
                    }
                    catch (Exception e) when (e is not OperationCanceledException)
                    {
                        this.logger.Exception(e);
                        this.logger.Error("Failed to download zip file");
                    }
                }
                if (!this.validated)
                {
                    this.logger.Error("Failed to validate zip file. Returning `null`");
                }
                return this.GetFilePath(name);
            }
            finally
            {
                this.validating = false;
            }
        }

        protected abstract UniTask<string> FetchVersionAsync(IProgress<float>? progress, CancellationToken cancellationToken);

        protected abstract UniTask DownloadZipFileAsync(string version, string savePath, IProgress<float>? progress, CancellationToken cancellationToken);
        #else
        IEnumerator IExternalFileVersionManager.GetFilePathAsync(string name, Action<string?> callback, IProgress<float>? progress)
        {
            if (this.validated) callback(this.GetFilePath(name));
            if (this.validating) yield return new WaitUntil(() => !this.validating);
            this.validating = true;
            try
            {
                var subProgresses = progress.CreateSubProgresses(2).ToArray();
                if (!this.validated)
                {
                    yield return this.FetchVersionAsync(
                        callback: result => this.Version = result,
                        progress: subProgresses[0]
                    ).Catch(e =>
                    {
                        this.logger.Exception(e);
                        this.logger.Error("Failed to fetch version");
                    });
                    if (File.Exists(this.ZipFilePath))
                    {
                        yield return CoroutineRunner.Run(this.ValidateAndExtract);
                    }
                }
                if (!this.validated && !this.Version.IsNullOrWhiteSpace())
                {
                    yield return this.DownloadZipFileAsync(
                        version: this.Version,
                        savePath: this.ZipFilePath,
                        progress: subProgresses[1]
                    ).Catch(e =>
                    {
                        this.logger.Exception(e);
                        this.logger.Error("Failed to download zip file");
                    });
                    yield return CoroutineRunner.Run(this.ValidateAndExtract);
                }
                if (!this.validated)
                {
                    this.logger.Error("Failed to validate zip file. Returning `null`");
                }
                callback(this.GetFilePath(name));
            }
            finally
            {
                this.validating = false;
            }
        }

        protected abstract IEnumerator FetchVersionAsync(Action<string> callback, IProgress<float>? progress);

        protected abstract IEnumerator DownloadZipFileAsync(string version, string savePath, IProgress<float>? progress);
        #endif

        #endregion
    }
}