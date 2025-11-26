using Alba.Framework.Collections;
using Avalonia.Controls.Selection;

namespace Alba.Framework.Avalonia.Controls.Selection;

public static class SelectionModelExts
{
    extension<T>(SelectionModel<T> @this)
    {
        public int SourceCount =>
            @this.Source?.UntypedCount() ?? 0;

        public IEnumerable<T> AllSelectedItems =>
            @this.SelectedItems.WhereNotNull();

        public int GetSelectedIndexAfterRemoval()
        {
            if (@this.Count == 0 || @this.SourceCount == @this.Count)
                return -1;
            var unselectedIndexes = Enumerable.Range(0, @this.SourceCount).Where(i => !@this.IsSelected(i)).ToList();
            return unselectedIndexes[Math.Min(@this.SelectedIndexes.Min(), unselectedIndexes.Count - 1)];
        }

        public void Select(Index index) =>
            @this.Select(index.GetOffset(@this.SourceCount));

        public void SelectOnly(int index)
        {
            using var _ = @this.BatchUpdate();
            @this.Clear();
            @this.Select(index);
        }

        public void SelectOnly(Index index) =>
            @this.SelectOnly(index.GetOffset(@this.SourceCount));

        public void SelectRange(Range range)
        {
            var (start, end) = range.GetOffsets(@this.SourceCount);
            @this.SelectRange(start, end);
        }

        public void SelectRangeOnly(Range range)
        {
            using var _ = @this.BatchUpdate();
            @this.Clear();
            @this.SelectRange(range);
        }

        public void SelectPrevOnly()
        {
            var index = @this.SelectedIndex;
            if (index == -1)
                @this.SelectOnly(0);
            else if (index > 0)
                @this.SelectOnly(index - 1);
        }

        public void SelectNextOnly()
        {
            var index = @this.SelectedIndex;
            if (index == -1)
                @this.SelectOnly(0);
            else if (index < @this.SourceCount - 1)
                @this.SelectOnly(index + 1);
        }

        public void ToggleSelect(int index, bool? toggle)
        {
            toggle ??= !@this.IsSelected(index);
            if (toggle == true)
                @this.Select(index);
            else if (toggle == false)
                @this.Deselect(index);
        }
    }

    extension<T>(SelectionModelSelectionChangedEventArgs<T> @this)
    {
        public IEnumerable<T> AllSelectedItems =>
            @this.SelectedItems.WhereNotNull();

        public IEnumerable<T> AllDeselectedItems =>
            @this.DeselectedItems.WhereNotNull();
    }
}