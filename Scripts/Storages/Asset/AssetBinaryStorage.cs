#nullable enable
namespace UniT.Data.Storages.Asset
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
    public class AssetBinaryStorage : Storage<byte[]>, IFlushableStorage
    #else
    public class AssetBinaryStorage : Storage<byte[]>, IReadableStorage
    #endif
    {
        private readonly IAssetManager assetManager;

        [Preserve]
        public AssetBinaryStorage(IAssetManager assetManager)
        {
            this.assetManager = assetManager;
        }

        protected override bool CanStore(Type type) => !typeof(IWritableData).IsAssignableFrom(type);

        UniTask<bool> IReadableStorage.ContainsAsync(string key, IProgress<float>? progress, CancellationToken cancellationToken)
        {
            return this.assetManager.ContainsAsync<TextAsset>(key, progress, cancellationToken);
        }

        async UniTask<object> IReadableStorage.ReadAsync(string key, IProgress<float>? progress, CancellationToken cancellationToken)
        {
            var asset = await this.assetManager.LoadAsync<TextAsset>(key, progress, cancellationToken);
            var bytes = asset.bytes;
            this.assetManager.Unload(key);
            return bytes;
        }

        #if UNITY_EDITOR
        async UniTask IWritableStorage.WriteAsync(string key, object value, IProgress<float>? progress, CancellationToken cancellationToken)
        {
            var asset = await this.assetManager.LoadAsync<TextAsset>(key, progress, cancellationToken);
            var path  = AssetDatabase.GetAssetPath(asset);
            this.assetManager.Unload(key);
            await File.WriteAllBytesAsync(path, (byte[])value, cancellationToken);
        }

        UniTask IFlushableStorage.FlushAsync(IProgress<float>? progress, CancellationToken cancellationToken)
        {
            AssetDatabase.Refresh();
            return UniTask.CompletedTask;
        }
        #endif
    }
}