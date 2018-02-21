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
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;

namespace SharpDX.DirectWrite
{
    public partial class TextLayout
    {
        /// <summary>	
        ///  Takes a string, text format, and associated constraints, and produces an object that represents the fully analyzed and formatted result. 	
        /// </summary>	
        /// <param name="factory">an instance of <see cref = "SharpDX.DirectWrite.Factory" /></param>
        /// <param name="text">An array of characters that contains the string to create a new <see cref="SharpDX.DirectWrite.TextLayout"/> object from. This array must be of length stringLength and can contain embedded NULL characters.</param>
        /// <param name="textFormat">A pointer to an object that indicates the format to apply to the string.</param>
        /// <param name="maxWidth">The width of the layout box.</param>
        /// <param name="maxHeight">The height of the layout box.</param>
        /// <unmanaged>HRESULT CreateTextLayout([In, Buffer] const wchar* string,[None] UINT32 stringLength,[None] IDWriteTextFormat* textFormat,[None] FLOAT maxWidth,[None] FLOAT maxHeight,[Out] IDWriteTextLayout** textLayout)</unmanaged>
        public TextLayout(Factory factory, string text, SharpDX.DirectWrite.TextFormat textFormat, float maxWidth, float maxHeight)
            : base(IntPtr.Zero)
        {
            factory.CreateTextLayout(text, text.Length, textFormat, maxWidth, maxHeight, this);
        }

        /// <summary>	
        /// Create a Gdi Compatible TextLayout. Takes a string, format, and associated constraints, and produces an object representing the result, formatted for a particular display resolution and measuring mode.  	
        /// </summary>	
        /// <remarks>	
        /// The resulting text layout should only be used for the intended resolution, and for cases where text scalability is desired {{CreateTextLayout}} should be used instead. 	
        /// </remarks>	
        /// <param name="factory">an instance of <see cref = "SharpDX.DirectWrite.Factory" /></param>
        /// <param name="text">An array of characters that contains the string to create a new <see cref="T:SharpDX.DirectWrite.TextLayout" /> object from. This array must be of length stringLength and can contain embedded NULL characters. </param>
        /// <param name="textFormat">The text formatting object to apply to the string. </param>
        /// <param name="layoutWidth">The width of the layout box. </param>
        /// <param name="layoutHeight">The height of the layout box. </param>
        /// <param name="pixelsPerDip">The number of physical pixels per DIP (device independent pixel). For example, if rendering onto a 96 DPI device pixelsPerDip is 1. If rendering onto a 120 DPI device pixelsPerDip is 1.25 (120/96). </param>
        /// <param name="useGdiNatural">Instructs the text layout to use the same metrics as GDI bi-level text when set to FALSE. When set to TRUE, instructs the text layout to use the same metrics as text measured by GDI using a font created with CLEARTYPE_NATURAL_QUALITY.  </param>
        /// <unmanaged>HRESULT IDWriteFactory::CreateGdiCompatibleTextLayout([In, Buffer] const wchar_t* string,[None] int stringLength,[None] IDWriteTextFormat* textFormat,[None] float layoutWidth,[None] float layoutHeight,[None] float pixelsPerDip,[In, Optional] const DWRITE_MATRIX* transform,[None] BOOL useGdiNatural,[Out] IDWriteTextLayout** textLayout)</unmanaged>
        public TextLayout(Factory factory, string text, TextFormat textFormat, float layoutWidth, float layoutHeight, float pixelsPerDip, bool useGdiNatural)
            : this(factory, text, textFormat, layoutWidth, layoutHeight, pixelsPerDip, null , useGdiNatural)
        {
        }
        
        /// <summary>	
        /// Create a GDI Compatible TextLayout. Takes a string, format, and associated constraints, and produces an object representing the result, formatted for a particular display resolution and measuring mode.  	
        /// </summary>	
        /// <remarks>	
        /// The resulting text layout should only be used for the intended resolution, and for cases where text scalability is desired {{CreateTextLayout}} should be used instead. 	
        /// </remarks>	
        /// <param name="factory">an instance of <see cref = "SharpDX.DirectWrite.Factory" /></param>
        /// <param name="text">An array of characters that contains the string to create a new <see cref="T:SharpDX.DirectWrite.TextLayout" /> object from. This array must be of length stringLength and can contain embedded NULL characters. </param>
        /// <param name="textFormat">The text formatting object to apply to the string. </param>
        /// <param name="layoutWidth">The width of the layout box. </param>
        /// <param name="layoutHeight">The height of the layout box. </param>
        /// <param name="pixelsPerDip">The number of physical pixels per DIP (device independent pixel). For example, if rendering onto a 96 DPI device pixelsPerDip is 1. If rendering onto a 120 DPI device pixelsPerDip is 1.25 (120/96). </param>
        /// <param name="transform">An optional transform applied to the glyphs and their positions. This transform is applied after the scaling specifies the font size and pixels per DIP. </param>
        /// <param name="useGdiNatural">Instructs the text layout to use the same metrics as GDI bi-level text when set to FALSE. When set to TRUE, instructs the text layout to use the same metrics as text measured by GDI using a font created with CLEARTYPE_NATURAL_QUALITY.  </param>
        /// <unmanaged>HRESULT IDWriteFactory::CreateGdiCompatibleTextLayout([In, Buffer] const wchar_t* string,[None] int stringLength,[None] IDWriteTextFormat* textFormat,[None] float layoutWidth,[None] float layoutHeight,[None] float pixelsPerDip,[In, Optional] const DWRITE_MATRIX* transform,[None] BOOL useGdiNatural,[Out] IDWriteTextLayout** textLayout)</unmanaged>
        public TextLayout(Factory factory, string text, TextFormat textFormat, float layoutWidth, float layoutHeight, float pixelsPerDip, RawMatrix3x2? transform, bool useGdiNatural) : base(IntPtr.Zero)
        {
            factory.CreateGdiCompatibleTextLayout(text, text.Length, textFormat, layoutWidth, layoutHeight, pixelsPerDip, transform, useGdiNatural, this);
        }

        /// <summary>	
        ///  Draws text using the specified client drawing context.	
        /// </summary>	
        /// <remarks>	
        /// To draw text with this method, a textLayout object needs to be created by the application using <see cref="SharpDX.DirectWrite.Factory.CreateTextLayout"/>. After the textLayout object is obtained, the application calls the  IDWriteTextLayout::Draw method  to draw the text, decorations, and inline objects. The actual drawing is done through the callback interface passed in as the textRenderer argument; there, the corresponding DrawGlyphRun API is called. 	
        /// </remarks>	
        /// <param name="renderer">Pointer to the set of callback functions used to draw parts of a text string.</param>
        /// <param name="originX">The x-coordinate of the layout's left side.</param>
        /// <param name="originY">The y-coordinate of the layout's top side.</param>
        /// <returns>If the method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        /// <unmanaged>HRESULT Draw([None] void* clientDrawingContext,[None] IDWriteTextRenderer* renderer,[None] FLOAT originX,[None] FLOAT originY)</unmanaged>
        public void Draw(TextRenderer renderer, float originX, float originY)
        {
            Draw(null, renderer, originX, originY);
        }

        /// <summary>	
        ///  Draws text using the specified client drawing context.	
        /// </summary>	
        /// <remarks>	
        /// To draw text with this method, a textLayout object needs to be created by the application using <see cref="SharpDX.DirectWrite.Factory.CreateTextLayout"/>. After the textLayout object is obtained, the application calls the  IDWriteTextLayout::Draw method  to draw the text, decorations, and inline objects. The actual drawing is done through the callback interface passed in as the textRenderer argument; there, the corresponding DrawGlyphRun API is called. 	
        /// </remarks>	
        /// <param name="clientDrawingContext">An application-defined drawing context. </param>
        /// <param name="renderer">Pointer to the set of callback functions used to draw parts of a text string.</param>
        /// <param name="originX">The x-coordinate of the layout's left side.</param>
        /// <param name="originY">The y-coordinate of the layout's top side.</param>
        /// <returns>If the method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        /// <unmanaged>HRESULT Draw([None] void* clientDrawingContext,[None] IDWriteTextRenderer* renderer,[None] FLOAT originX,[None] FLOAT originY)</unmanaged>
        public void Draw(object clientDrawingContext, TextRenderer renderer, float originX, float originY) {
            var handle = GCHandle.Alloc(clientDrawingContext);
            try
            {
                this.Draw(GCHandle.ToIntPtr(handle), renderer, originX, originY);
            }
            finally
            {
                if (handle.IsAllocated) handle.Free();
            }
        }

        /// <summary>	
        /// Retrieves logical properties and measurements of each glyph cluster. 	
        /// </summary>	
        /// <remarks>	
        /// If maxClusterCount is not large enough, then E_NOT_SUFFICIENT_BUFFER, which is equivalent to HRESULT_FROM_WIN32(ERROR_INSUFFICIENT_BUFFER), is returned and actualClusterCount is set to the number of clusters needed.  	
        /// </remarks>	
        /// <returns>Returns metrics, such as line-break or total advance width, for a glyph cluster. </returns>
        /// <unmanaged>HRESULT IDWriteTextLayout::GetClusterMetrics([Out, Buffer, Optional] DWRITE_CLUSTER_METRICS* clusterMetrics,[None] int maxClusterCount,[Out] int* actualClusterCount)</unmanaged>
        public ClusterMetrics[] GetClusterMetrics()
        {
            var clusterMetrics = new ClusterMetrics[0];
            int clusterCount = 0;
            int maxClusterCount = 0;
            GetClusterMetrics(clusterMetrics, clusterCount, out maxClusterCount);
            if (maxClusterCount > 0)
            {
                clusterMetrics = new ClusterMetrics[maxClusterCount];
                GetClusterMetrics(clusterMetrics, maxClusterCount, out maxClusterCount);
            }
            return clusterMetrics;
        }


        /// <summary>	
        /// Sets the application-defined drawing effect. 	
        /// </summary>	
        /// <remarks>	
        /// An <see cref="SharpDX.Direct2D1.Brush"/>, such as a color or gradient brush, can be set as a drawing effect if you are using the <see cref="RenderTarget.DrawTextLayout(System.Drawing.PointF,SharpDX.DirectWrite.TextLayout,SharpDX.Direct2D1.Brush,SharpDX.Direct2D1.DrawTextOptions)"/> to draw text and that brush will be used to draw the specified range of text.  This drawing effect is associated with the specified range and will be passed back to the application by way of the callback when the range is drawn at drawing time.  	
        /// </remarks>	
        /// <param name="drawingEffect">Application-defined drawing effects that apply to the range. This data object will be passed back to the application's drawing callbacks for final rendering. </param>
        /// <param name="textRange">The text range to which this change applies. </param>
        /// <returns>If the method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code. </returns>
        /// <unmanaged>HRESULT IDWriteTextLayout::SetDrawingEffect([None] IUnknown* drawingEffect,[None] DWRITE_TEXT_RANGE textRange)</unmanaged>
        public void SetDrawingEffect(SharpDX.ComObject drawingEffect, SharpDX.DirectWrite.TextRange textRange)
        {
            var drawingEffectPtr = Utilities.GetIUnknownForObject(drawingEffect);
            SetDrawingEffect(drawingEffectPtr, textRange);
            if (drawingEffectPtr != IntPtr.Zero)
                Marshal.Release(drawingEffectPtr);
        }

        /// <summary>	
        /// Gets the application-defined drawing effect at the specified text position. 	
        /// </summary>	
        /// <param name="currentPosition">The position of the text whose drawing effect is to be retrieved. </param>
        /// <returns>a reference to  the current application-defined drawing effect. Usually this effect is a foreground brush that  is used in glyph drawing. </returns>
        /// <unmanaged>HRESULT IDWriteTextLayout::GetDrawingEffect([None] int currentPosition,[Out] IUnknown** drawingEffect,[Out, Optional] DWRITE_TEXT_RANGE* textRange)</unmanaged>
        public ComObject GetDrawingEffect(int currentPosition)
        {
            TextRange temp;
            return GetDrawingEffect(currentPosition, out temp);
        }

        /// <summary>	
        /// Gets the application-defined drawing effect at the specified text position. 	
        /// </summary>	
        /// <param name="currentPosition">The position of the text whose drawing effect is to be retrieved. </param>
        /// <param name="textRange">Contains the range of text that has the same  formatting as the text at the position specified by currentPosition.  This means the run has the exact  formatting as the position specified, including but not limited to the drawing effect. </param>
        /// <returns>a reference to  the current application-defined drawing effect. Usually this effect is a foreground brush that  is used in glyph drawing. </returns>
        /// <unmanaged>HRESULT IDWriteTextLayout::GetDrawingEffect([None] int currentPosition,[Out] IUnknown** drawingEffect,[Out, Optional] DWRITE_TEXT_RANGE* textRange)</unmanaged>
        public ComObject GetDrawingEffect(int currentPosition, out TextRange textRange)
        {
            return (ComObject)Utilities.GetObjectForIUnknown(GetDrawingEffect_(currentPosition, out textRange));
        }

        /// <summary>	
        /// Gets the font collection associated with the text at the specified position. 	
        /// </summary>	
        /// <param name="currentPosition">The position of the text to inspect. </param>
        /// <returns>a  reference to the current font collection.</returns>
        /// <unmanaged>HRESULT IDWriteTextLayout::GetFontCollection([None] int currentPosition,[Out] IDWriteFontCollection** fontCollection,[Out, Optional] DWRITE_TEXT_RANGE* textRange)</unmanaged>
        public FontCollection GetFontCollection(int currentPosition)
        {
            TextRange temp;
            return GetFontCollection(currentPosition, out temp);
        }

        /// <summary>	
        /// Gets the font family name of the text at the specified position. 	
        /// </summary>	
        /// <param name="currentPosition">The position of the text to examine. </param>
        /// <returns>the font family name </returns>
        /// <unmanaged>HRESULT IDWriteTextLayout::GetFontFamilyName([None] int currentPosition,[Out, Buffer] wchar_t* fontFamilyName,[None] int nameSize,[Out, Optional] DWRITE_TEXT_RANGE* textRange)</unmanaged>
        public string GetFontFamilyName(int currentPosition)
        {
            TextRange temp;
            return GetFontFamilyName(currentPosition, out temp);
        }

        /// <summary>	
        /// Gets the font family name of the text at the specified position. 	
        /// </summary>	
        /// <param name="currentPosition">The position of the text to examine. </param>
        /// <param name="textRange">The range of text that has the same  formatting as the text at the position specified by currentPosition.  This means the run has the exact  formatting as the position specified, including but not limited to the font family name. </param>
        /// <returns>the font family name </returns>
        /// <unmanaged>HRESULT IDWriteTextLayout::GetFontFamilyName([None] int currentPosition,[Out, Buffer] wchar_t* fontFamilyName,[None] int nameSize,[Out, Optional] DWRITE_TEXT_RANGE* textRange)</unmanaged>
        public string GetFontFamilyName(int currentPosition, out TextRange textRange)
        {
            unsafe
            {
                int nameLength;
                GetFontFamilyNameLength(currentPosition, out nameLength, out textRange);

                char* fontFamilyNamePtr = stackalloc char[nameLength + 1];

                GetFontFamilyName(currentPosition, new IntPtr(fontFamilyNamePtr), nameLength+1, out textRange);

                return new string(fontFamilyNamePtr, 0, nameLength);
            }
        }

        /// <summary>	
        /// Gets the font em height of the text at the specified position. 	
        /// </summary>	
        /// <param name="currentPosition">The position of the text to inspect. </param>
        /// <returns>The size of the font in ems of the text at the specified position. </returns>
        /// <unmanaged>HRESULT IDWriteTextLayout::GetFontSize([None] int currentPosition,[Out] float* fontSize,[Out, Optional] DWRITE_TEXT_RANGE* textRange)</unmanaged>
        public float GetFontSize(int currentPosition)
        {
            TextRange temp;
            return GetFontSize(currentPosition, out temp);         
        }

        /// <summary>	
        /// Gets the font stretch of the text at the specified position. 	
        /// </summary>	
        /// <param name="currentPosition">The position of the text to inspect. </param>
        /// <returns>a value which indicates the type of font stretch (also known as width) being applied at the specified position.</returns>
        /// <unmanaged>HRESULT IDWriteTextLayout::GetFontStretch([None] int currentPosition,[Out] DWRITE_FONT_STRETCH* fontStretch,[Out, Optional] DWRITE_TEXT_RANGE* textRange)</unmanaged>
        public FontStretch GetFontStretch(int currentPosition)
        {
            TextRange temp;
            return GetFontStretch(currentPosition, out temp);
        }

        /// <summary>	
        /// Gets the font style (also known as slope) of the text at the specified position. 	
        /// </summary>	
        /// <param name="currentPosition">The position of the text to inspect. </param>
        /// <returns>a value which indicates the type of font style (also known as slope or incline) being applied at the specified position.</returns>
        /// <unmanaged>HRESULT IDWriteTextLayout::GetFontStyle([None] int currentPosition,[Out] DWRITE_FONT_STYLE* fontStyle,[Out, Optional] DWRITE_TEXT_RANGE* textRange)</unmanaged>
        public FontStyle GetFontStyle(int currentPosition)
        {
            TextRange temp;
            return GetFontStyle(currentPosition, out temp);
        }

        /// <summary>	
        /// Gets the font weight of the text at the specified position. 	
        /// </summary>	
        /// <param name="currentPosition">The position of the text to inspect. </param>
        /// <returns>a value which indicates the type of font weight being applied at the specified position.</returns>
        /// <unmanaged>HRESULT IDWriteTextLayout::GetFontWeight([None] int currentPosition,[Out] DWRITE_FONT_WEIGHT* fontWeight,[Out, Optional] DWRITE_TEXT_RANGE* textRange)</unmanaged>
        public FontWeight GetFontWeight(int currentPosition)
        {
            TextRange temp;
            return GetFontWeight(currentPosition, out temp);
        }


        /// <summary>	
        /// Gets the inline object at the specified position. 	
        /// </summary>	
        /// <param name="currentPosition">The specified text position. </param>
        /// <returns>an application-defined inline object. </returns>
        /// <unmanaged>HRESULT IDWriteTextLayout::GetInlineObject([None] int currentPosition,[Out] IDWriteInlineObject** inlineObject,[Out, Optional] DWRITE_TEXT_RANGE* textRange)</unmanaged>
        public InlineObject GetInlineObject(int currentPosition)
        {
            TextRange temp;
            return GetInlineObject(currentPosition, out temp);
        }

        /// <summary>	
        /// Retrieves the information about each individual text line of the  text string. 	
        /// </summary>	
        /// <remarks>	
        /// If maxLineCount is not large enough E_NOT_SUFFICIENT_BUFFER, which is equivalent to HRESULT_FROM_WIN32(ERROR_INSUFFICIENT_BUFFER), is returned and *actualLineCount is set to the number of lines needed.  	
        /// </remarks>	
        /// <returns>If the method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code. </returns>
        /// <unmanaged>HRESULT IDWriteTextLayout::GetLineMetrics([Out, Buffer, Optional] DWRITE_LINE_METRICS* lineMetrics,[None] int maxLineCount,[Out] int* actualLineCount)</unmanaged>
        public LineMetrics[] GetLineMetrics()
        {
            var lineMetrics = new LineMetrics[0];
            int lineCount = 0;
            int maxLineCount = 0;
            GetLineMetrics(lineMetrics, lineCount, out maxLineCount);
            if (maxLineCount > 0)
            {
                lineMetrics = new LineMetrics[maxLineCount];
                GetLineMetrics(lineMetrics, maxLineCount, out maxLineCount);
            }

            return lineMetrics;            
        }

        /// <summary>	
        /// Gets the locale name of the text at the specified position. 	
        /// </summary>	
        /// <param name="currentPosition">The position of the text to inspect. </param>
        /// <returns>the locale name of the text at the specified position. </returns>
        /// <unmanaged>HRESULT IDWriteTextLayout::GetLocaleName([None] int currentPosition,[Out, Buffer] wchar_t* localeName,[None] int nameSize,[Out, Optional] DWRITE_TEXT_RANGE* textRange)</unmanaged>
        public string GetLocaleName(int currentPosition)
        {
            TextRange temp;
            return GetLocaleName(currentPosition, out temp);
        }

        /// <summary>	
        /// Gets the locale name of the text at the specified position. 	
        /// </summary>	
        /// <param name="currentPosition">The position of the text to inspect. </param>
        /// <param name="textRange">The range of text that has the same  formatting as the text at the position specified by currentPosition.  This means the run has the exact  formatting as the position specified, including but not limited to the locale name. </param>
        /// <returns>the locale name of the text at the specified position. </returns>
        /// <unmanaged>HRESULT IDWriteTextLayout::GetLocaleName([None] int currentPosition,[Out, Buffer] wchar_t* localeName,[None] int nameSize,[Out, Optional] DWRITE_TEXT_RANGE* textRange)</unmanaged>
        public string GetLocaleName(int currentPosition, out TextRange textRange)
        {
            unsafe
            {
                int nameLength;
                GetLocaleNameLength(currentPosition, out nameLength, out textRange);

                char* fontFamilyNamePtr = stackalloc char[nameLength + 1];

                GetLocaleName(currentPosition, new IntPtr(fontFamilyNamePtr), nameLength+1, out textRange);

                return new string(fontFamilyNamePtr, 0, nameLength);
            }
        }

        /// <summary>	
        /// Get the strikethrough presence of the text at the specified position. 	
        /// </summary>	
        /// <param name="currentPosition">The position of the text to inspect. </param>
        /// <returns>A Boolean  flag that indicates whether strikethrough is present at the position indicated by currentPosition. </returns>
        /// <unmanaged>HRESULT IDWriteTextLayout::GetStrikethrough([None] int currentPosition,[Out] BOOL* hasStrikethrough,[Out, Optional] DWRITE_TEXT_RANGE* textRange)</unmanaged>
        public bool HasStrikethrough(int currentPosition)
        {
            TextRange temp;
            return HasStrikethrough(currentPosition, out temp);
        }

        /// <summary>	
        /// Gets the typography setting of the text at the specified position. 	
        /// </summary>	
        /// <param name="currentPosition">The position of the text to inspect. </param>
        /// <returns>a  reference to the current typography setting. </returns>
        /// <unmanaged>HRESULT IDWriteTextLayout::GetTypography([None] int currentPosition,[Out] IDWriteTypography** typography,[Out, Optional] DWRITE_TEXT_RANGE* textRange)</unmanaged>
        public Typography GetTypography(int currentPosition)
        {
            TextRange temp;
            return GetTypography(currentPosition, out temp);
        }

        /// <summary>	
        /// Gets the underline presence of the text at the specified position. 	
        /// </summary>	
        /// <param name="currentPosition">The current text position. </param>
        /// <returns>A Boolean  flag that indicates whether underline is present at the position indicated by currentPosition. </returns>
        /// <unmanaged>HRESULT IDWriteTextLayout::GetUnderline([None] int currentPosition,[Out] BOOL* hasUnderline,[Out, Optional] DWRITE_TEXT_RANGE* textRange)</unmanaged>
        public bool HasUnderline(int currentPosition)
        {
            TextRange temp;
            return HasUnderline(currentPosition, out temp);
        }

        /// <summary>	
        /// The application calls this function to get a set of hit-test metrics corresponding to a range of text positions. 
        /// One of the main usages is to implement highlight selection of the text string. 
        /// The function returns E_NOT_SUFFICIENT_BUFFER, which is equivalent to HRESULT_FROM_WIN32(ERROR_INSUFFICIENT_BUFFER), 
        /// when the buffer size of hitTestMetrics is too small to hold all the regions calculated by the function. 
        /// In this situation, the function sets the output value *actualHitTestMetricsCount to the number of geometries calculated. 
        /// The application is responsible for allocating a new buffer of greater size and calling the function again. 
        /// A good value to use as an initial value for maxHitTestMetricsCount may be calculated from the following equation: 
        /// maxHitTestMetricsCount = lineCount * maxBidiReorderingDepth where lineCount is obtained from the value of the output argument *actualLineCount (from the function IDWriteTextLayout::GetLineLengths), and the maxBidiReorderingDepth value from the DWRITE_TEXT_METRICS structure of the output argument *textMetrics (from the function IDWriteFactory::CreateTextLayout). 	
        /// </summary>	
        /// <param name="textPosition">The first text position of the specified range. </param>
        /// <param name="textLength">The number of positions of the specified range. </param>
        /// <param name="originX">The origin pixel location X at the left of the layout box. This offset is added to the hit-test metrics returned. </param>
        /// <param name="originY">The origin pixel location Y at the top of the layout box. This offset is added to the hit-test metrics returned. </param>
        /// <returns>a reference to a buffer of the output geometry fully enclosing the specified position range.  The buffer must be at least as large as maxHitTestMetricsCount. </returns>
        /// <unmanaged>HRESULT IDWriteTextLayout::HitTestTextRange([None] int textPosition,[None] int textLength,[None] float originX,[None] float originY,[Out, Buffer, Optional] DWRITE_HIT_TEST_METRICS* hitTestMetrics,[None] int maxHitTestMetricsCount,[Out] int* actualHitTestMetricsCount)</unmanaged>
        public HitTestMetrics[] HitTestTextRange(int textPosition, int textLength, float originX, float originY)
        {
            var hitTestMetrics = new HitTestMetrics[0];

            int actualHitTestMetricsCount = 0;
            HitTestTextRange(textPosition, textLength, originX, originY, hitTestMetrics, 0, out actualHitTestMetricsCount);
            if (actualHitTestMetricsCount > 0)
            {
                hitTestMetrics = new HitTestMetrics[actualHitTestMetricsCount];
                HitTestTextRange(textPosition, textLength, originX, originY, hitTestMetrics, actualHitTestMetricsCount,
                                 out actualHitTestMetricsCount);
            }
            return hitTestMetrics;
        }
    }
}
