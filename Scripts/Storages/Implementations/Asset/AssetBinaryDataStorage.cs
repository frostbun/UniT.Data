#nullable enable
namespace UniT.Data.Storage
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

    public sealed class AssetBinaryDataStorage : EditorWritableDataStorage<byte[]>
    {
        private readonly IAssetsManager assetsManager;

        [Preserve]
        public AssetBinaryDataStorage(IAssetsManager assetsManager)
        {
            this.assetsManager = assetsManager;
        }

        public override byte[]? Read(string key)
        {
            var asset = this.assetsManager.Load<TextAsset>(key);
            var bytes = asset.bytes;
            this.assetsManager.Unload(key);
            return bytes.Length > 0 ? bytes : null;
        }

        public override void Write(string key, byte[] value)
        {
            #if UNITY_EDITOR
            var asset = this.assetsManager.Load<TextAsset>(key);
            var path  = AssetDatabase.GetAssetPath(asset);
            this.assetsManager.Unload(key);
            File.WriteAllBytes(path, value);
            #endif
        }

        public override void Flush()
        {
            #if UNITY_EDITOR
            AssetDatabase.Refresh();
            #endif
        }

        #if UNIT_UNITASK
        public override async UniTask<byte[]?> ReadAsync(string key, IProgress<float>? progress, CancellationToken cancellationToken)
        {
            var asset = await this.assetsManager.LoadAsync<TextAsset>(key, progress, cancellationToken);
            var bytes = asset.bytes;
            this.assetsManager.Unload(key);
            return bytes.Length > 0 ? bytes : null;
        }

        public override async UniTask WriteAsync(string key, byte[] value, IProgress<float>? progress, CancellationToken cancellationToken)
        {
            #if UNITY_EDITOR
            var asset = await this.assetsManager.LoadAsync<TextAsset>(key, progress, cancellationToken);
            var path = AssetDatabase.GetAssetPath(asset);
            this.assetsManager.Unload(key);
            await File.WriteAllBytesAsync(path, value, cancellationToken);
            #endif
        }

        public override UniTask FlushAsync(IProgress<float>? progress, CancellationToken cancellationToken)
        {
            #if UNITY_EDITOR
            AssetDatabase.Refresh();
            #endif
            return UniTask.CompletedTask;
        }
        #else
        public override IEnumerator ReadAsync(string key, Action<byte[]?> callback, IProgress<float>? progress)
        {
            var asset = default(TextAsset)!;
            yield return this.assetsManager.LoadAsync<TextAsset>(key, result => asset = result, progress);
            var bytes = asset.bytes;
            this.assetsManager.Unload(key);
            callback(bytes.Length > 0 ? bytes : null);
        }

        public override IEnumerator WriteAsync(string key, byte[] value, Action? callback, IProgress<float>? progress)
        {
            #if UNITY_EDITOR
            var asset = default(TextAsset)!;
            yield return this.assetsManager.LoadAsync<TextAsset>(key, result => asset = result, progress);
            var path = AssetDatabase.GetAssetPath(asset);
            this.assetsManager.Unload(key);
            yield return File.WriteAllBytesAsync(path, value).ToCoroutine();
            #endif
            callback?.Invoke();
            yield break;
        }

        public override IEnumerator FlushAsync(Action? callback, IProgress<float>? progress)
        {
            #if UNITY_EDITOR
            AssetDatabase.Refresh();
            #endif
            callback?.Invoke();
            yield break;
        }
        #endif
    }
}