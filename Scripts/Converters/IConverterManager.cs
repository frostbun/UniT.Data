#nullable enable
namespace UniT.Data.Converters
{
    using System;
    using System.Runtime.CompilerServices;

    public interface IConverterManager
    {
        public IConverter GetConverter(Type type);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public object? GetDefaultValue(Type type) => this.GetConverter(type).GetDefaultValue(type);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public object ConvertFromString(Type type, string str) => this.GetConverter(type).ConvertFromString(type, str);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string ConvertToString(Type type, object obj) => this.GetConverter(type).ConvertToString(type, obj);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T? GetDefaultValue<T>() => (T?)this.GetDefaultValue(typeof(T));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T ConvertFromString<T>(string str) => (T)this.ConvertFromString(typeof(T), str);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string ConvertToString<T>(T obj) where T : notnull => this.ConvertToString(typeof(T), obj);
    }
}