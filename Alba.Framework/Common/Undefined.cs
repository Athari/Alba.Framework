using System;
using System.Runtime.InteropServices;

namespace Alba.Framework.Common
{
    [StructLayout (LayoutKind.Sequential, Size = 1)]
    public struct Undefined : IEquatable<Undefined>
    {
        private static readonly object _default = new Undefined();

        public static object Default
        {
            get { return _default; }
        }

        public static bool operator == (Undefined x, Undefined y)
        {
            return true;
        }

        public static bool operator != (Undefined x, Undefined y)
        {
            return false;
        }

        public bool Equals (Undefined other)
        {
            return true;
        }

        public override bool Equals (object obj)
        {
            return obj is Undefined;
        }

        public override int GetHashCode ()
        {
            return 0;
        }

        public override string ToString ()
        {
            return "{Undefined}";
        }
    }
}