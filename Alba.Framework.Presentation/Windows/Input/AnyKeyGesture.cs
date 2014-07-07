using System.ComponentModel;
using System.Globalization;
using System.Windows.Input;
using System.Windows.Markup;
using Alba.Framework.ComponentModel;
using Alba.Framework.Text;

namespace Alba.Framework.Windows.Input
{
    /// <summary>Like <see cref="KeyGesture"/>, but without stupid validation which doesn't allow single-key gestures.</summary>
    [TypeConverter (typeof(AnyKeyGestureConverter))]
    [ValueSerializer (typeof(AnyKeyGestureValueSerializer))]
    public class AnyKeyGesture : InputGesture
    {
        private static readonly TypeConverter _keyGestureConverter = new AnyKeyGestureConverter();

        public Key Key { get; private set; }
        public ModifierKeys Modifiers { get; private set; }
        public string DisplayString { get; private set; }

        public AnyKeyGesture (Key key, ModifierKeys modifiers = ModifierKeys.None, string displayString = "")
        {
            Modifiers = modifiers;
            Key = key;
            DisplayString = displayString ?? "";
        }

        public string GetDisplayStringForCulture (CultureInfo culture)
        {
            return !DisplayString.IsNullOrEmpty() ? DisplayString : _keyGestureConverter.ConvertToString(this, culture);
        }

        public override bool Matches (object targetElement, InputEventArgs args)
        {
            var keyArgs = args as KeyEventArgs;
            return keyArgs != null && Key == keyArgs.Key && Modifiers == Keyboard.Modifiers;
        }
    }
}