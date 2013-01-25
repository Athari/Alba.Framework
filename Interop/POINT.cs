using System;
using System.Runtime.InteropServices;
using System.Windows;

namespace Alba.Framework.Interop
{
    /// <summary>The POINT structure defines the x- and y- coordinates of a point.</summary>
    [Serializable]
    [StructLayout (LayoutKind.Sequential)]
    public struct POINT
    {
        /// <summary>The x-coordinate of the point.</summary>
        public int X;
        /// <summary>The y-coordinate of the point.</summary>
        public int Y;

        public POINT (int x, int y)
        {
            X = x;
            Y = y;
        }

        private POINT (double x, double y)
        {
            X = (int)x;
            Y = (int)y;
        }

        public bool IsEmpty
        {
            get { return X == 0 && Y == 0; }
        }

        public static implicit operator Point (POINT p)
        {
            return new Point(p.X, p.Y);
        }

        public static implicit operator POINT (Point p)
        {
            return new POINT(p.X, p.Y);
        }

        public override string ToString ()
        {
            return string.Format("{{X = {0}, Y = {1}}}", X, Y);
        }
    }
}