namespace Alba.Framework.Collections;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
public static partial class EnumerableExts
{
    extension(int @this)
    {
        public IEnumerable<int> Range() =>
            Enumerable.Range(0, @this);

        public IEnumerable<int> Range(int count) =>
            Enumerable.Range(@this, count);
    }

    extension(Range @this)
    {
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