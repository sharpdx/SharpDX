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
    /// Internal ComputeTransform Callback
    /// </summary>
    internal class ComputeTransformShadow : TransformShadow
    {
        private static readonly ComputeTransformVtbl Vtbl = new ComputeTransformVtbl(0);

        /// <summary>
        /// Return a pointer to the unmanaged version of this callback.
        /// </summary>
        /// <param name="callback">The callback.</param>
        /// <returns>A pointer to a shadow c++ callback</returns>
        public static IntPtr ToIntPtr(ComputeTransform callback)
        {
            return ToCallbackPtr<ComputeTransform>(callback);
        }

        public class ComputeTransformVtbl : TransformShadow.TransformVtbl
        {
            public ComputeTransformVtbl(int methods) : base(2 + methods)
            {
                AddMethod(new SetComputeInformationDelegate(SetComputeInformationImpl));
                AddMethod(new CalculateThreadgroupsDelegate(CalculateThreadgroupsImpl));
            }

            /// <unmanaged>HRESULT ID2D1ComputeTransform::SetComputeInfo([In] ID2D1ComputeInfo* computeInfo)</unmanaged>	
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate int SetComputeInformationDelegate(IntPtr thisPtr, IntPtr computeInfo);
            private static int SetComputeInformationImpl(IntPtr thisPtr, IntPtr computeInfo)
            {
                try
                {
                    var shadow = ToShadow<ComputeTransformShadow>(thisPtr);
                    var callback = (ComputeTransform)shadow.Callback;
                    callback.SetComputeInformation(new ComputeInformation(computeInfo));
                }
                catch (Exception exception)
                {
                    return (int)SharpDX.Result.GetResultFromException(exception);
                }
                return Result.Ok.Code;
            }

            /// <unmanaged>HRESULT ID2D1ComputeTransform::CalculateThreadgroups([In] const RECT* outputRect,[Out] unsigned int* dimensionX,[Out] unsigned int* dimensionY,[Out] unsigned int* dimensionZ)</unmanaged>	
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate int CalculateThreadgroupsDelegate(IntPtr thisPtr, IntPtr rect, out int dimX, out int dimY, out int dimZ);
            private unsafe static int CalculateThreadgroupsImpl(IntPtr thisPtr, IntPtr rect, out int dimX, out int dimY, out int dimZ)
            {
                dimX = dimY = dimZ = 0;
                try
                {
                    var shadow = ToShadow<ComputeTransformShadow>(thisPtr);
                    var callback = (ComputeTransform)shadow.Callback;
                    var result = callback.CalculateThreadgroups(*(RawRectangle*)rect);
                    dimX = result.X;
                    dimY = result.Y;
                    dimZ = result.Z;
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