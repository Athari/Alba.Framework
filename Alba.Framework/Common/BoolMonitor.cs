using System.Diagnostics.CodeAnalysis;

namespace Alba.Framework.Common;

[PublicAPI]
public readonly struct BoolMonitor : IDisposable
{
    private readonly Action _setIsChangingFalse;

    public BoolMonitor([SuppressMessage("ReSharper", "RedundantAssignment")] ref bool isChanging, Action setIsChangingFalse)
    {
        isChanging = true;
        _setIsChangingFalse = setIsChangingFalse;
    }

    public void Dispose()
    {
        _setIsChangingFalse();
    }
}