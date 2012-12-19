using System;

namespace Alba.Framework.Mvvm.Dialogs
{
    [Flags]
    public enum DialogButton
    {
        None = 0,
        Ok = 1 << 0,
        Cancel = 1 << 1,
        Yes = 1 << 2,
        No = 1 << 3,

        OkCancel = Ok | Cancel,
        YesNo = Yes | No,
        YesNoCancel = Yes | No | Cancel,
    }
}