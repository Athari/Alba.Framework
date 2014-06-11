using System.Windows.Data;
using System.Windows.Markup;

namespace Alba.Framework.Windows.Markup
{
    [MarkupExtensionReturnType (typeof(bool))]
    public class RadioBindingStatic : Binding
    {
        private static readonly IValueConverter _converter = new EqualsParamConverter();

        [ConstructorArgument ("value")]
        public object Value
        {
            get { return ConverterParameter; }
            set { ConverterParameter = value; }
        }

        public RadioBindingStatic ()
        {
            Converter = _converter;
        }

        public RadioBindingStatic (string path, object value) : base(path)
        {
            Converter = _converter;
            Value = value;
        }
    }
}