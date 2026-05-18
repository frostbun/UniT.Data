#nullable enable
namespace UniT.Data.Storages
{
    using System;
    using System.Threading;
    using Cysharp.Threading.Tasks;
    using UnityEngine;
    using UnityEngine.Scripting;

    public class PlayerPrefsBinaryStorage : Storage<byte[]>, IFlushableStorage
    {
        [Preserve]
        public PlayerPrefsBinaryStorage()
        {
        }

        protected override bool CanStore(Type type) => typeof(IWritableData).IsAssignableFrom(type);

        UniTask<bool> IReadableStorage.ContainsAsync(string key, IProgress<float>? progress, CancellationToken cancellationToken)
        {
            return UniTask.FromResult(PlayerPrefs.HasKey(key));
        }

        UniTask<object> IReadableStorage.ReadAsync(string key, IProgress<float>? progress, CancellationToken cancellationToken)
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
    }
}