using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using Alba.Framework.Collections;
using Alba.Framework.Common;
using Alba.Framework.Linq;
using Alba.Framework.Text;

namespace Alba.Framework.Windows.Markup
{
    [ContentProperty ("Pairs")]
    public class DictionaryConverter : IValueConverter
    {
        public static readonly object Defaut = new NamedObject("DictionaryConverter.Default");

        private static readonly string FromPropName = Props.GetName((IFromTo o) => o.From);
        private static readonly string ToPropName = Props.GetName((IFromTo o) => o.To);

        public ICollection<IFromTo> Pairs { get; private set; }

        public DictionaryConverter ()
        {
            var pairs = new ObservableCollectionEx<IFromTo>();
            pairs.CollectionChanged += Pairs_OnCollectionChanged;
            Pairs = pairs;
        }

        private void Pairs_OnCollectionChanged (object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add) {
                List<IFromTo> actualPairs = Pairs.Where(p => !(p is ValidateFromToBase)).ToList();
                foreach (var validateFrom in e.NewItems.OfType<ValidateFromValues>())
                    ValidatePairs(validateFrom.From, actualPairs, ft => ft.From, FromPropName);
                foreach (var validateTo in e.NewItems.OfType<ValidateToValues>())
                    ValidatePairs(validateTo.To, actualPairs, ft => ft.To, ToPropName);
                Pairs.RemoveRange(Pairs.OfType<ValidateFromToBase>().ToList());
            }
        }

        private void ValidatePairs (ICollection expectedValues, ICollection<IFromTo> actualPairs, Func<IFromTo, object> getValue, string propName)
        {
            if (expectedValues.Count != actualPairs.Count)
                throw new ArgumentException("Validation of {0} values in DictionaryConverter failed: wrong item number (expected: {1}, actual: {2})."
                    .Fmt(propName, expectedValues.Count, actualPairs.Count));
            foreach (object value in expectedValues)
                if (!actualPairs.Any(p => Equals(getValue(p), value)))
                    throw new ArgumentException("Validation of {0} values in DictionaryConverter failed: value '{1}' is missing."
                        .Fmt(propName, value));
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

    public abstract class FromToBase<TFrom, TTo> : IFromTo
    {
        public TFrom From { get; set; }
        public TTo To { get; set; }

        object IFromTo.From
        {
            get { return From; }
        }

        object IFromTo.To
        {
            get { return To; }
        }
    }

    public class FromTo : FromToBase<object, object>
    {}

    public class FromToImage : FromToBase<object, ImageSource>
    {}

    public abstract class ValidateFromToBase : IFromTo
    {
        object IFromTo.From
        {
            get { throw new NotSupportedException(); }
        }

        object IFromTo.To
        {
            get { throw new NotSupportedException(); }
        }
    }

    public class ValidateFromValues : ValidateFromToBase
    {
        public ICollection From { get; set; }
    }

    public class ValidateToValues : ValidateFromToBase
    {
        public ICollection To { get; set; }
    }
}