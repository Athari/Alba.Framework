using System.Windows.Threading;

namespace Alba.Framework.Windows.Mvvm.Models
{
    public interface IModel
    {
        Dispatcher Dispatcher { get; }
    }
}