#nullable enable
namespace UniT.Data.Storage
{
    using System;
    using System.IO;
    using UnityEngine;
    using UnityEngine.Scripting;
    #if UNIT_UNITASK
    using System.Threading;
    using Cysharp.Threading.Tasks;
    #else
    using System.Collections;
    using UniT.Extensions;
    #endif

    public class FileTextDataStorage : WritableDataStorage<string>
    {
        private static readonly string PERSISTENT_DATA_PATH = Application.persistentDataPath;

        [Preserve]
        public FileTextDataStorage()
        {
        }

        public sealed override string? Read(string key)
        {
            var path = Path.Combine(PERSISTENT_DATA_PATH, key);
            return File.Exists(path) ? File.ReadAllText(path) : null;
        }

        public sealed override void Write(string key, string value)
        {
            var path = Path.Combine(PERSISTENT_DATA_PATH, key);
            Directory.CreateDirectory(Path.GetDirectoryName(path)!);
            File.WriteAllText(path, value);
        }

        public sealed override void Flush()
        {
        }

        #if UNIT_UNITASK
        public sealed override UniTask<string?> ReadAsync(string key, IProgress<float>? progress, CancellationToken cancellationToken)
        {
            var path = Path.Combine(PERSISTENT_DATA_PATH, key);
            return File.Exists(path) ? File.ReadAllTextAsync(path, cancellationToken).AsUniTask() : UniTask.FromResult(default(string?));
        }

        public sealed override UniTask WriteAsync(string key, string value, IProgress<float>? progress, CancellationToken cancellationToken)
        {
            var path = Path.Combine(PERSISTENT_DATA_PATH, key);
            Directory.CreateDirectory(Path.GetDirectoryName(path)!);
            return File.WriteAllTextAsync(path, value, cancellationToken).AsUniTask();
        }

        public sealed override UniTask FlushAsync(IProgress<float>? progress, CancellationToken cancellationToken)
        {
            return UniTask.CompletedTask;
        }
        #else
        public sealed override IEnumerator ReadAsync(string key, Action<string?> callback, IProgress<float>? progress)
        {
            var path = Path.Combine(PERSISTENT_DATA_PATH, key);
            if (!File.Exists(path))
            {
                callback(null);
                yield break;
            }
            yield return File.ReadAllTextAsync(path).ToCoroutine(callback);
        }

        public sealed override IEnumerator WriteAsync(string key, string value, Action? callback, IProgress<float>? progress)
        {
            var path = Path.Combine(PERSISTENT_DATA_PATH, key);
            Directory.CreateDirectory(Path.GetDirectoryName(path)!);
            return File.WriteAllTextAsync(path, value).ToCoroutine(callback);
        }

        public sealed override IEnumerator FlushAsync(Action? callback, IProgress<float>? progress)
        {
            callback?.Invoke();
            yield break;
        }
        #endif
    }
}