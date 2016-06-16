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

namespace SharpDX.Direct3D10
{
    public partial class Sprite
    {
        /// <summary>	
        /// Create a sprite for drawing a 2D texture.	
        /// </summary>	
        /// <param name="device">A reference to the device (see <see cref="SharpDX.Direct3D10.Device"/>) that will draw the sprite. </param>
        /// <param name="bufferSize">The size of the vertex buffer, in number of sprites, that will be sent to the device when <see cref="SharpDX.Direct3D10.Sprite.Flush"/> or <see cref="DrawSpritesBuffered(SpriteInstance[])"/> is called. This should be a small number if you know you will be rendering a small number of sprites at a time (to save memory) and a large number if you know you will be rendering a large number of sprites at a time. The maximum value is 4096. If 0 is specified, the vertex buffer size will automatically be set to 4096. </param>
        /// <unmanaged>HRESULT D3DX10CreateSprite([None] ID3D10Device* pDevice,[None] int cDeviceBufferSize,[None] LPD3DX10SPRITE* ppSprite)</unmanaged>
        public Sprite(Device device, int bufferSize = 0)
        {
            D3DX10.CreateSprite(device, bufferSize, this);
        }

        /// <summary>	
        /// Add an array of sprites to the batch of sprites to be rendered. This must be called in between calls to <see cref="SharpDX.Direct3D10.Sprite.Begin"/> and <see cref="SharpDX.Direct3D10.Sprite.End"/>, and <see cref="SharpDX.Direct3D10.Sprite.Flush"/> must be called before End to send all of the batched sprites to the device for rendering. This draw method is most useful when drawing a small number of sprites that you want buffered into a large batch, such as fonts.	
        /// </summary>	
        /// <param name="sprites">The array of sprites to draw. See <see cref="SharpDX.Direct3D10.SpriteInstance"/>. </param>
        /// <returns>If the method succeeds, the return value is S_OK. If the method fails, the return value can be one of the following: D3DERR_INVALIDCALL, D3DXERR_INVALIDDATA. </returns>
        /// <unmanaged>HRESULT ID3DX10Sprite::DrawSpritesBuffered([None] D3DX10_SPRITE* pSprites,[None] int cSprites)</unmanaged>
        public void DrawSpritesBuffered(SharpDX.Direct3D10.SpriteInstance[] sprites)
        {
            DrawSpritesBuffered(sprites, sprites.Length);
        }

        /// <summary>	
        /// Draw an array of sprites. This will immediately send the sprites to the device for rendering, which is different from <see cref="DrawSpritesBuffered(SpriteInstance[])"/> which only adds an array of sprites to a batch of sprites to be rendered when <see cref="SharpDX.Direct3D10.Sprite.Flush"/> is called. This draw method is most useful when drawing a large number of sprites that have already been sorted on the CPU (or do not need to be sorted), such as in a particle system. This must be called in between calls to <see cref="SharpDX.Direct3D10.Sprite.Begin"/> and <see cref="SharpDX.Direct3D10.Sprite.End"/>.	
        /// </summary>	
        /// <param name="sprites">The array of sprites to draw. See <see cref="SharpDX.Direct3D10.SpriteInstance"/>. </param>
        /// <returns>If the method succeeds, the return value is S_OK. If the method fails, the return value can be one of the following: D3DERR_INVALIDCALL, D3DXERR_INVALIDDATA. </returns>
        /// <unmanaged>HRESULT ID3DX10Sprite::DrawSpritesImmediate([In, Buffer] D3DX10_SPRITE* pSprites,[None] int cSprites,[None] int cbSprite,[None] int flags)</unmanaged>
        public void DrawSpritesImmediate(SharpDX.Direct3D10.SpriteInstance[] sprites)
        {
            DrawSpritesImmediate(sprites, sprites.Length, Utilities.SizeOf<SpriteInstance>(), 0);
        }
    }
}