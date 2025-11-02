using System.Runtime.InteropServices;

namespace Alba.Framework.Avalonia.Imaging;

[StructLayout(LayoutKind.Sequential)]
public struct Rgba32
{
    public byte R;
    public byte G;
    public byte B;
    public byte A;
}