using System.Numerics;

namespace Alba.Framework.Numerics;

public static class NumberVectorExts
{
    public static Vector<T> ToVector<T>(this T @this) where T : INumber<T> => new(@this);

    public static Vector<T> ToVector<T>(this Span<T> @this) where T : INumber<T> => new(@this);

    public static Vector<T> ToVector<T>(this ReadOnlySpan<T> @this) where T : INumber<T> => new(@this);
}