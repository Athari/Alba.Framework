using System;

namespace Alba.Framework.Sys
{
    public static class ValueExts
    {
        public static int MinMax (this int value, int min, int max)
        {
            return Math.Min(max, Math.Max(min, value));
        }

        public static double MinMax (this double value, double min, double max)
        {
            return Math.Min(max, Math.Max(min, value));
        }
    }
}