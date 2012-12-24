using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Interop;

namespace Alba.Framework.Interop
{
    //[SuppressUnmanagedCodeSecurity]
    public static class Native
    {
        private const int InvalidInt = Int32.MinValue;

        [DllImport ("user32.dll", SetLastError = true)]
        private static extern uint GetWindowLong (IntPtr hWnd, GWL nIndex);

        [DllImport ("user32.dll")]
        private static extern uint SetWindowLong (IntPtr hWnd, GWL nIndex, uint dwNewLong);

        [DllImport ("user32.dll")]
        private static extern bool SetWindowPos (IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, SWP uFlags);

        [DllImport ("Shlwapi.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern uint AssocQueryString (ASSOCF flags, ASSOCSTR str, string pszAssoc, string pszExtra,
            [Out] StringBuilder pszOut, [In] [Out] ref int pcchOut);

        //[DllImport ("user32.dll")]
        //private static extern IntPtr SendMessage (IntPtr hWnd, WM Msg, Int32 wParam, Int32 lParam);
        //[DllImport ("user32.dll", CharSet = CharSet.Auto)]
        //private static extern IntPtr SendMessage (IntPtr hWnd, WM Msg, IntPtr wParam, IntPtr lParam);
        //[DllImport ("user32.dll", CharSet = CharSet.Auto)]
        //private static extern IntPtr SendMessage (IntPtr hWnd, WM Msg, IntPtr wParam, string lParam);

        //[DllImport ("user32.dll", SetLastError = true)]
        //private static extern bool SystemParametersInfo (SPI uiAction, uint uiParam, IntPtr pvParam, SPIF fWinIni);
        //[DllImport ("user32.dll", SetLastError = true)]
        //private static extern bool SystemParametersInfo (SPI uiAction, uint uiParam, string pvParam, SPIF fWinIni);
        //[DllImport ("user32.dll", SetLastError = true)]
        //private static extern bool SystemParametersInfo (SPI uiAction, uint uiParam, ref bool pvParam, SPIF fWinIni);
        //[DllImport ("user32.dll", SetLastError = true)]
        //private static extern bool SystemParametersInfo (SPI uiAction, uint uiParam, ref int pvParam, SPIF fWinIni);

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

        public static bool SetWindowPos (this Window window, Window windowAfter = null,
            int left = InvalidInt, int top = InvalidInt, int width = InvalidInt, int height = InvalidInt, SWP flags = 0)
        {
            if (windowAfter == null)
                flags = flags | SWP.NOZORDER;
            if (left == InvalidInt || top == InvalidInt)
                flags = flags | SWP.NOMOVE;
            if (width == InvalidInt || height == InvalidInt)
                flags = flags | SWP.NOSIZE;
            return SetWindowPos(GetHandle(window), GetHandle(windowAfter), left, top, width, height, flags);
        }

        public static bool IsHandleInitialized (this Window window)
        {
            return GetHandle(window) != IntPtr.Zero;
        }

        private static IntPtr GetHandle (Window window)
        {
            return window != null ? new WindowInteropHelper(window).Handle : IntPtr.Zero;
        }

        public static string AssocQueryString (ASSOCF flags, ASSOCSTR assocStr, string doctype, string extra = null)
        {
            int bufferSize = 0;
            AssocQueryString(flags, assocStr, doctype, extra, null, ref bufferSize);
            if (bufferSize == 0)
                return "";
            var assoc = new StringBuilder(bufferSize);
            AssocQueryString(flags, assocStr, doctype, extra, assoc, ref bufferSize);
            return assoc.ToString();
        }
    }
}