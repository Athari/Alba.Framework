using System;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Alba.Framework.Attributes;

namespace Alba.Framework.Text
{
    public static class StringExts
    {
        private static readonly Regex _reNewlines = new Regex("@\r?\n", RegexOptions.Compiled);

        [Pure]
        public static string AppendSentence (this string @this, string sentence)
        {
            var sb = new StringBuilder(@this, @this.Length + sentence.Length + 2);
            sb.AppendSentence(sentence);
            return sb.ToString();
        }

        [Pure]
        public static bool IsNullOrEmpty (this string @this)
        {
            return string.IsNullOrEmpty(@this);
        }

        [Pure]
        public static bool IsNullOrWhitespace (this string @this)
        {
            return string.IsNullOrWhiteSpace(@this);
        }

        [Pure]
        public static string RemovePostfix (this string @this, string postfix)
        {
            if (!@this.EndsWith(postfix))
                throw new ArgumentException("String '{0}' does not contain postfix '{1}'.".Fmt(@this, postfix), "postfix");
            return @this.Remove(@this.Length - postfix.Length);
        }

        [Pure]
        public static string RemovePrefix (this string @this, string prefix)
        {
            if (!@this.StartsWith(prefix))
                throw new ArgumentException("String '{0}' does not contain prefix '{1}'.".Fmt(@this, prefix), "prefix");
            return @this.Substring(prefix.Length);
        }

        [Pure]
        public static string SingleLine (this string @this)
        {
            return _reNewlines.Replace(@this, " ");
        }

        [Pure]
        public static string SubstringEnd (this string @this, int length)
        {
            return @this.Length < length ? @this : @this.Substring(@this.Length - length, length);
        }

        public static void AppendSentence (this StringBuilder @this, string sentence)
        {
            @this.Append(@this[@this.Length - 1] == '.' ? " " : ". ");
            @this.Append(sentence);
        }

        [Pure, StringFormatMethod ("format")]
        public static string Fmt (this string format, IFormatProvider provider, params object[] args)
        {
            return string.Format(provider, format, args);
        }

        [Pure, StringFormatMethod ("format")]
        public static string Fmt (this string format, params object[] args)
        {
            return string.Format(format, args);
        }

        [Pure, StringFormatMethod ("format")]
        public static string FmtInvariant (this string format, params object[] args)
        {
            return string.Format(CultureInfo.InvariantCulture, format, args);
        }

        [Pure]
        public static bool Contains (this string @this, string value, StringComparison comparisonType)
        {
            return @this.IndexOf(value, comparisonType) >= 0;
        }

        [Pure]
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

        [Pure]
        public static string Sub (this string @this, int startIndex)
        {
            return startIndex >= 0
                ? @this.Substring(startIndex)
                : @this.Substring(@this.Length + startIndex);
        }
    }
}