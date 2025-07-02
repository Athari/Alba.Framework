using System.Text;
using System.Text.RegularExpressions;

namespace Alba.Framework.Text;

internal class WildcardPatternToRegexParser : WildcardPatternParser
{
    private const string RegexChars = @"()[.?*{}^$+|\";

    private RegexOptions _regexOptions;
    private StringBuilder _regexPattern = null!;

    protected override void AppendAsterix()
    {
        _regexPattern.Append(".*");
    }

    protected override void AppendCharacterRangeToBracketExpression(char startOfCharacterRange, char endOfCharacterRange)
    {
        AppendCharacterRangeToBracketExpression(_regexPattern, startOfCharacterRange, endOfCharacterRange);
    }

    internal static void AppendCharacterRangeToBracketExpression(StringBuilder regexPattern, char startOfCharacterRange, char endOfCharacterRange)
    {
        AppendLiteralCharacterToBracketExpression(regexPattern, startOfCharacterRange);
        regexPattern.Append('-');
        AppendLiteralCharacterToBracketExpression(regexPattern, endOfCharacterRange);
    }

    protected override void AppendLiteralCharacter(char c)
    {
        AppendLiteralCharacter(_regexPattern, c);
    }

    internal static void AppendLiteralCharacter(StringBuilder regexPattern, char c)
    {
        if (IsRegexChar(c))
            regexPattern.Append('\\');
        regexPattern.Append(c);
    }

    protected override void AppendLiteralCharacterToBracketExpression(char c)
    {
        AppendLiteralCharacterToBracketExpression(_regexPattern, c);
    }

    internal static void AppendLiteralCharacterToBracketExpression(StringBuilder regexPattern, char c)
    {
        if (c == '[')
            regexPattern.Append('[');
        else if (c == ']')
            regexPattern.Append(@"\]");
        else if (c == '-')
            regexPattern.Append(@"\x2d");
        else
            AppendLiteralCharacter(regexPattern, c);
    }

    protected override void AppendQuestionMark()
    {
        _regexPattern.Append('.');
    }

    protected override void BeginBracketExpression()
    {
        _regexPattern.Append('[');
    }

    protected override void BeginWildcardPattern(WildcardPattern pattern)
    {
        _regexPattern = new StringBuilder((pattern.Pattern.Length * 2) + 2);
        _regexPattern.Append('^');
        _regexOptions = TranslateWildcardOptionsIntoRegexOptions(pattern.Options);
    }

    protected override void EndBracketExpression()
    {
        _regexPattern.Append(']');
    }

    protected override void EndWildcardPattern()
    {
        _regexPattern.Append('$');
        string str = _regexPattern.ToString();
        if (str.Equals("^.*$", StringComparison.Ordinal))
            _regexPattern.Remove(0, 4);
        else {
            if (str.StartsWith("^.*", StringComparison.Ordinal))
                _regexPattern.Remove(0, 3);
            if (str.EndsWith(".*$", StringComparison.Ordinal))
                _regexPattern.Remove(_regexPattern.Length - 3, 3);
        }
    }

    private static bool IsRegexChar(char ch)
    {
        for (int i = 0; i < RegexChars.Length; i++)
            if (ch == RegexChars[i])
                return true;
        return false;
    }

    public static Regex Parse(WildcardPattern wildcardPattern)
    {
        Regex regex;
        var parser = new WildcardPatternToRegexParser();
        Parse(wildcardPattern, parser);
        try {
            regex = new Regex(parser._regexPattern.ToString(), parser._regexOptions);
        }
        catch (ArgumentException) {
            throw NewWildcardPatternException(wildcardPattern.Pattern);
        }
        return regex;
    }

    internal static RegexOptions TranslateWildcardOptionsIntoRegexOptions(WildcardOptions options)
    {
        var singleline = RegexOptions.Singleline;
        if ((options & WildcardOptions.Compiled) != WildcardOptions.None)
            singleline |= RegexOptions.Compiled;
        if ((options & WildcardOptions.IgnoreCase) != WildcardOptions.None)
            singleline |= RegexOptions.IgnoreCase;
        if ((options & WildcardOptions.CultureInvariant) == WildcardOptions.CultureInvariant)
            singleline |= RegexOptions.CultureInvariant;
        return singleline;
    }
}