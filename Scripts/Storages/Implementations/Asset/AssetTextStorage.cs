#nullable enable
namespace UniT.Data.Storages
{
    using System;
    using UniT.ResourceManagement;
    using UnityEngine;
    using UnityEngine.Scripting;
    #if UNIT_UNITASK
    using System.Threading;
    using Cysharp.Threading.Tasks;
    #else
    using System.Collections;
    using UniT.Extensions;
    #endif
    #if UNITY_EDITOR
    using System.IO;
    using UnityEditor;
    #endif

    public class AssetTextStorage : IFlushableStorage
    {
        private readonly IAssetsManager assetsManager;

        [Preserve]
        public AssetTextStorage(IAssetsManager assetsManager)
        {
            this.assetsManager = assetsManager;
        }

        protected virtual bool CanStore(Type type) => !typeof(IWritableData).IsAssignableFrom(type);

        Type IStorage.RawDataType => typeof(string);

        bool IStorage.CanStore(Type type) => this.CanStore(type);

        bool IStorage.Contains(string key)
        {
            return this.assetsManager.Contains<TextAsset>(key);
        }

        object IStorage.Read(string key)
        {
            #if !UNITY_WEBGL
            var asset = this.assetsManager.Load<TextAsset>(key);
            var bytes = asset.bytes;
            this.assetsManager.Unload(key);
            return bytes.Length > 0 ? bytes : throw new InvalidOperationException("Asset is empty");
            #else
            throw new NotSupportedException("Cannot `Read` synchronously on WebGL. Use `ReadAsync` instead.");
            #endif
        }

        void IWritableStorage.Write(string key, object value)
        {
            #if UNITY_EDITOR
            #if !UNITY_WEBGL
            var asset = this.assetsManager.Load<TextAsset>(key);
            var path  = AssetDatabase.GetAssetPath(asset);
            this.assetsManager.Unload(key);
            File.WriteAllText(path, (string)value);
            #else
            throw new NotSupportedException("Cannot `Write` synchronously on WebGL. Use `WriteAsync` instead.");
            #endif
            #else
            throw new InvalidOperationException("Cannot `Write` outside of the Editor");
            #endif
        }

        void IFlushableStorage.Flush()
        {
            #if UNITY_EDITOR
            AssetDatabase.Refresh();
            #else
            throw new InvalidOperationException("Cannot `Flush` outside of the Editor");
            #endif
        }

        #if UNIT_UNITASK
        UniTask<bool> IStorage.ContainsAsync(string key, IProgress<float>? progress, CancellationToken cancellationToken)
        {
            return this.assetsManager.ContainsAsync<TextAsset>(key, progress, cancellationToken);
        }

        async UniTask<object> IStorage.ReadAsync(string key, IProgress<float>? progress, CancellationToken cancellationToken)
        {
            var asset = await this.assetsManager.LoadAsync<TextAsset>(key, progress, cancellationToken);
            var bytes = asset.bytes;
            this.assetsManager.Unload(key);
            return bytes.Length > 0 ? bytes : throw new InvalidOperationException("Asset is empty");
        }

        async UniTask IWritableStorage.WriteAsync(string key, object value, IProgress<float>? progress, CancellationToken cancellationToken)
        {
            #if UNITY_EDITOR
            var asset = await this.assetsManager.LoadAsync<TextAsset>(key, progress, cancellationToken);
            var path  = AssetDatabase.GetAssetPath(asset);
            this.assetsManager.Unload(key);
            await File.WriteAllTextAsync(path, (string)value, cancellationToken);
            #else
            throw new InvalidOperationException("Cannot `Write` outside of the Editor");
            #endif
        }

        UniTask IFlushableStorage.FlushAsync(IProgress<float>? progress, CancellationToken cancellationToken)
        {
            #if UNITY_EDITOR
            AssetDatabase.Refresh();
            return UniTask.CompletedTask;
            #else
            throw new InvalidOperationException("Cannot `Flush` outside of the Editor");
            #endif
        }
        #else
        IEnumerator IStorage.ContainsAsync(string key, Action<bool> callback, IProgress<float>? progress)
        {
            return this.assetsManager.ContainsAsync<TextAsset>(key, callback, progress);
        }

        IEnumerator IStorage.ReadAsync(string key, Action<object> callback, IProgress<float>? progress)
        {
            var asset = default(TextAsset)!;
            yield return this.assetsManager.LoadAsync<TextAsset>(key, result => asset = result, progress);
            var bytes = asset.bytes;
            this.assetsManager.Unload(key);
            callback(bytes.Length > 0 ? bytes : throw new InvalidOperationException("Asset is empty"));
        }

        IEnumerator IWritableStorage.WriteAsync(string key, object value, Action? callback, IProgress<float>? progress)
        {
            #if UNITY_EDITOR
            var asset = default(TextAsset)!;
            yield return this.assetsManager.LoadAsync<TextAsset>(key, result => asset = result, progress);
            var path = AssetDatabase.GetAssetPath(asset);
            this.assetsManager.Unload(key);
            yield return File.WriteAllTextAsync(path, (string)value).ToCoroutine();
            callback?.Invoke();
            yield break;
            #else
            throw new InvalidOperationException("Cannot `Write` outside of the Editor");
            #endif
        }

        IEnumerator IFlushableStorage.FlushAsync(Action? callback, IProgress<float>? progress)
        {
            #if UNITY_EDITOR
            AssetDatabase.Refresh();
            callback?.Invoke();
            yield break;
            #else
            throw new InvalidOperationException("Cannot `Flush` outside of the Editor");
            #endif
        }
        #endif
    }
}