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
    /// Internal Transform Callback
    /// </summary>
    internal class TransformShadow : TransformNodeShadow
    {
        private static readonly TransformVtbl Vtbl = new TransformVtbl(0);

        /// <summary>
        /// Return a pointer to the unmanaged version of this callback.
        /// </summary>
        /// <param name="callback">The callback.</param>
        /// <returns>A pointer to a shadow c++ callback</returns>
        public static IntPtr ToIntPtr(Transform callback)
        {
            return ToCallbackPtr<Transform>(callback);
        }

        public class TransformVtbl : TransformNodeShadow.TransformNodeVtbl
        {
            public TransformVtbl(int methods) : base(3 + methods)
            {
                AddMethod(new MapOutputRectToInputRectsDelegate(MapOutputRectToInputRectsImpl));
                AddMethod(new MapInputRectsToOutputRectDelegate(MapInputRectsToOutputRectImpl));
                AddMethod(new MapInvalidRectDelegate(MapInvalidRectImpl));
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
                    var inputRectangles = new RawRectangle[inputRectsCount];
                    Utilities.Read(inputRects, inputRectangles, 0, inputRectsCount);
                    callback.MapOutputRectangleToInputRectangles(*(RawRectangle*)outputRect, inputRectangles);
                    Utilities.Write(inputRects, inputRectangles, 0, inputRectsCount);
                }
                catch (Exception exception)
                {
                    return (int)SharpDX.Result.GetResultFromException(exception);
                }
                return Result.Ok.Code;
            }

            /// <unmanaged>HRESULT ID2D1Transform::MapInputRectsToOutputRect([In, Buffer] const RECT* inputRects,[In] unsigned int inputRectsCount,[Out] RECT* outputRect)</unmanaged>	
            /// <unmanaged>HRESULT ID2D1Transform::MapInputRectsToOutputRect([In, Buffer] const RECT* inputRects,[In, Buffer] const RECT* inputOpaqueSubRects,[In] unsigned int inputRectCount,[Out] RECT* outputRect,[Out] RECT* outputOpaqueSubRect)</unmanaged>	
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate int MapInputRectsToOutputRectDelegate(IntPtr thisPtr, IntPtr inputRects, IntPtr inputOpaqueSubRects, int inputRectsCount, IntPtr outputRect, IntPtr outputOpaqueSubRect);
            private unsafe static int MapInputRectsToOutputRectImpl(IntPtr thisPtr, IntPtr inputRects, IntPtr inputOpaqueSubRects, int inputRectsCount, IntPtr outputRect, IntPtr outputOpaqueSubRect)
            {
                try
                {
                    var shadow = ToShadow<TransformShadow>(thisPtr);
                    var callback = (Transform)shadow.Callback;
                    var inputRectangles = new RawRectangle[inputRectsCount];
                    Utilities.Read(inputRects, inputRectangles, 0, inputRectsCount);
                    var inputOpaqueSubRectangles = new RawRectangle[inputRectsCount];
                    Utilities.Read(inputOpaqueSubRects, inputOpaqueSubRectangles, 0, inputRectsCount);
                    *(RawRectangle*)outputRect = callback.MapInputRectanglesToOutputRectangle(inputRectangles, inputOpaqueSubRectangles, out *(RawRectangle*)outputOpaqueSubRect);
                }
                catch (Exception exception)
                {
                    return (int)SharpDX.Result.GetResultFromException(exception);
                }
                return Result.Ok.Code;
            }

            /// <unmanaged>HRESULT ID2D1Transform::MapInvalidRect([In] unsigned int inputIndex,[In] RECT invalidInputRect,[Out] RECT* invalidOutputRect)</unmanaged>	
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate int MapInvalidRectDelegate(IntPtr thisPtr, int inputIndex, IntPtr invalidInputRect, IntPtr invalidOutputRect);
            private unsafe static int MapInvalidRectImpl(IntPtr thisPtr, int inputIndex, IntPtr invalidInputRect, IntPtr invalidOutputRect)
            {
                try
                {
                    var shadow = ToShadow<TransformShadow>(thisPtr);
                    var callback = (Transform)shadow.Callback;
                    *(RawRectangle*)invalidOutputRect = callback.MapInvalidRect(inputIndex, *(RawRectangle*)invalidInputRect);
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