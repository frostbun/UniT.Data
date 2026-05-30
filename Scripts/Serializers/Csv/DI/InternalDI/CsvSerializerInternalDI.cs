#nullable enable
namespace UniT.Data.Serializers.Csv.DI
{
    using System.Globalization;
    using CsvHelper.Configuration;
    using UniT.DI;

    public static class CsvSerializerInternalDI
    {
        public static void AddCsvSerializer(this DependencyContainer container)
        {
            container.AddCsvSerializer(new(CultureInfo.InvariantCulture)
            {
                MissingFieldFound     = null,
                PrepareHeaderForMatch = args => args.Header.ToLowerInvariant(),
            });
        }

        public static void AddCsvSerializer(this DependencyContainer container, CsvConfiguration configuration)
        {
            container.Add(configuration);
            container.AddInterfaces<CsvSerializer>();
        }
    }
}