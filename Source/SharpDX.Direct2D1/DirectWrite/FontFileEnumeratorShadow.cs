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

namespace SharpDX.DirectWrite
{
    /// <summary>
    /// Internal FontFileEnumerator Callback
    /// </summary>
    internal class FontFileEnumeratorShadow : SharpDX.ComObjectShadow
    {
        private static readonly FontFileEnumeratorVtbl Vtbl = new FontFileEnumeratorVtbl();

        /// <summary>
        /// Return a pointer to the unmanaged version of this callback.
        /// </summary>
        /// <param name="callback">The callback.</param>
        /// <returns>A pointer to a shadow c++ callback</returns>
        public static IntPtr ToIntPtr(FontFileEnumerator callback)
        {
            return ToCallbackPtr<FontFileEnumerator>(callback);
        }

        private class FontFileEnumeratorVtbl : ComObjectVtbl
        {
            public FontFileEnumeratorVtbl() : base(2)
            {
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
            private static int MoveNextImpl(IntPtr thisPtr, out int hasCurrentFile)
            {
                hasCurrentFile = 0;
                try
                {
                    var shadow = ToShadow<FontFileEnumeratorShadow>(thisPtr);
                    var callback = (FontFileEnumerator)shadow.Callback; 
                    hasCurrentFile = callback.MoveNext() ? 1 : 0;
                }
                catch (Exception exception)
                {
                    return (int)SharpDX.Result.GetResultFromException(exception);
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
            private static int GetCurrentFontFileImpl(IntPtr thisPtr, out IntPtr fontFile)
            {
                fontFile = IntPtr.Zero;
                try
                {
                    var shadow = ToShadow<FontFileEnumeratorShadow>(thisPtr);
                    var callback = (FontFileEnumerator)shadow.Callback;
                    fontFile = callback.CurrentFontFile.NativePointer;
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