using System;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using Alba.Framework.System;

namespace Alba.Framework.Commands
{
    public class ControlCommand : ICommand
    {
        private InputGestureCollection _inputGestureCollection;

        public string Name { get; private set; }

        public Type OwnerType { get; private set; }

        public InputGestureCollection InputGestures
        {
            get { return _inputGestureCollection ?? (_inputGestureCollection = new InputGestureCollection()); }
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public ControlCommand (string name, Type ownerType, InputGestureCollection inputGestures = null)
        {
            if (name.IsNullOrEmpty())
                throw new ArgumentNullException("name");
            if (ownerType == null)
                throw new ArgumentNullException("ownerType");

            Name = name;
            OwnerType = ownerType;
            _inputGestureCollection = inputGestures;
        }

        void ICommand.Execute (object parameter)
        {
            Execute(parameter, FilterInputElement(Keyboard.FocusedElement));
        }

        bool ICommand.CanExecute (object parameter)
        {
            bool unused;
            return CanExecuteImpl(parameter, FilterInputElement(Keyboard.FocusedElement), out unused);
        }

        public void Execute (object parameter, IInputElement target)
        {
            if (target != null && !InputElementIsValid(target))
                throw new InvalidOperationException(string.Format("Invalid input element: {0}", target.GetType()));
            if (target == null)
                target = FilterInputElement(Keyboard.FocusedElement);

            ExecuteImpl(parameter, target);
        }

        public bool CanExecute (object parameter, IInputElement target)
        {
            bool unused;
            return CanExecuteImpl(parameter, target, out unused);
        }

        private IInputElement FilterInputElement (IInputElement elem)
        {
            return elem != null && InputElementIsValid(elem) ? elem : null;
        }

        private bool CanExecuteImpl (object parameter, IInputElement target, out bool continueRouting)
        {
            if (target != null) {
                var args = CreateCanExecuteRoutedEventArgs(this, parameter);
                args.RoutedEvent = CommandManager.PreviewCanExecuteEvent;
                CriticalCanExecuteWrapper(target, args);
                if (!args.Handled) {
                    args.RoutedEvent = CommandManager.CanExecuteEvent;
                    CriticalCanExecuteWrapper(target, args);
                }

                continueRouting = args.ContinueRouting;
                return args.CanExecute;
            }
            else {
                continueRouting = false;
                return false;
            }
        }

        private void CriticalCanExecuteWrapper (IInputElement target, CanExecuteRoutedEventArgs args)
        {
            // ReSharper disable CanBeReplacedWithTryCastAndCheckForNull
            var targetAsDO = (DependencyObject)target;
            if (targetAsDO is UIElement)
                ((UIElement)targetAsDO).RaiseEvent(args);
            else if (targetAsDO is ContentElement)
                ((ContentElement)targetAsDO).RaiseEvent(args);
            else if (targetAsDO is UIElement3D)
                ((UIElement3D)targetAsDO).RaiseEvent(args);
            // ReSharper restore CanBeReplacedWithTryCastAndCheckForNull
        }

        private void ExecuteImpl (object parameter, IInputElement target)
        {
            if (target == null)
                return;

            var targetUIElement = target as UIElement;
            var targetAsContentElement = target as ContentElement;
            var targetAsUIElement3D = target as UIElement3D;

            var args = CreateExecutedRoutedEventArgs(this, parameter);
            args.RoutedEvent = CommandManager.PreviewExecutedEvent;

            if (targetUIElement != null)
                targetUIElement.RaiseEvent(args);
            else if (targetAsContentElement != null)
                targetAsContentElement.RaiseEvent(args);
            else if (targetAsUIElement3D != null)
                targetAsUIElement3D.RaiseEvent(args);

            if (!args.Handled) {
                args.RoutedEvent = CommandManager.ExecutedEvent;
                if (targetUIElement != null)
                    targetUIElement.RaiseEvent(args);
                else if (targetAsContentElement != null)
                    targetAsContentElement.RaiseEvent(args);
                else if (targetAsUIElement3D != null)
                    targetAsUIElement3D.RaiseEvent(args);
            }
        }

        private static bool InputElementIsValid (IInputElement element)
        {
            return element is UIElement || element is UIElement3D || element is ContentElement;
        }

        private static ExecutedRoutedEventArgs CreateExecutedRoutedEventArgs (ICommand command, object parameter)
        {
            var constr = typeof(ExecutedRoutedEventArgs).GetConstructor(
                BindingFlags.NonPublic | BindingFlags.Instance, null,
                new[] { typeof(ICommand), typeof(object) }, null);
            return (ExecutedRoutedEventArgs)constr.Invoke(new[] { command, parameter });
        }

        private static CanExecuteRoutedEventArgs CreateCanExecuteRoutedEventArgs (ICommand command, object parameter)
        {
            var constr = typeof(CanExecuteRoutedEventArgs).GetConstructor(
                BindingFlags.NonPublic | BindingFlags.Instance, null,
                new[] { typeof(ICommand), typeof(object) }, null);
            return (CanExecuteRoutedEventArgs)constr.Invoke(new[] { command, parameter });
        }
    }
}