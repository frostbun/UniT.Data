#nullable enable
namespace UniT.Data.Storages
{
    using System;
    using System.Threading;
    using Cysharp.Threading.Tasks;
    using UniT.ResourceManagement;
    using UnityEngine;
    using UnityEngine.Scripting;
    #if UNITY_EDITOR
    using System.IO;
    using UnityEditor;
    #endif

    #if UNITY_EDITOR
    public class AssetTextStorage : Storage<string>, IFlushableStorage
    #else
    public class AssetTextStorage : Storage<string>, IReadableStorage
    #endif
    {
        private readonly IAssetsManager assetsManager;

        [Preserve]
        public AssetTextStorage(IAssetsManager assetsManager)
        {
            this.assetsManager = assetsManager;
        }

        protected override bool CanStore(Type type) => !typeof(IWritableData).IsAssignableFrom(type);

        UniTask<bool> IReadableStorage.ContainsAsync(string key, IProgress<float>? progress, CancellationToken cancellationToken)
        {
            return this.assetsManager.ContainsAsync<TextAsset>(key, progress, cancellationToken);
        }

        async UniTask<object> IReadableStorage.ReadAsync(string key, IProgress<float>? progress, CancellationToken cancellationToken)
        {
            var asset = await this.assetsManager.LoadAsync<TextAsset>(key, progress, cancellationToken);
            var text = asset.text;
            this.assetsManager.Unload(key);
            return text;
        }

        #if UNITY_EDITOR
        async UniTask IWritableStorage.WriteAsync(string key, object value, IProgress<float>? progress, CancellationToken cancellationToken)
        {
            var asset = await this.assetsManager.LoadAsync<TextAsset>(key, progress, cancellationToken);
            var path  = AssetDatabase.GetAssetPath(asset);
            this.assetsManager.Unload(key);
            await File.WriteAllTextAsync(path, (string)value, cancellationToken);
        }

        UniTask IFlushableStorage.FlushAsync(IProgress<float>? progress, CancellationToken cancellationToken)
        {
            AssetDatabase.Refresh();
            return UniTask.CompletedTask;
        }
        #endif
    }
}