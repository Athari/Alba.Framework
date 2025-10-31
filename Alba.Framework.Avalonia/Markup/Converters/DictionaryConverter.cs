using System.ComponentModel;
using System.Globalization;
using Alba.Framework.Collections;
using Alba.Framework.Common;
using Avalonia.Data.Converters;

namespace Alba.Framework.Avalonia.Markup.Converters;

public class DictionaryConverter : Map<object, object>, IValueConverter, ISupportInitialize
{
    public static readonly object Defaut = new NamedObject("DictionaryConverter.Default");

    private object? _defaultValue;

    public ICollection<object>? ValidateKeys { get; set; }
    public ICollection<object>? ValidateValues { get; set; }

    protected override void AddItem(object key, object value)
    {
        if (key == Defaut) {
            _defaultValue = value;
            return;
        }
        base.AddItem(key, value);
    }

    void ISupportInitialize.BeginInit() { }

    void ISupportInitialize.EndInit()
    {
        if (ValidateKeys != null)
            ValidateItems(ValidateKeys, Keys, "key");
        if (ValidateValues != null)
            ValidateItems(ValidateValues, Values, "value");
    }

    private static void ValidateItems(ICollection<object> expected, ICollection<object> actual, string propName)
    {
        if (expected.Count != actual.Count)
            throw new ArgumentException($"Validation of {propName}s in DictionaryConverter failed: wrong item number (expected: {expected.Count}, actual: {actual.Count}).");
        foreach (object value in expected)
            if (!actual.Any(a => a.EqualsValue(value)))
                throw new ArgumentException($"Validation of {propName}s in DictionaryConverter failed: {propName} '{value}' is missing.");
    }

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        this.Get(value!, _defaultValue);

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        throw new NotSupportedException();
}