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

    public class AssetTextDataStorage : EditorWritableDataStorage<string>
    {
        private readonly IAssetsManager assetsManager;

        [Preserve]
        public AssetTextDataStorage(IAssetsManager assetsManager)
        {
            this.assetsManager = assetsManager;
        }

        public sealed override string? Read(string key)
        {
            #if !UNITY_WEBGL
            var asset = this.assetsManager.Load<TextAsset>(key);
            var text  = asset.text;
            this.assetsManager.Unload(key);
            return text.NullIfWhiteSpace();
            #else
            throw new NotSupportedException("Cannot `Read` synchronously on WebGL. Use `ReadAsync` instead.");
            #endif
        }

        public sealed override void Write(string key, string value)
        {
            #if UNITY_EDITOR
            #if !UNITY_WEBGL
            var asset = this.assetsManager.Load<TextAsset>(key);
            var path  = AssetDatabase.GetAssetPath(asset);
            this.assetsManager.Unload(key);
            File.WriteAllText(path, value);
            #else
            throw new NotSupportedException("Cannot `Write` synchronously on WebGL. Use `ReadAsync` instead.");
            #endif
            #else
            throw new InvalidOperationException("Cannot `Write` outside of the Editor");
            #endif
        }

        public sealed override void Flush()
        {
            #if UNITY_EDITOR
            AssetDatabase.Refresh();
            #else
            throw new InvalidOperationException("Cannot `Flush` outside of the Editor");
            #endif
        }

        #if UNIT_UNITASK
        public sealed override async UniTask<string?> ReadAsync(string key, IProgress<float>? progress, CancellationToken cancellationToken)
        {
            var asset = await this.assetsManager.LoadAsync<TextAsset>(key, progress, cancellationToken);
            var text  = asset.text;
            this.assetsManager.Unload(key);
            return text.NullIfWhiteSpace();
        }

        public sealed override async UniTask WriteAsync(string key, string value, IProgress<float>? progress, CancellationToken cancellationToken)
        {
            #if UNITY_EDITOR
            var asset = await this.assetsManager.LoadAsync<TextAsset>(key, progress, cancellationToken);
            var path  = AssetDatabase.GetAssetPath(asset);
            this.assetsManager.Unload(key);
            await File.WriteAllTextAsync(path, value, cancellationToken);
            #else
            throw new InvalidOperationException("Cannot `Write` outside of the Editor");
            #endif
        }

        public sealed override UniTask FlushAsync(IProgress<float>? progress, CancellationToken cancellationToken)
        {
            #if UNITY_EDITOR
            AssetDatabase.Refresh();
            return UniTask.CompletedTask;
            #else
            throw new InvalidOperationException("Cannot `Flush` outside of the Editor");
            #endif
        }
        #else
        public sealed override IEnumerator ReadAsync(string key, Action<string?> callback, IProgress<float>? progress)
        {
            var asset = default(TextAsset)!;
            yield return this.assetsManager.LoadAsync<TextAsset>(key, result => asset = result, progress);
            var text = asset.text;
            this.assetsManager.Unload(key);
            callback(text.NullIfWhiteSpace());
        }

        public sealed override IEnumerator WriteAsync(string key, string value, Action? callback, IProgress<float>? progress)
        {
            #if UNITY_EDITOR
            var asset = default(TextAsset)!;
            yield return this.assetsManager.LoadAsync<TextAsset>(key, result => asset = result, progress);
            var path = AssetDatabase.GetAssetPath(asset);
            this.assetsManager.Unload(key);
            yield return File.WriteAllTextAsync(path, value).ToCoroutine();
            callback?.Invoke();
            yield break;
            #else
            throw new InvalidOperationException("Cannot `Write` outside of the Editor");
            #endif
        }

        public sealed override IEnumerator FlushAsync(Action? callback, IProgress<float>? progress)
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