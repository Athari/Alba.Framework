namespace Alba.Framework;

public static class RangeExts
{
    extension(Range @this)
    {
        public (int Start, int End) GetOffsets(int length) =>
            (@this.Start.GetOffset(length), @this.End.GetOffset(length));

        public IEnumerable<int> ToRange(bool endInclusive = false)
        {
            if (@this.Start.IsFromEnd || @this.End.IsFromEnd)
                throw new ArgumentOutOfRangeException(nameof(@this), @this, "Range start and end must both be from start");
            (int offset, int length) = @this.GetOffsetAndLength(int.MaxValue);
            return Enumerable.Range(offset, length + (endInclusive ? 1 : 0));
        }

        public IEnumerable<int> ToRange(int count, bool endInclusive = false)
        {
            (int offset, int length) = @this.GetOffsetAndLength(count);
            return Enumerable.Range(offset, length + (endInclusive ? 1 : 0));
        }
    }
}