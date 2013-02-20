using System.Windows.Threading;

namespace Alba.Framework.Mvvm.Models
{
    public interface IModel
    {
        Dispatcher Dispatcher { get; }
    }
}