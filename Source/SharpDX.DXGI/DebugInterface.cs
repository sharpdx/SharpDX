using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace SharpDX.DXGI
{
    internal static class DebugInterface
    {
        [UnmanagedFunctionPointer(CallingConvention.Winapi)]
        private delegate Result GetDebugInterface(ref Guid guid, out IntPtr result);

        private static readonly GetDebugInterface getDebugInterface;

        static DebugInterface()
        {
            // https://blogs.msdn.microsoft.com/chuckw/2015/07/27/dxgi-debug-device/
#if DESKTOP_APP
            IntPtr moduleHandle = Kernel32.LoadLibraryEx("dxgidebug.dll", IntPtr.Zero, Kernel32.LoadLibraryFlags.LoadLibrarySearchSystem32);
            if (moduleHandle != IntPtr.Zero)
            {
                IntPtr procedureHandle = Kernel32.GetProcAddress(moduleHandle, "DXGIGetDebugInterface");
                if (procedureHandle != IntPtr.Zero)
                {
                    getDebugInterface = (GetDebugInterface)Marshal.GetDelegateForFunctionPointer(procedureHandle, typeof(GetDebugInterface));
                }
            }
#else
            getDebugInterface = null;
#endif
        }

        public static bool TryCreateComPtr<T>(out IntPtr comPtr) where T : class
        {
            comPtr = IntPtr.Zero;
            if (getDebugInterface == null)
                return false;

            var guid = typeof(T).GetTypeInfo().GUID;
            var result = getDebugInterface(ref guid, out comPtr);
            if (result.Failure)
                return false;

            return comPtr != IntPtr.Zero;
        }

    }
}