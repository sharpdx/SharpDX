// Copyright (c) 2010 SharpDX - Alexandre Mutel
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

using System.Globalization;
using SharpDX.DXGI;

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// Describes the display mode. This is equivalent to <see cref="ModeDescription"/>
    /// </summary>
    /// <msdn-id>bb173064</msdn-id>	
    /// <unmanaged>DXGI_MODE_DESC</unmanaged>	
    /// <unmanaged-short>DXGI_MODE_DESC</unmanaged-short>	
    public class DisplayMode
    {
        private DXGI.Format pixelFormat;

        private int width;

        private int height;

        private Rational refreshRate;

        public DisplayMode(Format pixelFormat, int width, int height, Rational refreshRate)
        {
            this.pixelFormat = pixelFormat;
            this.width = width;
            this.height = height;
            this.refreshRate = refreshRate;
        }

        /// <summary>
        /// Gets the aspect ratio used by the graphics device.
        /// </summary>
        public float AspectRatio
        {
            get
            {
                if ((Height != 0) && (Width != 0))
                {
                    return ((float)Width) / Height;
                }
                return 0f;
            }
        }

        /// <summary>
        /// Gets a value indicating the surface format of the display mode.
        /// </summary>
        /// <msdn-id>bb173064</msdn-id>	
        /// <unmanaged>DXGI_FORMAT Format</unmanaged>	
        /// <unmanaged-short>DXGI_FORMAT Format</unmanaged-short>	
        public PixelFormat Format
        {
            get
            {
                return (PixelFormat)pixelFormat;   
            }            
        }

        /// <summary>
        /// Gets a value indicating the screen width, in pixels.
        /// </summary>
        /// <msdn-id>bb173064</msdn-id>	
        /// <unmanaged>unsigned int Width</unmanaged>	
        /// <unmanaged-short>unsigned int Width</unmanaged-short>	
        public int Width
        {
            get
            {
                return width;
            }
        }

        /// <summary>
        /// Gets a value indicating the screen height, in pixels.
        /// </summary>
        /// <msdn-id>bb173064</msdn-id>	
        /// <unmanaged>unsigned int Height</unmanaged>	
        /// <unmanaged-short>unsigned int Height</unmanaged-short>	
        public int Height
        {
            get
            {
                return height;
            }
        }

        /// <summary>
        /// Gets a value indicating the refresh rate
        /// </summary>
        /// <msdn-id>bb173064</msdn-id>	
        /// <unmanaged>DXGI_RATIONAL RefreshRate</unmanaged>	
        /// <unmanaged-short>DXGI_RATIONAL RefreshRate</unmanaged-short>	
        public Rational RefreshRate
        {
            get
            {
                return refreshRate;
            }
        }

        /// <summary>
        /// Retrieves a string representation of this object.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, "Width:{0} Height:{1} Format:{2} AspectRatio:{3} RefreshRate:{4}", new object[] { Width, Height, Format, AspectRatio, (float)RefreshRate.Numerator / RefreshRate.Denominator });
        }

        public ModeDescription ToDescription()
        {
            return new ModeDescription(Width, Height, RefreshRate, Format);
        }

        public static DisplayMode FromDescription(ModeDescription description)
        {
            return new DisplayMode(description.Format, description.Width, description.Height, description.RefreshRate);
        }
    }
}
