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
    /// Internal FontFileLoader Callback
    /// </summary>
    internal class FontFileLoaderShadow : SharpDX.ComObjectShadow
    {
        private static readonly FontFileLoaderVtbl Vtbl = new FontFileLoaderVtbl();

        /// <summary>
        /// Return a pointer to the unmanaged version of this callback.
        /// </summary>
        /// <param name="callback">The callback.</param>
        /// <returns>A pointer to a shadow c++ callback</returns>
        public static IntPtr ToIntPtr(FontFileLoader callback)
        {
            return ToCallbackPtr<FontFileLoader>(callback);
        }

        private class FontFileLoaderVtbl : ComObjectVtbl
        {
            public FontFileLoaderVtbl() : base(1)
            {
                AddMethod(new CreateStreamFromKeyDelegate(CreateStreamFromKeyImpl));
            }

            /// <unmanaged>HRESULT IDWriteFontFileLoader::CreateStreamFromKey([In, Buffer] const void* fontFileReferenceKey,[None] int fontFileReferenceKeySize,[Out] IDWriteFontFileStream** fontFileStream)</unmanaged>
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate int CreateStreamFromKeyDelegate(IntPtr thisPtr, IntPtr fontFileReferenceKey, int fontFileReferenceKeySize, out IntPtr fontFileStream);

            private static int CreateStreamFromKeyImpl(IntPtr thisPtr, IntPtr fontFileReferenceKey, int fontFileReferenceKeySize, out IntPtr fontFileStreamPtr)
            {
                fontFileStreamPtr = IntPtr.Zero;
                try
                {
                    var shadow = ToShadow<FontFileLoaderShadow>(thisPtr);
                    var callback = (FontFileLoader)shadow.Callback;
                    var fontFileStream = callback.CreateStreamFromKey(new DataPointer(fontFileReferenceKey, fontFileReferenceKeySize));
                    fontFileStreamPtr = FontFileStreamShadow.ToIntPtr(fontFileStream);
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