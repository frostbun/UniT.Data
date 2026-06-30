#nullable enable
namespace UniT.Data.Storages.Asset
{
    using System;
    using System.Threading;
    using Cysharp.Threading.Tasks;
    using UniT.ResourceManagement;
    using UnityEngine;
    using UnityEngine.Scripting;
    using Object = UnityEngine.Object;

    public sealed class AssetStorage : IReadableStorage
    {
        private readonly IAssetManager assetManager;

        [Preserve]
        public AssetStorage(IAssetManager assetManager)
        {
            this.assetManager = assetManager;
        }

        bool IStorage.CanStore(Type type) => type == typeof(byte[]) || type == typeof(string) || type == typeof(Object);

        public UniTask<bool> ContainsAsync(string key, IProgress<float>? progress, CancellationToken cancellationToken)
        {
            return this.assetManager.ContainsAsync(key, progress, cancellationToken);
        }

        public async UniTask<object> ReadAsync(string key, Type type, IProgress<float>? progress, CancellationToken cancellationToken)
        {
            if (type == typeof(byte[]))
            {
                var asset = await this.assetManager.LoadAsync<TextAsset>(key, progress, cancellationToken);
                var bytes = asset.bytes;
                this.assetManager.Unload(key);
                return bytes;
            }
            if (type == typeof(string))
            {
                var asset = await this.assetManager.LoadAsync<TextAsset>(key, progress, cancellationToken);
                var text  = asset.text;
                this.assetManager.Unload(key);
                return text;
            }
            if (type == typeof(Object))
            {
                var asset = await this.assetManager.LoadAsync<Object>(key, progress, cancellationToken);
                return asset;
            }
            throw new NotSupportedException($"Unsupported type: {type.Name}");
        }
    }
}