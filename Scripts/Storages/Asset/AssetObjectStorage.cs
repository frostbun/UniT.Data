#nullable enable
namespace UniT.Data.Storages.Asset
{
    using System;
    using System.Threading;
    using Cysharp.Threading.Tasks;
    using UniT.ResourceManagement;
    using UnityEngine.Scripting;
    using Object = UnityEngine.Object;
    #if UNITY_EDITOR
    using UnityEditor;
    #endif

    #if UNITY_EDITOR
    public class AssetObjectStorage : Storage<Object>, IFlushableStorage
    #else
    public class AssetObjectStorage : Storage<Object>, IReadableStorage
    #endif
    {
        private readonly IAssetManager assetManager;

        [Preserve]
        public AssetObjectStorage(IAssetManager assetManager)
        {
            this.assetManager = assetManager;
        }

        protected override bool CanStore(Type type) => !typeof(IWritableData).IsAssignableFrom(type);

        UniTask<bool> IReadableStorage.ContainsAsync(string key, IProgress<float>? progress, CancellationToken cancellationToken)
        {
            return this.assetManager.ContainsAsync<Object>(key, progress, cancellationToken);
        }

        UniTask<object> IReadableStorage.ReadAsync(string key, IProgress<float>? progress, CancellationToken cancellationToken)
        {
            return this.assetManager.LoadAsync<Object>(key, progress, cancellationToken).ContinueWith(result => (object)result);
        }

        #if UNITY_EDITOR
        UniTask IWritableStorage.WriteAsync(string key, object value, IProgress<float>? progress, CancellationToken cancellationToken)
        {
            EditorUtility.SetDirty((Object)value);
            return UniTask.CompletedTask;
        }

        UniTask IFlushableStorage.FlushAsync(IProgress<float>? progress, CancellationToken cancellationToken)
        {
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            return UniTask.CompletedTask;
        }
        #endif
    }
}