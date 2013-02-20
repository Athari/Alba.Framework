using System;
using System.Text;
using System.Text.RegularExpressions;

// ReSharper disable LoopCanBeConvertedToQuery
// ReSharper disable ForCanBeConvertedToForeach
namespace Alba.Framework.Text
{
    internal class WildcardPatternToRegexParser : WildcardPatternParser
    {
        private const string RegexChars = @"()[.?*{}^$+|\";

        private RegexOptions regexOptions;
        private StringBuilder regexPattern;

        protected override void AppendAsterix ()
        {
            regexPattern.Append(".*");
        }

        protected override void AppendCharacterRangeToBracketExpression (char startOfCharacterRange, char endOfCharacterRange)
        {
            AppendCharacterRangeToBracketExpression(regexPattern, startOfCharacterRange, endOfCharacterRange);
        }

        internal static void AppendCharacterRangeToBracketExpression (StringBuilder regexPattern, char startOfCharacterRange, char endOfCharacterRange)
        {
            AppendLiteralCharacterToBracketExpression(regexPattern, startOfCharacterRange);
            regexPattern.Append('-');
            AppendLiteralCharacterToBracketExpression(regexPattern, endOfCharacterRange);
        }

        protected override void AppendLiteralCharacter (char c)
        {
            AppendLiteralCharacter(regexPattern, c);
        }

        internal static void AppendLiteralCharacter (StringBuilder regexPattern, char c)
        {
            if (IsRegexChar(c))
                regexPattern.Append('\\');
            regexPattern.Append(c);
        }

        protected override void AppendLiteralCharacterToBracketExpression (char c)
        {
            AppendLiteralCharacterToBracketExpression(regexPattern, c);
        }

        internal static void AppendLiteralCharacterToBracketExpression (StringBuilder regexPattern, char c)
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

        protected override void AppendQuestionMark ()
        {
            regexPattern.Append('.');
        }

        protected override void BeginBracketExpression ()
        {
            regexPattern.Append('[');
        }

        protected override void BeginWildcardPattern (WildcardPattern pattern)
        {
            regexPattern = new StringBuilder((pattern.Pattern.Length * 2) + 2);
            regexPattern.Append('^');
            regexOptions = TranslateWildcardOptionsIntoRegexOptions(pattern.Options);
        }

        protected override void EndBracketExpression ()
        {
            regexPattern.Append(']');
        }

        protected override void EndWildcardPattern ()
        {
            regexPattern.Append('$');
            string str = regexPattern.ToString();
            if (str.Equals("^.*$", StringComparison.Ordinal))
                regexPattern.Remove(0, 4);
            else {
                if (str.StartsWith("^.*", StringComparison.Ordinal))
                    regexPattern.Remove(0, 3);
                if (str.EndsWith(".*$", StringComparison.Ordinal))
                    regexPattern.Remove(regexPattern.Length - 3, 3);
            }
        }

        private static bool IsRegexChar (char ch)
        {
            for (int i = 0; i < RegexChars.Length; i++)
                if (ch == RegexChars[i])
                    return true;
            return false;
        }

        public static Regex Parse (WildcardPattern wildcardPattern)
        {
            Regex regex;
            var parser = new WildcardPatternToRegexParser();
            Parse(wildcardPattern, parser);
            try {
                regex = new Regex(parser.regexPattern.ToString(), parser.regexOptions);
            }
            catch (ArgumentException) {
                throw NewWildcardPatternException(wildcardPattern.Pattern);
            }
            return regex;
        }

        internal static RegexOptions TranslateWildcardOptionsIntoRegexOptions (WildcardOptions options)
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
}