#nullable enable
namespace UniT.Data.Converters.Default
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using UniT.Extensions;
    using UnityEngine.Scripting;

    public sealed class ConverterManager : IConverterManager
    {
        private readonly IReadOnlyList<IConverter> converters;

        private readonly ConcurrentDictionary<Type, IConverter> converterCache = new();

        [Preserve]
        public ConverterManager(IReadOnlyList<IConverter> converters)
        {
            this.converters = converters;
            foreach (var converter in this.converters) converter.Manager = this;
        }

        IConverter IConverterManager.GetConverter(Type type)
        {
            return this.converterCache.GetOrAdd(type, static state =>
            {
                return state.converters.LastOrDefault(converter => converter.CanConvert(state.type))
                    ?? throw new KeyNotFoundException($"No converter found for {state.type.Name}");
            }, (this.converters, type));
        }
    }
}