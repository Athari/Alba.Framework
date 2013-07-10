using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Alba.Framework.Interop
{
    //[SuppressUnmanagedCodeSecurity]
    public static class Native
    {
        /// <summary>Retrieves the thread identifier of the calling thread.</summary>
        [DllImport (Dll.Kernel, SetLastError = true, ExactSpelling = true)]
        public static extern uint GetCurrentThreadId ();

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