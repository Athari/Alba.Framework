using System;
using System.Windows.Markup;

namespace Alba.Framework.Windows.Markup
{
    public class ValueExtensionBase<TEnum> : MarkupExtension
    {
        public TEnum Value { get; set; }

        public ValueExtensionBase (TEnum value)
        {
            Value = value;
        }

        public override object ProvideValue (IServiceProvider provider)
        {
            return Value;
        }
    }
}