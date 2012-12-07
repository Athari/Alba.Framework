using System;
using System.Collections.Generic;

namespace Alba.Framework.Events
{
    internal static class WeakEvents
    {
        public static void AddHandler (ref List<WeakReference> handlers, EventHandler handler, int defaultListSize = 0)
        {
            if (handlers == null)
                handlers = defaultListSize > 0 ? new List<WeakReference>(defaultListSize) : new List<WeakReference>();
            handlers.Add(new WeakReference(handler));
        }

        public static void RemoveHandler (List<WeakReference> handlers, EventHandler handler)
        {
            if (handlers == null)
                return;
            for (int i = handlers.Count - 1; i >= 0; i--) {
                WeakReference reference = handlers[i];
                var existingHandler = (EventHandler)reference.Target;
                if (existingHandler == null || existingHandler == handler)
                    handlers.RemoveAt(i);
            }
        }

        public static void Call (List<WeakReference> handlers, Action<EventHandler> action)
        {
            if (handlers == null)
                return;
            for (int i = handlers.Count - 1; i >= 0; i--) {
                WeakReference reference = handlers[i];
                var handler = (EventHandler)reference.Target;
                if (handler == null)
                    handlers.RemoveAt(i);
                else
                    action(handler);
            }
        }

        public static void AddHandler<T> (ref List<WeakReference> handlers, EventHandler<T> handler, int defaultListSize = 0)
        {
            if (handlers == null)
                handlers = defaultListSize > 0 ? new List<WeakReference>(defaultListSize) : new List<WeakReference>();
            handlers.Add(new WeakReference(handler));
        }

        public static void RemoveHandler<T> (List<WeakReference> handlers, EventHandler<T> handler)
        {
            if (handlers == null)
                return;
            for (int i = handlers.Count - 1; i >= 0; i--) {
                WeakReference reference = handlers[i];
                var existingHandler = (EventHandler<T>)reference.Target;
                if (existingHandler == null || existingHandler == handler)
                    handlers.RemoveAt(i);
            }
        }

        public static void Call<T> (List<WeakReference> handlers, Action<EventHandler<T>> action)
        {
            if (handlers == null)
                return;
            for (int i = handlers.Count - 1; i >= 0; i--) {
                WeakReference reference = handlers[i];
                var handler = (EventHandler<T>)reference.Target;
                if (handler == null)
                    handlers.RemoveAt(i);
                else
                    action(handler);
            }
        }
    }
}