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
    [ShadowAttribute(typeof(TextAnalysisSinkShadow))]
    public partial interface TextAnalysisSink
    {
        /// <summary>	
        /// Reports script analysis for the specified text range.	
        /// </summary>	
        /// <param name="textPosition">The starting position from which to report. </param>
        /// <param name="textLength">The number of UTF16 units of the reported range. </param>
        /// <param name="scriptAnalysis">A reference to a structure that contains a zero-based index representation of a writing system script and a value indicating whether additional shaping of text is required. </param>
        /// <returns>A successful code or error code to stop analysis. </returns>
        /// <unmanaged>HRESULT IDWriteTextAnalysisSink::SetScriptAnalysis([None] int textPosition,[None] int textLength,[In] const DWRITE_SCRIPT_ANALYSIS* scriptAnalysis)</unmanaged>
        void SetScriptAnalysis(int textPosition, int textLength, SharpDX.DirectWrite.ScriptAnalysis scriptAnalysis);

        /// <summary>	
        /// Sets line-break opportunities for each character, starting from the specified position.	
        /// </summary>	
        /// <param name="textPosition">The starting text position from which to report. </param>
        /// <param name="textLength">The number of UTF16 units of the reported range. </param>
        /// <param name="lineBreakpoints">A reference to a structure that contains breaking conditions set for each character from the starting position to the end of the specified range. </param>
        /// <returns>A successful code or error code to stop analysis. </returns>
        /// <unmanaged>HRESULT IDWriteTextAnalysisSink::SetLineBreakpoints([None] int textPosition,[None] int textLength,[In, Buffer] const DWRITE_LINE_BREAKPOINT* lineBreakpoints)</unmanaged>
        void SetLineBreakpoints(int textPosition, int textLength, SharpDX.DirectWrite.LineBreakpoint[] lineBreakpoints);

        /// <summary>	
        /// Sets a bidirectional level on the range, which is  called once per  run change (either explicit or resolved implicit).	
        /// </summary>	
        /// <param name="textPosition">The starting position from which to report. </param>
        /// <param name="textLength">The number of UTF16 units of the reported range. </param>
        /// <param name="explicitLevel">The explicit level from the paragraph reading direction and any embedded control codes RLE/RLO/LRE/LRO/PDF, which is determined before any additional rules. </param>
        /// <param name="resolvedLevel">The final implicit level considering the explicit level and characters' natural directionality, after all Bidi rules have been applied. </param>
        /// <returns>A successful code or error code to stop analysis. </returns>
        /// <unmanaged>HRESULT IDWriteTextAnalysisSink::SetBidiLevel([None] int textPosition,[None] int textLength,[None] int explicitLevel,[None] int resolvedLevel)</unmanaged>
        void SetBidiLevel(int textPosition, int textLength, byte explicitLevel, byte resolvedLevel);

        /// <summary>	
        /// Sets the number substitution on the text range affected by the text analysis.	
        /// </summary>	
        /// <param name="textPosition">The starting position from which to report. </param>
        /// <param name="textLength">The number of UTF16 units of the reported range. </param>
        /// <param name="numberSubstitution">An object that holds the appropriate digits and numeric punctuation for a given locale. Use <see cref="SharpDX.DirectWrite.Factory.CreateNumberSubstitution"/> to create this object. </param>
        /// <returns>If the method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code. </returns>
        /// <unmanaged>HRESULT IDWriteTextAnalysisSink::SetNumberSubstitution([None] int textPosition,[None] int textLength,[None] IDWriteNumberSubstitution* numberSubstitution)</unmanaged>
        void SetNumberSubstitution(int textPosition, int textLength, SharpDX.DirectWrite.NumberSubstitution numberSubstitution);
    }
}