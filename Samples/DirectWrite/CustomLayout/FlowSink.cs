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

using System.Collections.Generic;
using SharpDX.DirectWrite;

namespace CustomLayout
{
    /// <summary>
    /// Sink interface for where text is placed.
    /// </summary>
    internal class FlowLayoutSink
    {
        List<CustomGlyphRun> glyphRuns_;
        List<short> glyphIndices_;
        List<float> glyphAdvances_;
        List<GlyphOffset> glyphOffsets_;

        public FlowLayoutSink()
        {
            glyphRuns_ = new List<CustomGlyphRun>();
            glyphIndices_ = new List<short>();
            glyphAdvances_ = new List<float>();
            glyphOffsets_ = new List<GlyphOffset>();
        }
        public void Reset()
        {
            glyphRuns_.Clear();
            glyphIndices_.Clear();
            glyphAdvances_.Clear();
            glyphOffsets_.Clear();
        }
        public void SetGlyphRun(float x, float y, int glyphCount, short[] glyphIndices, float[] glyphAdvances, GlyphOffset[] glyphOffsets, FontFace fontface, float fontEmSize, int BidiLevel, bool isSideways)
        {
            // Append this glyph run to the list.
            int glyphStart = glyphAdvances_.Count;
            glyphAdvances_.AddRange(glyphAdvances);
            glyphIndices_.AddRange(glyphIndices);
            glyphOffsets_.AddRange(glyphOffsets);
            glyphRuns_.Add(new CustomGlyphRun(fontface, fontEmSize, x, y, glyphStart, glyphCount, BidiLevel, isSideways));
        }
        public void DrawGlyphRuns(SharpDX.Direct2D1.WindowRenderTarget renderTarget, SharpDX.Direct2D1.Brush brush)
        {
            // Just iterate through all the saved glyph runs
            // and have DWrite to draw each one.

            for (int i = 0; i < glyphRuns_.Count; i++)
            {
                CustomGlyphRun customGlyphRun = glyphRuns_[i];
                if (customGlyphRun.glyphCount == 0)
                    continue;

                GlyphRun glyphRun = customGlyphRun.Convert(glyphIndices_, glyphAdvances_, glyphOffsets_);
                if (glyphRun != null)
                    renderTarget.DrawGlyphRun(new System.Drawing.PointF(customGlyphRun.x, customGlyphRun.y), ref glyphRun, brush, MeasuringMode.Natural);
            }
        }
    }


    /// <summary>
    /// This glyph run is based off DWRITE_GLYPH_RUN
    /// and is trivially convertable to it, but stores
    /// pointers as relative indices instead 
    /// of raw pointers, which makes it more useful for
    /// storing in a vector. Additionally, it stores
    /// the x,y coordinate.
    /// </summary>
    internal struct CustomGlyphRun
    {
        public FontFace fontFace;
        public float fontEmSize;
        public float x;
        public float y;
        public int glyphStart;
        public int glyphCount;
        public int bidiLevel;
        public bool isSideways;

        public CustomGlyphRun(FontFace _fontFace, float _fontEmSize, float _x, float _y, int _glyphStart, int _glyphCount, int _bidiLevel, bool _isSideways)
        {
            fontFace = _fontFace;
            fontEmSize = _fontEmSize;
            x = _x;
            y = _y;
            glyphStart = _glyphStart;
            glyphCount = _glyphCount;
            bidiLevel = _bidiLevel;
            isSideways = _isSideways;
        }

        internal GlyphRun Convert(List<short> glyphIndices, List<float> glyphAdvances, List<GlyphOffset> glyphOffsets)
        {
            try
            {
                GlyphRun gr = new GlyphRun();
                gr.Items = new GlyphRunItem[glyphCount];
                for (int i = 0; i < glyphCount; i++)
                {
                    gr.Items[i].Index = glyphIndices[glyphStart + i];
                    gr.Items[i].Advance = glyphAdvances[glyphStart + i];
                    gr.Items[i].Offset = glyphOffsets[glyphStart + i];
                }
                gr.FontSize = fontEmSize;
                gr.FontFace = fontFace;
                gr.BidiLevel = bidiLevel;
                gr.IsSideways = isSideways;
                return gr;
            }
            catch
            {
                return null;
            }
        }
    }
}
