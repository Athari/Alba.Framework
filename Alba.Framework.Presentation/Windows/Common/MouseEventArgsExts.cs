using System.Windows;
using System.Windows.Input;

namespace Alba.Framework.Windows
{
    public static class MouseEventArgsExts
    {
        public static Point GetSourcePosition (this MouseEventArgs @this)
        {
            return @this.GetPosition((IInputElement)@this.Source);
        }
    }
}