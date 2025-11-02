using System.Runtime.InteropServices;

namespace Alba.Framework.Drawing;

[StructLayout(LayoutKind.Sequential)]
public record struct Argb32(byte A, byte R, byte G, byte B);

[StructLayout(LayoutKind.Sequential)]
public record struct Argb64(ushort A, ushort R, ushort G, ushort B);

[StructLayout(LayoutKind.Sequential)]
public record struct Bgr24(byte B, byte G, byte R);

[StructLayout(LayoutKind.Sequential)]
public record struct Bgr48(ushort B, ushort G, ushort R);

[StructLayout(LayoutKind.Sequential)]
public record struct Bgra32(byte B, byte G, byte R, byte A);

[StructLayout(LayoutKind.Sequential)]
public record struct Bgra64(ushort B, ushort G, ushort R, ushort A);

[StructLayout(LayoutKind.Sequential)]
public record struct Gray8(byte L);

[StructLayout(LayoutKind.Sequential)]
public record struct Gray16(ushort L);

[StructLayout(LayoutKind.Sequential)]
public record struct La8(byte L, byte A);

[StructLayout(LayoutKind.Sequential)]
public record struct La16(ushort L, ushort A);

[StructLayout(LayoutKind.Sequential)]
public record struct Rgb24(byte R, byte G, byte B);

[StructLayout(LayoutKind.Sequential)]
public record struct Rgb48(ushort R, ushort G, ushort B);

[StructLayout(LayoutKind.Sequential)]
public record struct Rgba32(byte R, byte G, byte B, byte A);

[StructLayout(LayoutKind.Sequential)]
public record struct Rgba64(ushort R, ushort G, ushort B, ushort A);