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
    /// Enumerator callback for DirectInput EnumObjects.
    /// </summary>
    internal class EnumObjectsCallback
    {
        private readonly IntPtr _nativePointer;
        private readonly DirectInputEnumObjectsDelegate _callback;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumObjectsCallback"/> class.
        /// </summary>
        public EnumObjectsCallback()
        {
            unsafe
            {
                _callback = new DirectInputEnumObjectsDelegate(DirectInputEnumObjectsImpl);
                _nativePointer = Marshal.GetFunctionPointerForDelegate(_callback);
                Objects = new List<DeviceObjectInstance>();
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
        /// Gets or sets the device object instances.
        /// </summary>
        /// <value>The device object instances.</value>
        public List<DeviceObjectInstance> Objects { get; private set; }

        // BOOL DIEnumObjectsCallback(LPCDIEffectInfo pdei,LPVOID pvRef)
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private unsafe delegate int DirectInputEnumObjectsDelegate(void* deviceInstance, IntPtr data);
        private unsafe int DirectInputEnumObjectsImpl(void* deviceInstance, IntPtr data)
        {
            var newObject = new DeviceObjectInstance();
            newObject.__MarshalFrom(ref *((DeviceObjectInstance.__Native*)deviceInstance));
            Objects.Add(newObject);
            // Return true to continue iterating
            return 1;
        }
    }
}