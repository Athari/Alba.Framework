using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Alba.Framework.Collections
{
    internal class CollectionDebugView<T>
    {
        private readonly ICollection<T> _collection;

        [DebuggerBrowsable (DebuggerBrowsableState.RootHidden)]
        public T[] Items
        {
            get { return _collection.ToArray(); }
        }

        public CollectionDebugView (ICollection<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");
            _collection = collection;
        }
    }
}