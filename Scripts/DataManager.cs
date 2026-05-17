#nullable enable
namespace UniT.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UniT.Data.Serializers;
    using UniT.Data.Storages;
    using UniT.Extensions;
    using UniT.Logging;
    using UnityEngine.Scripting;
    #if UNIT_UNITASK
    using System.Threading;
    using Cysharp.Threading.Tasks;
    #else
    using System.Collections;
    #endif

    public sealed class DataManager : IDataManager
    {
        #region Constructor

        private readonly IReadOnlyList<ISerializer> serializers;
        private readonly IReadOnlyList<IStorage>    storages;
        private readonly ILogger                    logger;

        private readonly Dictionary<Type, (ISerializer, IStorage)[]> serializerAndStorageCache = new();

        [Preserve]
        public DataManager(IEnumerable<ISerializer> serializers, IEnumerable<IStorage> storages, ILoggerManager loggerManager)
        {
            this.serializers = serializers.ToArray();
            this.storages    = storages.ToArray();
            this.logger      = loggerManager.GetLogger(this);
            this.logger.Debug("Constructed");
        }

        #endregion

        #region Sync

        object IDataManager.Load(string key, Type type)
        {
            foreach (var (serializer, storage) in this.GetSerializerAndStorage(type).AsSpan())
            {
                if (!storage.Contains(key)) continue;
                var rawData   = storage.Read(key);
                var savedData = serializer.Deserialize(type, rawData);
                this.logger.Debug($"Loaded {key}");
                return savedData;
            }
            var newData = type.GetEmptyConstructor()();
            this.logger.Debug($"Instantiated {key}");
            return newData;
        }

        void IDataManager.Save(string key, object data)
        {
            foreach (var (serializer, storage) in this.GetSerializerAndStorage(data.GetType()).AsSpan())
            {
                if (storage is not IWritableStorage writableStorage) continue;
                var rawData = serializer.Serialize(data);
                writableStorage.Write(key, rawData);
                this.logger.Debug($"Saved {key}");
                return;
            }
            throw new InvalidOperationException($"No writable storage found for {key}");
        }

        void IDataManager.Flush()
        {
            this.storages.OfType<IFlushableStorage>().ForEach(storage => storage.Flush());
        }

        #endregion

        #region Async

        #if UNIT_UNITASK
        async UniTask<object> IDataManager.LoadAsync(string key, Type type, IProgress<float>? progress, CancellationToken cancellationToken)
        {
            foreach (var (serializer, storage) in this.GetSerializerAndStorage(type))
            {
                if (!await storage.ContainsAsync(key, cancellationToken: cancellationToken)) continue;
                var rawData   = await storage.ReadAsync(key, progress, cancellationToken);
                var savedData = await serializer.DeserializeAsync(type, rawData, cancellationToken: cancellationToken);
                this.logger.Debug($"Loaded {key}");
                return savedData;
            }
            var newData = type.GetEmptyConstructor()();
            this.logger.Debug($"Instantiated {key}");
            return newData;
        }

        async UniTask IDataManager.SaveAsync(string key, object data, IProgress<float>? progress, CancellationToken cancellationToken)
        {
            foreach (var (serializer, storage) in this.GetSerializerAndStorage(data.GetType()))
            {
                if (storage is not IWritableStorage writableStorage) continue;
                var rawData = await serializer.SerializeAsync(data, cancellationToken);
                await writableStorage.WriteAsync(key, rawData, progress, cancellationToken);
                this.logger.Debug($"Saved {key}");
                return;
            }
            throw new InvalidOperationException($"No writable storage found for {key}");
        }

        UniTask IDataManager.FlushAsync(IProgress<float>? progress, CancellationToken cancellationToken)
        {
            return this.storages.OfType<IFlushableStorage>().ForEachAsync(
                static (storage, progress, cancellationToken) => storage.FlushAsync(progress, cancellationToken),
                progress,
                cancellationToken
            );
        }
        #else
        IEnumerator IDataManager.LoadAsync(string key, Type type, Action<object> callback, IProgress<float>? progress)
        {
            foreach (var (serializer, storage) in this.GetSerializerAndStorage(type))
            {
                var contains = false;
                yield return storage.ContainsAsync(key, result => contains = result);
                if (!contains) continue;
                var rawData = default(object)!;
                yield return storage.ReadAsync(key, result => rawData = result, progress);
                var savedData = default(object)!;
                yield return serializer.DeserializeAsync(type, rawData, result => savedData = result);
                this.logger.Debug($"Loaded {key}");
                callback(savedData);
                yield break;
            }
            var newData = type.GetEmptyConstructor()();
            this.logger.Debug($"Instantiated {key}");
            callback(newData);
        }

        IEnumerator IDataManager.SaveAsync(string key, object data, Action? callback, IProgress<float>? progress)
        {
            foreach (var (serializer, storage) in this.GetSerializerAndStorage(data.GetType()))
            {
                if (storage is not IWritableStorage writableStorage) continue;
                var rawData = default(object)!;
                yield return serializer.SerializeAsync(data, result => rawData = result);
                yield return writableStorage.WriteAsync(key, rawData, progress: progress);
                this.logger.Debug($"Saved {key}");
                yield break;
            }
            throw new InvalidOperationException($"No writable storage found for {key}");
        }

        IEnumerator IDataManager.FlushAsync(Action? callback, IProgress<float>? progress)
        {
            return this.storages.OfType<IFlushableStorage>().ForEachAsync(
                static (storage, progress) => storage.FlushAsync(progress: progress),
                callback,
                progress
            );
        }
        #endif

        #endregion

        #region Private

        private (ISerializer, IStorage)[] GetSerializerAndStorage(Type type)
        {
            return this.serializerAndStorageCache.GetOrAdd(
                type,
                static state =>
                {
                    var result = IterTools.Product(
                            state.@this.serializers.Where(static (serializer, type) => serializer.CanSerialize(type), state.type),
                            state.@this.storages.Where(static (storage,       type) => storage.CanStore(type), state.type)
                        )
                        .Where(static (serializer, storage) => serializer.RawDataType == storage.RawDataType)
                        .Reverse()
                        .ToArray();
                    if (result.Length is 0) throw new InvalidOperationException($"No serializer or storage found for {state.type.Name}");
                    return result;
                },
                (@this: this, type)
            );
        }

        #endregion
    }
}