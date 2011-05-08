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
    /// Internal FontFileLoader Callback
    /// </summary>
    internal class FontFileLoaderCallback : SharpDX.ComObjectCallback
    {
        /// <summary>
        /// Gets or sets the callback.
        /// </summary>
        /// <value>The callback.</value>
        public FontFileLoader Callback { get; private set; }

        private List<FontFileStreamCallback> _streamCallbacks;

        /// <summary>
        /// Initializes a new instance of the <see cref="FontFileLoaderCallback"/> class.
        /// </summary>
        /// <param name="callback">The callback.</param>
        public FontFileLoaderCallback(FontFileLoader callback)
            : base(callback, 1)
        {
            Callback = callback;
            _streamCallbacks = new List<FontFileStreamCallback>();
            AddMethod(new CreateStreamFromKeyDelegate(CreateStreamFromKeyImpl));
        }

        /// <unmanaged>HRESULT IDWriteFontFileLoader::CreateStreamFromKey([In, Buffer] const void* fontFileReferenceKey,[None] int fontFileReferenceKeySize,[Out] IDWriteFontFileStream** fontFileStream)</unmanaged>
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int CreateStreamFromKeyDelegate(
            IntPtr thisPtr, IntPtr fontFileReferenceKey, int fontFileReferenceKeySize, out IntPtr fontFileStream);
        private int CreateStreamFromKeyImpl(
            IntPtr thisPtr, IntPtr fontFileReferenceKey, int fontFileReferenceKeySize, out IntPtr fontFileStreamPtr)
        {
            fontFileStreamPtr = IntPtr.Zero;
            try
            {
                var fontFileStream = Callback.CreateStreamFromKey(new DataStream(fontFileReferenceKey, fontFileReferenceKeySize, true, true));

                var fontFileStreamCallback = new FontFileStreamCallback(fontFileStream);
                _streamCallbacks.Add(fontFileStreamCallback);

                fontFileStreamPtr = fontFileStreamCallback.NativePointer;
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