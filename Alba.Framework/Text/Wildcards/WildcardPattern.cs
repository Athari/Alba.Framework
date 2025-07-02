using System.Diagnostics.CodeAnalysis;

namespace Alba.Framework.Text;

/// <summary>
/// Burrowed from System.Management.Automation.WildcardPattern.
/// Documentation for patterns: http://msdn.microsoft.com/en-US/library/aa717088 (the escape char is ^, not `).
/// </summary>
[PublicAPI]
public sealed class WildcardPattern
{
    internal const char EscapeChar = '^';

    private Predicate<string>? _isMatch;

    internal WildcardOptions Options { get; private set; }
    internal string Pattern { get; private set; }

    public WildcardPattern(string pattern, WildcardOptions options = WildcardOptions.DefaultFileSystem)
    {
        Guard.IsNotNull(pattern, nameof(pattern));
        Pattern = pattern;
        Options = options;
    }

    [MemberNotNull(nameof(_isMatch))]
    private bool Init()
    {
        _isMatch ??= new WildcardPatternMatcher(this).IsMatch;
        return true;
    }

    public bool IsMatch(string? input)
    {
        return input != null && Init() && _isMatch(input);
    }

    public static bool ContainsWildcardCharacters(string pattern)
    {
        if (string.IsNullOrEmpty(pattern))
            return false;
        for (int i = 0; i < pattern.Length; i++) {
            if (IsWildcardChar(pattern[i]))
                return true;
            if (pattern[i] == EscapeChar)
                i++;
        }
        return false;
    }

    public static bool IsMatch(string input, string pattern, WildcardOptions options = WildcardOptions.DefaultFileSystem)
    {
        return new WildcardPattern(pattern, options).IsMatch(input);
    }

    public static string Escape(string pattern)
    {
        Guard.IsNotNull(pattern, nameof(pattern));
        var chArray = new char[(pattern.Length * 2) + 1];
        int length = 0;
        for (int i = 0; i < pattern.Length; i++) {
            char ch = pattern[i];
            if (IsWildcardChar(ch))
                chArray[length++] = EscapeChar;
            chArray[length++] = ch;
        }
        return length > 0 ? new(chArray, 0, length) : string.Empty;
    }

    public static string Unescape(string pattern)
    {
        Guard.IsNotNull(pattern, nameof(pattern));
        var chArray = new char[pattern.Length];
        int length = 0;
        bool isEscaped = false;
        for (int i = 0; i < pattern.Length; i++) {
            char ch = pattern[i];
            if (ch == EscapeChar) {
                if (!isEscaped)
                    isEscaped = true;
                else {
                    chArray[length++] = ch;
                    isEscaped = false;
                }
            }
            else {
                if (isEscaped && !IsWildcardChar(ch))
                    chArray[length++] = EscapeChar;
                chArray[length++] = ch;
                isEscaped = false;
            }
        }
        if (isEscaped)
            chArray[length++] = EscapeChar;
        return length > 0 ? new(chArray, 0, length) : string.Empty;
    }

    private static bool IsWildcardChar(char ch) => ch is '*' or '?' or '[' or ']';
}