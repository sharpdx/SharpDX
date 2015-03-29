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

namespace SharpDX.DXGI
{
    /// <summary>
    /// Internal VirtualSurfaceUpdatesCallbackNative Callback
    /// </summary>
    internal class VirtualSurfaceUpdatesCallbackNativeShadow : SharpDX.ComObjectShadow
    {
        private static readonly VirtualSurfaceUpdatesCallbackNativeVtbl Vtbl = new VirtualSurfaceUpdatesCallbackNativeVtbl();

        /// <summary>
        /// Get a native callback pointer from a managed callback.
        /// </summary>
        /// <param name="virtualSurfaceUpdatesCallbackNative">The geometry sink.</param>
        /// <returns>A pointer to the unmanaged geometry sink counterpart</returns>
        public static IntPtr ToIntPtr(IVirtualSurfaceUpdatesCallbackNative virtualSurfaceUpdatesCallbackNative)
        {
            return ToCallbackPtr<IVirtualSurfaceUpdatesCallbackNative>(virtualSurfaceUpdatesCallbackNative);
        }

        public class VirtualSurfaceUpdatesCallbackNativeVtbl : ComObjectVtbl
        {
            public VirtualSurfaceUpdatesCallbackNativeVtbl() : base(1)
            {
                AddMethod(new UpdatesNeededDelegate(UpdatesNeededImpl));
            }

            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate int UpdatesNeededDelegate(IntPtr thisPtr);
            private static int UpdatesNeededImpl(IntPtr thisPtr)
            {
                try
                {
                    var shadow = ToShadow<VirtualSurfaceUpdatesCallbackNativeShadow>(thisPtr);
                    var callback = (IVirtualSurfaceUpdatesCallbackNative)shadow.Callback;
                    callback.UpdatesNeeded();
                }
                catch (Exception exception)
                {
                    return (int)SharpDX.Result.GetResultFromException(exception);
                }
                return Result.Ok.Code;
            }
        }

        protected override CppObjectVtbl GetVtbl
        {
            get { return Vtbl; }
        }
    }
}