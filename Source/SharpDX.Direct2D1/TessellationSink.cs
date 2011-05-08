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
    public partial interface TessellationSink
    {
        /// <summary>	
        /// Copies the specified triangles to the sink.  	
        /// </summary>	
        /// <param name="triangles">An array of <see cref="SharpDX.Direct2D1.Triangle"/> structures that describe the triangles to add to the sink.</param>
        /// <unmanaged>void AddTriangles([In, Buffer] const D2D1_TRIANGLE* triangles,[None] UINT trianglesCount)</unmanaged>
        void AddTriangles(SharpDX.Direct2D1.Triangle[] triangles);

        /// <summary>	
        ///  Closes the sink.	
        /// </summary>	
        /// <unmanaged>HRESULT Close()</unmanaged>
        void Close();
    }

    internal partial class TessellationSinkNative
    {
        public void AddTriangles(Triangle[] triangles)
        {
            AddTriangles_(triangles, triangles.Length);
        }

        public void Close()
        {
            Close_();
        }
    }

    /// <summary>
    /// Internal TessellationSink Callback
    /// </summary>
    internal class TessellationSinkCallback : SharpDX.ComObjectCallback
    {
        TessellationSink Callback { get; set; }

        public TessellationSinkCallback(TessellationSink callback) : base(callback, 2)
        {
            Callback = callback;
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
