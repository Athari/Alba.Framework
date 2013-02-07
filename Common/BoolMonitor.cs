using System;

// ReSharper disable RedundantAssignment
namespace Alba.Framework.Common
{
    public struct BoolMonitor : IDisposable
    {
        private readonly Action _setIsChangingFalse;

        public BoolMonitor (ref bool isChanging, Action setIsChangingFalse)
        {
            isChanging = true;
            _setIsChangingFalse = setIsChangingFalse;
        }

        public void Dispose ()
        {
            _setIsChangingFalse();
        }
    }
}