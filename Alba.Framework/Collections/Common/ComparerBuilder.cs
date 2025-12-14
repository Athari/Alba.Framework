using System.ComponentModel;

namespace Alba.Framework.Collections;

public class ComparerBuilder<T>
{
    private readonly List<IComparer<T>> _comparers = [ ];

    public static ComparerBuilder<T> Empty => new();

    public static ComparerBuilder<T> Order(IComparer<T>? comparer = null, ListSortDirection direction = ListSortDirection.Ascending) =>
        new ComparerBuilder<T>().AddComparer(comparer, direction);

    public static ComparerBuilder<T> OrderBy<TKey>(Func<T, TKey>? keySelector, IComparer<TKey>? comparer = null) =>
        OrderByDirection(keySelector, ListSortDirection.Ascending, comparer);

    public static ComparerBuilder<T> OrderByDescending<TKey>(Func<T, TKey>? keySelector, IComparer<TKey>? comparer = null) =>
        OrderByDirection(keySelector, ListSortDirection.Descending, comparer);

    public static ComparerBuilder<T> OrderByDirection<TKey>(
        Func<T, TKey>? keySelector, ListSortDirection direction, IComparer<TKey>? comparer = null) =>
        new ComparerBuilder<T>().AddComparer(KeyComparer(keySelector, comparer, direction));

    public ComparerBuilder<T> Then(IComparer<T>? comparer = null, ListSortDirection direction = ListSortDirection.Ascending) =>
        AddComparer(comparer, direction);

    public ComparerBuilder<T> ThenBy<TKey>(Func<T, TKey>? keySelector, IComparer<TKey>? comparer = null) =>
        ThenByDirection(keySelector, ListSortDirection.Ascending, comparer);

    public ComparerBuilder<T> ThenByDescending<TKey>(Func<T, TKey>? keySelector, IComparer<TKey>? comparer = null) =>
        ThenByDirection(keySelector, ListSortDirection.Descending, comparer);

    public ComparerBuilder<T> ThenByDirection<TKey>(
        Func<T, TKey>? keySelector, ListSortDirection direction, IComparer<TKey>? comparer = null) =>
        keySelector != null ? AddComparer(KeyComparer(keySelector, comparer, direction)) : this;

    private ComparerBuilder<T> AddComparer(IComparer<T>? comparer, ListSortDirection direction = ListSortDirection.Ascending)
    {
        if (comparer is ChainComparer chain)
            _comparers.AddRange(chain._comparers);
        else
            _comparers.Add((comparer ?? Comparer<T>.Default).WithDirection(direction));
        return this;
    }

    public IComparer<T> ToComparer()
    {
        return _comparers.Where(c => c is not EmptyComparer).ToList() switch {
            { Count: 0 } => new EmptyComparer(),
            [ var comparer ] => comparer,
            _ => new ChainComparer(_comparers),
        };
    }

    private static IComparer<T> KeyComparer<TKey>(
        Func<T, TKey>? keySelector, IComparer<TKey>? comparer, ListSortDirection direction)
    {
        if (keySelector == null)
            return EmptyComparer.Instance;
        comparer ??= Comparer<TKey>.Default;
        return Comparer<T>.Create(direction == ListSortDirection.Ascending
            ? (x, y) => comparer.Compare(keySelector(x), keySelector(y))
            : (x, y) => -comparer.Compare(keySelector(x), keySelector(y)));
    }

    private class ChainComparer(List<IComparer<T>> comparers) : IComparer<T>
    {
        public readonly List<IComparer<T>> _comparers = comparers;

        public int Compare(T? x, T? y)
        {
            int r = 0;
            for (int i = 0; r == 0 && i < _comparers.Count; i++)
                r = _comparers[i].Compare(x, y);
            return r;
        }
    }

    private class EmptyComparer : IComparer<T>
    {
        public static readonly EmptyComparer Instance = new();

        public int Compare(T? x, T? y) => 0;
    }
}