namespace Alba.Framework.Win32;

internal abstract class ComWrapper<TI> : SafeComHandle, IComWrapper<TI>
{
    protected ComWrapper() { }
    protected ComWrapper(IntPtr handle, bool ownsHandle = true) : base(handle, ownsHandle) { }

    protected TI Com { get; init; } = default!;

    TI IComWrapper<TI>.Com
    {
        get => Com;
        init => Com = value;
    }

    protected nint Ptr { get; init; }

    nint IComWrapper<TI>.Ptr
    {
        get => Ptr;
        init => Ptr = value;
    }
}