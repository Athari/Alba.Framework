using System.Text;

namespace Alba.Framework.Text;

internal abstract class WildcardPatternParser
{
    protected abstract void AppendAsterix();
    protected abstract void AppendCharacterRangeToBracketExpression(char startOfCharacterRange, char endOfCharacterRange);
    protected abstract void AppendLiteralCharacter(char c);
    protected abstract void AppendLiteralCharacterToBracketExpression(char c);
    protected abstract void AppendQuestionMark();
    protected abstract void BeginBracketExpression();
    protected abstract void EndBracketExpression();

    internal void AppendBracketExpression(string brackedExpressionContents, string bracketExpressionOperators, string pattern)
    {
        BeginBracketExpression();
        int i = 0;
        while (i < brackedExpressionContents.Length) {
            if (i + 2 < brackedExpressionContents.Length && bracketExpressionOperators[i + 1] == '-') {
                char startOfCharacterRange = brackedExpressionContents[i];
                char endOfCharacterRange = brackedExpressionContents[i + 2];
                i += 3;
                if (startOfCharacterRange > endOfCharacterRange)
                    throw NewWildcardPatternException(pattern);
                AppendCharacterRangeToBracketExpression(startOfCharacterRange, endOfCharacterRange);
            }
            else {
                AppendLiteralCharacterToBracketExpression(brackedExpressionContents[i]);
                i++;
            }
        }
        EndBracketExpression();
    }

    protected virtual void BeginWildcardPattern(WildcardPattern pattern) { }

    protected virtual void EndWildcardPattern() { }

    protected static WildcardPatternException NewWildcardPatternException(string invalidPattern)
    {
        return new($"The specified wildcard pattern is not valid: {invalidPattern}");
    }

    public static void Parse(WildcardPattern pattern, WildcardPatternParser parser)
    {
        parser.BeginWildcardPattern(pattern);
        bool isEscaped = false, isBracketExpr = false, isBracketExprStart = false;
        StringBuilder? sbBracketContents = null, sbBracketOperators = null;
        foreach (char ch in pattern.Pattern) {
            if (isBracketExpr) {
                if (ch == ']' && !isBracketExprStart && !isEscaped) {
                    isBracketExpr = false;
                    parser.AppendBracketExpression(sbBracketContents?.ToString() ?? "", sbBracketOperators?.ToString() ?? "", pattern.Pattern);
                    sbBracketContents = null;
                    sbBracketOperators = null;
                }
                else if (ch != WildcardPattern.EscapeChar || isEscaped) {
                    sbBracketContents!.Append(ch);
                    sbBracketOperators!.Append(ch == '-' && !isEscaped ? '-' : ' ');
                }
                isBracketExprStart = false;
            }
            else if (ch == '*' && !isEscaped) {
                parser.AppendAsterix();
            }
            else if (ch == '?' && !isEscaped) {
                parser.AppendQuestionMark();
            }
            else if (ch == '[' && !isEscaped) {
                isBracketExpr = true;
                isBracketExprStart = true;
                sbBracketContents = new();
                sbBracketOperators = new();
            }
            else if (ch != WildcardPattern.EscapeChar || isEscaped) {
                parser.AppendLiteralCharacter(ch);
            }
            isEscaped = ch == WildcardPattern.EscapeChar && !isEscaped;
        }
        if (isBracketExpr)
            throw NewWildcardPatternException(pattern.Pattern);
        if (isEscaped && !pattern.Pattern.Equals(new(WildcardPattern.EscapeChar, 1), StringComparison.Ordinal))
            parser.AppendLiteralCharacter(pattern.Pattern[^1]);
        parser.EndWildcardPattern();
    }
}