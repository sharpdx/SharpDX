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

using SharpDX.Direct3D;

namespace SharpDX.Direct3D9
{
    public partial class Resource
    {
        /// <summary>
        /// Gets or sets the debug-name for this object.
        /// </summary>
        /// <value>
        /// The debug name.
        /// </value>
        public string DebugName
        {
            get
            {
                unsafe
                {
                    byte* pname = stackalloc byte[1024];
                    int size = 1024 - 1;
                    if (GetPrivateData(CommonGuid.DebugObjectName, new IntPtr(pname), ref size).Failure)
                        return string.Empty;
                    pname[size] = 0;
                    return Marshal.PtrToStringAnsi(new IntPtr(pname));
                }
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    SetPrivateData(CommonGuid.DebugObjectName, IntPtr.Zero, 0, 0);
                }
                else
                {
                    var namePtr = Marshal.StringToHGlobalAnsi(value);
                    SetPrivateData(CommonGuid.DebugObjectName, namePtr, value.Length, 0);
                }
            }
        }

        protected override void NativePointerUpdated(IntPtr oldNativePointer)
        {
            DisposeDevice();
            base.NativePointerUpdated(oldNativePointer);
        }

        protected override unsafe void Dispose(bool disposing)
        {
            if (disposing)
            {
                DisposeDevice();
            }
            base.Dispose(disposing);
        }

        private void DisposeDevice()
        {
            if (Device__ != null)
            {
                // Don't use Dispose() in order to avoid circular references with DeviceContext
                ((IUnknown)Device__).Release();
                Device__ = null;
            }
        }
    }
}