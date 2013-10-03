// ReSharper disable RedundantUsingDirective
// ReSharper disable RedundantCast
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using FPM = System.Windows.FrameworkPropertyMetadata;
using FPMO = System.Windows.FrameworkPropertyMetadataOptions;

namespace Alba.Framework.Windows.Controls
{
    public static partial class GridProps
    {
        public static readonly DependencyProperty CalculateCellSizesProperty = DependencyProperty.RegisterAttached(
            "CalculateCellSizes", typeof(bool), typeof(GridProps),
            new FPM(default(bool), CalculateCellSizes_Changed));
 
        public static readonly DependencyProperty ForceCellSizesProperty = DependencyProperty.RegisterAttached(
            "ForceCellSizes", typeof(bool), typeof(GridProps),
            new FPM(default(bool), ForceCellSizes_Changed));
 
        private static readonly DependencyProperty DummyGridProperty = DependencyProperty.RegisterAttached(
            "DummyGrid", typeof(Grid), typeof(GridProps),
            new FPM(default(Grid)));
 
        private static readonly DependencyPropertyKey RowActualHeightPropertyKey = DependencyProperty.RegisterAttachedReadOnly(
            "RowActualHeight", typeof(double), typeof(GridProps),
            new FPM(default(double)));
        public static readonly DependencyProperty RowActualHeightProperty = RowActualHeightPropertyKey.DependencyProperty;
 
        private static readonly DependencyPropertyKey ColumnActualWidthPropertyKey = DependencyProperty.RegisterAttachedReadOnly(
            "ColumnActualWidth", typeof(double), typeof(GridProps),
            new FPM(default(double)));
        public static readonly DependencyProperty ColumnActualWidthProperty = ColumnActualWidthPropertyKey.DependencyProperty;
 
        public static bool GetCalculateCellSizes (Grid d)
        {
            return (bool)d.GetValue(CalculateCellSizesProperty);
        }
 
        public static void SetCalculateCellSizes (Grid d, bool value)
        {
            d.SetValue(CalculateCellSizesProperty, value);
        }
 
        private static void CalculateCellSizes_Changed (DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            CalculateCellSizes_Changed((Grid)d, new DpChangedEventArgs<bool>(args));
        }

        public static bool GetForceCellSizes (Grid d)
        {
            return (bool)d.GetValue(ForceCellSizesProperty);
        }
 
        public static void SetForceCellSizes (Grid d, bool value)
        {
            d.SetValue(ForceCellSizesProperty, value);
        }
 
        private static void ForceCellSizes_Changed (DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            ForceCellSizes_Changed((Grid)d, new DpChangedEventArgs<bool>(args));
        }

        private static Grid GetDummyGrid (Grid d)
        {
            return (Grid)d.GetValue(DummyGridProperty);
        }
 
        private static void SetDummyGrid (Grid d, Grid value)
        {
            d.SetValue(DummyGridProperty, value);
        }
 
        public static double GetRowActualHeight (RowDefinition d)
        {
            return (double)d.GetValue(RowActualHeightProperty);
        }
 
        private static void SetRowActualHeight (RowDefinition d, double value)
        {
            d.SetValue(RowActualHeightPropertyKey, value);
        }
 
        public static double GetColumnActualWidth (ColumnDefinition d)
        {
            return (double)d.GetValue(ColumnActualWidthProperty);
        }
 
        private static void SetColumnActualWidth (ColumnDefinition d, double value)
        {
            d.SetValue(ColumnActualWidthPropertyKey, value);
        }
 
    }
}
