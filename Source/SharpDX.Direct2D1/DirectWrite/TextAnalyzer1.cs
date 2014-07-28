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
#if DIRECTX11_1
using System;
using System.Runtime.InteropServices;

namespace SharpDX.DirectWrite
{
    public partial class TextAnalyzer1
    {
        /// <summary>	
        /// <p>Analyzes a text range for script orientation, reading text and attributes from the source and reporting results to the sink.</p>	
        /// </summary>	
        /// <param name="analysisSource"><dd>  <p>Source object to analyze.</p> </dd></param>	
        /// <param name="textPosition"><dd>  <p>Starting position within the source object.</p> </dd></param>	
        /// <param name="textLength"><dd>  <p>Length to analyze.</p> </dd></param>	
        /// <param name="analysisSink"><dd>  <p>Length to analyze.</p> </dd></param>	
        /// <returns><p>If this method succeeds, it returns <strong><see cref="SharpDX.Result.Ok"/></strong>. Otherwise, it returns an <strong><see cref="SharpDX.Result"/></strong> error code.</p></returns>	
        /// <include file='..\..\Documentation\CodeComments.xml' path="/comments/comment[@id='IDWriteTextAnalyzer1::AnalyzeVerticalGlyphOrientation']/*"/>	
        /// <msdn-id>Hh780429</msdn-id>	
        /// <unmanaged>HRESULT IDWriteTextAnalyzer1::AnalyzeVerticalGlyphOrientation([In] IDWriteTextAnalysisSource1* analysisSource,[In] unsigned int textPosition,[In] unsigned int textLength,[In] IDWriteTextAnalysisSink1* analysisSink)</unmanaged>	
        /// <unmanaged-short>IDWriteTextAnalyzer1::AnalyzeVerticalGlyphOrientation</unmanaged-short>	
        public void AnalyzeVerticalGlyphOrientation(TextAnalysisSource1 analysisSource, int textPosition, int textLength, TextAnalysisSink1 analysisSink)
        {
            AnalyzeVerticalGlyphOrientation__(TextAnalysisSource1Shadow.ToIntPtr(analysisSource), textPosition,
                                              textLength, TextAnalysisSink1Shadow.ToIntPtr(analysisSink));
        }
    }
}
#endif