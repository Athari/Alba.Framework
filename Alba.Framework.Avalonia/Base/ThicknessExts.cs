using Avalonia;

namespace Alba.Framework.Avalonia;

public static class ThicknessExts
{
    extension(Thickness @this)
    {
        public bool IsZero => @this == new Thickness();
    }
}