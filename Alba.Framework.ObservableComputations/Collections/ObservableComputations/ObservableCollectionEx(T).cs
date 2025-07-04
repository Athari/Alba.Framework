using ObservableComputations;

namespace Alba.Framework.Collections.ObservableComputations;

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

    public void AddRange(IEnumerable<T> items)
    {
        foreach (var item in items)
            Add(item);
    }

    public void AddRangeWithPause(IEnumerable<T> items)
    {
        using var _ = WithPause();
        AddRange(items);
    }

    public virtual void RemoveRange(IEnumerable<T> items)
    {
        foreach (T item in items)
            Remove(item);
    }

    public virtual void RemoveRangeWithPause(IEnumerable<T> items)
    {
        using var _ = WithPause();
        RemoveRange(items);
    }

    public virtual void InsertRange(int index, IEnumerable<T> items)
    {
        foreach (T item in items)
            Insert(index++, item);
    }

    public virtual void InsertRangeWithPause(int index, IEnumerable<T> items)
    {
        using var _ = WithPause();
        InsertRange(index, items);
    }

    public void ReplaceAll(IEnumerable<T> items)
    {
        Clear();
        AddRange(items);
    }

    public void ReplaceAllWithPause(IEnumerable<T> items)
    {
        using var _ = WithPause();
        ReplaceAll(items);
    }

    public int RemoveAtWithSelectedIndex(int selectedIndex)
    {
        var newSelectedIndex = -1;
        using var _ = this.WithSelectedIndex(selectedIndex, i => newSelectedIndex = i);
        return newSelectedIndex;
    }

    public T? RemoveWithSelectedItem(T? selectedItem)
    {
        var newSelectedItem = default(T?);
        using var _ = this.WithSelectedItem(selectedItem, i => newSelectedItem = i);
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
            _collection._pausing.IsPaused = true;
        }

        public void Dispose()
        {
            _collection._pausing.IsPaused = false;
        }
    }
}