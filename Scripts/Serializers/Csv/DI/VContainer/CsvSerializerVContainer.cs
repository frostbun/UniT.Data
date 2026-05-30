#nullable enable
namespace UniT.Data.Serializers.Csv.DI
{
    using System.Globalization;
    using CsvHelper.Configuration;
    using VContainer;

    public static class CsvSerializerVContainer
    {
        public static void RegisterCsvSerializer(this IContainerBuilder builder)
        {
            builder.RegisterCsvSerializer(new(CultureInfo.InvariantCulture)
            {
                MissingFieldFound     = null,
                PrepareHeaderForMatch = args => args.Header.ToLowerInvariant(),
            });
        }

        public static void RegisterCsvSerializer(this IContainerBuilder builder, CsvConfiguration configuration)
        {
            builder.RegisterInstance(configuration);
            builder.Register<CsvSerializer>(Lifetime.Singleton).AsImplementedInterfaces();
        }
    }
}