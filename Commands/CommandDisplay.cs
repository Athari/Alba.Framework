using System.Windows.Media;

namespace Alba.Framework.Commands
{
    public class CommandDisplay
    {
        public EventCommand Command { get; set; }
        public string Label { get; set; }
        public ImageSource Icon { get; set; }
        public ImageSource LargeIcon { get; set; }
    }
}