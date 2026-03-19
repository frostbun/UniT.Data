#if UNIT_DI
#nullable enable
namespace UniT.Data.Serialization.DI
{
    using UniT.DI;
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

    public static class SerializationDI
    {
        public static void AddSerializers(this DependencyContainer container)
        {
            #if UNIT_JSON
            if (!container.Contains<JsonSerializerSettings>())
            {
                container.Add(DefaultJsonSerializerSettings.Value);
            }
            container.AddInterfacesAndSelf<JsonSerializer>();
            #endif

            container.AddInterfacesAndSelf<UnityObjectSerializer>();

            #if UNIT_CSV
            if (!container.Contains<CsvConfiguration>())
            {
                container.Add(new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    MissingFieldFound     = null,
                    PrepareHeaderForMatch = args => args.Header.ToLowerInvariant(),
                });
            }
            container.AddInterfacesAndSelf<CsvSerializer>();
            #endif

            #if UNIT_MEMORYPACK
            if (!container.Contains<MemoryPackSerializerOptions>())
            {
                container.Add(MemoryPackSerializerOptions.Default);
            }
            container.AddInterfacesAndSelf<MemoryPackSerializer>();
            #endif

            #if UNIT_MESSAGEPACK
            if (!container.Contains<MessagePackSerializerOptions>())
            {
                container.Add(MessagePackSerializerOptions.Standard);
            }
            container.AddInterfacesAndSelf<MessagePackSerializer>();
            #endif

            #if UNIT_PROTOBUF
            container.AddInterfacesAndSelf<ProtobufSerializer>();
            #endif
        }
    }
}
#endif