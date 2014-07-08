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

namespace SharpDX.Win32
{
    /// <summary>
    /// Internal FontFileEnumerator Callback
    /// </summary>
    internal class ComStreamBaseShadow : SharpDX.ComObjectShadow
    {
        private static readonly ComStreamBaseVtbl Vtbl = new ComStreamBaseVtbl(0);

        internal class ComStreamBaseVtbl : ComObjectVtbl
        {
            //public static IntPtr CallbackToPtr(IStream fontFileEnumerator)
            //{
            //    return CallbackToPtr<IStreamBase, ComStreamBaseShadow>(fontFileEnumerator);
            //}

            public ComStreamBaseVtbl(int numberOfMethods)
                : base(numberOfMethods + 2)
            {
                AddMethod(new ReadDelegate(ReadImpl));
                AddMethod(new WriteDelegate(WriteImpl));
            }

            /// <unmanaged>HRESULT ISequentialStream::Read([Out, Buffer] void* pv,[In] unsigned int cb,[Out, Optional] unsigned int* pcbRead)</unmanaged>	
            /* public int Read(System.IntPtr vRef, int cb) */
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate int ReadDelegate(IntPtr thisPtr, IntPtr buffer, int sizeOfBytes, out int bytesRead);
            private static int ReadImpl(IntPtr thisPtr, IntPtr buffer, int sizeOfBytes, out int bytesRead)
            {
                bytesRead = 0;
                try
                {
                    var shadow = ToShadow<ComStreamBaseShadow>(thisPtr);
                    var callback = ((IStream) shadow.Callback);
                    bytesRead = callback.Read(buffer, sizeOfBytes);
                }
                catch (Exception exception)
                {
                    return (int)SharpDX.Result.GetResultFromException(exception);
                }
                return Result.Ok.Code;
            }

            /// <unmanaged>HRESULT ISequentialStream::Write([In, Buffer] const void* pv,[In] unsigned int cb,[Out, Optional] unsigned int* pcbWritten)</unmanaged>	
            /* public int Write(System.IntPtr vRef, int cb) */
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate int WriteDelegate(IntPtr thisPtr, IntPtr buffer, int sizeOfBytes, out int bytesWrite);
            private static int WriteImpl(IntPtr thisPtr, IntPtr buffer, int sizeOfBytes, out int bytesWrite)
            {
                bytesWrite = 0;
                try
                {
                    var shadow = ToShadow<ComStreamBaseShadow>(thisPtr);
                    var callback = ((IStream)shadow.Callback);
                    bytesWrite = callback.Write(buffer, sizeOfBytes);
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

