using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Alba.Framework.Common;

namespace Alba.Framework.Collections;

[PublicAPI]
[DebuggerDisplay("Count = {Count}"), DebuggerTypeProxy(typeof(CollectionDebugView<>))]
public class TypedList<T>(IList list, CollectionOptions options = CollectionOptions.Default)
    : IList<T>, IReadOnlyList<T>, IList
{
    private readonly IList _list = Ensure.NotNull(list);
    private readonly CollectionOptions _options = options | CollectionOptions.None;

    public int Count => _list.Count;
    public bool IsReadOnly => _list.IsReadOnly || (_options & CollectionOptions.ReadOnly) != 0;

    bool IList.IsFixedSize => _list.IsFixedSize;
    bool ICollection.IsSynchronized => _list.IsSynchronized;
    object ICollection.SyncRoot => _list.SyncRoot;

    public T this[int index] {
        get => (T)_list[index]!;
        set => SetItem(index, value);
    }

    object? IList.this[int index] {
        get => this[index];
        set => this[index] = Ensure.NullableOrNotNullObject<T>(value);
    }

    // public

    public bool Contains(T item) =>
        _list.Cast<T>().Contains(item);

    public int IndexOf(T item) =>
        _list.IndexOf(item);

    public void Add(T item) =>
        Insert(Count, item);

    public void Insert(int index, T item)
    {
        EnsureNotReadOnly();
        if (index < 0 || index > _list.Count)
            throw new ArgumentOutOfRangeException(nameof(index));
        InsertItem(index, item);
    }

    public bool Remove(T item)
    {
        EnsureNotReadOnly();
        int index = IndexOf(item);
        if (index < 0)
            return false;
        RemoveItem(index);
        return true;
    }

    public void RemoveAt(int index)
    {
        EnsureNotReadOnly();
        if (index < 0 || index >= _list.Count)
            throw new ArgumentOutOfRangeException(nameof(index));
        RemoveItem(index);
    }

    public void Clear() =>
        EnsureNotReadOnly(ClearItems);

    public void CopyTo(T[] array, int index) =>
        _list.CopyTo(array, index);

    public IEnumerator<T> GetEnumerator() =>
        _list.Cast<T>().GetEnumerator();

    // IList

    bool IList.Contains(object? value) =>
        Ensure.TryNullableOrNotNullObject<T>(value, out var v) && Contains(v);

    int IList.IndexOf(object? value) =>
        Ensure.TryNullableOrNotNullObject<T>(value, out var v) ? IndexOf(v) : -1;

    int IList.Add(object? value)
    {
        Add(Ensure.NullableOrNotNullObject<T>(value));
        return Count - 1;
    }

    void IList.Insert(int index, object? value) =>
        Insert(index, Ensure.NullableOrNotNullObject<T>(value));

    void IList.Remove(object? value) =>
        Ensure.IfNullableOrNotNullObject<T>(value, v => Remove(v));

    // ICollection

    void ICollection.CopyTo(Array array, int index) =>
        _list.CopyTo(array, index);

    // IEnumerable

    IEnumerator IEnumerable.GetEnumerator() =>
        _list.GetEnumerator();

    // virtual

    protected virtual void SetItem(int index, T item) =>
        _list[index] = item;

    protected virtual void InsertItem(int index, T item) =>
        _list.Insert(index, item);

    protected virtual void RemoveItem(int index) =>
        _list.RemoveAt(index);

    protected virtual void ClearItems() =>
        _list.Clear();

    // utility

    private static NotSupportedException ReadOnly() =>
        new("List is read-only.");

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void EnsureNotReadOnly()
    {
        if (IsReadOnly)
            throw ReadOnly();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void EnsureNotReadOnly(Action action)
    {
        EnsureNotReadOnly();
        action();
    }
}