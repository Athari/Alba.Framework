using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Alba.Framework.Text;

internal class WildcardPatternMatcher
{
    private readonly CharacterNormalizer _characterNormalizer;
    private readonly PatternElement[] _patternElements;

    internal WildcardPatternMatcher(WildcardPattern wildcardPattern)
    {
        _characterNormalizer = new(wildcardPattern.Options);
        _patternElements = MyWildcardPatternParser.Parse(wildcardPattern, _characterNormalizer);
    }

    internal bool IsMatch(string str)
    {
        var builder = new StringBuilder(str.Length);
        foreach (char ch in str)
            builder.Append(_characterNormalizer.Normalize(ch));
        str = builder.ToString();
        var patternPositionsForCurrentStringPosition = new PatternPositionsVisitor(_patternElements.Length);
        patternPositionsForCurrentStringPosition.Add(0);
        var patternPositionsForNextStringPosition = new PatternPositionsVisitor(_patternElements.Length);
        for (int i = 0; i < str.Length; i++) {
            char currentStringCharacter = str[i];
            patternPositionsForCurrentStringPosition.StringPosition = i;
            patternPositionsForNextStringPosition.StringPosition = i + 1;
            while (patternPositionsForCurrentStringPosition.MoveNext(out int num2)) {
                _patternElements[num2].ProcessStringCharacter(currentStringCharacter, num2,
                    patternPositionsForCurrentStringPosition, patternPositionsForNextStringPosition);
            }
            (patternPositionsForCurrentStringPosition, patternPositionsForNextStringPosition) =
                (patternPositionsForNextStringPosition, patternPositionsForCurrentStringPosition);
        }
        while (patternPositionsForCurrentStringPosition.MoveNext(out int num3))
            _patternElements[num3].ProcessEndOfString(num3, patternPositionsForCurrentStringPosition);
        return patternPositionsForCurrentStringPosition.IsEndOfPattern;
    }

    private class CharacterNormalizer(WildcardOptions options)
    {
        private readonly bool _caseInsensitive = WildcardOptions.IgnoreCase == (options & WildcardOptions.IgnoreCase);
        private readonly CultureInfo _cultureInfo = WildcardOptions.CultureInvariant == (options & WildcardOptions.CultureInvariant)
            ? CultureInfo.InvariantCulture : CultureInfo.CurrentCulture;

        public char Normalize(char x)
        {
            return _caseInsensitive ? char.ToLower(x, _cultureInfo) : x;
        }
    }

    private abstract class PatternElement
    {
        public abstract void ProcessEndOfString(int currentPatternPosition, PatternPositionsVisitor patternPositionsForEndOfStringPosition);

        public abstract void ProcessStringCharacter(char currentStringCharacter, int currentPatternPosition,
            PatternPositionsVisitor patternPositionsForCurrentStringPosition, PatternPositionsVisitor patternPositionsForNextStringPosition);
    }

    private class AsterixElement : PatternElement
    {
        public override void ProcessEndOfString(int currentPatternPosition, PatternPositionsVisitor patternPositionsForEndOfStringPosition)
        {
            patternPositionsForEndOfStringPosition.Add(currentPatternPosition + 1);
        }

        public override void ProcessStringCharacter(char currentStringCharacter, int currentPatternPosition,
            PatternPositionsVisitor patternPositionsForCurrentStringPosition, PatternPositionsVisitor patternPositionsForNextStringPosition)
        {
            patternPositionsForCurrentStringPosition.Add(currentPatternPosition + 1);
            patternPositionsForNextStringPosition.Add(currentPatternPosition);
        }
    }

    private class QuestionMarkElement : PatternElement
    {
        public override void ProcessEndOfString(int currentPatternPosition, PatternPositionsVisitor patternPositionsForEndOfStringPosition) { }

        public override void ProcessStringCharacter(char currentStringCharacter, int currentPatternPosition,
            PatternPositionsVisitor patternPositionsForCurrentStringPosition, PatternPositionsVisitor patternPositionsForNextStringPosition)
        {
            patternPositionsForNextStringPosition.Add(currentPatternPosition + 1);
        }
    }

    private class LiteralCharacterElement(char literalCharacter) : QuestionMarkElement
    {
        public override void ProcessStringCharacter(char currentStringCharacter, int currentPatternPosition,
            PatternPositionsVisitor patternPositionsForCurrentStringPosition, PatternPositionsVisitor patternPositionsForNextStringPosition)
        {
            if (literalCharacter == currentStringCharacter) {
                base.ProcessStringCharacter(currentStringCharacter, currentPatternPosition,
                    patternPositionsForCurrentStringPosition, patternPositionsForNextStringPosition);
            }
        }
    }

    private class BracketExpressionElement(Regex regex) : QuestionMarkElement
    {
        public override void ProcessStringCharacter(char currentStringCharacter, int currentPatternPosition,
            PatternPositionsVisitor patternPositionsForCurrentStringPosition, PatternPositionsVisitor patternPositionsForNextStringPosition)
        {
            if (regex.IsMatch(new(currentStringCharacter, 1))) {
                base.ProcessStringCharacter(currentStringCharacter, currentPatternPosition,
                    patternPositionsForCurrentStringPosition, patternPositionsForNextStringPosition);
            }
        }
    }

    private class MyWildcardPatternParser : WildcardPatternParser
    {
        private StringBuilder _bracketExpressionBuilder = null!;
        private CharacterNormalizer _characterNormalizer = null!;
        private readonly List<PatternElement> _patternElements = [ ];
        private RegexOptions _regexOptions;

        protected override void AppendAsterix()
        {
            _patternElements.Add(new AsterixElement());
        }

        protected override void AppendQuestionMark()
        {
            _patternElements.Add(new QuestionMarkElement());
        }

        protected override void AppendLiteralCharacter(char c)
        {
            c = _characterNormalizer.Normalize(c);
            _patternElements.Add(new LiteralCharacterElement(c));
        }

        protected override void AppendCharacterRangeToBracketExpression(char startOfCharacterRange, char endOfCharacterRange)
        {
            WildcardPatternToRegexParser.AppendCharacterRangeToBracketExpression(_bracketExpressionBuilder, startOfCharacterRange, endOfCharacterRange);
        }

        protected override void AppendLiteralCharacterToBracketExpression(char c)
        {
            WildcardPatternToRegexParser.AppendLiteralCharacterToBracketExpression(_bracketExpressionBuilder, c);
        }

        protected override void BeginBracketExpression()
        {
            _bracketExpressionBuilder = new();
            _bracketExpressionBuilder.Append('[');
        }

        protected override void EndBracketExpression()
        {
            _bracketExpressionBuilder.Append(']');
            var regex = new Regex(_bracketExpressionBuilder.ToString(), _regexOptions);
            _patternElements.Add(new BracketExpressionElement(regex));
        }

        public static PatternElement[] Parse(WildcardPattern pattern, CharacterNormalizer characterNormalizer)
        {
            var parser = new MyWildcardPatternParser {
                _characterNormalizer = characterNormalizer,
                _regexOptions = WildcardPatternToRegexParser.TranslateWildcardOptionsIntoRegexOptions(pattern.Options)
            };
            Parse(pattern, parser);
            return [ .. parser._patternElements ];
        }
    }

    private class PatternPositionsVisitor
    {
        private readonly int[] _isPatternPositionVisitedMarker;
        private readonly int _lengthOfPattern;
        private readonly int[] _patternPositionsForFurtherProcessing;
        private int _patternPositionsForFurtherProcessingCount;

        public int StringPosition { get; set; }

        public PatternPositionsVisitor(int lengthOfPattern)
        {
            _lengthOfPattern = lengthOfPattern;
            _isPatternPositionVisitedMarker = new int[lengthOfPattern + 1];
            for (int i = 0; i < _isPatternPositionVisitedMarker.Length; i++)
                _isPatternPositionVisitedMarker[i] = -1;
            _patternPositionsForFurtherProcessing = new int[lengthOfPattern];
            _patternPositionsForFurtherProcessingCount = 0;
        }

        public bool IsEndOfPattern
        {
            get { return (_isPatternPositionVisitedMarker[_lengthOfPattern] >= StringPosition); }
        }

        public void Add(int patternPosition)
        {
            if (_isPatternPositionVisitedMarker[patternPosition] != StringPosition) {
                _isPatternPositionVisitedMarker[patternPosition] = StringPosition;
                if (patternPosition < _lengthOfPattern) {
                    _patternPositionsForFurtherProcessing[_patternPositionsForFurtherProcessingCount] = patternPosition;
                    _patternPositionsForFurtherProcessingCount++;
                }
            }
        }

        public bool MoveNext(out int patternPosition)
        {
            if (_patternPositionsForFurtherProcessingCount == 0) {
                patternPosition = -1;
                return false;
            }
            _patternPositionsForFurtherProcessingCount--;
            patternPosition = _patternPositionsForFurtherProcessing[_patternPositionsForFurtherProcessingCount];
            return true;
        }
    }
}