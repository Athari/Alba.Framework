using ObservableComputations;

namespace Alba.Framework.Collections.ObservableComputations;

[SuppressMessage("Style", "IDE0063: Use simple using statement", Justification = "Disposes too late")]
[SuppressMessage("ReSharper", "ConvertToUsingDeclaration", Justification = "Disposes too late")]
public class ObservableCollectionEx<T> : ObservableCollectionExtended<T>
{
    private readonly CollectionPausing<T> _pausing;

    public ObservableCollectionEx()
    {
        _pausing = this.CollectionPausing();
    }

    public ObservableCollectionEx(List<T> list) : base(list)
    {
        _pausing = this.CollectionPausing();
    }

    public ObservableCollectionEx(IEnumerable<T> collection) : base(collection)
    {
        _pausing = this.CollectionPausing();
    }

    public bool IsPaused {
        get => _pausing.IsPaused;
        set => _pausing.IsPaused = value;
    }

    public void AddRange([InstantHandle] IEnumerable<T> items)
    {
        foreach (var item in items)
            Add(item);
    }

    public void AddRangeWithPause([InstantHandle] IEnumerable<T> items)
    {
        using var _ = WithPause();
        AddRange(items);
    }

    public virtual void RemoveRange([InstantHandle] IEnumerable<T> items)
    {
        foreach (T item in items)
            Remove(item);
    }

    public virtual void RemoveRangeWithPause([InstantHandle] IEnumerable<T> items)
    {
        using var _ = WithPause();
        RemoveRange(items);
    }

    public virtual void InsertRange(int index, [InstantHandle] IEnumerable<T> items)
    {
        foreach (T item in items)
            Insert(index++, item);
    }

    public virtual void InsertRangeWithPause(int index, [InstantHandle] IEnumerable<T> items)
    {
        using var _ = WithPause();
        InsertRange(index, items);
    }

    public void ReplaceAll([InstantHandle] IEnumerable<T> items)
    {
        Clear();
        AddRange(items);
    }

    public void ReplaceAllWithPause([InstantHandle] IEnumerable<T> items)
    {
        using var _ = WithPause();
        ReplaceAll(items);
    }

    public int RemoveAtWithSelectedIndex(int selectedIndex)
    {
        Guard.IsInRangeFor(selectedIndex, (IList<T>)this);
        var newSelectedIndex = -1;
        using (var _ = this.WithSelectedIndex(selectedIndex, i => newSelectedIndex = i))
            RemoveAt(selectedIndex);
        return newSelectedIndex;
    }

    public T? RemoveWithSelectedItem(T? selectedItem)
    {
        Guard.IsNotNull(selectedItem);
        var newSelectedItem = default(T?);
        using (var _ = this.WithSelectedItem(selectedItem, i => newSelectedItem = i))
            Remove(selectedItem);
        return newSelectedItem;
    }

    public CollectionPauseDisposable WithPause() =>
        new(this);

    public readonly struct CollectionPauseDisposable : IDisposable
    {
        private readonly ObservableCollectionEx<T> _collection;

        public CollectionPauseDisposable(ObservableCollectionEx<T> collection)
        {
            _collection = collection;
            _collection.IsPaused = true;
        }

        public void Dispose()
        {
            _collection.IsPaused = false;
        }
    }
}