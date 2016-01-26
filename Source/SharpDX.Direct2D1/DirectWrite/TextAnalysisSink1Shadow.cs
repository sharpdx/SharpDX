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
using SharpDX.Mathematics.Interop;

namespace SharpDX.DirectWrite
{
    /// <summary>
    /// Internal TextAnalysisSink1 Callback
    /// </summary>
    internal class TextAnalysisSink1Shadow : TextAnalysisSinkShadow
    {
        private static readonly TextAnalysisSink1Vtbl Vtbl = new TextAnalysisSink1Vtbl();

        /// <summary>
        /// Return a pointer to the unmanaged version of this callback.
        /// </summary>
        /// <param name="callback">The callback.</param>
        /// <returns>A pointer to a shadow c++ callback</returns>
        public static IntPtr ToIntPtr(TextAnalysisSink1 callback)
        {
            return ToCallbackPtr<TextAnalysisSink1>(callback);
        }

        protected class TextAnalysisSink1Vtbl : TextAnalysisSinkVtbl
        {
            public TextAnalysisSink1Vtbl()
                : base(1)
            {
                unsafe
                {
                    AddMethod(new SetGlyphOrientationDelegate(SetGlyphOrientationImpl));
                }
            }

            /// <unmanaged>HRESULT IDWriteTextAnalysisSink1::SetGlyphOrientation([In] unsigned int textPosition,[In] unsigned int textLength,[In] DWRITE_GLYPH_ORIENTATION_ANGLE glyphOrientationAngle,[In] unsigned char adjustedBidiLevel,[In] BOOL isSideways,[In] BOOL isRightToLeft)</unmanaged>	
            /// <unmanaged-short>IDWriteTextAnalysisSink1::SetGlyphOrientation</unmanaged-short>	
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private unsafe delegate int SetGlyphOrientationDelegate(IntPtr thisPtr, int textPosition, int textLength,
                                     GlyphOrientationAngle glyphOrientationAngle, byte adjustedBidiLevel,
                                     RawBool isSideways, RawBool isRightToLeft);
            private static int SetGlyphOrientationImpl(IntPtr thisPtr, int textPosition, int textLength,
                                     GlyphOrientationAngle glyphOrientationAngle, byte adjustedBidiLevel,
                                     RawBool isSideways, RawBool isRightToLeft)
            {
                try
                {
                    var shadow = ToShadow<TextAnalysisSink1Shadow>(thisPtr);
                    var callback = (TextAnalysisSink1) shadow.Callback;
                    callback.SetGlyphOrientation(textPosition, textLength, glyphOrientationAngle, adjustedBidiLevel, isSideways, isRightToLeft);
                }
                catch (Exception exception)
                {
                    return (int) Result.GetResultFromException(exception);
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
