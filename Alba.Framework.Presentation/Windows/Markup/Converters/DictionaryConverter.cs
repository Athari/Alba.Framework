using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using Alba.Framework.Collections;
using Alba.Framework.Common;
using Alba.Framework.Sys;
using Alba.Framework.Text;

namespace Alba.Framework.Windows.Markup
{
    public class DictionaryConverter : Map<object, object>, IValueConverter, ISupportInitialize
    {
        public static readonly object Defaut = new NamedObject("DictionaryConverter.Default");

        private object _defaultValue;

        public ICollection ValidateKeys { get; set; }
        public ICollection ValidateValues { get; set; }

        protected override void AddItem (object key, object value)
        {
            if (key == Defaut) {
                _defaultValue = value;
                return;
            }
            base.AddItem(key, value);
        }

        void ISupportInitialize.BeginInit ()
        {}

        void ISupportInitialize.EndInit ()
        {
            if (ValidateKeys != null)
                ValidateItems(ValidateKeys, (ICollection)Keys, "key");
            if (ValidateValues != null)
                ValidateItems(ValidateValues, (ICollection)Values, "value");
        }

        private void ValidateItems (ICollection expected, ICollection actual, string propName)
        {
            if (expected.Count != actual.Count)
                throw new ArgumentException("Validation of {0}s in DictionaryConverter failed: wrong item number (expected: {1}, actual: {2})."
                    .Fmt(propName, expected.Count, actual.Count));
            foreach (object value in expected)
                if (!actual.OfType<object>().Any(a => a.EqualsValue(value)))
                    throw new ArgumentException("Validation of {0}s in DictionaryConverter failed: {0} '{1}' is missing."
                        .Fmt(propName, value));
        }

        public object Convert (object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (ReferenceEquals(value, DependencyProperty.UnsetValue))
                return DependencyProperty.UnsetValue;
            return this.GetOrDefault(value, _defaultValue);
        }

        public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}