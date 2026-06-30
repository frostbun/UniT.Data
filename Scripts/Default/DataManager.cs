#nullable enable
namespace UniT.Data.Default
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using Cysharp.Threading.Tasks;
    using UniT.Data.Serializers;
    using UniT.Data.Storages;
    using UniT.Extensions;
    using UniT.Logging;
    using UnityEngine.Scripting;

    public sealed class DataManager : IDataManager
    {
        #region Constructor

        private readonly IReadOnlyList<ISerializer> serializers;
        private readonly IReadOnlyList<IStorage>    storages;
        private readonly ILogger                    logger;

        private readonly Dictionary<Type, IReadOnlyList<(ISerializer, IStorage)>> serializerAndStorageCache = new();

        [Preserve]
        public DataManager(IReadOnlyList<ISerializer> serializers, IReadOnlyList<IStorage> storages, ILoggerManager loggerManager)
        {
            this.serializers = serializers;
            this.storages    = storages;
            this.logger      = loggerManager.GetLogger(this);
            this.logger.Debug("Constructed");
        }

        #endregion

        async UniTask<object> IDataManager.LoadAsync(string key, Type type, IProgress<float>? progress, CancellationToken cancellationToken)
        {
            foreach (var (serializer, storage) in this.GetSerializerAndStorage(type))
            {
                if (storage is not IReadableStorage readableStorage) continue;
                if (!await readableStorage.ContainsAsync(key, cancellationToken: cancellationToken)) continue;
                var rawData = await readableStorage.ReadAsync(key, serializer.RawDataType, progress, cancellationToken);
                #if !UNITY_WEBGL
                var savedData = await UniTask.RunOnThreadPool(() => serializer.Deserialize(type, rawData), cancellationToken: cancellationToken);
                #else
                var savedData = serializer.Deserialize(type, rawData);
                #endif
                this.logger.Debug($"Loaded {key} - {serializer.GetType().Name} - {storage.GetType().Name}");
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
                var rawData = serializer.Serialize(data.GetType(), data);
                await writableStorage.WriteAsync(key, rawData, serializer.RawDataType, progress, cancellationToken);
                this.logger.Debug($"Saved {key} - {serializer.GetType().Name} - {storage.GetType().Name}");
                return;
            }
            throw new InvalidOperationException($"No writable storage found for {key}");
        }

        void IDataManager.Flush()
        {
            foreach (var storage in this.storages.OfType<IWritableStorage>())
            {
                storage.Flush();
                this.logger.Debug($"Flushed {storage.GetType().Name}");
            }
        }

        private IReadOnlyList<(ISerializer, IStorage)> GetSerializerAndStorage(Type type)
        {
            return this.serializerAndStorageCache.GetOrAdd(
                type,
                static state =>
                {
                    var result = IterTools.Product(
                            state.@this.serializers.Where(static (serializer, type) => serializer.CanSerialize(type), state.type),
                            state.@this.storages.Where(static (storage,       type) => typeof(IWritableData).IsAssignableFrom(type) == storage is IWritableStorage, state.type)
                        )
                        .Where(static (serializer, storage) => storage.CanStore(serializer.RawDataType))
                        .Reverse()
                        .ToArray();
                    if (result.Length is 0) throw new KeyNotFoundException($"No serializer or storage found for {state.type.Name}");
                    return result;
                },
                (@this: this, type)
            );
        }
    }
}