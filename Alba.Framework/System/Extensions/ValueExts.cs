using static System.Math;

namespace Alba.Framework;

[PublicAPI]
public static class ValueExts
{
    public static int MinMax(this int value, int min, int max) =>
        Min(max, Max(min, value));

    public static double MinMax(this double value, double min, double max) =>
        Min(max, Max(min, value));

    public static void Swap<T>(ref T a, ref T b) =>
        (a, b) = (b, a);
}