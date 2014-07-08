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
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SharpDX.DirectWrite
{
    /// <summary>
    /// Internal FontFileStream Callback
    /// </summary>
    internal class FontFileStreamShadow : SharpDX.ComObjectShadow
    {
        private static readonly FontFileStreamVtbl Vtbl = new FontFileStreamVtbl();

        /// <summary>
        /// Return a pointer to the unmanaged version of this callback.
        /// </summary>
        /// <param name="callback">The callback.</param>
        /// <returns>A pointer to a shadow c++ callback</returns>
        public static IntPtr ToIntPtr(FontFileStream callback)
        {
            return ToCallbackPtr<FontFileStream>(callback);
        }

        private class FontFileStreamVtbl : ComObjectVtbl
        {
            public FontFileStreamVtbl() : base(4)
            {
                AddMethod(new ReadFileFragmentDelegate(ReadFileFragmentImpl));
                AddMethod(new ReleaseFileFragmentDelegate(ReleaseFileFragmentImpl));
                AddMethod(new GetFileSizeDelegate(GetFileSizeImpl));
                AddMethod(new GetLastWriteTimeDelegate(GetLastWriteTimeImpl));
            }

            /// <unmanaged>HRESULT IDWriteFontFileStream::ReadFileFragment([Out, Buffer] const void** fragmentStart,[None] __int64 fileOffset,[None] __int64 fragmentSize,[Out] void** fragmentContext)</unmanaged>
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate int ReadFileFragmentDelegate(IntPtr thisPtr, out IntPtr fragmentStart, long fileOffset, long fragmentSize, out IntPtr fragmentContext);
            private static int ReadFileFragmentImpl(IntPtr thisPtr, out IntPtr fragmentStart, long fileOffset, long fragmentSize, out IntPtr fragmentContext)
            {
                fragmentStart = IntPtr.Zero;
                fragmentContext = IntPtr.Zero;
                try
                {
                    var shadow = ToShadow<FontFileStreamShadow>(thisPtr);
                    var callback = (FontFileStream)shadow.Callback; 
                    callback.ReadFileFragment(out fragmentStart, fileOffset, fragmentSize, out fragmentContext);
                }
                catch (Exception exception)
                {
                    return (int)SharpDX.Result.GetResultFromException(exception);
                }
                return Result.Ok.Code;
            }

            /// <unmanaged>void IDWriteFontFileStream::ReleaseFileFragment([None] void* fragmentContext)</unmanaged>
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate void ReleaseFileFragmentDelegate(IntPtr thisPtr, IntPtr fragmentContext);
            private static void ReleaseFileFragmentImpl(IntPtr thisPtr, IntPtr fragmentContext)
            {
                var shadow = ToShadow<FontFileStreamShadow>(thisPtr);
                var callback = (FontFileStream)shadow.Callback;
                callback.ReleaseFileFragment(fragmentContext);
            }

            /// <unmanaged>HRESULT IDWriteFontFileStream::GetFileSize([Out] __int64* fileSize)</unmanaged>
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate int GetFileSizeDelegate(IntPtr thisPtr, out long fileSize);
            private static int GetFileSizeImpl(IntPtr thisPtr, out long fileSize)
            {
                fileSize = 0;
                try
                {
                    var shadow = ToShadow<FontFileStreamShadow>(thisPtr);
                    var callback = (FontFileStream)shadow.Callback;
                    fileSize = callback.GetFileSize();
                }
                catch (Exception exception)
                {
                    return (int)SharpDX.Result.GetResultFromException(exception);
                }
                return Result.Ok.Code;
            }
            /// <unmanaged>HRESULT IDWriteFontFileStream::GetLastWriteTime([Out] __int64* lastWriteTime)</unmanaged>
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate int GetLastWriteTimeDelegate(IntPtr thisPtr, out long lastWriteTime);
            private static int GetLastWriteTimeImpl(IntPtr thisPtr, out long lastWriteTime)
            {
                lastWriteTime = 0;
                try
                {
                    var shadow = ToShadow<FontFileStreamShadow>(thisPtr);
                    var callback = (FontFileStream)shadow.Callback;
                    lastWriteTime = callback.GetLastWriteTime();
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