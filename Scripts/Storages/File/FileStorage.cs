#nullable enable
namespace UniT.Data.Storages.File
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using Cysharp.Threading.Tasks;
    using UniT.Extensions;
    using UnityEngine;
    using UnityEngine.Scripting;

    public sealed class FileStorage : IReadableStorage, IWritableStorage
    {
        private static readonly string PersistentDataPath = Application.persistentDataPath;

        private readonly HashSet<string> dirtyKeys = new();

        [Preserve]
        public FileStorage()
        {
        }

        bool IStorage.CanStore(Type type) => type == typeof(byte[]) || type == typeof(string);

        public UniTask<bool> ContainsAsync(string key, IProgress<float>? progress, CancellationToken cancellationToken)
        {
            return UniTask.FromResult(File.Exists(GetPath(key)));
        }

        public async UniTask<object> ReadAsync(string key, Type type, IProgress<float>? progress, CancellationToken cancellationToken)
        {
            if (type == typeof(byte[]))
            {
                return await File.ReadAllBytesAsync(GetPath(key), cancellationToken).AsUniTask();
            }
            if (type == typeof(string))
            {
                return await File.ReadAllTextAsync(GetPath(key), cancellationToken).AsUniTask();
            }
            throw new NotSupportedException($"Unsupported type: {type.Name}");
        }

        public async UniTask WriteAsync(string key, object value, Type type, IProgress<float>? progress, CancellationToken cancellationToken)
        {
            var tempPath = GetTempPath(key);
            Directory.CreateDirectory(Path.GetDirectoryName(tempPath)!);
            this.dirtyKeys.Remove(key);
            switch (value)
            {
                case byte[] bytes:
                {
                    await File.WriteAllBytesAsync(tempPath, bytes, cancellationToken).AsUniTask();
                    break;
                }
                case string text:
                {
                    await File.WriteAllTextAsync(tempPath, text, cancellationToken).AsUniTask();
                    break;
                }
                default: throw new NotSupportedException($"Unsupported type: {value.GetType().Name}");
            }
            this.dirtyKeys.Add(key);
        }

        public void Flush()
        {
            this.dirtyKeys.SafeForEach(static (key, dirtyKeys) =>
            {
                var tempPath = GetTempPath(key);
                var path     = GetPath(key);
                if (File.Exists(path))
                {
                    File.Replace(tempPath, path, null);
                }
                else
                {
                    File.Move(tempPath, path);
                }
                dirtyKeys.Remove(key);
            }, this.dirtyKeys);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static string GetPath(string key) => Path.Combine(PersistentDataPath, key);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static string GetTempPath(string key) => GetPath(key) + ".tmp";
    }
}