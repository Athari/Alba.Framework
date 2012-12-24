using System;
using System.Globalization;
using System.Windows.Data;

namespace Alba.Framework.Markup
{
    public class StaticExtension : System.Windows.Markup.StaticExtension
    {
        public string StringFormat { get; set; }

        public IValueConverter Converter { get; set; }

        public object ConverterParameter { get; set; }

        public StaticExtension ()
        {}

        public StaticExtension (string member) : base(member)
        {}

        public override object ProvideValue (IServiceProvider provider)
        {
            object value = base.ProvideValue(provider);
            if (Converter != null)
                value = Converter.Convert(value, typeof(object), ConverterParameter, CultureInfo.CurrentCulture);
            if (StringFormat != null)
                value = string.Format(CultureInfo.CurrentCulture, StringFormat, value);
            return value;
        }
    }
}