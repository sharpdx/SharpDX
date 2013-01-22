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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonDX
{
    public class TextureLoader
    {
        /// <summary>
        /// Loads a bitmap using WIC.
        /// </summary>
        /// <param name="deviceManager"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static SharpDX.WIC.BitmapSource LoadBitmap(SharpDX.WIC.ImagingFactory2 factory, string filename)
        {
            var bitmapDecoder = new SharpDX.WIC.BitmapDecoder(
                factory,
                filename,
                SharpDX.WIC.DecodeOptions.CacheOnDemand
                );

            var formatConverter = new SharpDX.WIC.FormatConverter(factory);

            formatConverter.Initialize(
                bitmapDecoder.GetFrame(0),
                SharpDX.WIC.PixelFormat.Format32bppPRGBA,
                SharpDX.WIC.BitmapDitherType.None, 
                null,
                0.0, 
                SharpDX.WIC.BitmapPaletteType.Custom);

            return formatConverter;
        }

        /// <summary>
        /// Creates a <see cref="SharpDX.Direct3D11.Texture2D"/> from a WIC <see cref="SharpDX.WIC.BitmapSource"/>
        /// </summary>
        /// <param name="device">The Direct3D11 device</param>
        /// <param name="bitmapSource">The WIC bitmap source</param>
        /// <returns>A Texture2D</returns>
        public static SharpDX.Direct3D11.Texture2D CreateTexture2DFromBitmap(SharpDX.Direct3D11.Device device, SharpDX.WIC.BitmapSource bitmapSource)
        {
            // Allocate DataStream to receive the WIC image pixels
            int stride = bitmapSource.Size.Width * 4;
            using (var buffer = new SharpDX.DataStream(bitmapSource.Size.Height * stride, true, true))
            {
                // Copy the content of the WIC to the buffer
                bitmapSource.CopyPixels(stride, buffer);
                return new SharpDX.Direct3D11.Texture2D(device, new SharpDX.Direct3D11.Texture2DDescription()
                {
                    Width = bitmapSource.Size.Width,
                    Height = bitmapSource.Size.Height,
                    ArraySize = 1,
                    BindFlags = SharpDX.Direct3D11.BindFlags.ShaderResource,
                    Usage = SharpDX.Direct3D11.ResourceUsage.Immutable,
                    CpuAccessFlags = SharpDX.Direct3D11.CpuAccessFlags.None,
                    Format = SharpDX.DXGI.Format.R8G8B8A8_UNorm,
                    MipLevels = 1,
                    OptionFlags = SharpDX.Direct3D11.ResourceOptionFlags.None,
                    SampleDescription = new SharpDX.DXGI.SampleDescription(1, 0),
                }, new SharpDX.DataRectangle(buffer.DataPointer, stride));
            }
        }
    }
}
