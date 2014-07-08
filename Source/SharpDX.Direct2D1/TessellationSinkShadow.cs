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
    /// Internal TessellationSink Callback
    /// </summary>
    internal class TessellationSinkShadow : SharpDX.ComObjectShadow
    {
        private static readonly TessellationSinkVtbl Vtbl = new TessellationSinkVtbl();

        /// <summary>
        /// Get a native callback pointer from a managed callback.
        /// </summary>
        /// <param name="tessellationSink">The geometry sink.</param>
        /// <returns>A pointer to the unmanaged geometry sink counterpart</returns>
        public static IntPtr ToIntPtr(TessellationSink tessellationSink)
        {
            return ToCallbackPtr<TessellationSink>(tessellationSink);
        }

        public class TessellationSinkVtbl : ComObjectVtbl
        {
            public TessellationSinkVtbl() : base(2)
            {
                AddMethod(new AddTrianglesDelegate(AddTrianglesImpl));
                AddMethod(new CloseDelegate(CloseImpl));
            }

            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate void AddTrianglesDelegate(IntPtr thisPtr, IntPtr triangles, int trianglesCount);
            private static void AddTrianglesImpl(IntPtr thisPtr, IntPtr triangles, int trianglesCount)
            {
                unsafe
                {
                    var shadow = ToShadow<TessellationSinkShadow>(thisPtr);
                    var callback = (TessellationSink) shadow.Callback;
                    var managedTriangles = new Triangle[trianglesCount];
                    Utilities.Read(triangles, managedTriangles, 0, trianglesCount);
                    callback.AddTriangles(managedTriangles);
                }
            }


            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate int CloseDelegate(IntPtr thisPtr);
            private static int CloseImpl(IntPtr thisPtr)
            {
                try
                {
                    var shadow = ToShadow<TessellationSinkShadow>(thisPtr);
                    var callback = (TessellationSink)shadow.Callback;
                    callback.Close();
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