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

namespace SharpDX.DirectInput
{
    /// <summary>
    /// Enumerator callback for DirectInput EnumDevices.
    /// </summary>
    internal class EnumDevicesCallback
    {
        private readonly IntPtr _nativePointer;
        private readonly DirectInputEnumDevicesDelegate _callback;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumDevicesCallback"/> class.
        /// </summary>
        public EnumDevicesCallback()
        {
            unsafe
            {
                _callback = new DirectInputEnumDevicesDelegate(DirectInputEnumDevicesImpl);
                _nativePointer = Marshal.GetFunctionPointerForDelegate(_callback);
                DeviceInstances = new List<DeviceInstance>();
            }
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
        /// Gets or sets the device instances.
        /// </summary>
        /// <value>The device instances.</value>
        public List<DeviceInstance> DeviceInstances { get; private set; }

        // typedef BOOL (FAR PASCAL * LPDIENUMDEVICESCALLBACKW)(LPCDIDEVICEINSTANCEW, LPVOID);
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private unsafe delegate int DirectInputEnumDevicesDelegate(void* deviceInstance, IntPtr data);
        private unsafe int  DirectInputEnumDevicesImpl(void* deviceInstance, IntPtr data)
        {
            var newDevice = new DeviceInstance();
            newDevice.__MarshalFrom(ref *((DeviceInstance.__Native*)deviceInstance));
            DeviceInstances.Add(newDevice);
            // Return true to continue iterating
            return 1;
        }
    }
}