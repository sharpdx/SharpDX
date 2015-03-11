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
using SharpDX.Mathematics.Interop;

namespace SharpDX.Direct3D9
{
    public partial class Sprite
    {
        /// <summary>	
        /// Creates a sprite object which is associated with a particular device. Sprite objects are used to draw 2D images to the screen.	
        /// </summary>	
        /// <param name="device">A reference to the device (see <see cref="SharpDX.Direct3D9.Device"/>) that will draw the sprite. </param>
        /// <remarks>	
        /// This interface can be used to draw two dimensional images in screen space of the associated device.	
        /// </remarks>	
        /// <unmanaged>HRESULT D3DXCreateSprite([In] IDirect3DDevice9* pDevice,[Out, Fast] ID3DXSprite** ppSprite)</unmanaged>	
        public Sprite(Device device)
        {
            D3DX9.CreateSprite(device, this);
        }

        /// <summary>	
        /// <p>Adds a sprite to the list of batched sprites.</p>	
        /// </summary>	
        /// <param name="textureRef"><dd>  <p>Pointer to an <strong><see cref="SharpDX.Direct3D9.Texture"/></strong> interface that represents the sprite texture.</p> </dd></param>	
        /// <param name="color"><dd>  <p> <strong><see cref="RawColor4"/></strong> type. The color and alpha channels are modulated by this value. A value of 0xFFFFFFFF maintains the original source color and alpha data. Use the <strong>D3DCOLOR_RGBA</strong> macro to help generate this color.</p> </dd></param>	
        /// <returns><p>If the method succeeds, the return value is <see cref="SharpDX.Result.Ok"/>. If the method fails, the return value can be one of the following: <see cref="SharpDX.Direct3D9.ResultCode.InvalidCall"/>, D3DXERR_INVALIDDATA.</p></returns>	
        /// <remarks>	
        /// <p>To scale, rotate, or translate a sprite, call <strong><see cref="SharpDX.Direct3D9.Sprite.SetTransform"/></strong> with a matrix that contains the scale, rotate, and translate (SRT) values, before calling <see cref="SharpDX.Direct3D9.Sprite.Draw"/>. For information about setting SRT values in a matrix, see Matrix Transforms.</p>	
        /// </remarks>	
        /// <msdn-id>bb174251</msdn-id>	
        /// <unmanaged>HRESULT ID3DXSprite::Draw([In] IDirect3DTexture9* pTexture,[In] const RECT* pSrcRect,[In] const D3DXVECTOR3* pCenter,[In] const D3DXVECTOR3* pPosition,[In] D3DCOLOR Color)</unmanaged>	
        /// <unmanaged-short>ID3DXSprite::Draw</unmanaged-short>	
        public void Draw(SharpDX.Direct3D9.Texture textureRef, RawColorBGRA color)
        {
            Draw(textureRef, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, color);
        }

        /// <summary>	
        /// <p>Adds a sprite to the list of batched sprites.</p>	
        /// </summary>	
        /// <param name="textureRef"><dd>  <p>Pointer to an <strong><see cref="SharpDX.Direct3D9.Texture"/></strong> interface that represents the sprite texture.</p> </dd></param>	
        /// <param name="srcRectRef"><dd>  <p>Pointer to a <strong><see cref="RawRectangle"/></strong> structure that indicates the portion of the source texture to use for the sprite. If this parameter is <strong><c>null</c></strong>, then the entire source image is used for the sprite.</p> </dd></param>	
        /// <param name="centerRef"><dd>  <p>Pointer to a <strong><see cref="RawVector3"/></strong> vector that identifies the center of the sprite. If this argument is <strong><c>null</c></strong>, the point (0,0,0) is used, which is the upper-left corner.</p> </dd></param>	
        /// <param name="positionRef"><dd>  <p>Pointer to a <strong><see cref="RawVector3"/></strong> vector that identifies the position of the sprite. If this argument is <strong><c>null</c></strong>, the point (0,0,0) is used, which is the upper-left corner.</p> </dd></param>	
        /// <param name="color"><dd>  <p> <strong><see cref="RawColor4"/></strong> type. The color and alpha channels are modulated by this value. A value of 0xFFFFFFFF maintains the original source color and alpha data. Use the <strong>D3DCOLOR_RGBA</strong> macro to help generate this color.</p> </dd></param>	
        /// <returns><p>If the method succeeds, the return value is <see cref="SharpDX.Result.Ok"/>. If the method fails, the return value can be one of the following: <see cref="SharpDX.Direct3D9.ResultCode.InvalidCall"/>, D3DXERR_INVALIDDATA.</p></returns>	
        /// <remarks>	
        /// <p>To scale, rotate, or translate a sprite, call <strong><see cref="SharpDX.Direct3D9.Sprite.SetTransform"/></strong> with a matrix that contains the scale, rotate, and translate (SRT) values, before calling <see cref="SharpDX.Direct3D9.Sprite.Draw"/>. For information about setting SRT values in a matrix, see Matrix Transforms.</p>	
        /// </remarks>	
        /// <msdn-id>bb174251</msdn-id>	
        /// <unmanaged>HRESULT ID3DXSprite::Draw([In] IDirect3DTexture9* pTexture,[In] const RECT* pSrcRect,[In] const D3DXVECTOR3* pCenter,[In] const D3DXVECTOR3* pPosition,[In] D3DCOLOR Color)</unmanaged>	
        /// <unmanaged-short>ID3DXSprite::Draw</unmanaged-short>	
        public unsafe void Draw(SharpDX.Direct3D9.Texture textureRef, RawColorBGRA color, RawRectangle? srcRectRef = null, RawVector3? centerRef = null, RawVector3? positionRef = null)
        {
            RawRectangle localRect = default(RawRectangle);
            RawVector3 localCenter;
            RawVector3 localPosition;
            if (srcRectRef.HasValue)
                localRect = srcRectRef.Value;
            if (centerRef.HasValue)
                localCenter = centerRef.Value;
            if (positionRef.HasValue)
                localPosition = positionRef.Value;
            Draw(textureRef, srcRectRef.HasValue ? (IntPtr)(void*)&localRect : IntPtr.Zero, centerRef.HasValue ? (IntPtr)(void*)&localCenter : IntPtr.Zero, positionRef.HasValue ? (IntPtr)(void*)&localPosition : IntPtr.Zero, color);
        }
    }
}