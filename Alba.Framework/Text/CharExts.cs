using System.Globalization;

namespace Alba.Framework.Text;

public static class CharExts
{
    private static readonly CultureInfo Inv = CultureInfo.InvariantCulture;

    extension(char @this)
    {
        public char ToLower() => char.ToLower(@this);
        public char ToLowerInv() => char.ToLower(@this, Inv);
        public char ToUpper() => char.ToUpper(@this);
        public char ToUpperInv() => char.ToUpper(@this, Inv);
    }
}