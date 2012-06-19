// Copyright (c) 2010-2012 SharpDX - Alexandre Mutel
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
using SharpDX.Direct3D11;

namespace SharpDX.Toolkit.Graphics
{
    public class RenderTarget2D : Texture2D
    {
        protected RenderTarget2D(Texture2DDescription description) : this(GraphicsDevice.Current, description)
        {
        }

        protected RenderTarget2D(GraphicsDevice device, Texture2DDescription description) : base(device, description)
        {
        }
        
        public new RenderTarget2D Clone()
        {
            return new RenderTarget2D(GraphicsDevice, Description);
        }

        public static new RenderTarget2D New(Texture2DDescription description)
        {
            return new RenderTarget2D(description);
        }

        public static new RenderTarget2D New(int width, int height, PixelFormat format, bool isReadWrite = false, int mipCount = 1)
        {
            return new RenderTarget2D(NewDescription(width, height, format, isReadWrite, mipCount));
        }

        protected static new Texture2DDescription NewDescription(int width, int height, PixelFormat format, bool isReadWrite = false, int mipCount = 1)
        {
            var desc = Texture2D.NewDescription(width, height, format, isReadWrite, mipCount);
            desc.BindFlags |= BindFlags.RenderTarget;
            return desc;
        }
    }
}