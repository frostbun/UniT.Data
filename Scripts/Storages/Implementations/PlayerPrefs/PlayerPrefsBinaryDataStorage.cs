#nullable enable
namespace UniT.Data.Storage
{
    using System;
    using UniT.Extensions;
    using UnityEngine;
    using UnityEngine.Scripting;
    #if UNIT_UNITASK
    using System.Threading;
    using Cysharp.Threading.Tasks;
    #else
    using System.Collections;
    #endif

    public class PlayerPrefsBinaryDataStorage : WritableDataStorage<byte[]>
    {
        [Preserve]
        public PlayerPrefsBinaryDataStorage()
        {
        }

        public sealed override byte[]? Read(string key) => PlayerPrefs.GetString(key).NullIfWhiteSpace() is { } str ? Convert.FromBase64String(str) : null;

        public sealed override void Write(string key, byte[] value) => PlayerPrefs.SetString(key, Convert.ToBase64String(value));

        public sealed override void Flush() => PlayerPrefs.Save();

        #if UNIT_UNITASK
        public sealed override UniTask<byte[]?> ReadAsync(string key, IProgress<float>? progress, CancellationToken cancellationToken)
        {
            return UniTask.FromResult(this.Read(key));
        }

        public sealed override UniTask WriteAsync(string key, byte[] value, IProgress<float>? progress, CancellationToken cancellationToken)
        {
            this.Write(key, value);
            return UniTask.CompletedTask;
        }

        public sealed override UniTask FlushAsync(IProgress<float>? progress, CancellationToken cancellationToken)
        {
            this.Flush();
            return UniTask.CompletedTask;
        }
        #else
        public sealed override IEnumerator ReadAsync(string key, Action<byte[]?> callback, IProgress<float>? progress)
        {
            callback(this.Read(key));
            yield break;
        }

        public sealed override IEnumerator WriteAsync(string key, byte[] value, Action? callback, IProgress<float>? progress)
        {
            this.Write(key, value);
            callback?.Invoke();
            yield break;
        }

        public sealed override IEnumerator FlushAsync(Action? callback, IProgress<float>? progress)
        {
            this.Flush();
            callback?.Invoke();
            yield break;
        }
        #endif
    }
}