using System;

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

        public static string RemovePostfix (this string @this, string postfix)
        {
            if (!@this.EndsWith(postfix))
                throw new ArgumentException("string does not contain postfix", "postfix");
            return @this.Remove(@this.Length - postfix.Length);
        }
    }
}