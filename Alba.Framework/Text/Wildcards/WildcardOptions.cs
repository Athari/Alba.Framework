namespace Alba.Framework.Text;

[Flags]
public enum WildcardOptions
{
    None = 0,
    /// <summary>Specifies that the regular expression is compiled to an assembly. This yields faster execution but increases startup time.</summary>
    Compiled = 1 << 0,
    /// <summary>Specifies case-insensitive matching.</summary>
    IgnoreCase = 1 << 1,
    /// <summary>Specifies that cultural differences in language is ignored.</summary>
    CultureInvariant = 1 << 2,

    /// <summary>Technically speaking, it should be UPPERCASE, then binary comparison.</summary>
    DefaultFileSystem = IgnoreCase | CultureInvariant,
    /// <summary>IRIs should be normalized before using, it's not responsibility of a client application.</summary>
    DefaultUri = DefaultFileSystem,
}