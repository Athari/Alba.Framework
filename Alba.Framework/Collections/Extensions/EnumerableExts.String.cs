namespace Alba.Framework.Collections;

public static partial class EnumerableExts
{
    extension<T>(IEnumerable<T> @this)
    {
        public string JoinString(string? separator) =>
            string.Join(separator, @this);

        public string JoinString(char separator) =>
            string.Join(separator, @this);

        public string ConcatString() =>
            string.Concat(@this);
    }

    extension(IEnumerable<string?> @this)
    {
        public string JoinString(string? separator) =>
            string.Join(separator, @this);

        public string ConcatString() =>
            string.Concat(@this);
    }
}