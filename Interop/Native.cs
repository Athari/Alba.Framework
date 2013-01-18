using System;
using System.ComponentModel;
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

        /// <summary>Retrieves the thread identifier of the calling thread.</summary>
        [DllImport (Dll.Kernel, SetLastError = true, ExactSpelling = true)]
        public static extern uint GetCurrentThreadId ();

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

        [DllImport (Dll.ShellLight, SetLastError = true, CharSet = CharSet.Auto)]
        private static extern uint AssocQueryString (ASSOCF flags, ASSOCSTR str, string pszAssoc, string pszExtra,
            [Out] StringBuilder pszOut, [In] [Out] ref int pcchOut);

        //[DllImport (Dll.User, SetLastError = true)]
        //private static extern bool SystemParametersInfo (SPI uiAction, uint uiParam, IntPtr pvParam, SPIF fWinIni);
        //[DllImport (Dll.User, SetLastError = true)]
        //private static extern bool SystemParametersInfo (SPI uiAction, uint uiParam, string pvParam, SPIF fWinIni);
        //[DllImport (Dll.User, SetLastError = true)]
        //private static extern bool SystemParametersInfo (SPI uiAction, uint uiParam, ref bool pvParam, SPIF fWinIni);
        //[DllImport (Dll.User, SetLastError = true)]
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

        private static IntPtr GetHandle (Window window)
        {
            return window != null ? new WindowInteropHelper(window).Handle : IntPtr.Zero;
        }

        /// <summary>Searches for and retrieves a file or protocol association-related string from the registry.</summary>
        /// <param name="flags">The flags that can be used to control the search. It can be any combination of <see cref="ASSOCF"/> values, except that only one ASSOCF.INIT value can be included.</param>
        /// <param name="assocStr">The <see cref="ASSOCSTR"/> value that specifies the type of string that is to be returned.</param>
        /// <param name="doctype">A string that is used to determine the root key. The following four types of strings can be used. File name extension: A file name extension, such as .txt. CLSID: A CLSID GUID in the standard "{GUID}" format. ProgID: An application's ProgID, such as Word.Document.8. Executable name: The name of an application's .exe file. The <see cref="ASSOCF.OPEN_BYEXENAME"/> flag must be set in flags.</param>
        /// <param name="extra">An optional string with additional information about the location of the string. It is typically set to a Shell verb such as open. Set this parameter to NULL if it is not used.</param>
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

        public static void Inc<T> (ref IntPtr ptr)
        {
            ptr = (IntPtr)((int)ptr + Marshal.SizeOf(typeof(T)));
        }

        public static void StructureToPtrInc<T> (T structure, ref IntPtr ptr, bool fDeleteOld = false)
        {
            Marshal.StructureToPtr(structure, ptr, fDeleteOld);
            Inc<T>(ref ptr);
        }

        public static IntPtr AllocHGlobalArray<T> (int size)
        {
            return Marshal.AllocHGlobal(Marshal.SizeOf(typeof(T)) * size);
        }

        public static void FreeHGlobalArray<T> (IntPtr hglobal, int size)
        {
            if (hglobal == IntPtr.Zero)
                return;
            IntPtr ptr = hglobal;
            for (int i = 0; i < size; ++i) {
                Marshal.DestroyStructure(ptr, typeof(T));
                Inc<T>(ref ptr);
            }
            Marshal.FreeHGlobal(hglobal);
        }
    }
}