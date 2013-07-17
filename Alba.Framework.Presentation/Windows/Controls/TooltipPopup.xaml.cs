using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Alba.Framework.Windows.Mvvm;

namespace Alba.Framework.Windows.Controls
{
    public partial class TooltipPopup
    {
        public const int ShadowMargin = 10;

        public TooltipPopup ()
        {
            InitializeComponent();
        }

        private void TooltipPopup_MouseDown (object sender, MouseButtonEventArgs e)
        {
            Hide();
        }
    }

    internal class TooltipPopupErrorContentTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate (object item, DependencyObject container)
        {
            var resources = (FrameworkElement)container;
            if (item is String)
                return (DataTemplate)resources.FindResource("tplString");
            if (item is ValidationError)
                return (DataTemplate)resources.FindResource("tplError");
            if (item is ValidationMessage)
                return (DataTemplate)resources.FindResource("tplMessage");
            if (item is IEnumerable<object>) // ErrorContentConverter does not know type of error content and cannot cast to IEnumerable<ValidationMessage>
                return (DataTemplate)resources.FindResource("tplMessages");
            return null;
        }
    }
}