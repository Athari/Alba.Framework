using System.Runtime.InteropServices;

namespace Alba.Framework.Common;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public readonly struct Undefined : IEquatable<Undefined>
{
    public static object Default { get; } = new Undefined();
    public static bool operator ==(Undefined _1, Undefined _2) => true;
    public static bool operator !=(Undefined _1, Undefined _2) => false;
    public bool Equals(Undefined other) => true;
    public override bool Equals(object? obj) => obj is Undefined;
    public override int GetHashCode() => 0;
    public override string ToString() => "{Undefined}";
}