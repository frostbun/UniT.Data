#if UNIT_CSV
#nullable enable
namespace UniT.Data.Serializers
{
    using System;
    using System.Collections;

    public interface ICsvData
    {
        public Type RowType { get; }

        public void Add(object key, object value);

        public IEnumerator GetValues();
    }
}
#endif