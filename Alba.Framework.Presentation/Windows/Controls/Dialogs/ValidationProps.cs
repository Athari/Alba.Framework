using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using Alba.Framework.Text;
using Alba.Framework.Windows.Media;

namespace Alba.Framework.Windows.Controls
{
    public static partial class ValidationProps
    {
        private const int PopupPosDelta = 4;

        private static void ValidationIcon_Changed (FrameworkElement control, DpChangedEventArgs<FrameworkElement> args)
        {
            if (args.OldValue != null) {
                control.SizeChanged -= UpdatePopup;
                control.GotKeyboardFocus -= UpdatePopup;
                control.LostKeyboardFocus -= UpdatePopup;
                if (control is TextBoxBase)
                    ((TextBox)control).TextChanged -= UpdatePopup;
                control.ClearValue(Validation.ErrorTemplateProperty);
            }
            if (args.NewValue != null) {
                GetOrCreateTooltipPopup(control);
                control.SizeChanged += UpdatePopup;
                control.GotKeyboardFocus += UpdatePopup;
                control.LostKeyboardFocus += UpdatePopup;
                if (control is TextBoxBase)
                    ((TextBox)control).TextChanged += UpdatePopup;
                control.SetValue(Validation.ErrorTemplateProperty, null);
            }
            UpdatePopup(control, EventArgs.Empty);
        }

        private static TooltipPopup GetOrCreateTooltipPopup (FrameworkElement control)
        {
            var window = control.GetParent<Window>();
            if (window == null)
                return null;
            TooltipPopup popup = GetTooltipPopup(window);
            if (popup == null) {
                popup = new TooltipPopup { Owner = window };
                SetTooltipPopup(window, popup);
                window.LocationChanged += UpdatePopup;
                window.SizeChanged += UpdatePopup;
            }
            return popup;
        }

        private static void UpdatePopup (object sender, EventArgs args)
        {
            var window = sender as Window;
            FrameworkElement control;
            if (window != null) // window change event, apply to previously validated control
                control = GetCurrentValidatedControl(window);
            else { // validated control change event
                control = (FrameworkElement)sender;
                window = control.GetParent<Window>();
            }
            if (control == null) // no controls validated yet
                return;
            TooltipPopup popup = GetOrCreateTooltipPopup(window);
            if (popup == null) // controls not yet loaded, can't get root window
                return;
            var icon = GetValidationIcon(control);
            SetCurrentValidatedControl(window, control);

            object message = GetMessage(icon);
            if (icon == null || !control.IsKeyboardFocused
                || (message == null)
                || (message is string && ((string)message).IsNullOrEmpty())
                || (message is IEnumerable<object> && !((IEnumerable<object>)message).Any())) {
                popup.Hide();
            }
            else {
                var pos = icon.PointToScreen(new Point(0, 0));
                var popupContent = BindingOperations.GetBinding(popup, ContentControl.ContentProperty);
                if (popupContent == null || !ReferenceEquals(popupContent.Source, icon))
                    BindingOperations.SetBinding(popup, ContentControl.ContentProperty,
                        new Binding { Source = icon, Path = new PropertyPath(MessageProperty) });
                popup.Show();
                if (pos.X + icon.ActualWidth + PopupPosDelta + popup.Width - TooltipPopup.ShadowMargin
                    <= SystemParameters.VirtualScreenTop + SystemParameters.VirtualScreenWidth) {
                    popup.Left = pos.X + icon.ActualWidth + PopupPosDelta;
                    popup.Top = pos.Y - PopupPosDelta;
                }
                else {
                    popup.Left = pos.X + icon.ActualWidth - popup.Width + TooltipPopup.ShadowMargin;
                    popup.Top = pos.Y + icon.ActualHeight + PopupPosDelta;
                }
            }
        }
    }
}