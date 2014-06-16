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
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SharpDX.DirectSound
{
    /// <summary>
    /// Enumerator callback for DirectSound and DirectCaptureSound.
    /// </summary>
    internal class EnumDelegateCallback
    {
        private IntPtr _nativePointer;
        private DirectSoundEnumDelegate _callback;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumDelegateCallback"/> class.
        /// </summary>
        public EnumDelegateCallback()
        {
            _callback = new DirectSoundEnumDelegate(DirectSoundEnumImpl);
            _nativePointer =  Marshal.GetFunctionPointerForDelegate(_callback);
            Informations = new List<DeviceInformation>();
        }

        /// <summary>
        /// Natives the pointer.
        /// </summary>
        /// <returns></returns>
        public IntPtr NativePointer
        {
            get { return _nativePointer; }
        }

        /// <summary>
        /// Gets or sets the device informations.
        /// </summary>
        /// <value>The device informations.</value>
        public List<DeviceInformation> Informations { get; private set; }

        // typedef BOOL (CALLBACK *LPDSENUMCALLBACKW)(LPGUID, LPCWSTR, LPCWSTR, LPVOID);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int DirectSoundEnumDelegate(IntPtr guid, IntPtr description, IntPtr module, IntPtr lpContext);
        private int DirectSoundEnumImpl(IntPtr guidPtr, IntPtr description, IntPtr module, IntPtr lpContext)
        {
            unsafe
            {
                Guid guid;
                if (guidPtr == IntPtr.Zero)
                    guid = Guid.Empty;
                else
                {
                    guid = *((Guid*) guidPtr);
                }

                Informations.Add(new DeviceInformation(guid, Marshal.PtrToStringUni( description), Marshal.PtrToStringUni( module)));
            }
            // Return true to continue enumerate the devices.
            return 1;
        }
    }
}
