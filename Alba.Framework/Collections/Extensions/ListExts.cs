namespace Alba.Framework.Collections;

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

        public T AtRandom(Random? rnd = null) =>
            @this[(rnd ?? Rnd).Next(@this.Count)];

        public T? AtOrDefault(int index) =>
            index >= 0 && index < @this.Count ? @this[index] : default;

        public T AtOrDefault(int index, T defaultValue) =>
            index >= 0 && index < @this.Count ? @this[index] : defaultValue;

        public T AtWrapped(int index) =>
            @this[WrapIndex(index, @this.Count)];

        public T? AtWrappedOrDefault(int index) =>
            @this.Count > 0 ? @this[WrapIndex(index, @this.Count)] : default;

        public T AtWrappedOrDefault(int index, T defaultValue) =>
            @this.Count > 0 ? @this[WrapIndex(index, @this.Count)] : defaultValue;

        public void SetAtWrapped(int index, T value) =>
            @this[WrapIndex(index, @this.Count)] = value;

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

        public ListSelectedIndex<T> WithSelectedIndex(int selectedIndex) =>
            new(@this, selectedIndex);

        public ListSelectedItem<T> WithSelectedItem(T? selectedItem) =>
            new(@this, selectedItem);
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
        Guard.IsGreaterThan(count, 0);
        return index % count + (index < 0 ? count : 0);
    }

    public readonly struct ListSelectedIndex<T>(IList<T> list, int selectedIndex)
    {
        public int Value => list.Count > 0 ? Math.Min(selectedIndex, list.Count - 1) : -1;
        public static implicit operator int(ListSelectedIndex<T> @this) => @this.Value;
    }

    public readonly struct ListSelectedItem<T>(IList<T> list, T? selectedItem)
    {
        private readonly int _selectedIndex = selectedItem != null ? list.IndexOf(selectedItem) : -1;
        public T? Value => list.Count > 0 ? list[Math.Min(_selectedIndex, list.Count - 1)] : default;
        public static implicit operator T?(ListSelectedItem<T> @this) => @this.Value;
    }
}