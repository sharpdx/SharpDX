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
using SharpDX.Mathematics.Interop;

namespace SharpDX.Direct2D1
{
    /// <summary>
    /// Internal SourceTransform Callback
    /// </summary>
    internal class SourceTransformShadow : TransformShadow
    {
        private static readonly SourceTransformVtbl Vtbl = new SourceTransformVtbl(0);

        /// <summary>
        /// Return a pointer to the unmanaged version of this callback.
        /// </summary>
        /// <param name="callback">The callback.</param>
        /// <returns>A pointer to a shadow c++ callback</returns>
        public static IntPtr ToIntPtr(SourceTransform callback)
        {
            return ToCallbackPtr<SourceTransform>(callback);
        }

        public class SourceTransformVtbl : TransformShadow.TransformVtbl
        {
            public SourceTransformVtbl(int methods) : base(2 + methods)
            {
                AddMethod(new SetRenderInformationDelegate(SetRenderInformationImpl));
                AddMethod(new DrawDelegate(DrawImpl));
            }


            /// <unmanaged>HRESULT ID2D1SourceTransform::SetRenderInfo([In] ID2D1RenderInfo* renderInfo)</unmanaged>	
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate int SetRenderInformationDelegate(IntPtr thisPtr, IntPtr renderInfo);
            private static int SetRenderInformationImpl(IntPtr thisPtr, IntPtr renderInfo)
            {
                try
                {
                    var shadow = ToShadow<SourceTransformShadow>(thisPtr);
                    var callback = (SourceTransform)shadow.Callback;
                    callback.SetRenderInformation(new RenderInformation(renderInfo));
                }
                catch (Exception exception)
                {
                    return (int)SharpDX.Result.GetResultFromException(exception);
                }
                return Result.Ok.Code;
            }

            /// <unmanaged>HRESULT ID2D1SourceTransform::Draw([In] ID2D1Bitmap1* target,[In] const RECT* drawRect,[In] D2D_POINT_2U targetOrigin)</unmanaged>	
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate int DrawDelegate(IntPtr thisPtr, IntPtr target, IntPtr drawRect, RawPoint targetOrigin);
            private unsafe static int DrawImpl(IntPtr thisPtr, IntPtr target, IntPtr drawRect, RawPoint targetOrigin)
            {
                try
                {
                    var shadow = ToShadow<SourceTransformShadow>(thisPtr);
                    var callback = (SourceTransform)shadow.Callback;
                    callback.Draw(new Bitmap1(target), *(RawRectangle*)drawRect, targetOrigin);
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