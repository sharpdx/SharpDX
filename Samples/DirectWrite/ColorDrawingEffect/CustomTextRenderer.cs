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
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;

namespace ColorDrawingEffect
{
    /// <summary>
    /// Custom TextRenderer
    /// </summary>
    public class CustomTextRenderer : SharpDX.DirectWrite.TextRenderer
    {
        readonly SharpDX.Direct2D1.Factory _d2DFactory;
        readonly WindowRenderTarget _renderTarget;

        public CustomTextRenderer(SharpDX.Direct2D1.Factory d2DFactory, WindowRenderTarget renderTarget)
        {
            _d2DFactory = d2DFactory;
            _renderTarget = renderTarget;
        }

        #region TextRenderer Members

        public Result DrawGlyphRun(object clientDrawingContext, float baselineOriginX, float baselineOriginY, MeasuringMode measuringMode, GlyphRun glyphRun, GlyphRunDescription glyphRunDescription, ComObject clientDrawingEffect)
        {
            var pathGeometry = new PathGeometry(_d2DFactory);
            var geometrySink = pathGeometry.Open();
            float[] advances;
            GlyphOffset[] offsets;
            var indecies = glyphRun.ToArrays(out advances, out offsets);

            var result = glyphRun.FontFace.GetGlyphRunOutline(glyphRun.FontSize, indecies, advances, offsets, glyphRun.IsSideways, glyphRun.BidiLevel % 2 != 0, geometrySink);
            geometrySink.Close();
            var matrix = new Matrix3x2()
            {
                M11 = 1,
                M12 = 0,
                M21 = 0,
                M22 = 1,
                M31 = baselineOriginX,
                M32 = baselineOriginY
            };

            var transformedGeometry = new TransformedGeometry(_d2DFactory, pathGeometry, matrix);

            var  brushColor = new Color4(1,0,0,0);

            if (clientDrawingEffect != null && clientDrawingEffect is ColorDrawingEffect)
                brushColor = (clientDrawingEffect as ColorDrawingEffect).Color;

            var brush = new SolidColorBrush(_renderTarget, brushColor);
            
            _renderTarget.DrawGeometry(transformedGeometry, brush);
            _renderTarget.FillGeometry(transformedGeometry, brush);

            pathGeometry.Dispose();
            transformedGeometry.Dispose();
            brush.Dispose();

            return SharpDX.Result.Ok;
        }

        public Result DrawInlineObject(object clientDrawingContext, float originX, float originY, InlineObject inlineObject, bool isSideways, bool isRightToLeft, ComObject clientDrawingEffect)
        {
            return SharpDX.Result.NotImplemented;
        }

        public Result DrawStrikethrough(object clientDrawingContext, float baselineOriginX, float baselineOriginY, ref Strikethrough strikethrough, ComObject clientDrawingEffect)
        {
            var rect = new SharpDX.RectangleF(0, strikethrough.Offset, strikethrough.Width, strikethrough.Offset + strikethrough.Thickness);
            var rectangleGeometry = new RectangleGeometry(_d2DFactory, rect);
            var matrix = new Matrix3x2()
            {
                M11 = 1,
                M12 = 0,
                M21 = 0,
                M22 = 1,
                M31 = baselineOriginX,
                M32 = baselineOriginY
            };
            var transformedGeometry = new TransformedGeometry(_d2DFactory, rectangleGeometry, matrix);
            
            var  brushColor = new Color4(1,0,0,0);

            if (clientDrawingEffect != null && clientDrawingEffect is ColorDrawingEffect)
                brushColor = (clientDrawingEffect as ColorDrawingEffect).Color;

            var brush = new SolidColorBrush(_renderTarget, brushColor);

            _renderTarget.DrawGeometry(transformedGeometry, brush);
            _renderTarget.FillGeometry(transformedGeometry, brush);

            rectangleGeometry.Dispose();
            transformedGeometry.Dispose();
            brush.Dispose();

            return Result.Ok;
        }

        public Result DrawUnderline(object clientDrawingContext, float baselineOriginX, float baselineOriginY, ref Underline underline, ComObject clientDrawingEffect)
        {
            var rect = new SharpDX.RectangleF(0, underline.Offset, underline.Width, underline.Offset + underline.Thickness);
            var rectangleGeometry = new RectangleGeometry(_d2DFactory, rect);
            var matrix = new Matrix3x2()
            {
                M11 = 1,
                M12 = 0,
                M21 = 0,
                M22 = 1,
                M31 = baselineOriginX,
                M32 = baselineOriginY
            };
            var transformedGeometry = new TransformedGeometry(_d2DFactory, rectangleGeometry, matrix);

            var brushColor = new Color4(1, 0, 0, 0);
            if (clientDrawingEffect != null && clientDrawingEffect is ColorDrawingEffect)
                brushColor = (clientDrawingEffect as ColorDrawingEffect).Color;

            var brush = new SolidColorBrush(_renderTarget, brushColor);

            _renderTarget.DrawGeometry(transformedGeometry, brush);
            _renderTarget.FillGeometry(transformedGeometry, brush);

            rectangleGeometry.Dispose();
            transformedGeometry.Dispose();
            brush.Dispose();

            return SharpDX.Result.Ok;
        }

        #endregion

        #region PixelSnapping Members

        public SharpDX.DirectWrite.Matrix GetCurrentTransform(object clientDrawingContext)
        {
            Matrix3x2 d2Dmatrix = _renderTarget.Transform;
            var dwMatrix = new SharpDX.DirectWrite.Matrix()
            {
                M11 = d2Dmatrix.M11,
                M12 = d2Dmatrix.M12,
                M21 = d2Dmatrix.M22,
                M22 = d2Dmatrix.M22,
                Dx = d2Dmatrix.M31,
                Dy = d2Dmatrix.M32
            };
            return dwMatrix;
        }

        public float GetPixelsPerDip(object clientDrawingContext)
        {
            return _renderTarget.PixelSize.Width / 96f;
        }

        public bool IsPixelSnappingDisabled(object clientDrawingContext)
        {
            return false;
        }

        #endregion
    }
}