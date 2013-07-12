using System.Windows.Threading;

namespace Alba.Framework.Windows.Mvvm
{
    public interface IModel
    {
        Dispatcher Dispatcher { get; }
    }
}