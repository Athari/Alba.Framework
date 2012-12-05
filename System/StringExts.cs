namespace Alba.Framework.System
{
    public static class StringExts
    {
        public static bool IsNullOrEmpty (this string @this)
        {
            return string.IsNullOrEmpty(@this);
        }

        public static bool IsNullOrWhitespace (this string @this)
        {
            return string.IsNullOrWhiteSpace(@this);
        }
    }
}