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

namespace SharpDX.Direct2D1
{
    /// <summary>
    /// Internal DrawTransform Callback
    /// </summary>
    internal class DrawTransformShadow : TransformShadow
    {
        private static readonly DrawTransformVtbl Vtbl = new DrawTransformVtbl(0);

        /// <summary>
        /// Return a pointer to the unmanaged version of this callback.
        /// </summary>
        /// <param name="callback">The callback.</param>
        /// <returns>A pointer to a shadow c++ callback</returns>
        public static IntPtr ToIntPtr(DrawTransform callback)
        {
            return ToCallbackPtr<DrawTransform>(callback);
        }

        public class DrawTransformVtbl : TransformShadow.TransformVtbl
        {
            public DrawTransformVtbl(int methods) : base(1 + methods)
            {
                AddMethod(new SetDrawInfoDelegate(SetDrawInfoImpl));
            }

            /// <unmanaged>HRESULT ID2D1DrawTransform::SetDrawInfo([In] ID2D1DrawInfo* drawInfo)</unmanaged>	
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate int SetDrawInfoDelegate(IntPtr thisPtr, IntPtr drawInfo);
            private static int SetDrawInfoImpl(IntPtr thisPtr, IntPtr drawInfo)
            {
                try
                {
                    var shadow = ToShadow<DrawTransformShadow>(thisPtr);
                    var callback = (DrawTransform)shadow.Callback;
                    callback.SetDrawInformation(new DrawInformation(drawInfo));
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