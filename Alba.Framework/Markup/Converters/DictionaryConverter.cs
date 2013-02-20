using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using Alba.Framework.Common;

namespace Alba.Framework.Markup.Converters
{
    [ContentProperty ("Pairs")]
    public class DictionaryConverter : IValueConverter
    {
        public static readonly object Defaut = new NamedObject("DictionaryConverter.Default");

        public List<IFromTo> Pairs { get; private set; }

        public DictionaryConverter ()
        {
            Pairs = new List<IFromTo>();
        }

        public object Convert (object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (ReferenceEquals(value, DependencyProperty.UnsetValue))
                return DependencyProperty.UnsetValue;
            object def = null;
            foreach (IFromTo pair in Pairs) {
                if (Equals(pair.From, value))
                    return pair.To;
                if (ReferenceEquals(pair.From, Defaut))
                    def = pair.To;
            }
            return def;
        }

        public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    public interface IFromTo
    {
        object From { get; }
        object To { get; }
    }

    public class FromTo : IFromTo
    {
        public object From { get; set; }
        public object To { get; set; }
    }

    public class FromToImage : IFromTo
    {
        public object From { get; set; }
        public ImageSource To { get; set; }
        object IFromTo.To
        {
            get { return To; }
        }
    }
}