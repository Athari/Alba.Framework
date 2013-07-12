using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Alba.Framework.Collections;
using Alba.Framework.Reflection;
using Alba.Framework.Sys;
using Alba.Framework.Text;

namespace Alba.Framework.Windows.Mvvm
{
    public static class EventCommands
    {
        private const string ErrorUnexpectedCommandParam = "Command '{0}' expected parameter of type '{1}', but received '{2}'.";

        private static readonly Dictionary<EventCommand, ConditionalWeakTable<IModel, ModelHandlers>> _commandsHandlers = new Dictionary<EventCommand, ConditionalWeakTable<IModel, ModelHandlers>>();

        public static void Subscribe (IModel model, EventCommand command, Action execute, Func<bool> canExecute = null)
        {
            ModelHandlers handlers = GetModelHandlers(command, model, true);

            lock (handlers) {
                handlers.Execute += (s, a) => execute();
                if (canExecute != null)
                    handlers.CanExecute += (s, a) => { a.CanExecute = canExecute(); };
            }
        }

        public static void Subscribe<T> (IModel model, EventCommand command, Action<T> execute, Func<T, bool> canExecute = null)
        {
            ModelHandlers handlers = GetModelHandlers(command, model, true);

            lock (handlers) {
                handlers.Execute += InvokeWithParamTypeCheck<ExecuteEventArgs, T>(command,
                    a => execute((T)a.Parameter));
                if (canExecute != null)
                    handlers.CanExecute += InvokeWithParamTypeCheck<CanExecuteEventArgs, T>(command,
                        a => a.CanExecute = canExecute((T)a.Parameter));
            }
        }

        public static void Unsubscribe (IModel model, EventCommand command)
        {
            lock (_commandsHandlers) {
                ConditionalWeakTable<IModel, ModelHandlers> modelsHandlers = _commandsHandlers.GetOrDefault(command);
                if (modelsHandlers != null)
                    modelsHandlers.Remove(model);
            }
        }

        public static void Unsubscribe (IModel model)
        {
            lock (_commandsHandlers) {
                foreach (ConditionalWeakTable<IModel, ModelHandlers> modelsHandlers in _commandsHandlers.Values)
                    modelsHandlers.Remove(model);
            }
        }

        internal static void AddCommandRef (IModel model, EventCommand command, EventCommandRef commandRef)
        {
            ModelHandlers handlers = GetModelHandlers(command, model, true);

            lock (handlers) {
                handlers.CommandRefs.Add(new WeakReference<EventCommandRef>(commandRef));
                handlers.CommandRefs.RemoveStaleReferences();
            }
        }

        internal static IEnumerable<EventCommandRef> GetCommandRefs (IModel model, EventCommand command)
        {
            ModelHandlers handlers = GetModelHandlers(command, model, false);
            if (handlers == null)
                return Enumerable.Empty<EventCommandRef>();

            lock (handlers) {
                var result = new List<EventCommandRef>();
                foreach (var wr in handlers.CommandRefs) {
                    EventCommandRef commandRef;
                    if (wr.TryGetTarget(out commandRef))
                        result.Add(commandRef);
                }
                return result;
            }
        }

        private static EventHandler<TArgs> InvokeWithParamTypeCheck<TArgs, TParam> (
            EventCommand command, Action<TArgs> handler)
            where TArgs : ExecuteEventArgs
        {
            return (s, a) => {
                if (a.Parameter != null && !(a.Parameter is TParam))
                    throw new ArgumentException(ErrorUnexpectedCommandParam
                        .Fmt(command.Name, typeof(TParam).FullName, a.Parameter.GetTypeFullName()));
                handler(a);
            };
        }

        private static void OnCommandExecute (object sender, ExecuteEventArgs args)
        {
            ModelHandlers handlers = GetModelHandlers((EventCommand)sender, args.Model, false);
            if (handlers == null)
                return;
            handlers.RaiseExecute(sender, args);
        }

        private static void OnCommandCanExecute (object sender, CanExecuteEventArgs args)
        {
            ModelHandlers handlers = GetModelHandlers((EventCommand)sender, args.Model, false);
            if (handlers == null)
                return;
            handlers.RaiseCanExecute(sender, args);
        }

        private static ModelHandlers GetModelHandlers (EventCommand command, IModel model, bool create)
        {
            lock (_commandsHandlers) {
                if (create) {
                    ConditionalWeakTable<IModel, ModelHandlers> modelsHandlers = _commandsHandlers.GetOrAdd(command, () => {
                        command.Subscribe(OnCommandExecute, OnCommandCanExecute);
                        return new ConditionalWeakTable<IModel, ModelHandlers>();
                    });
                    return modelsHandlers.GetValue(model, k => new ModelHandlers());
                }
                else {
                    ConditionalWeakTable<IModel, ModelHandlers> modelsHandlers = _commandsHandlers.GetOrDefault(command);
                    return modelsHandlers == null ? null : modelsHandlers.GetOrDefault(model);
                }
            }
        }

        public static EventCommand Register (Expression<Func<EventCommand>> propExpr, bool isAutoRequery = true)
        {
            return new EventCommand(Props.GetName(propExpr), isAutoRequery);
        }

        internal class ModelHandlers
        {
            public event EventHandler<ExecuteEventArgs> Execute;
            public event EventHandler<CanExecuteEventArgs> CanExecute;
            public List<WeakReference<EventCommandRef>> CommandRefs { get; private set; }

            public ModelHandlers ()
            {
                CommandRefs = new List<WeakReference<EventCommandRef>>();
            }

            public void RaiseExecute (object sender, ExecuteEventArgs args)
            {
                Execute.NullableInvoke(sender, args);
            }

            public void RaiseCanExecute (object sender, CanExecuteEventArgs args)
            {
                CanExecute.NullableInvoke(sender, args);
            }
        }
    }
}