using System.Windows.Data;

namespace Alba.Framework.Windows.Markup
{
    public class MultiBinding : System.Windows.Data.MultiBinding
    {
        public MultiBinding (BindingBase b1, BindingBase b2)
        {
            Bindings.Add(b1);
            Bindings.Add(b2);
        }

        public MultiBinding (BindingBase b1, BindingBase b2, BindingBase b3)
        {
            Bindings.Add(b1);
            Bindings.Add(b2);
            Bindings.Add(b3);
        }
    }
}