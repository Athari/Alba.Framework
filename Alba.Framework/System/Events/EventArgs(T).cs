namespace Alba.Framework;

public class EventArgs<T>(T value) : EventArgs
{
    public new static readonly EventArgs<T> Empty = new(default!);

    public T Value { get; } = value;
}