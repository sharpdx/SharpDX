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
using System.IO;
using System.Runtime.InteropServices;

namespace SharpDX.DirectWrite
{
   /// <summary>
    /// Internal TextAnalysisSource1 Callback
    /// </summary>
    internal class TextAnalysisSource1Shadow : TextAnalysisSourceShadow
    {
        private static readonly TextAnalysisSource1Vtbl Vtbl = new TextAnalysisSource1Vtbl();

        protected override CppObjectVtbl GetVtbl
        {
            get { return Vtbl; }
        }

        /// <summary>
        /// Return a pointer to the unmanaged version of this callback.
        /// </summary>
        /// <param name="callback">The callback.</param>
        /// <returns>A pointer to a shadow c++ callback</returns>
        public static IntPtr ToIntPtr(TextAnalysisSource1 callback)
        {
            return ToCallbackPtr<TextAnalysisSource1>(callback);
        }

        private class TextAnalysisSource1Vtbl : TextAnalysisSourceVtbl
        {
            public TextAnalysisSource1Vtbl()
                : base(1)
            {
                AddMethod(new GetVerticalGlyphOrientationDelegate(GetVerticalGlyphOrientationImpl));
            }

            /// <unmanaged>HRESULT IDWriteTextAnalysisSource1::GetVerticalGlyphOrientation([In] unsigned int textPosition,[Out] unsigned int* textLength,[Out] DWRITE_VERTICAL_GLYPH_ORIENTATION* glyphOrientation,[Out] unsigned char* bidiLevel)</unmanaged>	
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            private delegate int GetVerticalGlyphOrientationDelegate(IntPtr thisPtr, int textPosition, out int textLength,
                                             out SharpDX.DirectWrite.VerticalGlyphOrientation glyphOrientation,
                                             out byte bidiLevel);
            private static int GetVerticalGlyphOrientationImpl(IntPtr thisPtr, int textPosition, out int textLength,
                                             out SharpDX.DirectWrite.VerticalGlyphOrientation glyphOrientation,
                                             out byte bidiLevel)
            {
                textLength = 0;
                glyphOrientation = VerticalGlyphOrientation.Default;
                bidiLevel = 0;
                try
                {
                    var shadow = ToShadow<TextAnalysisSource1Shadow>(thisPtr);
                    var callback = (TextAnalysisSource1)shadow.Callback;
                    callback.GetVerticalGlyphOrientation(textPosition, out textLength, out glyphOrientation,
                                                         out bidiLevel);
                }
                catch (Exception exception)
                {
                    return (int)SharpDX.Result.GetResultFromException(exception);
                }
                return Result.Ok.Code;
            }
        }
    }
}