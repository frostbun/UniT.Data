#nullable enable
namespace UniT.Data.Converters
{
    using System;

    public interface IConverter
    {
        public IConverterManager Manager { set; }

        public bool CanConvert(Type type);

        public object? GetDefaultValue(Type type);

        public object ConvertFromString(Type type, string str);

        public string ConvertToString(Type type, object obj);
    }
}