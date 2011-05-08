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

namespace SharpDX.DirectWrite
{
    /// <summary>
    /// Internal FontFileEnumerator Callback
    /// </summary>
    internal class FontFileEnumeratorCallback : SharpDX.ComObjectCallback
    {
        /// <summary>
        /// Gets or sets the callback.
        /// </summary>
        /// <value>The callback.</value>
        public FontFileEnumerator Callback { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FontFileEnumeratorCallback"/> class.
        /// </summary>
        /// <param name="callback">The callback.</param>
        public FontFileEnumeratorCallback(FontFileEnumerator callback) : base(callback, 2)
        {
            Callback = callback;
            AddMethod(new MoveNextDelegate(MoveNextImpl));
            AddMethod(new GetCurrentFontFileDelegate(GetCurrentFontFileImpl));
        }

        /// <summary>	
        /// Advances to the next font file in the collection. When it is first created, the enumerator is positioned before the first element of the collection and the first call to MoveNext advances to the first file. 	
        /// </summary>	
        /// <returns>the value TRUE if the enumerator advances to a file; otherwise, FALSE if the enumerator advances past the last file in the collection.</returns>
        /// <unmanaged>HRESULT IDWriteFontFileEnumerator::MoveNext([Out] BOOL* hasCurrentFile)</unmanaged>
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int MoveNextDelegate(IntPtr thisPtr, out int hasCurrentFile);
        private int MoveNextImpl(IntPtr thisPtr, out int hasCurrentFile)
        {
            hasCurrentFile = 0;
            try
            {
                hasCurrentFile = Callback.MoveNext() ? 1 : 0;
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

        /// <summary>	
        /// Gets a reference to the current font file. 	
        /// </summary>	
        /// <returns>a reference to the newly created <see cref="SharpDX.DirectWrite.FontFile"/> object.</returns>
        /// <unmanaged>HRESULT IDWriteFontFileEnumerator::GetCurrentFontFile([Out] IDWriteFontFile** fontFile)</unmanaged>
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int GetCurrentFontFileDelegate(IntPtr thisPtr, out IntPtr fontFile);
        private int GetCurrentFontFileImpl(IntPtr thisPtr, out IntPtr fontFile)
        {
            fontFile = IntPtr.Zero;
            try
            {
                fontFile = Callback.CurrentFontFile.NativePointer;
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