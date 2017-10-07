// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using System.Runtime.InteropServices;
using SharpDX.XAudio2;

namespace SharpDX.XAPO.Fx
{
    internal partial class XAPOFx
    {
        internal static Guid CLSID_FXEcho = new Guid("5039D740-F736-449A-84D3-A56202557B87");
        internal static Guid CLSID_FXEQ = new Guid("F5E01117-D6C4-485A-A3F5-695196F3DBFA");
        internal static Guid CLSID_FXMasteringLimiter = new Guid("C4137916-2BE1-46FD-8599-441536F49856");
        internal static Guid CLSID_FXReverb = new Guid("7D9ACA56-CB68-4807-B632-B137352E8596");

        public static void CreateFX(SharpDX.XAudio2.XAudio2 device, System.Guid clsid, SharpDX.ComObject effectRef)
        {
#if !WINDOWS_UWP
            if (device.Version == XAudio2Version.Version27)
            {
                var clsid15 = clsid;
                if (clsid15 == CLSID_FXEcho)
                {
                    clsid15 = CLSID_FXEcho_15;
                }
                else if (clsid15 == CLSID_FXEQ)
                {
                    clsid15 = CLSID_FXEQ_15;
                }
                else if (clsid15 == CLSID_FXMasteringLimiter)
                {
                    clsid15 = CLSID_FXMasteringLimiter_15;
                }
                else if (clsid15 == CLSID_FXReverb)
                {
                    clsid15 = CLSID_FXReverb_15;
                }

                IntPtr nativePtr;
                var result = (Result)CreateFX15(ref clsid15, out nativePtr);
                if(result.Success)
                {
                    effectRef.NativePointer = nativePtr;
                    return;
                }
            }
#endif
            if(device.Version == XAudio2Version.Version28)
            {
                CreateFX28(clsid, effectRef, IntPtr.Zero, 0);
            }
#if WINDOWS_UWP
            else if (device.Version == XAudio2Version.Version29)
            {
                CreateFX29(clsid, effectRef, IntPtr.Zero, 0);
            }
#endif
            else
            {
                throw new NotSupportedException(string.Format("XAudio2 Version [{0}] is not supported for this effect", device.Version));
            }
        }
        /// <summary>Constant None.</summary>
        private static Guid CLSID_FXEcho_15 = new Guid("a90bc001-e897-e897-7439-435500000003");
        /// <summary>Constant None.</summary>
        private static Guid CLSID_FXEQ_15 = new Guid("a90bc001-e897-e897-7439-435500000000");
        /// <summary>Constant None.</summary>
        private static Guid CLSID_FXMasteringLimiter_15 = new Guid("a90bc001-e897-e897-7439-435500000001");
        /// <summary>Constant None.</summary>
        private static Guid CLSID_FXReverb_15 = new Guid("a90bc001-e897-e897-7439-435500000002");

#if !WINDOWS_UWP
        [DllImport("XAPOFX1_5.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "CreateFX")]
        private unsafe static extern int CreateFX15(ref Guid guid, out IntPtr arg1);
#endif
    }
}