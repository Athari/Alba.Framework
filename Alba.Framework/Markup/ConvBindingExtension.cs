using System.Windows.Data;

namespace Alba.Framework.Markup
{
    public class ConvBindingExtension : Binding
    {
        public ConvBindingExtension ()
        {}

        public ConvBindingExtension (string path) : base(path)
        {}

        public ConvBindingExtension (string path, IValueConverter converter) : base(path)
        {
            Converter = converter;
        }

        public ConvBindingExtension (string path, IValueConverter converter, object param) : base(path)
        {
            Converter = converter;
            ConverterParameter = param;
        }
    }
}