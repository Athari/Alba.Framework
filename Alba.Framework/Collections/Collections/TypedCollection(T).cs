using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Alba.Framework.Sys;
using Alba.Framework.Text;

namespace Alba.Framework.Collections
{
    [Serializable]
    [DebuggerDisplay ("Count = {Count}"), DebuggerTypeProxy (typeof(CollectionDebugView<>))]
    internal class TypedCollection<T> : IReadOnlyList<T>, ICollection
    {
        private readonly ICollection _collection;
        [NonSerialized]
        private readonly PropertyInfo _indexProp;

        public TypedCollection (ICollection collection)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");
            _collection = collection;
            _indexProp = _collection.GetType().GetProperties().Where(prop => {
                ParameterInfo[] indexParams = prop.GetIndexParameters();
                return indexParams.Length == 1 && indexParams[0].ParameterType == typeof(int) && prop.Name == "Item";
            }).FirstOrDefault();
        }

        public int Count
        {
            get { return _collection.Count; }
        }

        int IReadOnlyCollection<T>.Count
        {
            get { return Count; }
        }

        public object SyncRoot
        {
            get { return _collection.SyncRoot; }
        }

        public bool IsSynchronized
        {
            get { return _collection.IsSynchronized; }
        }

        public T this [int index]
        {
            get
            {
                if (_indexProp == null)
                    throw new NotSupportedException("Collection '{0}' does not have an indexed property.".Fmt(_collection.GetTypeFullName()));
                return (T)_indexProp.GetValue(_collection, new object[] { index });
            }
        }

        public IEnumerator<T> GetEnumerator ()
        {
            return _collection.GetEnumerator().Cast<T>();
        }

        IEnumerator IEnumerable.GetEnumerator ()
        {
            return _collection.GetEnumerator();
        }

        public void CopyTo (Array array, int index)
        {
            _collection.CopyTo(array, index);
        }
    }
}