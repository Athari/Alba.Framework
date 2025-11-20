using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Alba.Framework.Text;

public static partial class StringExts
{
    private static readonly CultureInfo Inv = CultureInfo.InvariantCulture;

    [GeneratedRegex(@"\r?\n", RegexOptions.Compiled)]
    private static partial Regex ReNewLines { get; }

    extension(string @this)
    {
        public string Q(char q) => $"{q}{@this}{q}";
        public string Q(char ql, char qr) => $"{ql}{@this}{qr}";
        public string Qg() => $"`{@this}`"; // grave
        public string Qs1() => $"'{@this}'"; // straight inner
        public string Qs2() => $"\"{@this}\""; // straight outer
        public string Qt1() => $"‘{@this}’"; // typographic inner
        public string Qt2() => $"“{@this}”"; // typographic outer
        public string Qtg1() => $"‚{@this}‘"; // typographic german inner
        public string Qtg2() => $"„{@this}“"; // typographic german outer
        public string Qa1() => $"‹{@this}›"; // angle inner (french)
        public string Qa2() => $"«{@this}»"; // angle outer (french)
        public string Qai1() => $"›{@this}‹"; // angle inverted inner
        public string Qai2() => $"»{@this}«"; // angle inverted outer
        public string Qc1() => $"「{@this}」"; // corner inner (chinese primary)
        public string Qc2() => $"『{@this}』"; // corner outer (chinese secondary)
        public string Qch1() => $"｢{@this}｣"; // corner half-width inner
        public string Qch2() => $"『{@this}』"; // corner half-width outer
        public string Qk1() => $"〈{@this}〉"; // chinese book / korean inner
        public string Qk2() => $"《{@this}》"; // chinese book / korean outer[Pure]

        [Pure]
        public string AppendSentence(string sentence)
        {
            var sb = new StringBuilder(@this, @this.Length + sentence.Length + 2);
            sb.AppendSentence(sentence);
            return sb.ToString();
        }

        [Pure]
        public string NormalizeWhitespace(string? nl = null) =>
            ReNewLines.Replace(@this, nl ?? Environment.NewLine);

        [Pure]
        public string RemovePostfix(string postfix,
            StringComparison comparison = StringComparison.InvariantCulture) =>
            !@this.EndsWith(postfix, comparison)
                ? throw new ArgumentException($"String '{@this}' does not contain postfix '{postfix}'.", nameof(postfix))
                : @this[..^postfix.Length];

        [Pure]
        public string RemovePostfixSafe(string postfix,
            StringComparison comparison = StringComparison.InvariantCulture) =>
            @this.EndsWith(postfix, comparison) ? @this[..^postfix.Length] : @this;

        [Pure]
        public string RemovePrefix(string prefix,
            StringComparison comparison = StringComparison.InvariantCulture) =>
            !@this.StartsWith(prefix, comparison)
                ? throw new ArgumentException($"String '{@this}' does not contain prefix '{prefix}'.", nameof(prefix))
                : @this[prefix.Length..];

        [Pure]
        public string RemovePrefixSafe(string prefix,
            StringComparison comparison = StringComparison.InvariantCulture) =>
            @this.StartsWith(prefix, comparison) ? @this[prefix.Length..] : @this;

        [Pure]
        public string RemoveSuffixes(string prefix, string postfix,
            StringComparison comparison = StringComparison.InvariantCulture)
        {
            if (!@this.StartsWith(prefix, comparison))
                throw new ArgumentException($"String '{@this}' does not contain prefix '{prefix}'.", nameof(prefix));
            if (!@this.EndsWith(postfix, comparison))
                throw new ArgumentException($"String '{@this}' does not contain postfix '{postfix}'.", nameof(postfix));
            return @this[prefix.Length..^postfix.Length];
        }

        [Pure]
        public string RemoveSuffixesSafe(string prefix, string postfix,
            StringComparison comparison = StringComparison.InvariantCulture) =>
            @this.StartsWith(prefix, comparison) && @this.EndsWith(postfix, comparison)
                ? @this[prefix.Length..^postfix.Length] : @this;

        [Pure]
        public string SingleLine() =>
            ReNewLines.Replace(@this, " ");

        [Pure]
        public string SubstringEnd(int length) =>
            @this.Length < length ? @this : @this.Substring(@this.Length - length, length);

        [Pure]
        public void Split(out string s1, out string s2, params char[] separator)
        {
            string[] values = @this.Split(separator, 2);
            s1 = values[0];
            s2 = values[1];
        }

        [Pure]
        public string ToLowerInv() =>
            @this.ToLowerInvariant();

        [Pure]
        public string ToUpperInv() =>
            @this.ToUpperInvariant();

        [Pure]
        public string Sub(int startIndex, int length) =>
            startIndex >= 0
                ? length >= 0
                    ? @this.Substring(startIndex, length)
                    : @this.Substring(startIndex, @this.Length - startIndex + length)
                : length >= 0
                    ? @this.Substring(@this.Length + startIndex, length)
                    : @this.Substring(@this.Length + startIndex, @this.Length + startIndex + length);

        [Pure]
        public string Sub(int startIndex) =>
            startIndex >= 0 ? @this[startIndex..] : @this[(@this.Length + startIndex)..];

        [Pure]
        public byte[] ToBytes(Encoding? encoding = null) =>
            (encoding ?? Encodings.Utf16).GetBytes(@this);

        [Pure]
        public byte[] ToAsciiBytes() =>
            Encoding.ASCII.GetBytes(@this);

        [Pure]
        public byte[] ToUtf8Bytes() =>
            Encodings.Utf8.GetBytes(@this);

        [Pure]
        public byte[] ToUtf16Bytes() =>
            Encodings.Utf16.GetBytes(@this);

        [Pure]
        public string Indent(int indentLength) =>
            @this.ReReplace("(?m)^(.*)$", new string(' ', indentLength) + "$1");

        [Pure]
        public string Indent(string indentStr) =>
            @this.ReReplace("(?m)^(.*)$", indentStr.ReEscapeReplacement() + "$1");

        [Pure]
        public string Unindent() =>
            UnindentInternal(@this, @"[ \t]*");

        [Pure]
        public string Unindent(int indentLength) =>
            UnindentInternal(@this, $@"[ \t]{{{indentLength}}}");

        [Pure]
        public string Unindent(string indentStr) =>
            UnindentInternal(@this, indentStr.ReEscape());

        [Pure]
        private string UnindentInternal(string indentPattern) =>
            @this.ReReplace($"(?m)^{indentPattern}(.*)$", "$1");
    }

    extension(string? @this)
    {
        // TODO Add TryParse methods for the rest of types (see Parse class)

        [Pure]
        public int? TryParseInt32() =>
            @this != null && int.TryParse(@this, NumberStyles.Integer, Inv, out var v) ? v : null;

        [Pure]
        public int? TryParseInt32(NumberStyles style) =>
            @this != null && int.TryParse(@this, style, Inv, out var v) ? v : null;

        [Pure, ContractAnnotation("null => null; notnull => canbenull")]
        public string? NullIfEmpty() =>
            @this.IsNullOrEmpty() ? null : @this;
    }

    extension([NotNullWhen(false)] string? @this)
    {
        [Pure, ContractAnnotation("null => true")]
        public bool IsNullOrEmpty() =>
            string.IsNullOrEmpty(@this);

        [Pure, ContractAnnotation("null => true")]
        public bool IsNullOrWhitespace() =>
            string.IsNullOrWhiteSpace(@this);
    }

    extension(string format)
    {
        [Pure, StringFormatMethod("format"), Obsolete("Use string interpolation")]
        public string Fmt(IFormatProvider provider, params object?[] args) =>
            string.Format(provider, format, args);

        [Pure, StringFormatMethod("format"), Obsolete("Use string interpolation")]
        public string Fmt(params object?[] args) =>
            string.Format(format, args);

        [Pure, StringFormatMethod("format"), Obsolete("Use string interpolation")]
        public string FmtInv(params object?[] args) =>
            string.Format(Inv, format, args);
    }
}