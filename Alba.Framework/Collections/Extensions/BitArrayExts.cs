using System.Collections;
using System.Runtime.InteropServices;

namespace Alba.Framework.Collections;

[PublicAPI]
public static class BitArrayExts
{
    public static byte[] ToByteArray(this BitArray @this)
    {
        var result = new byte[@this.GetArrayLength<byte>()];
        @this.CopyTo(result, 0);
        return result;
    }

    public static int[] ToInt32Array(this BitArray @this)
    {
        var result = new int[@this.GetArrayLength<int>()];
        @this.CopyTo(result, 0);
        return result;
    }

    public static int GetArrayLength<T>(this BitArray @this) =>
        GetArrayLength(@this.Length, Marshal.SizeOf<T>() * 8);

    private static int GetArrayLength(int n, int div) =>
        n > 0 ? (n - 1) / div + 1 : 0;
}