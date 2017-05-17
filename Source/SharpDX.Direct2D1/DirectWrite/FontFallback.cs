// Copyright (c) 2010-2013 SharpDX - Alexandre Mutel
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

namespace SharpDX.DirectWrite
{
    public partial class FontFallback
    {
        /// <summary>
        /// <p>Determines an appropriate font to use to render the beginning range of text.</p>
        /// </summary>
        /// <param name="analysisSource"><dd>  <p>The text source implementation holds the text and locale.</p> </dd></param>
        /// <param name="textPosition"><dd>  <p>Starting position to analyze.</p> </dd></param>
        /// <param name="textLength"><dd>  <p>Length of the text to analyze.</p> </dd></param>
        /// <param name="baseFontCollection"><dd>  <p>Default font collection to use.</p> </dd></param>
        /// <param name="baseFamilyName"><dd>  <p>Family name of the base font. If you pass null, no matching     will be done against the family.</p> </dd></param>
        /// <param name="baseWeight"><dd>  <p>The desired weight.</p> </dd></param>
        /// <param name="baseStyle"><dd>  <p>The desired style.</p> </dd></param>
        /// <param name="baseStretch"><dd>  <p>The desired stretch.</p> </dd></param>
        /// <param name="mappedLength"><dd>  <p>Length of text mapped to the mapped font. This will always be less than     or equal to the text length and greater than zero (if the text length is non-zero) so     the caller advances at least one character.</p> </dd></param>
        /// <param name="mappedFont"><dd>  <p>The font that should be used to render the first <em>mappedLength</em> characters of the text. If it returns <c>null</c>, that means that no font can render the     text, and <em>mappedLength</em> is the number of characters to skip (rendered with a missing glyph).</p> </dd></param>
        /// <param name="scale"><dd>  <p>Scale factor to multiply the em size of the returned font by.</p> </dd></param>
        public void MapCharacters(
            TextAnalysisSource analysisSource,
            int textPosition,
            int textLength,
            FontCollection baseFontCollection,
            string baseFamilyName,
            FontWeight baseWeight,
            FontStyle baseStyle,
            FontStretch baseStretch,
            out int mappedLength,
            out Font mappedFont,
            out float scale
        )
        {
            MapCharacters_(
                TextAnalysisSourceShadow.ToIntPtr(analysisSource),
                textPosition,
                textLength,
                baseFontCollection,
                baseFamilyName,
                baseWeight,
                baseStyle,
                baseStretch,
                out mappedLength,
                out mappedFont,
                out scale
            );
        }
    }
}
