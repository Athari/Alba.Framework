using System;
using System.Runtime.InteropServices;
using System.Windows;

namespace Alba.Framework.Interop
{
    /// <summary>The RECT structure defines the coordinates of the upper-left and lower-right corners of a rectangle.</summary>
    [Serializable]
    [StructLayout (LayoutKind.Sequential)]
    public struct RECT
    {
        /// <summary>The x-coordinate of the upper-left corner of the rectangle.</summary>
        public int Left;
        /// <summary>The y-coordinate of the upper-left corner of the rectangle.</summary>
        public int Top;
        /// <summary>The x-coordinate of the lower-right corner of the rectangle.</summary>
        public int Right;
        /// <summary>The y-coordinate of the lower-right corner of the rectangle.</summary>
        public int Bottom;

        public RECT (int left, int top, int right, int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        private RECT (double left, double top, double right, double bottom)
        {
            Left = (int)left;
            Top = (int)top;
            Right = (int)right;
            Bottom = (int)bottom;
        }

        public int Width
        {
            get { return Right - Left; }
            set { Right = value + Left; }
        }

        public int Height
        {
            get { return Bottom - Top; }
            set { Bottom = value + Top; }
        }

        public static implicit operator Rect (RECT r)
        {
            return new Rect(r.Left, r.Top, r.Width, r.Height);
        }

        public static implicit operator RECT (Rect r)
        {
            return new RECT(r.Left, r.Top, r.Right, r.Bottom);
        }

        public override string ToString ()
        {
            return string.Format("{{Left = {0}, Top = {1}, Right = {2}, Bottom = {3}}}",
                Left, Top, Right, Bottom);
        }
    }
}