using System.Globalization;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Alba.Framework.Text;

[PublicAPI]
public static partial class StringExts
{
    [GeneratedRegex(@"\r?\n", RegexOptions.Compiled)]
    private static partial Regex ReNewLines();

    [Pure]
    public static string AppendSentence(this string @this, string sentence)
    {
        var sb = new StringBuilder(@this, @this.Length + sentence.Length + 2);
        sb.AppendSentence(sentence);
        return sb.ToString();
    }

    [Pure]
    public static bool IsNullOrEmpty(this string? @this) =>
        string.IsNullOrEmpty(@this);

    [Pure]
    public static bool IsNullOrWhitespace(this string? @this) =>
        string.IsNullOrWhiteSpace(@this);

    [Pure]
    public static string NormalizeWhitespace(this string @this, string? nl = null) =>
        ReNewLines().Replace(@this, nl ?? Environment.NewLine);

    [Pure]
    public static string RemovePostfix(this string @this, string postfix) =>
        !@this.EndsWith(postfix)
            ? throw new ArgumentException($"String '{@this}' does not contain postfix '{postfix}'.", nameof(postfix))
            : @this.Remove(@this.Length - postfix.Length);

    [Pure]
    public static string RemovePostfixSafe(this string @this, string postfix) =>
        @this.EndsWith(postfix) ? @this.Remove(@this.Length - postfix.Length) : @this;

    [Pure]
    public static string RemovePrefix(this string @this, string prefix) =>
        !@this.StartsWith(prefix)
            ? throw new ArgumentException($"String '{@this}' does not contain prefix '{prefix}'.", nameof(prefix))
            : @this[prefix.Length..];

    [Pure]
    public static string RemovePrefixSafe(this string @this, string prefix) =>
        @this.StartsWith(prefix) ? @this[prefix.Length..] : @this;

    [Pure]
    public static string RemoveSuffixes(this string @this, string prefix, string postfix)
    {
        if (!@this.StartsWith(prefix))
            throw new ArgumentException($"String '{@this}' does not contain prefix '{prefix}'.", nameof(prefix));
        if (!@this.EndsWith(postfix))
            throw new ArgumentException($"String '{@this}' does not contain postfix '{postfix}'.", nameof(postfix));
        return @this.Substring(prefix.Length, @this.Length - prefix.Length - postfix.Length);
    }

    [Pure]
    public static string RemoveSuffixesSafe(this string @this, string prefix, string postfix) =>
        @this.StartsWith(prefix) && @this.EndsWith(postfix)
            ? @this.Substring(prefix.Length, @this.Length - prefix.Length - postfix.Length) : @this;

    [Pure]
    public static string SingleLine(this string @this) =>
        ReNewLines().Replace(@this, " ");

    [Pure]
    public static string SubstringEnd(this string @this, int length) =>
        @this.Length < length ? @this : @this.Substring(@this.Length - length, length);

    public static void Split(this string @this, out string s1, out string s2, params char[] separator)
    {
        string[] values = @this.Split(separator, 2);
        s1 = values[0];
        s2 = values[1];
    }

    public static void AppendSentence(this StringBuilder @this, string sentence)
    {
        if (@this.Length > 0)
            @this.Append(@this[^1] == '.' ? " " : ". ");
        @this.Append(sentence);
    }

    [Pure, StringFormatMethod("format"), Obsolete("Use string interpolation")]
    public static string Fmt(this string format, IFormatProvider provider, params object?[] args) =>
        string.Format(provider, format, args);

    [Pure, StringFormatMethod("format"), Obsolete("Use string interpolation")]
    public static string Fmt(this string format, params object?[] args) =>
        string.Format(format, args);

    [Pure, StringFormatMethod("format"), Obsolete("Use string interpolation")]
    public static string FmtInv(this string format, params object?[] args) =>
        string.Format(CultureInfo.InvariantCulture, format, args);

    [Pure]
    public static bool EqualsCulture(this string @this, string other) =>
        @this.Equals(other, StringComparison.CurrentCulture);

    [Pure]
    public static bool EqualsInv(this string @this, string other) =>
        @this.Equals(other, StringComparison.InvariantCulture);

    [Pure]
    public static bool EqualsOrd(this string @this, string other) =>
        @this.Equals(other, StringComparison.Ordinal);

    [Pure]
    public static bool EqualsCaseCulture(this string @this, string other) =>
        @this.Equals(other, StringComparison.CurrentCultureIgnoreCase);

    [Pure]
    public static bool EqualsCaseInv(this string @this, string other) =>
        @this.Equals(other, StringComparison.InvariantCultureIgnoreCase);

    [Pure]
    public static bool EqualsCaseOrd(this string @this, string other) =>
        @this.Equals(other, StringComparison.OrdinalIgnoreCase);

    [Pure]
    public static string ReEscape(this string @this) =>
        Regex.Escape(@this);

    [Pure]
    public static string ReEscapeReplacement(this string @this) =>
        @this.Replace("$", "$$");

    [Pure]
    public static bool IsReMatch(this string @this, string pattern, RegexOptions options = RegexOptions.None) =>
        Regex.IsMatch(@this, pattern, options);

    [Pure]
    public static bool IsReMatch(this string @this, Regex regex) =>
        regex.IsMatch(@this);

    [Pure]
    public static Match ReMatch(this string @this, string pattern, RegexOptions options = RegexOptions.None) =>
        Regex.Match(@this, pattern, options);

    [Pure]
    public static string ReMatchGet(this string @this, string pattern, int groupNum = 1, RegexOptions options = RegexOptions.None) =>
        Regex.Match(@this, pattern, options).Get(groupNum);

    [Pure]
    public static string ReMatchGet(this string @this, string pattern, string groupName, RegexOptions options = RegexOptions.None) =>
        Regex.Match(@this, pattern, options).Get(groupName);

    [Pure]
    public static Match ReMatch(this string @this, Regex regex) =>
        regex.Match(@this);

    [Pure]
    public static string ReMatchGet(this string @this, Regex regex, int groupNum = 1) =>
        regex.Match(@this).Get(groupNum);

    [Pure]
    public static string ReMatchGet(this string @this, Regex regex, string groupName) =>
        regex.Match(@this).Get(groupName);

    [Pure]
    public static MatchCollection ReMatches(this string @this, string pattern, RegexOptions options = RegexOptions.None) =>
        Regex.Matches(@this, pattern, options);

    [Pure]
    public static IEnumerable<string> ReMatchesGet(this string @this, string pattern, int groupNum = 1, RegexOptions options = RegexOptions.None) =>
        Regex.Matches(@this, pattern, options).Get(groupNum);

    [Pure]
    public static IEnumerable<string> ReMatchesGet(this string @this, string pattern, string groupName, RegexOptions options = RegexOptions.None) =>
        Regex.Matches(@this, pattern, options).Get(groupName);

    [Pure]
    public static MatchCollection ReMatches(this string @this, Regex regex) =>
        regex.Matches(@this);

    [Pure]
    public static IEnumerable<string> ReMatchesGet(this string @this, Regex regex, int groupNum = 1) =>
        regex.Matches(@this).Get(groupNum);

    [Pure]
    public static IEnumerable<string> ReMatchesGet(this string @this, Regex regex, string groupName) =>
        regex.Matches(@this).Get(groupName);

    [Pure]
    public static string ReReplace(this string @this, string pattern, string replacement, RegexOptions options = RegexOptions.None) =>
        Regex.Replace(@this, pattern, replacement, options);

    [Pure]
    public static string ReReplace(this string @this, Regex regex, string replacement) =>
        regex.Replace(@this, replacement);

    [Pure]
    public static string ReReplace(this string @this, string pattern, MatchEvaluator evaluator, RegexOptions options = RegexOptions.None) =>
        Regex.Replace(@this, pattern, evaluator, options);

    [Pure]
    public static string ReReplace(this string @this, Regex regex, MatchEvaluator evaluator) =>
        regex.Replace(@this, evaluator);

    [Pure]
    public static string[] ReSplit(this string @this, string pattern, RegexOptions options = RegexOptions.None) =>
        Regex.Split(@this, pattern, options);

    [Pure]
    public static string[] ReSplit(this string @this, Regex regex) =>
        regex.Split(@this);

    [Pure]
    public static string ReUnescape(this string @this) =>
        Regex.Unescape(@this);

    [Pure]
    public static string Sub(this string @this, int startIndex, int length) =>
        startIndex >= 0
            ? length >= 0
                ? @this.Substring(startIndex, length)
                : @this.Substring(startIndex, @this.Length - startIndex + length)
            : length >= 0
                ? @this.Substring(@this.Length + startIndex, length)
                : @this.Substring(@this.Length + startIndex, @this.Length + startIndex + length);

    [Pure]
    public static string Sub(this string @this, int startIndex) =>
        startIndex >= 0 ? @this[startIndex..] : @this[(@this.Length + startIndex)..];

    [Pure]
    public static byte[] ToBytes(this string @this, Encoding? encoding = null) =>
        (encoding ?? Encodings.Utf16).GetBytes(@this);

    [Pure]
    public static byte[] ToAsciiBytes(this string @this) =>
        Encoding.ASCII.GetBytes(@this);

    [Pure]
    public static byte[] ToUtf8Bytes(this string @this) =>
        Encodings.Utf8.GetBytes(@this);

    [Pure]
    public static byte[] ToUtf16Bytes(this string @this) =>
        Encodings.Utf16.GetBytes(@this);

    [Pure]
    public static string Indent(this string @this, int indentLength) =>
        @this.ReReplace("(?m)^(.*)$", new string(' ', indentLength) + "$1");

    [Pure]
    public static string Indent(this string @this, string indentStr) =>
        @this.ReReplace("(?m)^(.*)$", indentStr.ReEscapeReplacement() + "$1");

    [Pure]
    public static string Unindent(this string @this) =>
        UnindentInternal(@this, @"[ \t]*");

    [Pure]
    public static string Unindent(this string @this, int indentLength) =>
        UnindentInternal(@this, $@"[ \t]{{{indentLength}}}");

    [Pure]
    public static string Unindent(this string @this, string indentStr) =>
        UnindentInternal(@this, indentStr.ReEscape());

    private static string UnindentInternal(this string @this, string indentPattern) =>
        @this.ReReplace($"(?m)^{indentPattern}(.*)$", "$1");

    public static string HtmlEncode(this string @this) =>
        WebUtility.HtmlEncode(@this);

    public static string HtmlDecode(this string @this) =>
        WebUtility.HtmlDecode(@this);

    public static string UrlEncode(this string @this) =>
        WebUtility.UrlEncode(@this);

    public static string UrlDecode(this string @this) =>
        WebUtility.UrlDecode(@this);
}