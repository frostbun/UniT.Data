#nullable enable
namespace UniT.Data.Converters.Default
{
    public sealed class SeparatorConfig
    {
        public string CollectionSeparator { get; }
        public string KeyValueSeparator   { get; }
        public string TupleSeparator      { get; }

        public SeparatorConfig(string collectionSeparator = ";", string keyValueSeparator = ":", string tupleSeparator = "|")
        {
            this.CollectionSeparator = collectionSeparator;
            this.KeyValueSeparator   = keyValueSeparator;
            this.TupleSeparator      = tupleSeparator;
        }
    }
}