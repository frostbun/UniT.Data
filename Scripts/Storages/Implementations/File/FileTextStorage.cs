#nullable enable
namespace UniT.Data.Storages
{
    using System;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using Cysharp.Threading.Tasks;
    using UnityEngine;
    using UnityEngine.Scripting;

    public class FileTextStorage : Storage<string>, IWritableStorage
    {
        private static readonly string PersistentDataPath = Application.persistentDataPath;

        [Preserve]
        public FileTextStorage()
        {
        }

        protected override bool CanStore(Type type) => typeof(IWritableData).IsAssignableFrom(type);

        UniTask<bool> IReadableStorage.ContainsAsync(string key, IProgress<float>? progress, CancellationToken cancellationToken)
        {
            return UniTask.FromResult(File.Exists(GetPath(key)));
        }

        UniTask<object> IReadableStorage.ReadAsync(string key, IProgress<float>? progress, CancellationToken cancellationToken)
        {
            return File.ReadAllTextAsync(GetPath(key), cancellationToken).AsUniTask().ContinueWith(result => (object)result);
        }

        UniTask IWritableStorage.WriteAsync(string key, object value, IProgress<float>? progress, CancellationToken cancellationToken)
        {
            var path = GetPath(key);
            Directory.CreateDirectory(Path.GetDirectoryName(path)!);
            return File.WriteAllTextAsync(path, (string)value, cancellationToken).AsUniTask();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static string GetPath(string key) => Path.Combine(PersistentDataPath, key);
    }
}