using System.Diagnostics;

namespace Alba.Framework.Collections;

internal class CollectionDebugView<T>
{
    private readonly ICollection<T> _collection;

    [PublicAPI]
    [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
    public T[] Items => [ .._collection ];

    public CollectionDebugView(ICollection<T> collection)
    {
        Guard.IsNotNull(collection, nameof(collection));
        _collection = collection;
    }
}