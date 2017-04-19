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

using SharpDX.Mathematics.Interop;

namespace SharpDX.DirectWrite
{
    [ShadowAttribute(typeof(TextAnalysisSink1Shadow))]
    public partial interface TextAnalysisSink1
    {
        /// <summary>	
        /// <p>The text analyzer calls back to this to report the actual orientation of each character for shaping and drawing.</p>	
        /// </summary>	
        /// <param name="textPosition"><dd>  <p>The starting position to report from.</p> </dd></param>	
        /// <param name="textLength"><dd>  <p>Number of UTF-16 units of the reported range.</p> </dd></param>	
        /// <param name="glyphOrientationAngle"><dd>  <p>A <strong><see cref="SharpDX.DirectWrite.GlyphOrientationAngle"/></strong>-typed value that specifies the angle of the glyphs within the text range (pass to <strong><see cref="SharpDX.DirectWrite.TextAnalyzer1.GetGlyphOrientationTransform"/></strong> to get the world relative transform).</p> </dd></param>	
        /// <param name="adjustedBidiLevel"><dd>  <p>The adjusted bidi level to be used by the client layout for reordering runs. This will differ from the resolved bidi level retrieved from the source for cases such as Arabic stacked top-to-bottom, where the glyphs are still shaped as RTL, but the runs are TTB along with any CJK or Latin.</p> </dd></param>	
        /// <param name="isSideways"><dd>  <p>Whether the glyphs are rotated on their side, which is the default case for CJK and the case stacked Latin</p> </dd></param>	
        /// <param name="isRightToLeft"><dd>  <p>Whether the script should be shaped as right-to-left. For Arabic stacked top-to-bottom, even when the adjusted bidi level is coerced to an even level, this will still be true.</p> </dd></param>	
        /// <returns><p>Returns a successful code or an error code to abort analysis.</p></returns>	
        /// <include file='Documentation\CodeComments.xml' path="/comments/comment[@id='IDWriteTextAnalysisSink1::SetGlyphOrientation']/*"/>	
        /// <msdn-id>Hh780425</msdn-id>	
        /// <unmanaged>HRESULT IDWriteTextAnalysisSink1::SetGlyphOrientation([In] unsigned int textPosition,[In] unsigned int textLength,[In] DWRITE_GLYPH_ORIENTATION_ANGLE glyphOrientationAngle,[In] unsigned char adjustedBidiLevel,[In] BOOL isSideways,[In] BOOL isRightToLeft)</unmanaged>	
        /// <unmanaged-short>IDWriteTextAnalysisSink1::SetGlyphOrientation</unmanaged-short>	
        void SetGlyphOrientation(int textPosition, int textLength,
                                 GlyphOrientationAngle glyphOrientationAngle, byte adjustedBidiLevel,
                                 RawBool isSideways, RawBool isRightToLeft);

    }
}