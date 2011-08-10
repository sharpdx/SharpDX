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

namespace SharpDX.Win32
{
    /// <summary>
    /// Internal FontFileEnumerator Callback
    /// </summary>
    internal class ComStreamBaseCallback : SharpDX.ComObjectCallbackNative
    {
        /// <summary>
        /// Gets or sets the callback.
        /// </summary>
        /// <value>The callback.</value>
        private IStreamBase Callback { get; set; }

        //public static IntPtr CallbackToPtr(IStream fontFileEnumerator)
        //{
        //    return CallbackToPtr<IStreamBase, ComStreamBaseCallback>(fontFileEnumerator);
        //}

        public override void Attach<T>(T callback)
        {
            Attach(callback, 2);
            Callback = (IStreamBase)callback;
            AddMethod(new ReadDelegate(ReadImpl));
            AddMethod(new WriteDelegate(WriteImpl));
        }

        /// <unmanaged>HRESULT ISequentialStream::Read([Out, Buffer] void* pv,[In] unsigned int cb,[Out, Optional] unsigned int* pcbRead)</unmanaged>	
        /* public int Read(System.IntPtr vRef, int cb) */
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int ReadDelegate(IntPtr thisPtr, IntPtr buffer, int sizeOfBytes, out int bytesRead);
        private int ReadImpl(IntPtr thisPtr, IntPtr buffer, int sizeOfBytes, out int bytesRead)
        {
            bytesRead = 0;
            try
            {
                bytesRead = Callback.Read(buffer, sizeOfBytes);
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

        /// <unmanaged>HRESULT ISequentialStream::Write([In, Buffer] const void* pv,[In] unsigned int cb,[Out, Optional] unsigned int* pcbWritten)</unmanaged>	
        /* public int Write(System.IntPtr vRef, int cb) */
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int WriteDelegate(IntPtr thisPtr, IntPtr buffer, int sizeOfBytes, out int bytesWrite);
        private int WriteImpl(IntPtr thisPtr, IntPtr buffer, int sizeOfBytes, out int bytesWrite)
        {
            bytesWrite = 0;
            try
            {
                bytesWrite = Callback.Write(buffer, sizeOfBytes);
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

