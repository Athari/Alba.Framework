// ReSharper disable RedundantUsingDirective
// ReSharper disable RedundantCast
using System.ComponentModel;
using System.Windows;
using Alba.Framework.Windows;
using FPM = System.Windows.FrameworkPropertyMetadata;
using FPMO = System.Windows.FrameworkPropertyMetadataOptions;

namespace Alba.Framework.Caliburn
{
    public static partial class Bind
    {
        public static readonly DependencyProperty TemplateProperty = DependencyProperty.RegisterAttached(
            "Template", typeof(bool), typeof(Bind),
            new FPM(default(bool), Template_Changed));
 
        [Category("Action")]
        [AttachedPropertyBrowsableForType(typeof(DependencyObject))]
        public static bool GetTemplate (DependencyObject d)
        {
            return (bool)d.GetValue(TemplateProperty);
        }
 
        public static void SetTemplate (DependencyObject d, bool value)
        {
            d.SetValue(TemplateProperty, value);
        }
 
        private static void Template_Changed (DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            Template_Changed((DependencyObject)d, new DpChangedEventArgs<bool>(args));
        }

    }
}
