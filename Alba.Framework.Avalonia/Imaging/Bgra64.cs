using System.Runtime.InteropServices;

namespace Alba.Framework.Avalonia.Imaging;

[StructLayout(LayoutKind.Sequential)]
public record struct Bgra64(ushort B, ushort G, ushort R, ushort A);