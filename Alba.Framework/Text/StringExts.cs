using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Alba.Framework.Text
{
    public static class StringExts
    {
        private static readonly Regex _reNewlines = new Regex("@\r?\n", RegexOptions.Compiled);

        public static string AppendSentence (this string @this, string sentence)
        {
            var sb = new StringBuilder(@this, @this.Length + sentence.Length + 2);
            sb.AppendSentence(sentence);
            return sb.ToString();
        }

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

        public static string SingleLine (this string @this)
        {
            return _reNewlines.Replace(@this, " ");
        }

        public static string SubstringEnd (this string @this, int length)
        {
            return @this.Length < length ? @this : @this.Substring(@this.Length - length, length);
        }

        public static void AppendSentence (this StringBuilder @this, string sentence)
        {
            @this.Append(@this[@this.Length - 1] == '.' ? " " : ". ");
            @this.Append(sentence);
        }

        public static string Format (this string format, IFormatProvider provider, params object[] args)
        {
            return string.Format(provider, format, args);
        }

        public static string Format (this string format, params object[] args)
        {
            return string.Format(format, args);
        }

        public static string FormatInvariant (this string format, params object[] args)
        {
            return string.Format(CultureInfo.InvariantCulture, format, args);
        }

        public static bool Contains (this string @this, string value, StringComparison comparisonType)
        {
            return @this.IndexOf(value, comparisonType) >= 0;
        }

        public static string Sub (this string @this, int startIndex, int length)
        {
            return startIndex >= 0
                ? (length >= 0
                    ? @this.Substring(startIndex, length)
                    : @this.Substring(startIndex, @this.Length - startIndex + length))
                : (length >= 0
                    ? @this.Substring(@this.Length + startIndex, length)
                    : @this.Substring(@this.Length + startIndex, @this.Length + startIndex + length));
        }

        public static string Sub (this string @this, int startIndex)
        {
            return startIndex >= 0
                ? @this.Substring(startIndex)
                : @this.Substring(@this.Length + startIndex);
        }
    }
}