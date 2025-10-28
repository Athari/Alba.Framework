namespace Alba.Framework.Collections;

[PublicAPI]
public class ComparerBuilder<T> : IComparer<T>
{
    private readonly IComparer<T> _comparer;

    private ComparerBuilder(IComparer<T> comparer) =>
        _comparer = comparer;

    public static ComparerBuilder<T> Order(IComparer<T>? comparer = null) =>
        new(comparer ?? Comparer<T>.Default);

    public static ComparerBuilder<T> OrderBy<TKey>(Func<T, TKey> keySelector, IComparer<TKey>? comparer = null) =>
        new(KeyComparer(keySelector, comparer, false));

    public static ComparerBuilder<T> OrderByDescending<TKey>(Func<T, TKey> keySelector, IComparer<TKey>? comparer = null) =>
        new(KeyComparer(keySelector, comparer, true));

    public ComparerBuilder<T> Then(IComparer<T>? comparer = null) =>
        new(new ChainComparer(_comparer, comparer ?? Comparer<T>.Default));

    public ComparerBuilder<T> ThenBy<TKey>(Func<T, TKey> keySelector, IComparer<TKey>? comparer = null) =>
        Then(KeyComparer(keySelector, comparer, false));

    public ComparerBuilder<T> ThenByDescending<TKey>(Func<T, TKey> keySelector, IComparer<TKey>? comparer = null) =>
        Then(KeyComparer(keySelector, comparer, true));

    public int Compare(T? x, T? y) => _comparer.Compare(x, y);

    private static Comparer<T> KeyComparer<TKey>(Func<T, TKey> keySelector, IComparer<TKey>? comparer, bool isDescending)
    {
        comparer ??= Comparer<TKey>.Default;
        return Comparer<T>.Create(isDescending
            ? (x, y) => -comparer.Compare(keySelector(x), keySelector(y))
            : (x, y) => comparer.Compare(keySelector(x), keySelector(y)));
    }

    private class ChainComparer : IComparer<T>
    {
        private readonly List<IComparer<T>> _comparers = [ ];

        public ChainComparer(params IEnumerable<IComparer<T>> comparers)
        {
            foreach (var comparer in comparers)
                AddComparer(comparer);
        }

        private void AddComparer(IComparer<T> comparer)
        {
            if (comparer is not ChainComparer chain)
                _comparers.Add(comparer);
            else
                foreach (var subComparer in chain._comparers)
                    AddComparer(subComparer);
        }

        public int Compare(T? x, T? y)
        {
            int r = 0;
            for (int i = 0; r == 0 && i < _comparers.Count; i++)
                r = _comparers[i].Compare(x, y);
            return r;
        }
    }
}