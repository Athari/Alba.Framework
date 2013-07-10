using System;
using System.Runtime.InteropServices;

namespace Alba.Framework.Interop
{
    /// <summary>Contains information about the placement of a window on the screen.</summary>
    [Serializable]
    [StructLayout (LayoutKind.Sequential)]
    public struct WINDOWPLACEMENT
    {
        [NonSerialized]
        public static WINDOWPLACEMENT Invalid = new WINDOWPLACEMENT { Length = -1 };

        /// <summary>The length of the structure, in bytes. Before calling the GetWindowPlacement or SetWindowPlacement functions, set this member to sizeof(WINDOWPLACEMENT). GetWindowPlacement and SetWindowPlacement fail if this member is not set correctly.</summary>
        [NonSerialized]
        public int Length;
        /// <summary>Specifies flags that control the position of the minimized window and the method by which the window is restored.</summary>
        public WPF Flags;
        /// <summary>The current show state of the window.</summary>
        public SW ShowCmd;
        /// <summary>The coordinates of the window's upper-left corner when the window is minimized.</summary>
        public POINT MinPosition;
        /// <summary>The coordinates of the window's upper-left corner when the window is maximized.</summary>
        public POINT MaxPosition;
        /// <summary>The window's coordinates when the window is in the restored position.</summary>
        public RECT NormalPosition;

        public bool IsEmpty
        {
            get { return Flags == 0 && ShowCmd == 0 && MinPosition.IsEmpty && MaxPosition.IsEmpty && NormalPosition.IsEmpty; }
        }
    }
}