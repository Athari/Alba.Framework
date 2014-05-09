using System;

namespace Alba.Framework.Sys
{
    public class EventArgs<T> : EventArgs
    {
        public new static readonly EventArgs<T> Empty = new EventArgs<T>(default(T));

        public EventArgs (T value)
        {
            Value = value;
        }

        public T Value { get; private set; }
    }
}