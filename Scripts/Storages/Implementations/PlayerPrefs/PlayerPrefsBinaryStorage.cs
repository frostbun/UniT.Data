#nullable enable
namespace UniT.Data.Storages
{
    using System;
    using UnityEngine;
    using UnityEngine.Scripting;
    #if UNIT_UNITASK
    using System.Threading;
    using Cysharp.Threading.Tasks;
    #else
    using System.Collections;
    #endif

    public class PlayerPrefsBinaryStorage : IFlushableStorage
    {
        [Preserve]
        public PlayerPrefsBinaryStorage()
        {
        }

        protected virtual bool CanStore(Type type) => typeof(IWritableData).IsAssignableFrom(type);

        Type IStorage.RawDataType => typeof(byte[]);

        bool IStorage.CanStore(Type type) => this.CanStore(type);

        bool IStorage.Contains(string key) => PlayerPrefs.HasKey(key);

        object IStorage.Read(string key) => Convert.FromBase64String(PlayerPrefs.GetString(key));

        void IWritableStorage.Write(string key, object value) => PlayerPrefs.SetString(key, Convert.ToBase64String((byte[])value));

        void IFlushableStorage.Flush() => PlayerPrefs.Save();

        #if UNIT_UNITASK
        UniTask<bool> IStorage.ContainsAsync(string key, IProgress<float>? progress, CancellationToken cancellationToken)
        {
            return UniTask.FromResult(PlayerPrefs.HasKey(key));
        }

        UniTask<object> IStorage.ReadAsync(string key, IProgress<float>? progress, CancellationToken cancellationToken)
        {
            return UniTask.FromResult<object>(Convert.FromBase64String(PlayerPrefs.GetString(key)));
        }

        UniTask IWritableStorage.WriteAsync(string key, object value, IProgress<float>? progress, CancellationToken cancellationToken)
        {
            PlayerPrefs.SetString(key, Convert.ToBase64String((byte[])value));
            return UniTask.CompletedTask;
        }

        UniTask IFlushableStorage.FlushAsync(IProgress<float>? progress, CancellationToken cancellationToken)
        {
            PlayerPrefs.Save();
            return UniTask.CompletedTask;
        }
        #else
        IEnumerator IStorage.ContainsAsync(string key, Action<bool> callback, IProgress<float>? progress)
        {
            callback(PlayerPrefs.HasKey(key));
            yield break;
        }

        IEnumerator IStorage.ReadAsync(string key, Action<object> callback, IProgress<float>? progress)
        {
            callback(Convert.FromBase64String(PlayerPrefs.GetString(key)));
            yield break;
        }

        IEnumerator IWritableStorage.WriteAsync(string key, object value, Action? callback, IProgress<float>? progress)
        {
            PlayerPrefs.SetString(key, Convert.ToBase64String((byte[])value));
            callback?.Invoke();
            yield break;
        }

        IEnumerator IFlushableStorage.FlushAsync(Action? callback, IProgress<float>? progress)
        {
            PlayerPrefs.Save();
            callback?.Invoke();
            yield break;
        }
        #endif
    }
}