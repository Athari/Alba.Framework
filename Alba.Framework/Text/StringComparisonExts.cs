namespace Alba.Framework.Text;

public static class StringComparisonExts
{
    extension(StringComparison @this)
    {
        public StringComparer ToComparer() => StringComparer.FromComparison(@this);

        public NaturalStringComparer ToNaturalComparer() => NaturalStringComparer.FromComparison(@this);
    }
}