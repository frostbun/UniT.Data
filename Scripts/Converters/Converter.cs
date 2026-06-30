#nullable enable
namespace UniT.Data.Converters
{
    using System;

    public abstract class Converter : IConverter
    {
        IConverterManager IConverter.Manager { set => this.Manager = value; }

        bool IConverter.CanConvert(Type type) => this.CanConvert(type);

        object? IConverter.GetDefaultValue(Type type) => this.GetDefaultValue(type);

        object IConverter.ConvertFromString(Type type, string str)
        {
            try
            {
                return this.ConvertFromString(type, str);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException($"Cannot convert '{str}' to '{type.Name}' with '{this.GetType().Name}': {e.Message}");
            }
        }

        string IConverter.ConvertToString(Type type, object obj)
        {
            try
            {
                return this.ConvertToString(type, obj);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException($"Cannot convert '{type.Name}' '{obj}' to string with '{this.GetType().Name}': {e.Message}");
            }
        }

        protected IConverterManager Manager { get; private set; } = null!;

        protected abstract bool CanConvert(Type type);

        protected virtual object? GetDefaultValue(Type type) => null;

        protected abstract object ConvertFromString(Type type, string str);

        protected abstract string ConvertToString(Type type, object obj);
    }

    public abstract class Converter<T> : Converter where T : notnull
    {
        protected sealed override bool CanConvert(Type type) => type == typeof(T);

        protected sealed override object ConvertFromString(Type type, string str) => this.ConvertFromString(str);

        protected sealed override string ConvertToString(Type type, object obj) => this.ConvertToString((T)obj);

        protected abstract T ConvertFromString(string str);

        protected abstract string ConvertToString(T obj);
    }
}