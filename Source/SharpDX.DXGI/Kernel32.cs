#if DESKTOP_APP
using System;
using System.Runtime.InteropServices;

namespace SharpDX.DXGI {
    internal static class Kernel32
    {
        [Flags]
        public enum LoadLibraryFlags : uint
        {
            DontResolveDllReferences = 0x00000001,
            LoadIgnoreCodeAuthzLevel = 0x00000010,
            LoadLibraryAsDatafile = 0x00000002,
            LoadLibraryAsDatafileExclusive = 0x00000040,
            LoadLibraryAsImageResource = 0x00000020,
            LoadLibrarySearchApplicationDir = 0x00000200,
            LoadLibrarySearchDefaultDirs = 0x00001000,
            LoadLibrarySearchDllLoadDir = 0x00000100,
            LoadLibrarySearchSystem32 = 0x00000800,
            LoadLibrarySearchUserDirs = 0x00000400,
            LoadWithAlteredSearchPath = 0x00000008
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr LoadLibraryEx(string lpFileName, IntPtr hReservedNull, LoadLibraryFlags dwFlags);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procName);
    }
}
#endif