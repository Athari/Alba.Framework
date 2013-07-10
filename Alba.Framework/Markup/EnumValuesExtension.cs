using System;
using System.Collections;
using System.Windows.Markup;

namespace Alba.Framework.Markup
{
    [MarkupExtensionReturnType (typeof(ICollection))]
    public class EnumValuesExtension : MarkupExtension
    {
        public Type Type { get; set; }

        public EnumValuesExtension (Type type)
        {
            Type = type;
        }

        public override object ProvideValue (IServiceProvider provider)
        {
            return Enum.GetValues(Type);
        }
    }
}