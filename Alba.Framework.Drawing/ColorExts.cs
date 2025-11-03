using System.Drawing;

namespace Alba.Framework.Drawing;

[PublicAPI]
public static class ColorExts
{
    extension(Color @this)
    {
        public Argb32 ToArgb32() =>
            new(@this.A, @this.R, @this.G, @this.B);

        public Argb64 ToArgb64() =>
            new(ByteToInt16(@this.A), ByteToInt16(@this.R), ByteToInt16(@this.G), ByteToInt16(@this.B));

        public Bgr24 ToBgr24() =>
            new(@this.B, @this.G, @this.R);

        public Bgr48 ToBgr48() =>
            new(ByteToInt16(@this.B), ByteToInt16(@this.G), ByteToInt16(@this.R));

        public Bgra32 ToBgra32() =>
            new(@this.B, @this.G, @this.R, @this.A);

        public Bgra64 ToBgra64() =>
            new(ByteToInt16(@this.B), ByteToInt16(@this.G), ByteToInt16(@this.R), ByteToInt16(@this.A));

        public Rgb24 ToRgb24() =>
            new(@this.R, @this.G, @this.B);

        public Rgb48 ToRgb48() =>
            new(ByteToInt16(@this.R), ByteToInt16(@this.G), ByteToInt16(@this.B));

        public Rgba32 ToRgba32() =>
            new(@this.R, @this.G, @this.B, @this.A);

        public Rgba64 ToRgba64() =>
            new(ByteToInt16(@this.R), ByteToInt16(@this.G), ByteToInt16(@this.B), ByteToInt16(@this.A));
    }

    private static ushort ByteToInt16(byte b) => (ushort)((b << 8) | b);
}