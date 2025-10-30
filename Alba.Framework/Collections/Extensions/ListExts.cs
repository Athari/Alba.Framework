namespace Alba.Framework.Collections;

[PublicAPI]
public static class ListExts
{
    private static readonly Random Rnd = new();

    extension<T>(List<T> @this)
    {
        public List<T> GetRange(Range range)
        {
            var (index, count) = range.GetOffsetAndLength(@this.Count);
            return @this.GetRange(index, count);
        }
    }

    extension<T>(IList<T> @this)
    {
        public void InsertRange(int index, [InstantHandle] IEnumerable<T> items)
        {
            Guard.IsInRange(index, 0, @this.Count + 1);
            foreach (T item in items)
                @this.Insert(index++, item);
        }

        public void RemoveRange(int index, int count)
        {
            Guard.IsInRangeFor(index, @this);
            Guard.IsInRangeFor(index + count, @this);
            while (--count >= 0)
                @this.RemoveAt(index + count);
        }

        public void ReplaceRange(int index, int count, [InstantHandle] IEnumerable<T> items)
        {
            @this.RemoveRange(index, count);
            @this.InsertRange(index, items);
        }

        public T AtRandom(Random? rnd = null)
        {
            return @this[(rnd ?? Rnd).Next(@this.Count)];
        }

        public T AtWrapped(int index)
        {
            return @this[WrapIndex(index, @this.Count)];
        }

        public T AtWrappedOrDefault(int index, T defaultValue = default!)
        {
            return @this.Count > 0 ? @this[WrapIndex(index, @this.Count)] : defaultValue;
        }

        public void SetAtWrapped(int index, T value)
        {
            @this[WrapIndex(index, @this.Count)] = value;
        }

        public int IndexOfOrDefault(T value, int defaultIndex)
        {
            int index = @this.IndexOf(value);
            return index != -1 ? index : defaultIndex;
        }

        public void SwapAt(int index, int indexOther)
        {
            if (@this is IOwnedList<T> ownedList) {
                ownedList.SwapAt(index, indexOther);
                return;
            }
            (@this[index], @this[indexOther]) = (@this[indexOther], @this[index]);
        }
    }

    extension<T>(IList<T> @this) where T : IDisposable
    {
        public void DisposeAll()
        {
            foreach (var item in @this)
                item.Dispose();
        }

        public void ClearAndDisposeAll()
        {
            foreach (var item in @this)
                item.Dispose();
            @this.Clear();
        }
    }

    private static int WrapIndex(int index, int count)
    {
        Guard.IsGreaterThan(count, 0, nameof(count));
        return index % count + (index < 0 ? count : 0);
    }

    public static ListSelectedIndexDisposable<T> WithSelectedIndex<T>(this IList<T> @this,
        int selectedIndex, Action<int> setSelectedIndex) =>
        new(@this, selectedIndex, setSelectedIndex);

    public static ListSelectedItemDisposable<T> WithSelectedItem<T>(this IList<T> @this,
        T? selectedItem, Action<T?> setSelectedItem) =>
        new(@this, selectedItem != null ? @this.IndexOf(selectedItem) : -1, setSelectedItem);

    public readonly struct ListSelectedIndexDisposable<T>(
        IList<T> list, int selectedIndex, Action<int> setSelectedIndex) : IDisposable
    {
        public void Dispose() =>
            setSelectedIndex(list.Count > 0 ? Math.Min(selectedIndex, list.Count - 1) : -1);
    }

    public readonly struct ListSelectedItemDisposable<T>(
        IList<T> list, int selectedIndex, Action<T?> setSelectedItem) : IDisposable
    {
        public void Dispose() =>
            setSelectedItem(list.Count > 0 ? list[Math.Min(selectedIndex, list.Count - 1)] : default);
    }
}