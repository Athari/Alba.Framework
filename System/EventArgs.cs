using System;

namespace Alba.Framework.System
{
    public class EventArgs<T> : EventArgs
    {
        public EventArgs (T value)
        {
            Value = value;
        }

        public T Value { get; private set; }
    }
}