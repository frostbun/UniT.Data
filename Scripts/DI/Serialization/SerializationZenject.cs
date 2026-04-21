#if UNIT_ZENJECT
#nullable enable
namespace UniT.Data.Serialization.DI
{
    using Zenject;
    #if UNIT_JSON
    using Newtonsoft.Json;
    using JsonSerializer = JsonSerializer;
    #endif
    #if UNIT_YAML
    using SharpYaml;
    using YamlSerializer = YamlSerializer;
    #endif
    #if UNIT_TOML
    using Tomlyn;
    using TomlSerializer = TomlSerializer;
    #endif
    #if UNIT_CSV
    using System.Globalization;
    using CsvHelper.Configuration;
    #endif
    #if UNIT_MESSAGEPACK
    using MessagePack;
    using MessagePackSerializer = MessagePackSerializer;
    #endif
    #if UNIT_MEMORYPACK
    using MemoryPack;
    using MemoryPackSerializer = MemoryPackSerializer;
    #endif

    public static class SerializationZenject
    {
        public static void BindSerializers(this DiContainer container)
        {
            #if UNIT_JSON
            if (!container.HasBinding<JsonSerializerSettings>())
            {
                container.BindInstance(DefaultJsonSerializerSettings.Value);
            }
            container.BindInterfacesAndSelfTo<JsonSerializer>().AsSingle();
            #endif

            #if UNIT_YAML
            if (!container.HasBinding<YamlSerializerOptions>())
            {
                container.BindInstance(new YamlSerializerOptions());
            }
            container.BindInterfacesAndSelfTo<YamlSerializer>().AsSingle();
            #endif

            #if UNIT_TOML
            if (!container.HasBinding<TomlSerializerOptions>())
            {
                container.BindInstance(new TomlSerializerOptions());
            }
            container.BindInterfacesAndSelfTo<TomlSerializer>().AsSingle();
            #endif

            container.BindInterfacesAndSelfTo<UnityObjectSerializer>().AsSingle();

            #if UNIT_CSV
            if (!container.HasBinding<CsvConfiguration>())
            {
                container.BindInstance(new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    MissingFieldFound     = null,
                    PrepareHeaderForMatch = args => args.Header.ToLowerInvariant(),
                });
            }
            container.BindInterfacesAndSelfTo<CsvSerializer>().AsSingle();
            #endif

            #if UNIT_PROTOBUF
            container.BindInterfacesAndSelfTo<ProtobufSerializer>().AsSingle();
            #endif

            #if UNIT_MESSAGEPACK
            if (!container.HasBinding<MessagePackSerializerOptions>())
            {
                container.BindInstance(MessagePackSerializerOptions.Standard);
            }
            container.BindInterfacesAndSelfTo<MessagePackSerializer>().AsSingle();
            #endif

            #if UNIT_MEMORYPACK
            if (!container.HasBinding<MemoryPackSerializerOptions>())
            {
                container.BindInstance(MemoryPackSerializerOptions.Default);
            }
            container.BindInterfacesAndSelfTo<MemoryPackSerializer>().AsSingle();
            #endif
        }
    }
}
#endif