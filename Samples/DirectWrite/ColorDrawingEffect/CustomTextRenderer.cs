// Copyright (c) 2010-2013 SharpDX - Alexandre Mutel
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
    public class CustomTextRenderer : CallbackBase, SharpDX.DirectWrite.TextRenderer
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

            var fontFace = glyphRun.FontFace;
            if (glyphRun.Indices.Length > 0)
                fontFace.GetGlyphRunOutline(glyphRun.FontSize, glyphRun.Indices, glyphRun.Advances, glyphRun.Offsets, glyphRun.IsSideways, glyphRun.BidiLevel % 2 != 0, geometrySink);
            geometrySink.Close();
            geometrySink.Dispose();
            fontFace.Dispose();

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

            var  brushColor = (Color4)Color.Black;

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
            
            var  brushColor = (Color4)Color.Black;

            if (clientDrawingEffect != null && clientDrawingEffect is ColorDrawingEffect)
                brushColor = (Color4)(clientDrawingEffect as ColorDrawingEffect).Color;

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

            var brushColor = (Color4)Color.Black;
            if (clientDrawingEffect != null && clientDrawingEffect is ColorDrawingEffect)
                brushColor = (Color4)(clientDrawingEffect as ColorDrawingEffect).Color;

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

        public Matrix3x2 GetCurrentTransform(object clientDrawingContext)
        {
            return _renderTarget.Transform;
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