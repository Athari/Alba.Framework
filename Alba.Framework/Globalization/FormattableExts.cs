using System.Globalization;

namespace Alba.Framework.Globalization;

public static class FormattableExts
{
    private static readonly CultureInfo Invariant = CultureInfo.InvariantCulture;

    extension<T>(T @this) where T : IFormattable
    {
        [Pure]
        public string ToStringInv() =>
            @this.ToString(null, Invariant);

        [Pure]
        public string ToStringInv(string format) =>
            @this.ToString(format, Invariant);
    }
}