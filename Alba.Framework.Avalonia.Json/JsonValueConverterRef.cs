namespace Alba.Framework.Avalonia.Json;

public abstract class JsonValueConverterRef
{
    public bool Invert { get; set; }
    public bool ConvertNull { get; set; }

    public object? ValueToRepr(object? o) => o != null || ConvertNull ? ValueToReprCore(o) : null;
    public object? ReprToValue(object? o) => o != null || ConvertNull ? ReprToValueCore(o) : null;

    private object? ValueToReprCore(object? o) => Invert ? ReprToValueOverride(o) : ValueToReprOverride(o);
    private object? ReprToValueCore(object? o) => Invert ? ValueToReprOverride(o) : ReprToValueOverride(o);

    protected abstract object? ValueToReprOverride(object? o);
    protected abstract object? ReprToValueOverride(object? o);
}

public abstract class JsonValueConverterRef<T, TRepr> : JsonValueConverterRef
{
    protected Type ValueType => Invert ? typeof(TRepr) : typeof(T);
    protected Type ReprType => Invert ? typeof(T) : typeof(TRepr);
}