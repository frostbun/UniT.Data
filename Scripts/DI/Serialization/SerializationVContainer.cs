#if UNIT_VCONTAINER
#nullable enable
namespace UniT.Data.Serialization.DI
{
    using VContainer;
    #if UNIT_JSON
    using Newtonsoft.Json;
    using JsonSerializer = JsonSerializer;
    #endif
    #if UNIT_CSV
    using System.Globalization;
    using CsvHelper.Configuration;
    #endif
    #if UNIT_MEMORYPACK
    using MemoryPack;
    using MemoryPackSerializer = MemoryPackSerializer;
    #endif
    #if UNIT_MESSAGEPACK
    using MessagePack;
    using MessagePackSerializer = MessagePackSerializer;
    #endif

    public static class SerializationVContainer
    {
        public static void RegisterSerializers(this IContainerBuilder builder)
        {
            #if UNIT_JSON
            if (!builder.Exists(typeof(JsonSerializerSettings)))
            {
                builder.RegisterInstance(DefaultJsonSerializerSettings.Value);
            }
            builder.Register<JsonSerializer>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            #endif

            builder.Register<UnityObjectSerializer>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();

            #if UNIT_CSV
            if (!builder.Exists(typeof(CsvConfiguration)))
            {
                builder.RegisterInstance(new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    MissingFieldFound     = null,
                    PrepareHeaderForMatch = args => args.Header.ToLowerInvariant(),
                });
            }
            builder.Register<CsvSerializer>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            #endif

            #if UNIT_MEMORYPACK
            if (!builder.Exists(typeof(MemoryPackSerializerOptions)))
            {
                builder.RegisterInstance(MemoryPackSerializerOptions.Default);
            }
            builder.Register<MemoryPackSerializer>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            #endif

            #if UNIT_MESSAGEPACK
            if (!builder.Exists(typeof(MessagePackSerializerOptions)))
            {
                builder.RegisterInstance(MessagePackSerializerOptions.Standard);
            }
            builder.Register<MessagePackSerializer>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            #endif

            #if UNIT_PROTOBUF
            builder.Register<ProtobufSerializer>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            #endif
        }
    }
}
#endif