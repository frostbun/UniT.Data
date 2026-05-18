#nullable enable
namespace UniT.Data.Storages
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

    public class AssetObjectStorage : Storage<Object>, IFlushableStorage
    {
        private readonly IAssetsManager assetsManager;

        [Preserve]
        public AssetObjectStorage(IAssetsManager assetsManager)
        {
            this.assetsManager = assetsManager;
        }

        protected override bool CanStore(Type type) => !typeof(IWritableData).IsAssignableFrom(type);

        UniTask<bool> IReadableStorage.ContainsAsync(string key, IProgress<float>? progress, CancellationToken cancellationToken)
        {
            return this.assetsManager.ContainsAsync<Object>(key, progress, cancellationToken);
        }

        UniTask<object> IReadableStorage.ReadAsync(string key, IProgress<float>? progress, CancellationToken cancellationToken)
        {
            return this.assetsManager.LoadAsync<Object>(key, progress, cancellationToken).ContinueWith(result => (object)result);
        }

        UniTask IWritableStorage.WriteAsync(string key, object value, IProgress<float>? progress, CancellationToken cancellationToken)
        {
            #if UNITY_EDITOR
            EditorUtility.SetDirty((Object)value);
            return UniTask.CompletedTask;
            #else
            throw new InvalidOperationException("Cannot `Write` outside of the Editor");
            #endif
        }

        UniTask IFlushableStorage.FlushAsync(IProgress<float>? progress, CancellationToken cancellationToken)
        {
            #if UNITY_EDITOR
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            return UniTask.CompletedTask;
            #else
            throw new InvalidOperationException("Cannot `Flush` outside of the Editor");
            #endif
        }
    }
}