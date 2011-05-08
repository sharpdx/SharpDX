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
using SharpDX.Direct2D1;

namespace SharpDX.DirectWrite
{
    public partial class FontFace
    {
        /// <summary>	
        /// Creates an object that represents a font face. 	
        /// </summary>	
        /// <param name="factory">A reference to a DirectWrite factory <see cref="Factory"/></param>
        /// <param name="fontFaceType">A value that indicates the type of file format of the font face. </param>
        /// <param name="fontFiles">A font file object representing the font face. Because<see cref="T:SharpDX.DirectWrite.FontFace" /> maintains its own references to the input font file objects, you may release them after this call. </param>
        /// <param name="faceIndex">The zero-based index of a font face, in cases when the font files contain a collection of font faces. If the font files contain a single face, this value should be zero. </param>
        /// <param name="fontFaceSimulationFlags">A value that indicates which, if any, font face simulation flags for algorithmic means of making text bold or italic are applied to the current font face. </param>
        /// <unmanaged>HRESULT IDWriteFactory::CreateFontFace([None] DWRITE_FONT_FACE_TYPE fontFaceType,[None] int numberOfFiles,[In, Buffer] const IDWriteFontFile** fontFiles,[None] int faceIndex,[None] DWRITE_FONT_SIMULATIONS fontFaceSimulationFlags,[Out] IDWriteFontFace** fontFace)</unmanaged>
        public FontFace(Factory factory, FontFaceType fontFaceType, FontFile[] fontFiles, int faceIndex, FontSimulations fontFaceSimulationFlags)
        {
            FontFace temp;
            factory.CreateFontFace(fontFaceType, fontFiles.Length, fontFiles, faceIndex, fontFaceSimulationFlags, out temp);
            NativePointer = temp.NativePointer;
        }

        /// <summary>	
        /// Creates a font face object for the font. 	
        /// </summary>	
        /// <param name="font">the <see cref="Font"/> to create the FontFace from.</param>
        /// <unmanaged>HRESULT IDWriteFont::CreateFontFace([Out] IDWriteFontFace** fontFace)</unmanaged>
        public FontFace(Font font)
        {
            FontFace temp;
            font.CreateFontFace(out temp);
            NativePointer = temp.NativePointer;
        }

        /// <summary>	
        /// Obtains ideal (resolution-independent) glyph metrics in font design units.  	
        /// </summary>	
        /// <remarks>	
        /// Design glyph metrics are used for glyph positioning. 	
        /// </remarks>	
        /// <param name="glyphIndices">An array of glyph indices for which to compute  metrics. The array must contain at least as many elements as specified by glyphCount. </param>
        /// <param name="isSideways">Indicates whether the font is being used in a sideways run. This can affect the glyph metrics if the font has oblique simulation because sideways oblique simulation differs from non-sideways oblique simulation </param>
        /// <returns>an array of <see cref="GlyphMetrics"/> structures. </returns>
        /// <unmanaged>HRESULT IDWriteFontFace::GetDesignGlyphMetrics([In, Buffer] const short* glyphIndices,[None] int glyphCount,[Out, Buffer] DWRITE_GLYPH_METRICS* glyphMetrics,[None] BOOL isSideways)</unmanaged>
        public GlyphMetrics[] GetDesignGlyphMetrics(short[] glyphIndices, bool isSideways)
        {
            var glyphMetrics = new GlyphMetrics[glyphIndices.Length];
            GetDesignGlyphMetrics(glyphIndices, glyphIndices.Length, glyphMetrics, isSideways);
            return glyphMetrics;
        }

        /// <summary>	
        /// Obtains glyph metrics in font design units with the return values compatible with what GDI would produce.	
        /// </summary>	
        /// <param name="fontSize">The ogical size of the font in DIP units. </param>
        /// <param name="pixelsPerDip">The number of physical pixels per DIP. </param>
        /// <param name="transform">An optional transform applied to the glyphs and their positions. This transform is applied after the scaling specified by the font size and pixelsPerDip. </param>
        /// <param name="useGdiNatural">When set to FALSE, the metrics are the same as the metrics of GDI aliased text.  When set to TRUE, the metrics are the same as the metrics of text measured by GDI using a font created with CLEARTYPE_NATURAL_QUALITY. </param>
        /// <param name="glyphIndices">An array of glyph indices for which to compute the metrics. </param>
        /// <param name="isSideways">A BOOL value that indicates whether the font is being used in a sideways run.  This can affect the glyph metrics if the font has oblique simulation because sideways oblique simulation differs from non-sideways oblique simulation. </param>
        /// <returns>An array of <see cref="T:SharpDX.DirectWrite.GlyphMetrics" /> structures filled by this function. The metrics are in font design units. </returns>
        /// <unmanaged>HRESULT IDWriteFontFace::GetGdiCompatibleGlyphMetrics([None] float emSize,[None] float pixelsPerDip,[In, Optional] const DWRITE_MATRIX* transform,[None] BOOL useGdiNatural,[In, Buffer] const short* glyphIndices,[None] int glyphCount,[Out, Buffer] DWRITE_GLYPH_METRICS* glyphMetrics,[None] BOOL isSideways)</unmanaged>
        public GlyphMetrics[] GetGdiCompatibleGlyphMetrics(float fontSize, float pixelsPerDip, Matrix? transform, bool useGdiNatural, short[] glyphIndices, bool isSideways)
        {
            var glyphMetrics = new GlyphMetrics[glyphIndices.Length];
            GetGdiCompatibleGlyphMetrics(fontSize, pixelsPerDip, transform, useGdiNatural, glyphIndices, glyphIndices.Length, glyphMetrics, isSideways);
            return glyphMetrics;
        }

        /// <summary>	
        /// Returns the nominal mapping of UCS4 Unicode code points to glyph indices as defined by the font 'CMAP' table. 	
        /// </summary>	
        /// <remarks>	
        /// Note that this mapping is primarily provided for line layout engines built on top of the physical font API. Because of OpenType glyph substitution and line layout character substitution, the nominal conversion does not always correspond to how a Unicode string will map to glyph indices when rendering using a particular font face. Also, note that Unicode variant selectors provide for alternate mappings for character to glyph. This call will always return the default variant.  	
        /// </remarks>	
        /// <param name="codePoints">An array of USC4 code points from which to obtain nominal glyph indices. The array must be allocated and be able to contain the number of elements specified by codePointCount. </param>
        /// <returns>a reference to an array of nominal glyph indices filled by this function.</returns>
        /// <unmanaged>HRESULT IDWriteFontFace::GetGlyphIndices([In, Buffer] const int* codePoints,[None] int codePointCount,[Out, Buffer] short* glyphIndices)</unmanaged>
        public short[] GetGlyphIndices(int[] codePoints)
        {
            var glyphIndices = new short[codePoints.Length];
            GetGlyphIndices(codePoints, codePoints.Length, glyphIndices);
            return glyphIndices;
        }

        /// <summary>	
        /// Obtains the font files representing a font face. 	
        /// </summary>	
        /// <remarks>	
        /// The IDWriteFontFace::GetFiles method should be called twice.  The first time you call GetFilesfontFiles should be NULL. When the method returns, numberOfFiles receives the number of font files that represent the font face. Then, call the method a second time, passing the numberOfFiles value that was output the first call, and a non-null buffer of the correct size to store the <see cref="SharpDX.DirectWrite.FontFile"/> references. 	
        /// </remarks>	
        /// <returns>An array that stores references to font files representing the font face. This parameter can be NULL if the user wants only the number of files representing the font face. This API increments reference count of the font file references returned according to COM conventions, and the client should release them when finished. </returns>
        /// <unmanaged>HRESULT IDWriteFontFace::GetFiles([InOut] int* numberOfFiles,[Out, Buffer, Optional] IDWriteFontFile** fontFiles)</unmanaged>
        public FontFile[] GetFiles()
        {
            int numberOfFiles = 0;
            GetFiles(ref numberOfFiles, null);
            var files = new FontFile[numberOfFiles];
            GetFiles(ref numberOfFiles, files);
            return files;
        }

        /// <summary>	
        /// Finds the specified OpenType font table if it exists and returns a reference to it. The function accesses the underlying font data through the <see cref="T:SharpDX.DirectWrite.FontFileStream" /> interface implemented by the font file loader. 	
        /// </summary>	
        /// <remarks>	
        /// The context for the same tag may be different for each call, so each one must be held and released separately.  	
        /// </remarks>	
        /// <param name="openTypeTableTag">The four-character tag of a OpenType font table to find. Use the DWRITE_MAKE_OPENTYPE_TAG macro to create it as an UINT32. Unlike GDI, it does not support the special TTCF and null tags to access the whole font. </param>
        /// <param name="tableData">When this method returns, contains the address of  a reference to the base of the table in memory. The reference is valid only as long as the font face used to get the font table still exists; (not any other font face, even if it actually refers to the same physical font).</param>
        /// <param name="tableContext">When this method returns, the address of a reference to  the opaque context, which must be freed by calling {{ReleaseFontTable}}. The context actually comes from the lower-level <see cref="T:SharpDX.DirectWrite.FontFileStream" />, which may be implemented by the application or DWrite itself. It is possible for a NULL tableContext to be returned, especially if the implementation performs direct memory mapping on the whole file. Nevertheless, always release it later, and do not use it as a test for function success. The same table can be queried multiple times, but because each returned context can be different, you must release each context separately.  </param>
        /// <returns>TRUE if the font table exists; otherwise, FALSE. </returns>
        /// <unmanaged>HRESULT IDWriteFontFace::TryGetFontTable([In] int openTypeTableTag,[Out, Buffer] const void** tableData,[Out] int* tableSize,[Out] void** tableContext,[Out] BOOL* exists)</unmanaged>
        public bool TryGetFontTable(int openTypeTableTag, out DataStream tableData, out IntPtr tableContext)
        {
            unsafe
            {
                tableData = null;
                IntPtr tableDataPtr = IntPtr.Zero;
                int tableDataSize;
                bool exists;
                TryGetFontTable(openTypeTableTag, new IntPtr(&tableDataPtr), out tableDataSize, out tableContext, out exists);
                if (tableDataPtr != IntPtr.Zero)
                    tableData = new DataStream(tableDataPtr, tableDataSize, true, true);
                return exists;
            }
        }

        /// <summary>	
        /// Computes the outline of a run of glyphs by calling back to the outline sink interface. 	
        /// </summary>	
        /// <param name="emSize">The logical size of the font in DIP units. A DIP ("device-independent pixel") equals 1/96 inch. </param>
        /// <param name="glyphIndices">An array of glyph indices. The glyphs are in logical order and the advance direction depends on the isRightToLeft parameter. The array must be allocated and be able to contain the number of elements specified by glyphCount. </param>
        /// <param name="glyphAdvances">An optional array of glyph advances in DIPs. The advance of a glyph is the amount to advance the position (in the direction of the baseline) after drawing the glyph. glyphAdvances contains the number of elements specified by glyphIndices.Length. </param>
        /// <param name="glyphOffsets">An optional array of glyph offsets, each of which specifies the offset along the baseline and offset perpendicular to the baseline of a glyph relative to the current pen position.   glyphOffsets contains the number of elements specified by glyphIndices.Length. </param>
        /// <param name="isSideways">If TRUE, the ascender of the glyph runs alongside the baseline. If FALSE, the glyph ascender runs perpendicular to the baseline. For example, an English alphabet on a vertical baseline would have isSideways set to FALSE. A client can render a vertical run by setting isSideways to TRUE and rotating the resulting geometry 90 degrees to the right using a transform. The isSideways and isRightToLeft parameters cannot both be true. </param>
        /// <param name="isRightToLeft">The visual order of the glyphs. If this parameter is FALSE, then glyph advances are from left to right. If TRUE, the advance direction is right to left. By default, the advance direction is left to right. </param>
        /// <param name="geometrySink">A reference to the interface that is called back to perform outline drawing operations. </param>
        /// <returns>If the method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code. </returns>
        /// <unmanaged>HRESULT IDWriteFontFace::GetGlyphRunOutline([None] float emSize,[In, Buffer] const short* glyphIndices,[In, Buffer, Optional] const float* glyphAdvances,[In, Buffer, Optional] const DWRITE_GLYPH_OFFSET* glyphOffsets,[None] int glyphCount,[None] BOOL isSideways,[None] BOOL isRightToLeft,[None] IDWriteGeometrySink* geometrySink)</unmanaged>
        public Result GetGlyphRunOutline(float emSize, short[] glyphIndices, float[] glyphAdvances, GlyphOffset[] glyphOffsets, bool isSideways, bool isRightToLeft, SimplifiedGeometrySink geometrySink)
        {
            SimplifiedGeometrySinkCallback callback;
            IntPtr ptr;
            if (geometrySink is ComObject)
                ptr = (geometrySink as ComObject).NativePointer;
            else
            {
                if ( geometrySink is GeometrySink)
                {
                    callback = new GeometrySinkCallback((GeometrySink)geometrySink);
                } else
                {
                    callback = new SimplifiedGeometrySinkCallback(geometrySink, 0);
                }
                ptr = callback.NativePointer;
            }
            return GetGlyphRunOutline_(emSize, glyphIndices, glyphAdvances, glyphOffsets, glyphIndices.Length, isSideways, isRightToLeft, ptr);
        }
    }
}