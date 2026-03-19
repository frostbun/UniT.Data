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

    public class FileBinaryDataStorage : WritableDataStorage<byte[]>
    {
        private static readonly string PERSISTENT_DATA_PATH = Application.persistentDataPath;

        [Preserve]
        public FileBinaryDataStorage()
        {
        }

        public sealed override byte[]? Read(string key)
        {
            var path = Path.Combine(PERSISTENT_DATA_PATH, key);
            return File.Exists(path) ? File.ReadAllBytes(path) : null;
        }

        public sealed override void Write(string key, byte[] value)
        {
            var path = Path.Combine(PERSISTENT_DATA_PATH, key);
            File.WriteAllBytes(path, value);
        }

        public sealed override void Flush()
        {
        }

        #if UNIT_UNITASK
        public sealed override UniTask<byte[]?> ReadAsync(string key, IProgress<float>? progress, CancellationToken cancellationToken)
        {
            var path = Path.Combine(PERSISTENT_DATA_PATH, key);
            return File.Exists(path) ? File.ReadAllBytesAsync(path, cancellationToken).AsUniTask() : UniTask.FromResult(default(byte[]?));
        }

        public sealed override UniTask WriteAsync(string key, byte[] value, IProgress<float>? progress, CancellationToken cancellationToken)
        {
            var path = Path.Combine(PERSISTENT_DATA_PATH, key);
            return File.WriteAllBytesAsync(path, value, cancellationToken).AsUniTask();
        }

        public sealed override UniTask FlushAsync(IProgress<float>? progress, CancellationToken cancellationToken)
        {
            return UniTask.CompletedTask;
        }
        #else
        public sealed override IEnumerator ReadAsync(string key, Action<byte[]?> callback, IProgress<float>? progress)
        {
            var path = Path.Combine(PERSISTENT_DATA_PATH, key);
            if (!File.Exists(path))
            {
                callback(null);
                yield break;
            }
            yield return File.ReadAllBytesAsync(path).ToCoroutine(callback);
        }

        public sealed override IEnumerator WriteAsync(string key, byte[] value, Action? callback, IProgress<float>? progress)
        {
            var path = Path.Combine(PERSISTENT_DATA_PATH, key);
            return File.WriteAllBytesAsync(path, value).ToCoroutine(callback);
        }

        public sealed override IEnumerator FlushAsync(Action? callback, IProgress<float>? progress)
        {
            callback?.Invoke();
            yield break;
        }
        #endif
    }
}