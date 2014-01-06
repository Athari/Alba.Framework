using System;
using System.Windows.Markup;

namespace Alba.Framework.Windows.Markup
{
    /// <summary></summary>
    /// <remarks>Named Bool and not BoolExtension to make access to constants cleaner.</remarks>
    public class Bool : MarkupExtension
    {
        public const bool True = true;
        public const bool False = false;

        public bool Value { get; private set; }

        public Bool (bool value)
        {
            Value = value;
        }

        public override object ProvideValue (IServiceProvider provider)
        {
            return Value;
        }
    }
}