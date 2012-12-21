using System.Collections;
using System.Windows;
using System.Windows.Controls.Primitives;
using Alba.Framework.Events;
using FPM = System.Windows.FrameworkPropertyMetadata;
using FPMO = System.Windows.FrameworkPropertyMetadataOptions;

// ReSharper disable RedundantCast
namespace Alba.Framework.Controls
{
    public static partial class SelectorProps
    {
        public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.RegisterAttached(
            "SelectedItems", typeof(IList), typeof(SelectorProps),
            new FPM(default(IList), FPMO.BindsTwoWayByDefault, SelectedItems_Changed));

        private static readonly DependencyProperty SelectedItemsSyncProperty = DependencyProperty.RegisterAttached(
            "SelectedItemsSync", typeof(SelectedItemsSync), typeof(SelectorProps),
            new FPM(default(SelectedItemsSync)));

        public static IList GetSelectedItems (Selector d)
        {
            return (IList)d.GetValue(SelectedItemsProperty);
        }

        public static void SetSelectedItems (Selector d, IList value)
        {
            d.SetValue(SelectedItemsProperty, value);
        }

        private static void SelectedItems_Changed (DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            SelectedItems_Changed((Selector)d, new DpChangedEventArgs<IList>(args));
        }

        private static SelectedItemsSync GetSelectedItemsSync (Selector d)
        {
            return (SelectedItemsSync)d.GetValue(SelectedItemsSyncProperty);
        }

        private static void SetSelectedItemsSync (Selector d, SelectedItemsSync value)
        {
            d.SetValue(SelectedItemsSyncProperty, value);
        }

    }
}
