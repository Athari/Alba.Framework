using System;

namespace Alba.Framework.Sys
{
    public static class LazyExts
    {
        public static Lazy<T> Force<T> (this Lazy<T> @this)
        {
#pragma warning disable 168 // Unused local variable
            T value = @this.Value;
#pragma warning restore 168
            return @this;
        }
    }
}