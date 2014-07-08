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
        /// <param name="analysisSource">A reference to the source object to analyze.</param>
        /// <param name="textPosition">The starting text position within the source object.</param>
        /// <param name="textLength">The text length to analyze.</param>
        /// <param name="analysisSink">A reference to the sink callback object that receives the text analysis.</param>
        /// <returns>
        /// If the method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.
        /// </returns>
        /// <unmanaged>HRESULT IDWriteTextAnalyzer::AnalyzeScript([None] IDWriteTextAnalysisSource* analysisSource,[None] int textPosition,[None] int textLength,[None] IDWriteTextAnalysisSink* analysisSink)</unmanaged>
        public void AnalyzeScript(TextAnalysisSource analysisSource, int textPosition, int textLength, TextAnalysisSink analysisSink)
        {
            AnalyzeScript__(TextAnalysisSourceShadow.ToIntPtr(analysisSource), textPosition, textLength, TextAnalysisSinkShadow.ToIntPtr(analysisSink));
        }

        /// <summary>
        /// Analyzes a text range for script directionality, reading attributes from the source and reporting levels to the sink callback {{SetBidiLevel}}.
        /// </summary>
        /// <param name="analysisSource">A reference to a source object to analyze.</param>
        /// <param name="textPosition">The starting text position within the source object.</param>
        /// <param name="textLength">The text length to analyze.</param>
        /// <param name="analysisSink">A reference to the sink callback object that receives the text analysis.</param>
        /// <returns>
        /// If the method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.
        /// </returns>
        /// <unmanaged>HRESULT IDWriteTextAnalyzer::AnalyzeBidi([None] IDWriteTextAnalysisSource* analysisSource,[None] int textPosition,[None] int textLength,[None] IDWriteTextAnalysisSink* analysisSink)</unmanaged>
        /// <remarks>
        /// While the function can handle multiple paragraphs, the text range should not arbitrarily split the middle of paragraphs. Otherwise, the returned levels may be wrong, because the Bidi algorithm is meant to apply to the paragraph as a whole.
        /// </remarks>
        public void AnalyzeBidi(TextAnalysisSource analysisSource, int textPosition, int textLength, TextAnalysisSink analysisSink)
        {
            AnalyzeBidi__(TextAnalysisSourceShadow.ToIntPtr(analysisSource), textPosition, textLength, TextAnalysisSinkShadow.ToIntPtr(analysisSink));
        }

        /// <summary>
        /// Analyzes a text range for spans where number substitution is applicable, reading attributes from the source and reporting substitutable ranges to the sink callback {{SetNumberSubstitution}}.
        /// </summary>
        /// <param name="analysisSource">The source object to analyze.</param>
        /// <param name="textPosition">The starting position within the source object.</param>
        /// <param name="textLength">The length to analyze.</param>
        /// <param name="analysisSink">A reference to the sink callback object that receives the text analysis.</param>
        /// <returns>
        /// If the method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.
        /// </returns>
        /// <unmanaged>HRESULT IDWriteTextAnalyzer::AnalyzeNumberSubstitution([None] IDWriteTextAnalysisSource* analysisSource,[None] int textPosition,[None] int textLength,[None] IDWriteTextAnalysisSink* analysisSink)</unmanaged>
        /// <remarks>
        /// Although the function can handle multiple ranges of differing number substitutions, the text ranges should not arbitrarily split the middle of numbers. Otherwise, it will treat the numbers separately and will not translate any intervening punctuation.
        /// </remarks>
        public void AnalyzeNumberSubstitution(TextAnalysisSource analysisSource, int textPosition, int textLength, TextAnalysisSink analysisSink)
        {
            AnalyzeNumberSubstitution__(TextAnalysisSourceShadow.ToIntPtr(analysisSource), textPosition, textLength, TextAnalysisSinkShadow.ToIntPtr(analysisSink));
        }

        /// <summary>
        /// Analyzes a text range for potential breakpoint opportunities, reading attributes from the source and reporting breakpoint opportunities to the sink callback {{SetLineBreakpoints}}.
        /// </summary>
        /// <param name="analysisSource">A reference to the source object to analyze.</param>
        /// <param name="textPosition">The starting text position within the source object.</param>
        /// <param name="textLength">The text length to analyze.</param>
        /// <param name="analysisSink">A reference to the  sink callback object that receives the text analysis.</param>
        /// <returns>
        /// If the method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.
        /// </returns>
        /// <unmanaged>HRESULT IDWriteTextAnalyzer::AnalyzeLineBreakpoints([None] IDWriteTextAnalysisSource* analysisSource,[None] int textPosition,[None] int textLength,[None] IDWriteTextAnalysisSink* analysisSink)</unmanaged>
        /// <remarks>
        /// Although the function can handle multiple paragraphs, the text range should not arbitrarily split the middle of paragraphs, unless the specified text span is considered a whole unit. Otherwise, the returned properties for the first and last characters will inappropriately allow breaks.
        /// </remarks>
        public void AnalyzeLineBreakpoints(TextAnalysisSource analysisSource, int textPosition, int textLength, TextAnalysisSink analysisSink)
        {
            AnalyzeLineBreakpoints__(TextAnalysisSourceShadow.ToIntPtr(analysisSource), textPosition, textLength, TextAnalysisSinkShadow.ToIntPtr(analysisSink));
        }

        /// <summary>
        /// Gets the glyphs (TODO doc)
        /// </summary>
        /// <param name="textString">The text string.</param>
        /// <param name="textLength">Length of the text.</param>
        /// <param name="fontFace">The font face.</param>
        /// <param name="isSideways">if set to <c>true</c> [is sideways].</param>
        /// <param name="isRightToLeft">if set to <c>true</c> [is right to left].</param>
        /// <param name="scriptAnalysis">The script analysis.</param>
        /// <param name="localeName">Name of the locale.</param>
        /// <param name="numberSubstitution">The number substitution.</param>
        /// <param name="features">The features.</param>
        /// <param name="featureRangeLengths">The feature range lengths.</param>
        /// <param name="maxGlyphCount">The max glyph count.</param>
        /// <param name="clusterMap">The cluster map.</param>
        /// <param name="textProps">The text props.</param>
        /// <param name="glyphIndices">The glyph indices.</param>
        /// <param name="glyphProps">The glyph props.</param>
        /// <param name="actualGlyphCount">The actual glyph count.</param>
        /// <returns>
        /// If the method succeeds, it returns <see cref="Result.Ok"/>.
        /// </returns>
        /// <unmanaged>HRESULT IDWriteTextAnalyzer::GetGlyphs([In, Buffer] const wchar_t* textString,[In] unsigned int textLength,[In] IDWriteFontFace* fontFace,[In] BOOL isSideways,[In] BOOL isRightToLeft,[In] const DWRITE_SCRIPT_ANALYSIS* scriptAnalysis,[In, Buffer, Optional] const wchar_t* localeName,[In, Optional] IDWriteNumberSubstitution* numberSubstitution,[In, Optional] const void** features,[In, Buffer, Optional] const unsigned int* featureRangeLengths,[In] unsigned int featureRanges,[In] unsigned int maxGlyphCount,[Out, Buffer] unsigned short* clusterMap,[Out, Buffer] DWRITE_SHAPING_TEXT_PROPERTIES* textProps,[Out, Buffer] unsigned short* glyphIndices,[Out, Buffer] DWRITE_SHAPING_GLYPH_PROPERTIES* glyphProps,[Out] unsigned int* actualGlyphCount)</unmanaged>
        public void GetGlyphs(string textString, int textLength, SharpDX.DirectWrite.FontFace fontFace, bool isSideways, bool isRightToLeft, SharpDX.DirectWrite.ScriptAnalysis scriptAnalysis, string localeName, SharpDX.DirectWrite.NumberSubstitution numberSubstitution, FontFeature[][] features, int[] featureRangeLengths, int maxGlyphCount, short[] clusterMap, SharpDX.DirectWrite.ShapingTextProperties[] textProps, short[] glyphIndices, SharpDX.DirectWrite.ShapingGlyphProperties[] glyphProps, out int actualGlyphCount)
        {

            var pFeatures = AllocateFeatures(features);
            try
            {
                GetGlyphs(
                    textString,
                    textLength,
                    fontFace,
                    isSideways,
                    isRightToLeft,
                    scriptAnalysis,
                    localeName,
                    numberSubstitution,
                    pFeatures,
                    featureRangeLengths,
                    featureRangeLengths == null ? 0 : featureRangeLengths.Length,
                    maxGlyphCount,
                    clusterMap,
                    textProps,
                    glyphIndices,
                    glyphProps,
                    out actualGlyphCount);
            } finally
            {
                if (pFeatures != IntPtr.Zero) Marshal.FreeHGlobal(pFeatures);
            }
        }

        /// <summary>
        /// Gets the glyph placements.
        /// </summary>
        /// <param name="textString">The text string.</param>
        /// <param name="clusterMap">The cluster map.</param>
        /// <param name="textProps">The text props.</param>
        /// <param name="textLength">Length of the text.</param>
        /// <param name="glyphIndices">The glyph indices.</param>
        /// <param name="glyphProps">The glyph props.</param>
        /// <param name="glyphCount">The glyph count.</param>
        /// <param name="fontFace">The font face.</param>
        /// <param name="fontEmSize">Size of the font in ems.</param>
        /// <param name="isSideways">if set to <c>true</c> [is sideways].</param>
        /// <param name="isRightToLeft">if set to <c>true</c> [is right to left].</param>
        /// <param name="scriptAnalysis">The script analysis.</param>
        /// <param name="localeName">Name of the locale.</param>
        /// <param name="features">The features.</param>
        /// <param name="featureRangeLengths">The feature range lengths.</param>
        /// <param name="glyphAdvances">The glyph advances.</param>
        /// <param name="glyphOffsets">The glyph offsets.</param>
        /// <returns>
        /// If the method succeeds, it returns <see cref="Result.Ok"/>.
        /// </returns>
        /// <unmanaged>HRESULT IDWriteTextAnalyzer::GetGlyphPlacements([In, Buffer] const wchar_t* textString,[In, Buffer] const unsigned short* clusterMap,[In, Buffer] DWRITE_SHAPING_TEXT_PROPERTIES* textProps,[In] unsigned int textLength,[In, Buffer] const unsigned short* glyphIndices,[In, Buffer] const DWRITE_SHAPING_GLYPH_PROPERTIES* glyphProps,[In] unsigned int glyphCount,[In] IDWriteFontFace* fontFace,[In] float fontEmSize,[In] BOOL isSideways,[In] BOOL isRightToLeft,[In] const DWRITE_SCRIPT_ANALYSIS* scriptAnalysis,[In, Buffer, Optional] const wchar_t* localeName,[In, Optional] const void** features,[In, Buffer, Optional] const unsigned int* featureRangeLengths,[In] unsigned int featureRanges,[Out, Buffer] float* glyphAdvances,[Out, Buffer] DWRITE_GLYPH_OFFSET* glyphOffsets)</unmanaged>
        public void GetGlyphPlacements(string textString, short[] clusterMap, SharpDX.DirectWrite.ShapingTextProperties[] textProps, int textLength, short[] glyphIndices, SharpDX.DirectWrite.ShapingGlyphProperties[] glyphProps, int glyphCount, SharpDX.DirectWrite.FontFace fontFace, float fontEmSize, bool isSideways, bool isRightToLeft, SharpDX.DirectWrite.ScriptAnalysis scriptAnalysis, string localeName, FontFeature[][] features, int[] featureRangeLengths, float[] glyphAdvances, SharpDX.DirectWrite.GlyphOffset[] glyphOffsets)
        {
            var pFeatures = AllocateFeatures(features);
            try
            {
                GetGlyphPlacements(
                    textString,
                    clusterMap,
                    textProps,
                    textLength,
                    glyphIndices,
                    glyphProps,
                    glyphCount,
                    fontFace,
                    fontEmSize,
                    isSideways,
                    isRightToLeft,
                    scriptAnalysis,
                    localeName,
                    pFeatures,
                    featureRangeLengths,
                    featureRangeLengths == null ? 0 : featureRangeLengths.Length,
                    glyphAdvances,
                    glyphOffsets
                    );
            }
            finally
            {
                if (pFeatures != IntPtr.Zero) Marshal.FreeHGlobal(pFeatures);
            }
        }

        /// <summary>
        /// Gets the GDI compatible glyph placements.
        /// </summary>
        /// <param name="textString">The text string.</param>
        /// <param name="clusterMap">The cluster map.</param>
        /// <param name="textProps">The text props.</param>
        /// <param name="textLength">Length of the text.</param>
        /// <param name="glyphIndices">The glyph indices.</param>
        /// <param name="glyphProps">The glyph props.</param>
        /// <param name="glyphCount">The glyph count.</param>
        /// <param name="fontFace">The font face.</param>
        /// <param name="fontEmSize">Size of the font em.</param>
        /// <param name="pixelsPerDip">The pixels per dip.</param>
        /// <param name="transform">The transform.</param>
        /// <param name="useGdiNatural">if set to <c>true</c> [use GDI natural].</param>
        /// <param name="isSideways">if set to <c>true</c> [is sideways].</param>
        /// <param name="isRightToLeft">if set to <c>true</c> [is right to left].</param>
        /// <param name="scriptAnalysis">The script analysis.</param>
        /// <param name="localeName">Name of the locale.</param>
        /// <param name="features">The features.</param>
        /// <param name="featureRangeLengths">The feature range lengths.</param>
        /// <param name="glyphAdvances">The glyph advances.</param>
        /// <param name="glyphOffsets">The glyph offsets.</param>
        /// <returns>
        /// If the method succeeds, it returns <see cref="Result.Ok"/>.
        /// </returns>
        /// <unmanaged>HRESULT IDWriteTextAnalyzer::GetGdiCompatibleGlyphPlacements([In, Buffer] const wchar_t* textString,[In, Buffer] const unsigned short* clusterMap,[In, Buffer] DWRITE_SHAPING_TEXT_PROPERTIES* textProps,[In] unsigned int textLength,[In, Buffer] const unsigned short* glyphIndices,[In, Buffer] const DWRITE_SHAPING_GLYPH_PROPERTIES* glyphProps,[In] unsigned int glyphCount,[In] IDWriteFontFace* fontFace,[In] float fontEmSize,[In] float pixelsPerDip,[In, Optional] const DWRITE_MATRIX* transform,[In] BOOL useGdiNatural,[In] BOOL isSideways,[In] BOOL isRightToLeft,[In] const DWRITE_SCRIPT_ANALYSIS* scriptAnalysis,[In, Buffer, Optional] const wchar_t* localeName,[In, Optional] const void** features,[In, Buffer, Optional] const unsigned int* featureRangeLengths,[In] unsigned int featureRanges,[Out, Buffer] float* glyphAdvances,[Out, Buffer] DWRITE_GLYPH_OFFSET* glyphOffsets)</unmanaged>
        public void GetGdiCompatibleGlyphPlacements(string textString, short[] clusterMap, SharpDX.DirectWrite.ShapingTextProperties[] textProps, int textLength, short[] glyphIndices, SharpDX.DirectWrite.ShapingGlyphProperties[] glyphProps, int glyphCount, SharpDX.DirectWrite.FontFace fontFace, float fontEmSize, float pixelsPerDip, Matrix3x2? transform, bool useGdiNatural, bool isSideways, bool isRightToLeft, SharpDX.DirectWrite.ScriptAnalysis scriptAnalysis, string localeName, FontFeature[][] features, int[] featureRangeLengths, float[] glyphAdvances, SharpDX.DirectWrite.GlyphOffset[] glyphOffsets)
        {
            var pFeatures = AllocateFeatures(features);
            try
            {
                GetGdiCompatibleGlyphPlacements(
                    textString,
                    clusterMap,
                    textProps,
                    textLength,
                    glyphIndices,
                    glyphProps,
                    glyphCount,
                    fontFace,
                    fontEmSize,
                    pixelsPerDip,
                    transform,
                    useGdiNatural,
                    isSideways,
                    isRightToLeft,
                    scriptAnalysis,
                    localeName,
                    pFeatures,
                    featureRangeLengths,
                    featureRangeLengths == null ? 0 : featureRangeLengths.Length,
                    glyphAdvances,
                    glyphOffsets
                    );
            }
            finally
            {
                if (pFeatures != IntPtr.Zero) Marshal.FreeHGlobal(pFeatures);
            }
        }

        /// <summary>
        /// Allocates the features from the jagged array..
        /// </summary>
        /// <param name="features">The features.</param>
        /// <returns>A pointer to the allocated native features or 0 if features is null or empty.</returns>
        private static IntPtr AllocateFeatures(FontFeature[][] features)
        {
            unsafe
            {
                var pFeatures = (byte*)0;

                if (features != null && features.Length > 0)
                {

                    // Calculate the total size of the buffer to allocate:
                    //      (0)              (1)                    (2)
                    // -------------------------------------------------------------
                    // |   array    | TypographicFeatures ||   FontFeatures        ||
                    // | ptr to (1) |     |       |       ||                       ||
                    // |            | ptr to FontFeatures ||                       ||
                    // -------------------------------------------------------------
                    // Offset in bytes to (1)
                    int offsetToTypographicFeatures = sizeof(IntPtr) * features.Length;
                    
                    // Add offset (1) and Size in bytes to (1)
                    int calcSize = offsetToTypographicFeatures + sizeof(TypographicFeatures) * features.Length;

                    // Calculate size (2)
                    foreach (var fontFeature in features)
                    {
                        if (fontFeature == null)
                            throw new ArgumentNullException("features", "FontFeature[] inside features array cannot be null.");

                        // calcSize += typographicFeatures.Length * sizeof(FontFeature)
                        calcSize += sizeof(FontFeature) * fontFeature.Length;
                    }

                    // Allocate the whole buffer
                    pFeatures = (byte*)Marshal.AllocHGlobal(calcSize);

                    // Pointer to (1)
                    var pTypographicFeatures = (TypographicFeatures*)(pFeatures + offsetToTypographicFeatures);

                    // Pointer to (2)
                    var pFontFeatures = (FontFeature*)(pTypographicFeatures + features.Length);

                    // Iterate on features and copy them to (2)
                    for (int i = 0; i < features.Length; i++)
                    {
                        // Write array pointers in (0)
                        ((void**)pFeatures)[i] = pTypographicFeatures;
                        
                        var featureSet = features[i];

                        // Write TypographicFeatures in (1)
                        pTypographicFeatures->Features = (IntPtr)pFontFeatures;
                        pTypographicFeatures->FeatureCount = featureSet.Length;
                        pTypographicFeatures++;

                        // Write FontFeatures in (2)
                        for (int j = 0; j < featureSet.Length; j++)
                        {
                            *pFontFeatures = featureSet[j];
                            pFontFeatures++;
                        }
                    }
                }
                return (IntPtr)pFeatures;
            }
        }
    }
}