using System.Collections;

namespace Alba.Framework.Collections;

[PublicAPI]
public static class ListExts
{
    private static readonly Random Rnd = new();

    public static T GetRandomItem<T>(this IList<T> @this, Random? rnd = null)
    {
        return @this[(rnd ?? Rnd).Next(@this.Count)];
    }

    public static List<T> GetRange<T>(this List<T> @this, Range range)
    {
        var (index, count) = range.GetOffsetAndLength(@this.Count);
        return @this.GetRange(index, count);
    }

    public static void InsertRange<T>(this IList<T> @this, int index, IEnumerable<T> items)
    {
        foreach (T item in items)
            @this.Insert(index++, item);
    }

    public static T AtWrapped<T>(this IList<T> @this, int index)
    {
        return @this[WrapIndex(index, @this.Count)];
    }

    public static T AtWrappedOrDefault<T>(this IList<T> @this, int index, T defaultValue = default!)
    {
        return @this.Count > 0 ? @this[WrapIndex(index, @this.Count)] : defaultValue;
    }

    public static void SetAtWrapped<T>(this IList<T> @this, int index, T value)
    {
        @this[WrapIndex(index, @this.Count)] = value;
    }

    public static int IndexOfOrDefault<T>(this IList<T> @this, T value, int defaultIndex)
    {
        int index = @this.IndexOf(value);
        return index != -1 ? index : defaultIndex;
    }

    public static void SwapAt<T>(this IList<T> @this, int index, int indexOther)
    {
        if (@this is IOwnedList<T> ownedList) {
            ownedList.SwapAt(index, indexOther);
            return;
        }
        (@this[index], @this[indexOther]) = (@this[indexOther], @this[index]);
    }

    public static void ClearAndDispose<T>(this IList<T> @this) where T : IDisposable
    {
        foreach (T item in @this)
            item.Dispose();
        @this.Clear();
    }

    public static void AddRangeUntyped(this IList @this, IEnumerable items)
    {
        foreach (object item in items)
            @this.Add(item);
    }

    public static void RemoveRangeUntyped(this IList @this, IEnumerable items)
    {
        foreach (object item in items)
            @this.Remove(item);
    }

    public static void ReplaceUntyped(this IList @this, IEnumerable items)
    {
        @this.Clear();
        @this.AddRangeUntyped(items);
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