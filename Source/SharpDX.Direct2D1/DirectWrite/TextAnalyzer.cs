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

namespace SharpDX.DirectWrite
{
    public partial class TextAnalyzer
    {
        /// <summary>	
        /// Returns an interface for performing text analysis. 	
        /// </summary>	
        /// <param name="factory">A reference to a DirectWrite factory <see cref="Factory"/></param>
        /// <unmanaged>HRESULT IDWriteFactory::CreateTextAnalyzer([Out] IDWriteTextAnalyzer** textAnalyzer)</unmanaged>
        public TextAnalyzer(Factory factory)
        {
            factory.CreateTextAnalyzer(this);
        }

        /// <summary>	
        /// Analyzes a text range for script boundaries, reading text attributes from the source and reporting the Unicode script ID to the sink  callback {{SetScript}}. 	
        /// </summary>	
        /// <param name="analysisSource">A reference to the source object to analyze. </param>
        /// <param name="textPosition">The starting text position within the source object. </param>
        /// <param name="textLength">The text length to analyze. </param>
        /// <param name="analysisSink">A reference to the sink callback object that receives the text analysis. </param>
        /// <returns>If the method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code. </returns>
        /// <unmanaged>HRESULT IDWriteTextAnalyzer::AnalyzeScript([None] IDWriteTextAnalysisSource* analysisSource,[None] int textPosition,[None] int textLength,[None] IDWriteTextAnalysisSink* analysisSink)</unmanaged>
        public SharpDX.Result AnalyzeScript(TextAnalysisSource analysisSource, int textPosition, int textLength, TextAnalysisSink analysisSink)
        {
            return AnalyzeScript__(TextAnalysisSourceShadow.ToIntPtr(analysisSource), textPosition, textLength, TextAnalysisSinkShadow.ToIntPtr(analysisSink));
        }

        /// <summary>	
        /// Analyzes a text range for script directionality, reading attributes from the source and reporting levels to the sink callback {{SetBidiLevel}}. 	
        /// </summary>	
        /// <remarks>	
        /// While the function can handle multiple paragraphs, the text range should not arbitrarily split the middle of paragraphs. Otherwise, the returned levels may be wrong, because the Bidi algorithm is meant to apply to the paragraph as a whole.  	
        /// </remarks>	
        /// <param name="analysisSource">A reference to a source object to analyze. </param>
        /// <param name="textPosition">The starting text position within the source object. </param>
        /// <param name="textLength">The text length to analyze. </param>
        /// <param name="analysisSink">A reference to the sink callback object that receives the text analysis. </param>
        /// <returns>If the method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code. </returns>
        /// <unmanaged>HRESULT IDWriteTextAnalyzer::AnalyzeBidi([None] IDWriteTextAnalysisSource* analysisSource,[None] int textPosition,[None] int textLength,[None] IDWriteTextAnalysisSink* analysisSink)</unmanaged>
        public SharpDX.Result AnalyzeBidi(TextAnalysisSource analysisSource, int textPosition, int textLength, TextAnalysisSink analysisSink)
        {
            return AnalyzeBidi__(TextAnalysisSourceShadow.ToIntPtr(analysisSource), textPosition, textLength, TextAnalysisSinkShadow.ToIntPtr(analysisSink));
        }

        /// <summary>	
        /// Analyzes a text range for spans where number substitution is applicable, reading attributes from the source and reporting substitutable ranges to the sink callback {{SetNumberSubstitution}}. 	
        /// </summary>	
        /// <remarks>	
        /// Although the function can handle multiple ranges of differing number substitutions, the text ranges should not arbitrarily split the middle of numbers. Otherwise, it will treat the numbers separately and will not translate any intervening punctuation.  	
        /// </remarks>	
        /// <param name="analysisSource">The source object to analyze. </param>
        /// <param name="textPosition">The starting position within the source object. </param>
        /// <param name="textLength">The length to analyze. </param>
        /// <param name="analysisSink">A reference to the sink callback object that receives the text analysis. </param>
        /// <returns>If the method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code. </returns>
        /// <unmanaged>HRESULT IDWriteTextAnalyzer::AnalyzeNumberSubstitution([None] IDWriteTextAnalysisSource* analysisSource,[None] int textPosition,[None] int textLength,[None] IDWriteTextAnalysisSink* analysisSink)</unmanaged>
        public SharpDX.Result AnalyzeNumberSubstitution(TextAnalysisSource analysisSource, int textPosition, int textLength, TextAnalysisSink analysisSink)
        {
            return AnalyzeNumberSubstitution__(TextAnalysisSourceShadow.ToIntPtr(analysisSource), textPosition, textLength, TextAnalysisSinkShadow.ToIntPtr(analysisSink));
        }

        /// <summary>	
        /// Analyzes a text range for potential breakpoint opportunities, reading attributes from the source and reporting breakpoint opportunities to the sink callback {{SetLineBreakpoints}}. 	
        /// </summary>	
        /// <remarks>	
        /// Although the function can handle multiple paragraphs, the text range should not arbitrarily split the middle of paragraphs, unless the specified text span is considered a whole unit. Otherwise, the returned properties for the first and last characters will inappropriately allow breaks.  	
        /// </remarks>	
        /// <param name="analysisSource">A reference to the source object to analyze. </param>
        /// <param name="textPosition">The starting text position within the source object. </param>
        /// <param name="textLength">The text length to analyze. </param>
        /// <param name="analysisSink">A reference to the  sink callback object that receives the text analysis. </param>
        /// <returns>If the method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code. </returns>
        /// <unmanaged>HRESULT IDWriteTextAnalyzer::AnalyzeLineBreakpoints([None] IDWriteTextAnalysisSource* analysisSource,[None] int textPosition,[None] int textLength,[None] IDWriteTextAnalysisSink* analysisSink)</unmanaged>
        public SharpDX.Result AnalyzeLineBreakpoints(TextAnalysisSource analysisSource, int textPosition, int textLength, TextAnalysisSink analysisSink)
        {
            return AnalyzeLineBreakpoints__(TextAnalysisSourceShadow.ToIntPtr(analysisSource), textPosition, textLength, TextAnalysisSinkShadow.ToIntPtr(analysisSink));
        }
    }
}