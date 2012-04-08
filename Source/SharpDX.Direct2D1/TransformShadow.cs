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
using System.Runtime.InteropServices;

namespace SharpDX.Direct2D1
{
    /// <summary>
    /// Internal Transform Callback
    /// </summary>
    internal class TransformShadow : SharpDX.ComObjectShadow
    {
        private static readonly TransformVtbl Vtbl = new TransformVtbl();

        /// <summary>
        /// Return a pointer to the unamanged version of this callback.
        /// </summary>
        /// <param name="callback">The callback.</param>
        /// <returns>A pointer to a shadow c++ callback</returns>
        public static IntPtr ToIntPtr(Transform callback)
        {
            return ToIntPtr<Transform>(callback);
        }

        public class TransformVtbl : ComObjectVtbl
        {
            public TransformVtbl() : base(3)
            {
                AddMethod(new SetInputRectsDelegate(SetInputRectsImpl));
                AddMethod(new MapOutputRectToInputRectsDelegate(MapOutputRectToInputRectsImpl));
                AddMethod(new MapInputRectsToOutputRectDelegate(MapInputRectsToOutputRectImpl));
            }

            /// <unmanaged>HRESULT ID2D1Transform::SetInputRects([In, Buffer] const RECT* inputRects,[In] unsigned int inputRectsCount)</unmanaged>	
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate int SetInputRectsDelegate(IntPtr thisPtr, IntPtr inputRects, int inputRectsCount);
            private static int SetInputRectsImpl(IntPtr thisPtr, IntPtr inputRects, int inputRectsCount)
            {
                try
                {
                    var shadow = ToShadow<TransformShadow>(thisPtr);
                    var callback = (Transform)shadow.Callback;
                    var inputRectangles = new SharpDX.Rectangle[inputRectsCount];
                    Utilities.Read(inputRects, inputRectangles, 0, inputRectsCount);
                    callback.InputRectangles = inputRectangles;
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

            /// <unmanaged>HRESULT ID2D1Transform::MapOutputRectToInputRects([In] const RECT* outputRect,[Out, Buffer] RECT* inputRects,[In] unsigned int inputRectsCount)</unmanaged>	
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate int MapOutputRectToInputRectsDelegate(IntPtr thisPtr, IntPtr outputRect, IntPtr inputRects, int inputRectsCount);
            private unsafe static int MapOutputRectToInputRectsImpl(IntPtr thisPtr, IntPtr outputRect, IntPtr inputRects, int inputRectsCount)
            {
                try
                {
                    var shadow = ToShadow<TransformShadow>(thisPtr);
                    var callback = (Transform)shadow.Callback;
                    var inputRectangles = new SharpDX.Rectangle[inputRectsCount];
                    callback.MapOutputRectangleToInputRectangles(*(SharpDX.Rectangle*)outputRect, inputRectangles);
                    Utilities.Write(outputRect, inputRectangles, 0, inputRectsCount);
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

            /// <unmanaged>HRESULT ID2D1Transform::MapInputRectsToOutputRect([In, Buffer] const RECT* inputRects,[In] unsigned int inputRectsCount,[Out] RECT* outputRect)</unmanaged>	
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate int MapInputRectsToOutputRectDelegate(IntPtr thisPtr, IntPtr inputRects, int inputRectsCount, IntPtr outputRect);
            private unsafe static int MapInputRectsToOutputRectImpl(IntPtr thisPtr, IntPtr inputRects, int inputRectsCount, IntPtr outputRect)
            {
                try
                {
                    var shadow = ToShadow<TransformShadow>(thisPtr);
                    var callback = (Transform)shadow.Callback;
                    var inputRectangles = new SharpDX.Rectangle[inputRectsCount];
                    Utilities.Read(inputRects, inputRectangles, 0, inputRectsCount);
                    *(SharpDX.Rectangle*)outputRect =  callback.MapInputRectanglesToOutputRectangle(inputRectangles);
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

        protected override CppObjectVtbl GetVtbl
        {
            get { return Vtbl; }
        }
    }
}
#endif