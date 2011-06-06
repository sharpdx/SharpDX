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
using System;
using System.Runtime.InteropServices;

namespace SharpDX.Direct2D1
{
    /// <summary>
    /// Internal TessellationSink Callback
    /// </summary>
    internal class TessellationSinkCallback : SharpDX.ComObjectCallbackNative
    {
        TessellationSink Callback { get; set; }

        /// <summary>
        /// Get a native callback pointer from a managed callback.
        /// </summary>
        /// <param name="tessellationSink">The geometry sink.</param>
        /// <returns>A pointer to the unmanaged geomerty sink counterpart</returns>
        public static IntPtr CallbackToPtr(TessellationSink tessellationSink)
        {
            return CallbackToPtr<TessellationSink, TessellationSinkCallback>(tessellationSink);
        }

        public override void Attach<T>(T callback)  
        {
            Attach(callback, 2);
            Callback = (TessellationSink)callback;
            AddMethod(new AddTrianglesDelegate(AddTrianglesImpl));
            AddMethod(new CloseDelegate(CloseImpl));
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void AddTrianglesDelegate(IntPtr thisPtr, IntPtr triangles, int trianglesCount);
        private void AddTrianglesImpl(IntPtr thisPtr, IntPtr triangles, int trianglesCount)
        {
            unsafe
            {
                Triangle[] managedTriangles = new Triangle[trianglesCount];
                Utilities.Read(triangles, managedTriangles, 0, trianglesCount);
                Callback.AddTriangles(managedTriangles);
            }
        }


        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int CloseDelegate(IntPtr thisPtr);
        private int CloseImpl(IntPtr thisPtr)
        {
            try
            {
                Callback.Close();
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