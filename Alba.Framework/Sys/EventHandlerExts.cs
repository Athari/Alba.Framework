using System;

namespace Alba.Framework.Sys
{
    public static class EventHandlerExts
    {
        public static void NullableInvoke (this EventHandler handler, object sender, EventArgs args = null)
        {
            if (handler != null)
                handler(sender, args ?? EventArgs.Empty);
        }

        public static void NullableInvoke (this EventHandler<EventArgs> handler, object sender, EventArgs args = null)
        {
            if (handler != null)
                handler(sender, args ?? EventArgs.Empty);
        }

        public static void NullableInvoke<T> (this EventHandler<T> handler, object sender, T args)
        {
            if (handler != null)
                handler(sender, args);
        }

        public static void NullableInvoke<T> (this EventHandler<EventArgs<T>> handler, object sender, T arg)
        {
            if (handler != null)
                handler(sender, new EventArgs<T>(arg));
        }

        public static void NullableInvoke<T> (this EventHandler<EventArgs<T>> handler, object sender)
        {
            if (handler != null)
                handler(sender, EventArgs<T>.Empty);
        }
    }
}