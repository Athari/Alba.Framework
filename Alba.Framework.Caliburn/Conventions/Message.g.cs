// ReSharper disable RedundantUsingDirective
// ReSharper disable RedundantCast
using System.Windows;
using Alba.Framework.Windows;
using FPM = System.Windows.FrameworkPropertyMetadata;
using FPMO = System.Windows.FrameworkPropertyMetadataOptions;

namespace Alba.Framework.Caliburn
{
    public static partial class Message
    {
        public static readonly DependencyProperty AttachManyProperty = DependencyProperty.RegisterAttached(
            "AttachMany", typeof(string), typeof(Message),
            new FPM(default(string), AttachMany_Changed));
 
        public static string GetAttachMany (DependencyObject d)
        {
            return (string)d.GetValue(AttachManyProperty);
        }
 
        public static void SetAttachMany (DependencyObject d, string value)
        {
            d.SetValue(AttachManyProperty, value);
        }
 
        private static void AttachMany_Changed (DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            AttachMany_Changed((DependencyObject)d, new DpChangedEventArgs<string>(args));
        }

    }
}
