using System.Runtime.InteropServices;

namespace Alba.Framework.Avalonia.Imaging;

[StructLayout(LayoutKind.Sequential)]
public record struct Bgra32(byte B, byte G, byte R, byte A);