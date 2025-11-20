using System.Collections.Specialized;
using ObservableComputations;

namespace Alba.Framework.Collections.ObservableComputations;

[SuppressMessage("Style", "IDE0063: Use simple using statement", Justification = "Disposes too late")]
[SuppressMessage("ReSharper", "ConvertToUsingDeclaration", Justification = "Disposes too late")]
public class ObservableCollectionEx<T> : ObservableCollectionExtended<T>
{
    private readonly CollectionPausing<T> _pausing;

    public ObservableCollectionEx<T> Self => this;

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

    protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
    {
        base.OnCollectionChanged(e);
        OnPropertyChanged(new(nameof(Self)));
    }

    public bool IsPaused {
        get => _pausing.IsPaused;
        set => _pausing.IsPaused = value;
    }

    public virtual void AddRange([InstantHandle] IEnumerable<T> items, bool pause = false)
    {
        using var _ = WithPause(pause);
        foreach (var item in items)
            Add(item);
    }

    public virtual void RemoveRange([InstantHandle] IEnumerable<T> items, bool pause = false)
    {
        using var _ = WithPause(pause);
        foreach (T item in items)
            Remove(item);
    }

    public virtual void InsertRange(int index, [InstantHandle] IEnumerable<T> items, bool pause = false)
    {
        using var _ = WithPause(pause);
        foreach (T item in items)
            Insert(index++, item);
    }

    public virtual void ReplaceAll([InstantHandle] IEnumerable<T> items)
    {
        using var _ = WithPause();
        Clear();
        AddRange(items);
    }

    [MustUseReturnValue]
    public int RemoveAtSelectedIndex(int selectedIndex)
    {
        var newSelectedIndex = this.WithSelectedIndex(selectedIndex);
        RemoveAt(selectedIndex);
        return newSelectedIndex;
    }

    [MustUseReturnValue]
    public T? RemoveSelectedItem(T selectedItem)
    {
        var newSelectedItem = this.WithSelectedItem(selectedItem);
        Remove(selectedItem);
        return newSelectedItem;
    }

    public CollectionPauseDisposable WithPause(bool pause = true) =>
        new(pause ? this : null);

    public readonly struct CollectionPauseDisposable : IDisposable
    {
        private readonly ObservableCollectionEx<T>? _collection;

        public CollectionPauseDisposable(ObservableCollectionEx<T>? collection)
        {
            _collection = collection;
            _collection?.IsPaused = true;
        }

        public void Dispose()
        {
            _collection?.IsPaused = false;
        }
    }
}