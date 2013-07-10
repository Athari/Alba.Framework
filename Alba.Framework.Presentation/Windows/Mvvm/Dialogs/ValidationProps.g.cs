// ReSharper disable RedundantUsingDirective
// ReSharper disable RedundantCast
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using Alba.Framework.Windows.Controls;
using Alba.Framework.Events;
using FPM = System.Windows.FrameworkPropertyMetadata;
using FPMO = System.Windows.FrameworkPropertyMetadataOptions;

namespace Alba.Framework.Windows.Mvvm.Dialogs
{
    public static partial class ValidationProps
    {
        public static readonly DependencyProperty ValidationIconProperty = DependencyProperty.RegisterAttached(
            "ValidationIcon", typeof(FrameworkElement), typeof(ValidationProps),
            new FPM((FrameworkElement)null, ValidationIcon_Changed));

        public static readonly DependencyProperty ValidatesControlProperty = DependencyProperty.RegisterAttached(
            "ValidatesControl", typeof(FrameworkElement), typeof(ValidationProps),
            new FPM((FrameworkElement)null));

        public static readonly DependencyProperty MessageProperty = DependencyProperty.RegisterAttached(
            "Message", typeof(object), typeof(ValidationProps),
            new FPM((object)null));

        private static readonly DependencyProperty TooltipPopupProperty = DependencyProperty.RegisterAttached(
            "TooltipPopup", typeof(TooltipPopup), typeof(ValidationProps),
            new FPM((TooltipPopup)null));

        private static readonly DependencyProperty CurrentValidatedControlProperty = DependencyProperty.RegisterAttached(
            "CurrentValidatedControl", typeof(FrameworkElement), typeof(ValidationProps),
            new FPM((FrameworkElement)null));

        public static FrameworkElement GetValidationIcon (FrameworkElement d)
        {
            return (FrameworkElement)d.GetValue(ValidationIconProperty);
        }

        public static void SetValidationIcon (FrameworkElement d, FrameworkElement value)
        {
            d.SetValue(ValidationIconProperty, value);
        }

        private static void ValidationIcon_Changed (DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            ValidationIcon_Changed((FrameworkElement)d, new DpChangedEventArgs<FrameworkElement>(args));
        }

        public static FrameworkElement GetValidatesControl (FrameworkElement d)
        {
            return (FrameworkElement)d.GetValue(ValidatesControlProperty);
        }

        public static void SetValidatesControl (FrameworkElement d, FrameworkElement value)
        {
            d.SetValue(ValidatesControlProperty, value);
        }

        public static object GetMessage (FrameworkElement d)
        {
            return (object)d.GetValue(MessageProperty);
        }

        public static void SetMessage (FrameworkElement d, object value)
        {
            d.SetValue(MessageProperty, value);
        }

        private static TooltipPopup GetTooltipPopup (Window d)
        {
            return (TooltipPopup)d.GetValue(TooltipPopupProperty);
        }

        private static void SetTooltipPopup (Window d, TooltipPopup value)
        {
            d.SetValue(TooltipPopupProperty, value);
        }

        private static FrameworkElement GetCurrentValidatedControl (Window d)
        {
            return (FrameworkElement)d.GetValue(CurrentValidatedControlProperty);
        }

        private static void SetCurrentValidatedControl (Window d, FrameworkElement value)
        {
            d.SetValue(CurrentValidatedControlProperty, value);
        }

    }
}
