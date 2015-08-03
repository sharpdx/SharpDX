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

using SharpDX.Mathematics.Interop;

namespace SharpDX.Direct2D1
{
    internal partial class CommandSink2Native
    {
        /// <summary>	
        /// No documentation for Direct3D12	
        /// </summary>	
        /// <param name="ink">No documentation.</param>	
        /// <param name="brush">No documentation.</param>	
        /// <param name="inkStyle">No documentation.</param>	
        /// <returns>No documentation.</returns>	
        /// <include file='.\Documentation\CodeComments.xml' path="/comments/comment[@id='ID2D1CommandSink2::DrawInk']/*"/>	
        /// <unmanaged>HRESULT ID2D1CommandSink2::DrawInk([In] ID2D1Ink* ink,[In] ID2D1Brush* brush,[In, Optional] ID2D1InkStyle* inkStyle)</unmanaged>	
        /// <unmanaged-short>ID2D1CommandSink2::DrawInk</unmanaged-short>	
        public void DrawInk(Ink ink, Brush brush, InkStyle inkStyle)
        {
            DrawInk_(ink, brush, inkStyle);
        }
        /// <summary>	
        /// No documentation for Direct3D12	
        /// </summary>	
        /// <param name="gradientMesh">No documentation.</param>	
        /// <returns>No documentation.</returns>	
        /// <include file='.\Documentation\CodeComments.xml' path="/comments/comment[@id='ID2D1CommandSink2::DrawGradientMesh']/*"/>	
        /// <unmanaged>HRESULT ID2D1CommandSink2::DrawGradientMesh([In] ID2D1GradientMesh* gradientMesh)</unmanaged>	
        /// <unmanaged-short>ID2D1CommandSink2::DrawGradientMesh</unmanaged-short>	
        public void DrawGradientMesh(GradientMesh gradientMesh)
        {
            DrawGradientMesh_(gradientMesh);
        }
        /// <summary>	
        /// No documentation for Direct3D12	
        /// </summary>	
        /// <param name="gdiMetafile">No documentation.</param>	
        /// <param name="destinationRectangle">No documentation.</param>	
        /// <param name="sourceRectangle">No documentation.</param>	
        /// <returns>No documentation.</returns>	
        /// <include file='.\Documentation\CodeComments.xml' path="/comments/comment[@id='ID2D1CommandSink2::DrawGdiMetafile']/*"/>	
        /// <unmanaged>HRESULT ID2D1CommandSink2::DrawGdiMetafile([In] ID2D1GdiMetafile* gdiMetafile,[In, Optional] const D2D_RECT_F* destinationRectangle,[In, Optional] const D2D_RECT_F* sourceRectangle)</unmanaged>	
        /// <unmanaged-short>ID2D1CommandSink2::DrawGdiMetafile</unmanaged-short>	
        public void DrawGdiMetafile(GdiMetafile gdiMetafile, RawRectangleF? destinationRectangle, RawRectangleF? sourceRectangle)
        {
            DrawGdiMetafile_(gdiMetafile, destinationRectangle, sourceRectangle);
        }
    }
}