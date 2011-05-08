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
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SharpDX.DirectWrite
{
    /// <summary>
    /// Internal FontFileStream Callback
    /// </summary>
    internal class FontFileStreamCallback : SharpDX.ComObjectCallback
    {
        /// <summary>
        /// Gets or sets the callback.
        /// </summary>
        /// <value>The callback.</value>
        public FontFileStream Callback { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FontFileStreamCallback"/> class.
        /// </summary>
        /// <param name="callback">The callback.</param>
        public FontFileStreamCallback(FontFileStream callback) : base(callback, 4)
        {
            Callback = callback;
            AddMethod(new ReadFileFragmentDelegate(ReadFileFragmentImpl));
            AddMethod(new ReleaseFileFragmentDelegate(ReleaseFileFragmentImpl));
            AddMethod(new GetFileSizeDelegate(GetFileSizeImpl));
            AddMethod(new GetLastWriteTimeDelegate(GetLastWriteTimeImpl));
        }

        /// <unmanaged>HRESULT IDWriteFontFileStream::ReadFileFragment([Out, Buffer] const void** fragmentStart,[None] __int64 fileOffset,[None] __int64 fragmentSize,[Out] void** fragmentContext)</unmanaged>
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int ReadFileFragmentDelegate(IntPtr thisPtr, out IntPtr fragmentStart, long fileOffset, long fragmentSize, out IntPtr fragmentContext);
        private int ReadFileFragmentImpl(IntPtr thisPtr, out IntPtr fragmentStart, long fileOffset, long fragmentSize, out IntPtr fragmentContext)
        {
            fragmentStart = IntPtr.Zero;
            fragmentContext = IntPtr.Zero;
            try
            {
                Callback.ReadFileFragment(out fragmentStart, fileOffset, fragmentSize, out fragmentContext);
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

        /// <unmanaged>void IDWriteFontFileStream::ReleaseFileFragment([None] void* fragmentContext)</unmanaged>
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void ReleaseFileFragmentDelegate(IntPtr thisPtr, IntPtr fragmentContext);
        private void ReleaseFileFragmentImpl(IntPtr thisPtr, IntPtr fragmentContext)
        {
            Callback.ReleaseFileFragment(fragmentContext);
        }

        /// <unmanaged>HRESULT IDWriteFontFileStream::GetFileSize([Out] __int64* fileSize)</unmanaged>
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int GetFileSizeDelegate(IntPtr thisPtr, out long fileSize);
        private int GetFileSizeImpl(IntPtr thisPtr, out long fileSize)
        {
            fileSize = 0;
            try
            {
                fileSize = Callback.GetFileSize();
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
        /// <unmanaged>HRESULT IDWriteFontFileStream::GetLastWriteTime([Out] __int64* lastWriteTime)</unmanaged>
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int GetLastWriteTimeDelegate(IntPtr thisPtr, out long lastWriteTime);
        private int GetLastWriteTimeImpl(IntPtr thisPtr, out long lastWriteTime)
        {
            lastWriteTime = 0;
            try
            {
                lastWriteTime = Callback.GetLastWriteTime();
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