#nullable enable
namespace UniT.Data.Storages
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

    public class AssetBlobStorage : IFlushableStorage
    {
        private readonly IAssetsManager assetsManager;

        [Preserve]
        public AssetBlobStorage(IAssetsManager assetsManager)
        {
            this.assetsManager = assetsManager;
        }

        protected virtual bool CanStore(Type type) => !typeof(IWritableData).IsAssignableFrom(type);

        Type IStorage.RawDataType => typeof(Object);

        bool IStorage.CanStore(Type type) => this.CanStore(type);

        bool IStorage.Contains(string key)
        {
            return this.assetsManager.Contains<Object>(key);
        }

        object IStorage.Read(string key)
        {
            #if !UNITY_WEBGL
            return this.assetsManager.Load<Object>(key);
            #else
            throw new NotSupportedException("Cannot `Read` synchronously on WebGL. Use `ReadAsync` instead.");
            #endif
        }

        void IWritableStorage.Write(string key, object value)
        {
            #if UNITY_EDITOR
            EditorUtility.SetDirty((Object)value);
            #else
            throw new InvalidOperationException("Cannot `Write` outside of the Editor");
            #endif
        }

        void IFlushableStorage.Flush()
        {
            #if UNITY_EDITOR
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            #else
            throw new InvalidOperationException("Cannot `Flush` outside of the Editor");
            #endif
        }

        #if UNIT_UNITASK
        UniTask<bool> IStorage.ContainsAsync(string key, IProgress<float>? progress, CancellationToken cancellationToken)
        {
            return this.assetsManager.ContainsAsync<Object>(key, progress, cancellationToken);
        }

        UniTask<object> IStorage.ReadAsync(string key, IProgress<float>? progress, CancellationToken cancellationToken)
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
        #else
        IEnumerator IStorage.ContainsAsync(string key, Action<bool> callback, IProgress<float>? progress)
        {
            return this.assetsManager.ContainsAsync<Object>(key, callback, progress);
        }

        IEnumerator IStorage.ReadAsync(string key, Action<object> callback, IProgress<float>? progress)
        {
            return this.assetsManager.LoadAsync<Object>(key, callback, progress);
        }

        IEnumerator IWritableStorage.WriteAsync(string key, object value, Action? callback, IProgress<float>? progress)
        {
            #if UNITY_EDITOR
            EditorUtility.SetDirty((Object)value);
            yield break;
            #else
            throw new InvalidOperationException("Cannot `Write` outside of the Editor");
            #endif
        }

        IEnumerator IFlushableStorage.FlushAsync(Action? callback, IProgress<float>? progress)
        {
            #if UNITY_EDITOR
            AssetDatabase.SaveAssets();
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