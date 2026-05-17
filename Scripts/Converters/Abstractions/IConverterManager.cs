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
        public object ConvertFromString(string str, Type type) => this.GetConverter(type).ConvertFromString(str, type);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string ConvertToString(object obj, Type type) => this.GetConverter(type).ConvertToString(obj, type);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T? GetDefaultValue<T>() => (T?)this.GetDefaultValue(typeof(T));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T ConvertFromString<T>(string str) => (T)this.ConvertFromString(str, typeof(T));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string ConvertToString<T>(T obj) where T : notnull => this.ConvertToString(obj, typeof(T));
    }
}