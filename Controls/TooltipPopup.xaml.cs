using System.Windows.Input;

namespace Alba.Framework.Controls
{
    public partial class TooltipPopup
    {
        public const int ShadowMargin = 10;

        public TooltipPopup ()
        {
            InitializeComponent();
        }

        private void TooltipPopup_MouseDown (object sender, MouseButtonEventArgs e)
        {
            Hide();
        }
    }
}