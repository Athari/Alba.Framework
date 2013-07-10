using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Markup;
using Alba.Framework.Collections;
using Alba.Framework.Common;

namespace Alba.Framework.Windows.Markup.Converters
{
    [ContentProperty ("Converters")]
    public class ChainConverter : IValueConverter
    {
        public static readonly object PassParameter = new NamedObject("ChainConverter.PassParameter");

        public List<ValueConverterRef> Converters { get; private set; }

        public ChainConverter ()
        {
            Converters = new List<ValueConverterRef>();
        }

        public object Convert (object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Converters.Aggregate(value, (v, conv) =>
                conv.Converter.Convert(v, typeof(object), GetParam(conv, parameter), culture));
        }

        public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Converters.Inverse().Aggregate(value, (v, conv) =>
                conv.Converter.ConvertBack(v, typeof(object), GetParam(conv, parameter), culture));
        }

        private object GetParam (ValueConverterRef conv, object chainParameter)
        {
            return ReferenceEquals(conv.Parameter, PassParameter) ? chainParameter : conv.Parameter;
        }
    }

    public class ValueConverterRef
    {
        public IValueConverter Converter { get; set; }
        public object Parameter { get; set; }
    }
}