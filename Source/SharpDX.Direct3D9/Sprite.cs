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
    }
}