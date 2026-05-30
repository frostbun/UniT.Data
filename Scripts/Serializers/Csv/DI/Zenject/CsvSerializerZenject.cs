#nullable enable
namespace UniT.Data.Serializers.Csv.DI
{
    using System.Globalization;
    using CsvHelper.Configuration;
    using Zenject;

    public static class CsvSerializerZenject
    {
        public static void BindCsvSerializer(this DiContainer container)
        {
            container.BindCsvSerializer(new(CultureInfo.InvariantCulture)
            {
                MissingFieldFound     = null,
                PrepareHeaderForMatch = args => args.Header.ToLowerInvariant(),
            });
        }

        public static void BindCsvSerializer(this DiContainer container, CsvConfiguration configuration)
        {
            container.BindInstance(configuration);
            container.BindInterfacesTo<CsvSerializer>().AsSingle();
        }
    }
}