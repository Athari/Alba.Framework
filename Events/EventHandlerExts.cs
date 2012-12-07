using System;

namespace Alba.Framework.Events
{
    public static class EventHandlerExts
    {
        public static void NullableInvoke (this EventHandler handler, object sender, EventArgs args = null)
        {
            if (args == null)
                args = EventArgs.Empty;
            if (handler != null)
                handler(sender, args);
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
    }
}