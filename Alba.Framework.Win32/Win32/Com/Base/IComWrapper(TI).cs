namespace Alba.Framework.Win32;

internal interface IComWrapper<TI> : IDisposable
{
    TI Com { get; init; }
    nint Ptr { get; init; }
}