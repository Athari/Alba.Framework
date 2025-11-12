namespace Alba.Framework.Text;

[PublicAPI]
public static class StringComparisonExts
{
    extension(StringComparison @this)
    {
        public StringComparer ToComparer() => StringComparer.FromComparison(@this);

        public NaturalStringComparer ToNaturalComparer() => NaturalStringComparer.FromComparison(@this);
    }
}