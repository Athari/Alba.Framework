using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Alba.Framework.Windows.Markup
{
    [MarkupExtensionReturnType (typeof(bool))]
    public class RadioBinding : System.Windows.Data.MultiBinding
    {
        public RadioBinding (BindingBase selectedBinding, Binding valueBinding)
        {
            if (Mode == BindingMode.Default)
                Mode = BindingMode.TwoWay;
            var selectedBindingSingle = selectedBinding as Binding;
            if (selectedBindingSingle != null && selectedBindingSingle.Mode == BindingMode.Default)
                selectedBindingSingle.Mode = Mode;
            var selectedBindingMulti = selectedBinding as MultiBinding;
            if (selectedBindingMulti != null && selectedBindingMulti.Mode == BindingMode.Default)
                selectedBindingMulti.Mode = Mode;
            if (valueBinding.Mode == BindingMode.Default)
                valueBinding.Mode = BindingMode.OneWay;
            Bindings.Add(selectedBinding);
            Bindings.Add(valueBinding);
            Converter = new RadioEqualsConverter();
        }

        private class RadioEqualsConverter : IMultiValueConverter
        {
            private static readonly IMultiValueConverter _converter = new EqualsConverter();

            private object[] _values;

            public object Convert (object[] values, Type targetType, object parameter, CultureInfo culture)
            {
                _values = (object[])values.Clone();
                return _converter.Convert(values, targetType, parameter, culture);
            }

            public object[] ConvertBack (object value, Type[] targetTypes, object parameter, CultureInfo culture)
            {
                var values = new object[targetTypes.Length];
                if (value == DependencyProperty.UnsetValue || _values == null) {
                    for (int i = 0; i < values.Length; i++)
                        values[i] = DependencyProperty.UnsetValue;
                }
                else {
                    for (int i = 0; i < values.Length; i++)
                        values[i] = Binding.DoNothing;
                    if ((bool)value)
                        values[0] = _values[1];
                }
                return values;
            }
        }
    }
}