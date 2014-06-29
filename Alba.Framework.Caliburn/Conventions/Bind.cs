using System.Windows;
using System.Windows.Data;
using Alba.Framework.Windows;
using CaliburnMicro = Caliburn.Micro;

namespace Alba.Framework.Caliburn
{
    public partial class Bind
    {
        private static void Template_Changed (DependencyObject d, DpChangedEventArgs<bool> args)
        {
            if (args.NewValue)
                BindingOperations.SetBinding(d, CaliburnMicro.Bind.ModelWithoutContextProperty, new Binding());
        }
    }
}