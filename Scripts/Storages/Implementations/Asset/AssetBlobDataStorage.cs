#nullable enable
namespace UniT.Data.Storage
{
    using System;
    using UniT.ResourceManagement;
    using UnityEngine.Scripting;
    using Object = UnityEngine.Object;
    #if UNIT_UNITASK
    using System.Threading;
    using Cysharp.Threading.Tasks;
    #else
    using System.Collections;
    #endif
    #if UNITY_EDITOR
    using UnityEditor;
    #endif

    public class AssetBlobDataStorage : EditorWritableDataStorage<Object>
    {
        private readonly IAssetsManager assetsManager;

        [Preserve]
        public AssetBlobDataStorage(IAssetsManager assetsManager)
        {
            this.assetsManager = assetsManager;
        }

        public sealed override Object? Read(string key)
        {
            return this.assetsManager.Load<Object>(key);
        }

        public sealed override void Write(string key, Object value)
        {
        }

        public sealed override void Flush()
        {
            #if UNITY_EDITOR
            AssetDatabase.Refresh();
            #endif
        }

        #if UNIT_UNITASK
        public sealed override UniTask<Object?> ReadAsync(string key, IProgress<float>? progress, CancellationToken cancellationToken)
        {
            return this.assetsManager.LoadAsync<Object>(key, progress, cancellationToken)!;
        }

        public sealed override UniTask WriteAsync(string key, Object value, IProgress<float>? progress, CancellationToken cancellationToken)
        {
            return UniTask.CompletedTask;
        }

        public sealed override UniTask FlushAsync(IProgress<float>? progress, CancellationToken cancellationToken)
        {
            #if UNITY_EDITOR
            AssetDatabase.Refresh();
            #endif
            return UniTask.CompletedTask;
        }
        #else
        public sealed override IEnumerator ReadAsync(string key, Action<Object?> callback, IProgress<float>? progress)
        {
            return this.assetsManager.LoadAsync<Object>(key, callback, progress);
        }

        public sealed override IEnumerator WriteAsync(string key, Object value, Action? callback, IProgress<float>? progress)
        {
            yield break;
        }

        public sealed override IEnumerator FlushAsync(Action? callback, IProgress<float>? progress)
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