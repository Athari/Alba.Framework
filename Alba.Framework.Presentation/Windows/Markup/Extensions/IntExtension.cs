using System;
using System.Windows.Markup;

namespace Alba.Framework.Windows.Markup
{
    public class IntExtension : MarkupExtension
    {
        public int Value { get; private set; }

        public IntExtension (int value)
        {
            Value = value;
        }

        public override object ProvideValue (IServiceProvider provider)
        {
            return Value;
        }
    }
}