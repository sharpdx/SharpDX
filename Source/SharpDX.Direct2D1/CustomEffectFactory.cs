// Copyright (c) 2010-2011 SharpDX - Alexandre Mutel
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
#if WIN8
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace SharpDX.Direct2D1
{
    /// <summary>
    /// Delegate used by to create a custom effect.
    /// </summary>
    /// <returns>A new instance of custom effect</returns>
    internal delegate CustomEffect CustomEffectFactoryDelegate();

    /// <summary>
    /// Internal class used to keep reference to factory.
    /// </summary>
    class CustomEffectFactory
    {
        private CreateCustomEffectDelegate callback;

        public CustomEffectFactory(CustomEffectFactoryDelegate factory)
        {
            unsafe
            {
                Factory = factory;
                callback = new CreateCustomEffectDelegate(CreateCustomEffectImpl);
                NativePointer = Marshal.GetFunctionPointerForDelegate(callback);
            }
        }

        public CustomEffectFactoryDelegate Factory { get; private set; }

        public IntPtr NativePointer { get; private set; }

        /// <unmanaged>HRESULT ID2D1EffectImpl::Initialize([In] ID2D1EffectContext* effectContext,[In] ID2D1TransformGraph* transformGraph)</unmanaged>	
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int CreateCustomEffectDelegate(out IntPtr nativeCustomEffectPtr);
        private int CreateCustomEffectImpl(out IntPtr nativeCustomEffectPtr)
        {
            nativeCustomEffectPtr = IntPtr.Zero;
            try
            {
                var customEffect = Factory();
                nativeCustomEffectPtr = CustomEffectShadow.ToIntPtr(customEffect);
            }
            catch (SharpDXException exception)
            {
                return exception.ResultCode.Code;
            }
            catch (Exception)
            {
                return Result.Fail.Code;
            }
            return Result.Ok.Code;
        }
    }
}
#endif