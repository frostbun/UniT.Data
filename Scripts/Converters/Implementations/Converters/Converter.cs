#nullable enable
namespace UniT.Data.Converters
{
    using System;

    public abstract class Converter : IConverter
    {
        IConverterManager IConverter.Manager { set => this.Manager = value; }

        bool IConverter.CanConvert(Type type) => this.CanConvert(type);

        object? IConverter.GetDefaultValue(Type type) => this.GetDefaultValue(type);

        object IConverter.ConvertFromString(string str, Type type)
        {
            try
            {
                var result     = this.ConvertFromString(str, type);
                var resultType = result.GetType();
                return type.IsAssignableFrom(resultType) ? result : throw new InvalidOperationException($"Expected '{type.Name}', got '{resultType.Name}'");
            }
            catch (Exception e)
            {
                throw new InvalidOperationException($"Cannot convert '{str}' to '{type.Name}' with '{this.GetType().Name}'", e);
            }
        }

        string IConverter.ConvertToString(object obj, Type type)
        {
            try
            {
                return this.ConvertToString(obj, type);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException($"Cannot convert '{type.Name}' '{obj}' to string with '{this.GetType().Name}'", e);
            }
        }

        protected IConverterManager Manager { get; private set; } = null!;

        protected abstract bool CanConvert(Type type);

        protected virtual object? GetDefaultValue(Type type) => null;

        protected abstract object ConvertFromString(string str, Type type);

        protected abstract string ConvertToString(object obj, Type type);
    }

    public abstract class Converter<T> : Converter
    {
        protected sealed override bool CanConvert(Type type) => type == typeof(T);
    }
}