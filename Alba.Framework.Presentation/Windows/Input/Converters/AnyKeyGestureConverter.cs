using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Input;
using Alba.Framework.ComponentModel;

// ReSharper disable CanBeReplacedWithTryCastAndCheckForNull
namespace Alba.Framework.Windows.Input
{
    public class AnyKeyGestureConverter : TypeConverter
    {
        private const char ModifiersDelimiter = '+';
        private const char DisplayStringSeparator = ',';

        private static readonly KeyConverter _keyConverter = new KeyConverter();
        private static readonly ModifierKeysConverter _modifierKeysConverter = new ModifierKeysConverter();

        public override bool CanConvertFrom (ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override bool CanConvertTo (ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(string);
        }

        public override object ConvertFrom (ITypeDescriptorContext context, CultureInfo culture, object source)
        {
            if (source is string) {
                var str = (string)source;
                string fullName = str.Trim();
                if (fullName == "")
                    return new AnyKeyGesture(Key.None);

                string keyToken;
                string modifiersToken;
                string displayString;

                // Break apart display string
                int index = fullName.IndexOf(DisplayStringSeparator);
                if (index >= 0) {
                    displayString = fullName.Substring(index + 1).Trim();
                    fullName = fullName.Substring(0, index).Trim();
                }
                else {
                    displayString = "";
                }

                // Break apart key and modifiers
                index = fullName.LastIndexOf(ModifiersDelimiter);
                if (index >= 0) {
                    modifiersToken = fullName.Substring(0, index);
                    keyToken = fullName.Substring(index + 1);
                }
                else {
                    modifiersToken = "";
                    keyToken = fullName;
                }

                return new AnyKeyGesture(
                    _keyConverter.ConvertFromString<Key>(keyToken, culture, context),
                    _modifierKeysConverter.ConvertFromString<ModifierKeys>(modifiersToken, culture, context),
                    displayString);
            }
            throw GetConvertFromException(source);
        }

        public override object ConvertTo (ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string)) {
                if (value == null)
                    return "";
                var keyGesture = value as AnyKeyGesture;
                if (keyGesture == null)
                    throw GetConvertToException(value, destinationType);
                if (keyGesture.Key == Key.None)
                    return "";
                string strKey = _keyConverter.ConvertToString(keyGesture.Key, culture, context);
                if (strKey == "")
                    return "";
                string strModifiers = _modifierKeysConverter.ConvertToString(keyGesture.Modifiers, culture, context);
                return (strModifiers != "" ? strModifiers + ModifiersDelimiter : "")
                    + strKey
                    + (keyGesture.DisplayString != "" ? DisplayStringSeparator + keyGesture.DisplayString : "");
            }
            throw GetConvertToException(value, destinationType);
        }
    }
}