#nullable enable
namespace UniT.Data.Storages.PlayerPrefs
{
    using System;
    using System.Threading;
    using Cysharp.Threading.Tasks;
    using UnityEngine;
    using UnityEngine.Scripting;

    public sealed class PlayerPrefsStorage : IReadableStorage, IWritableStorage
    {
        [Preserve]
        public PlayerPrefsStorage()
        {
        }

        bool IStorage.CanStore(Type type) => type == typeof(byte[]) || type == typeof(string);

        public UniTask<bool> ContainsAsync(string key, IProgress<float>? progress, CancellationToken cancellationToken)
        {
            return UniTask.FromResult(PlayerPrefs.HasKey(key));
        }

        public UniTask<object> ReadAsync(string key, Type type, IProgress<float>? progress, CancellationToken cancellationToken)
        {
            if (type == typeof(byte[]))
            {
                return UniTask.FromResult<object>(Convert.FromBase64String(PlayerPrefs.GetString(key)));
            }
            if (type == typeof(string))
            {
                return UniTask.FromResult<object>(PlayerPrefs.GetString(key));
            }
            throw new NotSupportedException($"Unsupported type: {type.Name}");
        }

        public UniTask WriteAsync(string key, object value, Type type, IProgress<float>? progress, CancellationToken cancellationToken)
        {
            switch (value)
            {
                case byte[] bytes:
                {
                    PlayerPrefs.SetString(key, Convert.ToBase64String(bytes));
                    break;
                }
                case string text:
                {
                    PlayerPrefs.SetString(key, text);
                    break;
                }
                default: throw new NotSupportedException($"Unsupported type: {value.GetType().Name}");
            }
            return UniTask.CompletedTask;
        }

        public void Flush()
        {
            PlayerPrefs.Save();
        }
    }
}