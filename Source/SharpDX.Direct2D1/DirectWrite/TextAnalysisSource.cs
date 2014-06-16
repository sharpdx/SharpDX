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
    [ShadowAttribute(typeof(TextAnalysisSourceShadow))]
    public partial interface TextAnalysisSource
    {
        /// <summary>	
        /// Gets a block of text starting at the specified text position. 	
        /// </summary>	
        /// <remarks>	
        /// Returning NULL indicates the end of text, which is the position after the last character. This function is called iteratively for each consecutive block, tying together several fragmented blocks in the backing store into a virtual contiguous string. Although applications can implement sparse textual content that  maps only part of the backing store, the application must map any text that is in the range passed to any analysis functions. 	
        /// </remarks>	
        /// <param name="textPosition">The first position of the piece to obtain. All positions are in UTF16 code units, not whole characters, which matters when supplementary characters are used. </param>      
        /// <returns>a block of text </returns>
        /// <unmanaged>HRESULT IDWriteTextAnalysisSource::GetTextAtPosition([None] int textPosition,[Out] const wchar_t** textString,[Out] int* textLength)</unmanaged>
        string GetTextAtPosition(int textPosition);

        /// <summary>	
        /// Gets a block of text immediately preceding the specified position.	
        /// </summary>	
        /// <remarks>	
        /// NULL indicates no chunk available at the specified position, either because textPosition equals 0,  textPosition is greater than the entire text content length, or the queried position is not mapped into the application's backing store. Although applications can implement sparse textual content that  maps only part of the backing store, the application must map any text that is in the range passed to any analysis functions. 	
        /// </remarks>	
        /// <param name="textPosition">The position immediately after the last position of the block of text to obtain. </param>
        /// <returns>text immediately preceding the specified position </returns>
        /// <unmanaged>HRESULT IDWriteTextAnalysisSource::GetTextBeforePosition([None] int textPosition,[Out] const wchar_t** textString,[Out] int* textLength)</unmanaged>
        string GetTextBeforePosition(int textPosition);

        /// <summary>	
        /// Gets the paragraph reading direction.	
        /// </summary>	
        /// <returns>The reading direction of the current paragraph. </returns>
        /// <unmanaged>DWRITE_READING_DIRECTION IDWriteTextAnalysisSource::GetParagraphReadingDirection()</unmanaged>
        SharpDX.DirectWrite.ReadingDirection ReadingDirection { get; }

        /// <summary>
        /// Gets the locale name on the range affected by the text analysis.
        /// </summary>
        /// <param name="textPosition">The text position to examine.</param>
        /// <param name="textLength">Contains the length of the text being affected by the text analysis up to the next differing locale.</param>
        /// <returns>
        /// the locale name on the range affected by the text analysis
        /// </returns>
        /// <unmanaged>HRESULT IDWriteTextAnalysisSource::GetLocaleName([None] int textPosition,[Out] int* textLength,[Out] const wchar_t** localeName)</unmanaged>
        /// <remarks>
        /// The localeName reference must remain valid until the next call or until the analysis returns.
        /// </remarks>
        string GetLocaleName(int textPosition, out int textLength);

        /// <summary>	
        /// Gets the number substitution from the text range affected by the text analysis.	
        /// </summary>	
        /// <remarks>	
        /// Any implementation should return the number substitution with an incremented reference count, and the analysis will release when finished with it (either before the next call or before it returns). However, the sink callback may hold onto it after that. 	
        /// </remarks>	
        /// <param name="textPosition">The starting position from which to report. </param>
        /// <param name="textLength">Contains  the length of the text, in characters, remaining in the text range up to the next differing number substitution. </param>
        /// <returns>the number substitution from the text range affected by the text analysis.</returns>
        /// <unmanaged>HRESULT IDWriteTextAnalysisSource::GetNumberSubstitution([None] int textPosition,[Out] int* textLength,[Out] IDWriteNumberSubstitution** numberSubstitution)</unmanaged>
        SharpDX.DirectWrite.NumberSubstitution GetNumberSubstitution(int textPosition, out int textLength);
    }
}