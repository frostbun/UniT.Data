#nullable enable
namespace UniT.Data.Storages
{
    using System;
    using System.IO;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.Scripting;
    #if UNIT_UNITASK
    using System.Threading;
    using Cysharp.Threading.Tasks;
    #else
    using System.Collections;
    using UniT.Extensions;
    #endif

    public class FileTextStorage : IWritableStorage
    {
        private static readonly string PersistentDataPath = Application.persistentDataPath;

        [Preserve]
        public FileTextStorage()
        {
        }

        protected virtual bool CanStore(Type type) => typeof(IWritableData).IsAssignableFrom(type);

        Type IStorage.RawDataType => typeof(string);

        bool IStorage.CanStore(Type type) => this.CanStore(type);

        bool IStorage.Contains(string key)
        {
            return File.Exists(GetPath(key));
        }

        object IStorage.Read(string key)
        {
            return File.ReadAllText(GetPath(key));
        }

        void IWritableStorage.Write(string key, object value)
        {
            var path = GetPath(key);
            Directory.CreateDirectory(Path.GetDirectoryName(path)!);
            File.WriteAllText(path, (string)value);
        }

        #if UNIT_UNITASK
        UniTask<bool> IStorage.ContainsAsync(string key, IProgress<float>? progress, CancellationToken cancellationToken)
        {
            return UniTask.FromResult(File.Exists(GetPath(key)));
        }

        UniTask<object> IStorage.ReadAsync(string key, IProgress<float>? progress, CancellationToken cancellationToken)
        {
            return File.ReadAllTextAsync(GetPath(key), cancellationToken).AsUniTask().ContinueWith(result => (object)result);
        }

        UniTask IWritableStorage.WriteAsync(string key, object value, IProgress<float>? progress, CancellationToken cancellationToken)
        {
            var path = GetPath(key);
            Directory.CreateDirectory(Path.GetDirectoryName(path)!);
            return File.WriteAllTextAsync(path, (string)value, cancellationToken).AsUniTask();
        }
        #else
        IEnumerator IStorage.ContainsAsync(string key, Action<bool> callback, IProgress<float>? progress)
        {
            callback(File.Exists(GetPath(key)));
            yield break;
        }

        IEnumerator IStorage.ReadAsync(string key, Action<object> callback, IProgress<float>? progress)
        {
            return File.ReadAllTextAsync(GetPath(key)).ToCoroutine(callback);
        }

        IEnumerator IWritableStorage.WriteAsync(string key, object value, Action? callback, IProgress<float>? progress)
        {
            var path = GetPath(key);
            Directory.CreateDirectory(Path.GetDirectoryName(path)!);
            return File.WriteAllTextAsync(path, (string)value).ToCoroutine(callback);
        }
        #endif

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static string GetPath(string key) => Path.Combine(PersistentDataPath, key);
    }
}