using System;

namespace Alba.Framework.Windows.Controls
{
    [Flags]
    public enum WindowButton
    {
        None = 0,
        System = 1 << 0,
        Minimize = 1 << 1,
        Maximize = 1 << 2,
        Help = 1 << 3,
        Icon = 1 << 4,

        Default = Icon | System | Minimize | Maximize,
        Dialog = System,
    }
}