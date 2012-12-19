using System.Windows;

namespace Alba.Framework.System
{
    public class DpChangedEventArgs<T>
    {
        private DependencyPropertyChangedEventArgs _args;

        public DpChangedEventArgs (DependencyPropertyChangedEventArgs args)
        {
            _args = args;
        }

        public DependencyProperty Property
        {
            get { return _args.Property; }
        }

        public T OldValue
        {
            get { return (T)_args.OldValue; }
        }

        public T NewValue
        {
            get { return (T)_args.NewValue; }
        }
    }
}