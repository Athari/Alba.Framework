using System.Collections;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Runtime.Serialization;
using Alba.Framework.Reflection;
using E = System.Linq.Expressions.Expression;

namespace Alba.Framework.Collections;

/// <summary>Typed wrapper of an untyped collection.</summary>
[Serializable]
[DebuggerDisplay("Count = {Count}"), DebuggerTypeProxy(typeof(CollectionDebugView<>))]
public class TypedCollection<T> : IReadOnlyList<T>, ICollection<T>, ICollection
{
    private static readonly ConcurrentDictionary<Type, Func<ICollection, int, T>> IndexerCache = new();

    private readonly ICollection _collection;
    [NonSerialized]
    private Func<int, T> _indexer;

    public TypedCollection(ICollection collection)
    {
        Guard.IsNotNull(collection);
        _collection = collection;
        _indexer = GetIndexer();
    }

    [OnDeserialized]
    private void OnDeserialized(StreamingContext context) => _indexer = GetIndexer();

    private Func<int, T> GetIndexer()
    {
        return _collection switch {
            IList il => i => (T)il[i]!,
            IReadOnlyList<T> irolg => i => irolg[i],
            _ => i => IndexerCache.GetOrAdd(_collection.GetType(), CreateIndexerAccessor)(_collection, i),
        };
    }

    private Func<ICollection, int, T> CreateIndexerAccessor(Type collectionType)
    {
        var indexProp = collectionType.GetProperty("Item", [ typeof(int) ]);
        if (indexProp == null || !indexProp.CanRead)
            throw new NotSupportedException(
                $"Collection type '{collectionType.GetFullName()}' does not have an accessible integer indexer.");

        var paramCol = E.Parameter(typeof(ICollection), "col");
        var paramI = E.Parameter(typeof(int), "i");
        var expr = E.Convert(E.Property(E.Convert(paramCol, collectionType), indexProp, paramI), typeof(T));
        return E.Lambda<Func<ICollection, int, T>>(expr, paramCol, paramI).Compile();
    }

    public T this[int index] => _indexer(index);

    public int Count => _collection.Count;
    bool ICollection<T>.IsReadOnly => true;
    int IReadOnlyCollection<T>.Count => Count;
    object ICollection.SyncRoot => _collection.SyncRoot;
    bool ICollection.IsSynchronized => _collection.IsSynchronized;

    public bool Contains(T item) => _collection.Cast<T>().Contains(item);

    public IEnumerator<T> GetEnumerator() => _collection.Cast<T>().GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => _collection.GetEnumerator();

    public void CopyTo(T[] array, int arrayIndex) => _collection.CopyTo(array, arrayIndex);
    void ICollection.CopyTo(Array array, int index) => _collection.CopyTo(array, index);

    public void Add(T item) => throw GetReadOnlyException();
    public void Clear() => throw GetReadOnlyException();
    public bool Remove(T item) => throw GetReadOnlyException();

    private static NotSupportedException GetReadOnlyException() => new("Typed collection wrapper is read-only.");
}