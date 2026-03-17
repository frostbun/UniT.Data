#nullable enable
namespace UniT.Data.Conversion
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UniT.Extensions;
    using UnityEngine.Scripting;

    public sealed class ConverterManager : IConverterManager
    {
        private readonly IReadOnlyList<IConverter> converters;

        private readonly Dictionary<Type, IConverter> converterCache = new Dictionary<Type, IConverter>();

        [Preserve]
        public ConverterManager(IEnumerable<IConverter> converters)
        {
            this.converters = converters.ToArray();
            foreach (var converter in this.converters) converter.Manager = this;
        }

        IConverter IConverterManager.GetConverter(Type type)
        {
            lock (this.converterCache)
            {
                return this.converterCache.GetOrAdd(type, state =>
                {
                    return state.converters.LastOrDefault(converter => converter.CanConvert(state.type))
                        ?? throw new ArgumentOutOfRangeException(nameof(state.type), state.type, $"No converter found for {state.type.Name}");
                }, (this.converters, type));
            }
        }
    }
}