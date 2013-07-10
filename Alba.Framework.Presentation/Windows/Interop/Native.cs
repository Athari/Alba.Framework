using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using Alba.Framework.Interop;

namespace Alba.Framework.Windows.Interop
{
    public static class Native
    {
        private const int InvalidInt = Int32.MinValue;

        [DllImport (Dll.User, SetLastError = true)]
        private static extern uint GetWindowLong (IntPtr hWnd, GWL nIndex);

        [DllImport (Dll.User, SetLastError = true)]
        private static extern uint SetWindowLong (IntPtr hWnd, GWL nIndex, uint dwNewLong);

        [DllImport (Dll.User, SetLastError = true)]
        private static extern bool SetWindowPos (IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, SWP uFlags);

        [DllImport (Dll.User, SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage (IntPtr hWnd, WM Msg, IntPtr wParam, IntPtr lParam);

        [DllImport (Dll.User, SetLastError = true)]
        private static extern bool GetWindowPlacement (IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);

        [DllImport (Dll.User, SetLastError = true)]
        private static extern bool SetWindowPlacement (IntPtr hWnd, [In] ref WINDOWPLACEMENT lpwndpl);

        //[DllImport (Dll.User, CharSet = CharSet.Auto)]
        //private static extern IntPtr SendMessage (IntPtr hWnd, WM Msg, IntPtr wParam, string lParam);

        public static WS GetWindowStyle (this Window window)
        {
            return (WS)GetWindowLong(GetHandle(window), GWL.STYLE);
        }

        public static WS_EX GetWindowStyleEx (this Window window)
        {
            return (WS_EX)GetWindowLong(GetHandle(window), GWL.EXSTYLE);
        }

        public static WS SetWindowStyle (this Window window, WS style)
        {
            return (WS)SetWindowLong(GetHandle(window), GWL.STYLE, (uint)style);
        }

        public static WS_EX SetWindowStyleEx (this Window window, WS_EX style)
        {
            return (WS_EX)SetWindowLong(GetHandle(window), GWL.EXSTYLE, (uint)style);
        }

        public static void ClearWindowIcons (this Window window)
        {
            var hwnd = GetHandle(window);
            SendMessage(hwnd, WM.SETICON, ICON.BIG, IntPtr.Zero);
            SendMessage(hwnd, WM.SETICON, ICON.SMALL, IntPtr.Zero);
        }

        /// <summary>Changes the size, position, and Z order of a child, pop-up, or top-level window. These windows are ordered according to their appearance on the screen. The topmost window receives the highest rank and is the first window in the Z order.</summary>
        public static void SetWindowPos (this Window window, Window windowAfter = null,
            int left = InvalidInt, int top = InvalidInt, int width = InvalidInt, int height = InvalidInt, SWP flags = 0)
        {
            if (windowAfter == null)
                flags = flags | SWP.NOZORDER;
            if (left == InvalidInt || top == InvalidInt)
                flags = flags | SWP.NOMOVE;
            if (width == InvalidInt || height == InvalidInt)
                flags = flags | SWP.NOSIZE;
            if (!SetWindowPos(GetHandle(window), GetHandle(windowAfter), left, top, width, height, flags))
                throw new Win32Exception();
        }

        /// <summary>Retrieves the show state and the restored, minimized, and maximized positions of the specified window.</summary>
        public static WINDOWPLACEMENT GetWindowPlacement (this Window window)
        {
            var place = new WINDOWPLACEMENT { Length = Marshal.SizeOf(typeof(WINDOWPLACEMENT)) };
            if (GetWindowPlacement(GetHandle(window), ref place))
                return place;
            else
                throw new Win32Exception();
        }

        /// <summary>Sets the show state and the restored, minimized, and maximized positions of the specified window.</summary>
        public static void SetWindowPlacement (this Window window, WINDOWPLACEMENT place)
        {
            place.Length = Marshal.SizeOf(typeof(WINDOWPLACEMENT));
            if (!SetWindowPlacement(GetHandle(window), ref place))
                throw new Win32Exception();
        }

        public static bool IsHandleInitialized (this Window window)
        {
            return GetHandle(window) != IntPtr.Zero;
        }

        public static void InvokeOnHandle (this Window window, Action action)
        {
            EventHandler actionHandler = null;
            actionHandler = (s, a) => {
                action();
                window.SourceInitialized -= actionHandler;
            };
            if (window.IsHandleInitialized())
                action();
            else
                window.SourceInitialized += actionHandler;
        }

        private static IntPtr GetHandle (Window window)
        {
            return window != null ? new WindowInteropHelper(window).Handle : IntPtr.Zero;
        }
    }
}