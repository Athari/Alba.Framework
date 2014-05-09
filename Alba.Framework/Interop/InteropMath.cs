using System;

namespace Alba.Framework.Interop
{
    public static class InteropMath
    {
        public static Int16 LowWord (Int32 value)
        {
            return (Int16)(value & 0xffff);
        }

        public static Int16 HighWord (Int32 value)
        {
            return (Int16)(value >> 16);
        }

        public static Int16 LowWord (IntPtr ptr)
        {
            return LowWord((int)ptr);
        }

        public static Int16 HighWord (IntPtr ptr)
        {
            return HighWord((int)ptr);
        }
    }
}