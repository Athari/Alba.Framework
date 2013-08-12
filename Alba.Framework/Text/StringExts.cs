using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using Alba.Framework.Attributes;

namespace Alba.Framework.Text
{
    public static class StringExts
    {
        private static readonly Regex ReNewlines = new Regex(@"\r?\n", RegexOptions.Compiled);

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
        public static string NormalizeWhitespace (this string @this, string nl = null)
        {
            if (nl == null)
                nl = Environment.NewLine;
            return ReNewlines.Replace(@this, nl);
        }

        [Pure]
        public static string RemovePostfix (this string @this, string postfix)
        {
            if (!@this.EndsWith(postfix))
                throw new ArgumentException("String '{0}' does not contain postfix '{1}'.".Fmt(@this, postfix), "postfix");
            return @this.Remove(@this.Length - postfix.Length);
        }

        [Pure]
        public static string RemovePostfixSafe (this string @this, string postfix)
        {
            return @this.EndsWith(postfix) ? @this.Remove(@this.Length - postfix.Length) : @this;
        }

        [Pure]
        public static string RemovePrefix (this string @this, string prefix)
        {
            if (!@this.StartsWith(prefix))
                throw new ArgumentException("String '{0}' does not contain prefix '{1}'.".Fmt(@this, prefix), "prefix");
            return @this.Substring(prefix.Length);
        }

        [Pure]
        public static string RemovePrefixSafe (this string @this, string prefix)
        {
            return @this.StartsWith(prefix) ? @this.Substring(prefix.Length) : @this;
        }

        [Pure]
        public static string SingleLine (this string @this)
        {
            return ReNewlines.Replace(@this, " ");
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
        public static string FmtInv (this string format, params object[] args)
        {
            return string.Format(CultureInfo.InvariantCulture, format, args);
        }

        [Pure]
        public static bool Contains (this string @this, char value)
        {
            return @this.IndexOf(value) >= 0;
        }

        [Pure]
        public static bool Contains (this string @this, char value, StringComparison comparisonType)
        {
            return @this.IndexOf(new string(value, 1), comparisonType) >= 0;
        }

        [Pure]
        public static bool Contains (this string @this, string value, StringComparison comparisonType)
        {
            return @this.IndexOf(value, comparisonType) >= 0;
        }

        [Pure]
        public static bool EqualsCulture (this string @this, string other)
        {
            return @this.Equals(other, StringComparison.CurrentCulture);
        }

        [Pure]
        public static bool EqualsInv (this string @this, string other)
        {
            return @this.Equals(other, StringComparison.InvariantCulture);
        }

        [Pure]
        public static bool EqualsOrd (this string @this, string other)
        {
            return @this.Equals(other, StringComparison.Ordinal);
        }

        [Pure]
        public static bool EqualsCaseCulture (this string @this, string other)
        {
            return @this.Equals(other, StringComparison.CurrentCultureIgnoreCase);
        }

        [Pure]
        public static bool EqualsCaseInv (this string @this, string other)
        {
            return @this.Equals(other, StringComparison.InvariantCultureIgnoreCase);
        }

        [Pure]
        public static bool EqualsCaseOrd (this string @this, string other)
        {
            return @this.Equals(other, StringComparison.OrdinalIgnoreCase);
        }

        [Pure]
        public static string ReEscape (this string @this)
        {
            return Regex.Escape(@this);
        }

        [Pure]
        public static string ReEscapeReplacement (this string @this)
        {
            return @this.Replace("$", "$$");
        }

        [Pure]
        public static bool IsReMatch (this string @this, string pattern, RegexOptions options = RegexOptions.None)
        {
            return Regex.IsMatch(@this, pattern, options);
        }

        [Pure]
        public static bool IsReMatch (this string @this, Regex regex)
        {
            return regex.IsMatch(@this);
        }

        [Pure]
        public static Match ReMatch (this string @this, string pattern, RegexOptions options = RegexOptions.None)
        {
            return Regex.Match(@this, pattern, options);
        }

        [Pure]
        public static string ReMatchGet (this string @this, string pattern, int groupNum = 1, RegexOptions options = RegexOptions.None)
        {
            return Regex.Match(@this, pattern, options).Get(groupNum);
        }

        [Pure]
        public static string ReMatchGet (this string @this, string pattern, string groupName, RegexOptions options = RegexOptions.None)
        {
            return Regex.Match(@this, pattern, options).Get(groupName);
        }

        [Pure]
        public static Match ReMatch (this string @this, Regex regex)
        {
            return regex.Match(@this);
        }

        [Pure]
        public static string ReMatchGet (this string @this, Regex regex, int groupNum = 1)
        {
            return regex.Match(@this).Get(groupNum);
        }

        [Pure]
        public static string ReMatchGet (this string @this, Regex regex, string groupName)
        {
            return regex.Match(@this).Get(groupName);
        }

        [Pure]
        public static MatchCollection ReMatches (this string @this, string pattern, RegexOptions options = RegexOptions.None)
        {
            return Regex.Matches(@this, pattern, options);
        }

        [Pure]
        public static IEnumerable<string> ReMatchesGet (this string @this, string pattern, int groupNum = 1, RegexOptions options = RegexOptions.None)
        {
            return Regex.Matches(@this, pattern, options).Get(groupNum);
        }

        [Pure]
        public static IEnumerable<string> ReMatchesGet (this string @this, string pattern, string groupName, RegexOptions options = RegexOptions.None)
        {
            return Regex.Matches(@this, pattern, options).Get(groupName);
        }

        [Pure]
        public static MatchCollection ReMatches (this string @this, Regex regex)
        {
            return regex.Matches(@this);
        }

        [Pure]
        public static IEnumerable<string> ReMatchesGet (this string @this, Regex regex, int groupNum = 1)
        {
            return regex.Matches(@this).Get(groupNum);
        }

        [Pure]
        public static IEnumerable<string> ReMatchesGet (this string @this, Regex regex, string groupName)
        {
            return regex.Matches(@this).Get(groupName);
        }

        [Pure]
        public static string ReReplace (this string @this, string pattern, string replacement, RegexOptions options = RegexOptions.None)
        {
            return Regex.Replace(@this, pattern, replacement, options);
        }

        [Pure]
        public static string ReReplace (this string @this, Regex regex, string replacement)
        {
            return regex.Replace(@this, replacement);
        }

        [Pure]
        public static string ReReplace (this string @this, string pattern, MatchEvaluator evaluator, RegexOptions options = RegexOptions.None)
        {
            return Regex.Replace(@this, pattern, evaluator, options);
        }

        [Pure]
        public static string ReReplace (this string @this, Regex regex, MatchEvaluator evaluator)
        {
            return regex.Replace(@this, evaluator);
        }

        [Pure]
        public static string[] ReSplit (this string @this, string pattern, RegexOptions options = RegexOptions.None)
        {
            return Regex.Split(@this, pattern, options);
        }

        [Pure]
        public static string[] ReSplit (this string @this, Regex regex)
        {
            return regex.Split(@this);
        }

        [Pure]
        public static string ReUnescape (this string @this)
        {
            return Regex.Unescape(@this);
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

        [Pure]
        public static byte[] ToBytes (this string @this, Encoding encoding = null)
        {
            return (encoding ?? Encodings.Utf16NoBom).GetBytes(@this);
        }

        [Pure]
        public static byte[] ToAsciiBytes (this string @this)
        {
            return Encoding.ASCII.GetBytes(@this);
        }

        [Pure]
        public static byte[] ToUtf8Bytes (this string @this)
        {
            return Encodings.Utf8NoBom.GetBytes(@this);
        }

        [Pure]
        public static byte[] ToUtf16Bytes (this string @this)
        {
            return Encodings.Utf16NoBom.GetBytes(@this);
        }

        [Pure]
        public static string Indent (this string @this, int indentLength)
        {
            return @this.ReReplace("(?m)^(.*)$", new string(' ', indentLength) + "$1");
        }

        [Pure]
        public static string Indent (this string @this, string indentStr)
        {
            return @this.ReReplace("(?m)^(.*)$", indentStr.ReEscapeReplacement() + "$1");
        }

        [Pure]
        public static string Unindent (this string @this)
        {
            return UnindentInternal(@this, @"\s*");
        }

        [Pure]
        public static string Unindent (this string @this, int indentLength)
        {
            return UnindentInternal(@this, @"\s{{{0}}}".FmtInv(indentLength));
        }

        [Pure]
        public static string Unindent (this string @this, string indentStr)
        {
            return UnindentInternal(@this, indentStr.ReEscape());
        }

        private static string UnindentInternal (this string @this, string indentPattern)
        {
            return @this.ReReplace("(?m)^{0}(.*)$".Fmt(indentPattern), "$1");
        }

        public static string HtmlEncode (this string @this)
        {
            return WebUtility.HtmlEncode(@this);
        }

        public static string HtmlDecode (this string @this)
        {
            return WebUtility.HtmlDecode(@this);
        }

        public static string UrlEncode (this string @this)
        {
            return WebUtility.UrlEncode(@this);
        }

        public static string UrlDecode (this string @this)
        {
            return WebUtility.UrlDecode(@this);
        }
    }
}