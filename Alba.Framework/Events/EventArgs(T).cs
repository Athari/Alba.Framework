using System;

namespace Alba.Framework.Events
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