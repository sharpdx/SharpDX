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

namespace SharpDX.DirectWrite
{
    [ShadowAttribute(typeof(TextAnalysisSource1Shadow))]
    public partial interface TextAnalysisSource1
    {
        /// <summary>	
        /// <p>Used by the text analyzer to obtain the desired glyph orientation and resolved bidi level.</p>	
        /// </summary>	
        /// <param name="textPosition"><dd>  <p>The text position.</p> </dd></param>	
        /// <param name="textLength"><dd>  <p>A reference to the text length.</p> </dd></param>	
        /// <param name="glyphOrientation"><dd>  <p>A <strong><see cref="SharpDX.DirectWrite.VerticalGlyphOrientation"/></strong>-typed value that specifies the desired kind of glyph orientation for the text.</p> </dd></param>	
        /// <param name="bidiLevel"><dd>  <p>A reference to the resolved bidi level.</p> </dd></param>	
        /// <returns><p>Returning an error will abort the analysis.</p></returns>	
        /// <remarks>	
        /// <p>The text analyzer calls back to this to get the desired glyph orientation and resolved bidi level, which it uses along with the script properties of the text to determine the actual orientation of each character, which it reports back to the client via the sink SetGlyphOrientation method.</p>	
        /// </remarks>	
        /// <include file='Documentation\CodeComments.xml' path="/comments/comment[@id='IDWriteTextAnalysisSource1::GetVerticalGlyphOrientation']/*"/>	
        /// <msdn-id>Hh780427</msdn-id>	
        /// <unmanaged>HRESULT IDWriteTextAnalysisSource1::GetVerticalGlyphOrientation([In] unsigned int textPosition,[Out] unsigned int* textLength,[Out] DWRITE_VERTICAL_GLYPH_ORIENTATION* glyphOrientation,[Out] unsigned char* bidiLevel)</unmanaged>	
        /// <unmanaged-short>IDWriteTextAnalysisSource1::GetVerticalGlyphOrientation</unmanaged-short>	
        void GetVerticalGlyphOrientation(int textPosition, out int textLength,
                                         out SharpDX.DirectWrite.VerticalGlyphOrientation glyphOrientation,
                                         out byte bidiLevel);

    }
}