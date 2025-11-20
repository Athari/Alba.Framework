using System.Diagnostics;

namespace Alba.Framework.Collections;

internal class CollectionDebugView<T>
{
    private readonly ICollection<T> _collection;

    [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
    public T[] Items => [ .._collection ];

    public CollectionDebugView(ICollection<T> collection)
    {
        Guard.IsNotNull(collection);
        _collection = collection;
    }
}