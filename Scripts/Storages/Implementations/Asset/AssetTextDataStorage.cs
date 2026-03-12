#nullable enable
namespace UniT.Data.Storage
{
    using System;
    using UniT.Extensions;
    using UniT.ResourceManagement;
    using UnityEngine;
    using UnityEngine.Scripting;
    #if UNIT_UNITASK
    using System.Threading;
    using Cysharp.Threading.Tasks;
    #else
    using System.Collections;
    #endif
    #if UNITY_EDITOR
    using System.IO;
    using UnityEditor;
    #endif

    public sealed class AssetTextDataStorage : EditorWritableDataStorage<string>
    {
        private readonly IAssetsManager assetsManager;

        [Preserve]
        public AssetTextDataStorage(IAssetsManager assetsManager)
        {
            this.assetsManager = assetsManager;
        }

        public override string? Read(string key)
        {
            var asset = this.assetsManager.Load<TextAsset>(key);
            var text  = asset.text;
            this.assetsManager.Unload(key);
            return text.NullIfWhiteSpace();
        }

        public override void Write(string key, string value)
        {
            #if UNITY_EDITOR
            var asset = this.assetsManager.Load<TextAsset>(key);
            var path  = AssetDatabase.GetAssetPath(asset);
            this.assetsManager.Unload(key);
            File.WriteAllText(path, value);
            #endif
        }

        public override void Flush()
        {
            #if UNITY_EDITOR
            AssetDatabase.Refresh();
            #endif
        }

        #if UNIT_UNITASK
        public override async UniTask<string?> ReadAsync(string key, IProgress<float>? progress, CancellationToken cancellationToken)
        {
            var asset = await this.assetsManager.LoadAsync<TextAsset>(key, progress, cancellationToken);
            var text = asset.text;
            this.assetsManager.Unload(key);
            return text.NullIfWhiteSpace();
        }

        public override async UniTask WriteAsync(string key, string value, IProgress<float>? progress, CancellationToken cancellationToken)
        {
            #if UNITY_EDITOR
            var asset = await this.assetsManager.LoadAsync<TextAsset>(key, progress, cancellationToken);
            var path = AssetDatabase.GetAssetPath(asset);
            this.assetsManager.Unload(key);
            await File.WriteAllTextAsync(path, value, cancellationToken);
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
        public override IEnumerator ReadAsync(string key, Action<string?> callback, IProgress<float>? progress)
        {
            var asset = default(TextAsset)!;
            yield return this.assetsManager.LoadAsync<TextAsset>(key, result => asset = result, progress);
            var text = asset.text;
            this.assetsManager.Unload(key);
            callback(text.NullIfWhiteSpace());
        }

        public override IEnumerator WriteAsync(string key, string value, Action? callback, IProgress<float>? progress)
        {
            #if UNITY_EDITOR
            var asset = default(TextAsset)!;
            yield return this.assetsManager.LoadAsync<TextAsset>(key, result => asset = result, progress);
            var path = AssetDatabase.GetAssetPath(asset);
            this.assetsManager.Unload(key);
            yield return File.WriteAllTextAsync(path, value).ToCoroutine();
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