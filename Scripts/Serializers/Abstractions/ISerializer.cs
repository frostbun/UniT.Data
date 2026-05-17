#nullable enable
namespace UniT.Data.Serializers
{
    using System;
    using System.Runtime.CompilerServices;
    #if UNIT_UNITASK
    using System.Threading;
    using Cysharp.Threading.Tasks;
    #else
    using System.Collections;
    using UniT.Extensions;
    #endif

    public interface ISerializer
    {
        public Type RawDataType { get; }

        public bool CanSerialize(Type type);

        public object Deserialize(Type type, object rawData);

        public object Serialize(object data);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Deserialize<T>(object rawData) => (T)this.Deserialize(typeof(T), rawData);

        #if UNIT_UNITASK
        public UniTask<object> DeserializeAsync(Type type, object rawData, CancellationToken cancellationToken = default)
        {
            #if !UNITY_WEBGL
            return UniTask.RunOnThreadPool(() => this.Deserialize(type, rawData), cancellationToken: cancellationToken);
            #else
            return UniTask.FromResult(this.Deserialize(type, rawData));
            #endif
        }

        public UniTask<object> SerializeAsync(object data, CancellationToken cancellationToken = default)
        {
            // serialize collections is commonly not thread safe
            return UniTask.FromResult(this.Serialize(data));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public UniTask<T> DeserializeAsync<T>(object rawData, CancellationToken cancellationToken = default) => this.DeserializeAsync(typeof(T), rawData, cancellationToken).ContinueWith(data => (T)data);
        #else
        public IEnumerator DeserializeAsync(Type type, object rawData, Action<object> callback)
        {
            return CoroutineRunner.Run(() => this.Deserialize(type, rawData), callback);
        }

        public IEnumerator SerializeAsync(object data, Action<object> callback)
        {
            callback(this.Serialize(data));
            yield break;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IEnumerator DeserializeAsync<T>(object rawData, Action<T> callback) => this.DeserializeAsync(typeof(T), rawData, data => callback((T)data));
        #endif
    }
}