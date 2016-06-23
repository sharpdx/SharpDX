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

namespace SharpDX.Direct2D1
{
    public partial interface CommandSink3
    {
        /// <summary>	
        /// <p>Renders part or all of the given sprite batch to the device context using the specified drawing options.</p>	
        /// </summary>	
        /// <param name="spriteBatch"><dd>  <p>The sprite batch to draw.</p> </dd></param>	
        /// <param name="startIndex"><dd>  <p>The index of the first sprite in the sprite batch to draw.</p> </dd></param>	
        /// <param name="spriteCount"><dd>  <p>The number of sprites to draw.</p> </dd></param>	
        /// <param name="bitmap"><dd>  <p>The bitmap from which the sprites are to be sourced. Each sprite?s source rectangle refers to a portion of this bitmap.</p> </dd></param>	
        /// <param name="interpolationMode"><dd>  <p>The interpolation mode to use when drawing this sprite batch. This determines how Direct2D interpolates pixels within the drawn sprites if scaling is performed.</p> </dd></param>	
        /// <param name="spriteOptions"><dd>  <p>The additional drawing options, if any, to be used for this sprite batch.</p> </dd></param>	
        /// <returns><p>If this method succeeds, it returns <strong><see cref="SharpDX.Result.Ok"/></strong>. Otherwise, it returns an <strong><see cref="SharpDX.Result"/></strong> error code.</p></returns>	
        /// <include file='.\..\Documentation\CodeComments.xml' path="/comments/comment[@id='ID2D1CommandSink3::DrawSpriteBatch']/*"/>	
        /// <msdn-id>mt619823</msdn-id>	
        /// <unmanaged>HRESULT ID2D1CommandSink3::DrawSpriteBatch([In] ID2D1SpriteBatch* spriteBatch,[In] unsigned int startIndex,[In] unsigned int spriteCount,[In] ID2D1Bitmap* bitmap,[In] D2D1_BITMAP_INTERPOLATION_MODE interpolationMode,[In] D2D1_SPRITE_OPTIONS spriteOptions)</unmanaged>	
        /// <unmanaged-short>ID2D1CommandSink3::DrawSpriteBatch</unmanaged-short>	
        void DrawSpriteBatch(SharpDX.Direct2D1.SpriteBatch spriteBatch,
            int startIndex,
            int spriteCount,
            SharpDX.Direct2D1.Bitmap bitmap,
            SharpDX.Direct2D1.BitmapInterpolationMode interpolationMode,
            SharpDX.Direct2D1.SpriteOptions spriteOptions);

    }
}