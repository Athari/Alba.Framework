using System;

namespace Alba.Framework.Windows.Controls
{
    [Flags]
    public enum DialogButton
    {
        None = 0,
        Ok = 1 << 0,
        Cancel = 1 << 1,
        Yes = 1 << 2,
        No = 1 << 3,
        Close = 1 << 4,

        OkCancel = Ok | Cancel,
        YesNo = Yes | No,
        YesNoCancel = Yes | No | Cancel,
    }
}