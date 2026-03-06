#nullable enable
namespace UniT.Data.Storage
{
    using System;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
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

    public abstract class ExternalFileVersionManager : IExternalFileVersionManager, IDisposable
    {
        private static readonly string PERSISTENT_DATA_PATH = Application.persistentDataPath;

        private readonly ILogger logger;

        protected ExternalFileVersionManager(ILoggerManager loggerManager)
        {
            this.logger = loggerManager.GetLogger(this);
        }

        private string ZipFilePath => Path.Combine(PERSISTENT_DATA_PATH, this.Version);

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

        private bool        validating;
        private ZipArchive? zipFile;

        #region Sync

        Stream? IExternalFileVersionManager.GetFile(string name)
        {
            this.logger.Warning("`GetFilePath` only use cached file. Use `GetFilePathAsync` to download new file from remote.");
            this.Validate();
            return this.zipFile?.GetEntry(name)?.Open();
        }

        private void Validate()
        {
            if (this.zipFile is { }) return;
            this.logger.Debug($"Validating {this.Version}");

            if (this.Version.IsNullOrWhiteSpace())
            {
                this.logger.Warning("Version not set");
                return;
            }

            if (!File.Exists(this.ZipFilePath))
            {
                this.logger.Warning($"Zip file not found: {this.ZipFilePath}");
                return;
            }

            try
            {
                this.zipFile = this.ValidateZipFile(this.Version, this.ZipFilePath);
            }
            catch (Exception e)
            {
                File.Delete(this.ZipFilePath);
                this.logger.Exception(e);
                this.logger.Error("Failed to validate zip file");
                return;
            }

            this.logger.Debug("Validated");
        }

        protected abstract ZipArchive ValidateZipFile(string version, string zipFilePath);

        #endregion

        #region Async

        #if UNIT_UNITASK
        async UniTask<Stream?> IExternalFileVersionManager.GetFileAsync(string name, IProgress<float>? progress, CancellationToken cancellationToken)
        {
            if (this.validating) await UniTask.WaitUntil(this, @this => !@this.validating, cancellationToken: cancellationToken);
            this.validating = true;
            try
            {
                var subProgresses = progress.CreateSubProgresses(2).ToArray();
                if (this.zipFile is null)
                {
                    try
                    {
                        var version = await this.FetchVersionAsync(
                            progress: subProgresses[0],
                            cancellationToken: cancellationToken
                        );
                        if (!version.IsNullOrWhiteSpace())
                        {
                            this.Version = version.Trim();
                            #if !UNITY_WEBGL
                            await UniTask.RunOnThreadPool(this.Validate, cancellationToken: cancellationToken);
                            #else
                            this.Validate();
                            #endif
                        }
                    }
                    catch (Exception e) when (e is not OperationCanceledException)
                    {
                        this.logger.Exception(e);
                        this.logger.Error("Failed to fetch version");
                    }
                }
                if (this.zipFile is null && !this.Version.IsNullOrWhiteSpace())
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
                        await UniTask.RunOnThreadPool(this.Validate, cancellationToken: cancellationToken);
                        #else
                        this.Validate();
                        #endif
                    }
                    catch (Exception e) when (e is not OperationCanceledException)
                    {
                        this.logger.Exception(e);
                        this.logger.Error("Failed to download zip file");
                    }
                }
                return this.zipFile?.GetEntry(name)?.Open();
            }
            finally
            {
                this.validating = false;
            }
        }

        protected abstract UniTask<string> FetchVersionAsync(IProgress<float>? progress, CancellationToken cancellationToken);

        protected abstract UniTask DownloadZipFileAsync(string version, string savePath, IProgress<float>? progress, CancellationToken cancellationToken);
        #else
        IEnumerator IExternalFileVersionManager.GetFileAsync(string name, Action<Stream?> callback, IProgress<float>? progress)
        {
            if (this.validating) yield return new WaitUntil(() => !this.validating);
            this.validating = true;
            try
            {
                var subProgresses = progress.CreateSubProgresses(2).ToArray();
                if (this.zipFile is null)
                {
                    yield return Wrapper().Catch(e =>
                    {
                        this.logger.Exception(e);
                        this.logger.Error("Failed to fetch version");
                    });

                    IEnumerator Wrapper()
                    {
                        var version = default(string)!;
                        yield return this.FetchVersionAsync(
                            callback: result => version = result,
                            progress: subProgresses[0]
                        );
                        if (version.IsNullOrWhiteSpace()) yield break;
                        this.Version = version.Trim();
                        yield return CoroutineRunner.Run(this.Validate);
                    }
                }
                if (this.zipFile is null && !this.Version.IsNullOrWhiteSpace())
                {
                    yield return Wrapper().Catch(e =>
                    {
                        this.logger.Exception(e);
                        this.logger.Error("Failed to download zip file");
                    });

                    IEnumerator Wrapper()
                    {
                        yield return this.DownloadZipFileAsync(
                            version: this.Version,
                            savePath: this.ZipFilePath,
                            progress: subProgresses[1]
                        );
                        yield return CoroutineRunner.Run(this.Validate);
                    }
                }
                callback(this.zipFile?.GetEntry(name)?.Open());
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

        void IDisposable.Dispose()
        {
            this.zipFile?.Dispose();
            this.zipFile = null;
        }
    }
}