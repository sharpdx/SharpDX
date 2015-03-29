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

namespace SharpDX.Direct2D1.Effects
{
    /// <summary>
    /// Built in BitmapSource effect.
    /// </summary>
    public class BitmapSource : Effect
    {
        private WIC.BitmapSource wicBitmapSource;

        /// <summary>
        /// Initializes a new instance of <see cref="BitmapSource"/> effect.
        /// </summary>
        /// <param name="context"></param>
        public BitmapSource(DeviceContext context) : base(context, Effect.BitmapSource)
        {
        }

        /// <summary>
        /// The <see cref="WIC.BitmapSource"/> containing the image data to be loaded.
        /// </summary>
        public WIC.BitmapSource WicBitmapSource
        {
            get
            {
                if (wicBitmapSource == null)
                    wicBitmapSource = GetComObjectValue<WIC.BitmapSource>((int)BitmapSourceProperties.WicBitmapSource);
                return wicBitmapSource;
            }
            set
            {
                wicBitmapSource = value;
                SetValue((int)BitmapSourceProperties.WicBitmapSource, wicBitmapSource);
            }
        }

        /// <summary>
        /// The scale amount in the X and Y direction. 
        /// The effect multiplies the width by the X value and the height by the Y value. 
        /// This property is a <see cref="Vector2"/> defined as: (X scale, Y scale). The scale amounts are FLOAT, unitless, and must be positive or 0.
        /// </summary>
        public RawVector2 ScaleSource
        {
            get
            {
                return GetVector2Value((int)BitmapSourceProperties.Scale);
            }
            set
            {
                SetValue((int)BitmapSourceProperties.Scale, value);
            }
        }

        /// <summary>
        /// The interpolation mode used to scale the image. See Interpolation modes for more info.
        /// If the mode disables the mipmap, then BitmapSouce will cache the image at the resolution determined by the Scale and EnableDPICorrection properties. 
        /// </summary>
        public InterpolationMode InterpolationMode
        {
            get
            {
                return GetEnumValue<InterpolationMode>((int)BitmapSourceProperties.InterpolationMode);
            }
            set
            {
                SetEnumValue((int)BitmapSourceProperties.InterpolationMode, value);
            }
        }

        /// <summary>
        /// If you set this to true, the effect will scale the input image to convert the DPI reported by IWICBitmapSource to the DPI of the device context. 
        /// The effect uses the interpolation mode you set with the InterpolationMode property. 
        /// If you set this to false, the effect uses a DPI of 96.0 for the output image.
        /// </summary>
        public bool EnableDpiCorrection
        {
            get
            {
                return GetBoolValue((int)BitmapSourceProperties.EnableDpiCorrection);
            }
            set
            {
                SetValue((int)BitmapSourceProperties.EnableDpiCorrection, value);
            }
        }

        /// <summary>
        /// The alpha mode of the output. This can be either premultiplied or straight. See Alpha modes for more info.
        /// </summary>
        public AlphaMode AlphaMode
        {
            get
            {
                return GetEnumValue<AlphaMode>((int)BitmapSourceProperties.AlphaMode);
            }
            set
            {
                SetEnumValue((int)BitmapSourceProperties.AlphaMode, value);
            }
        }

        /// <summary>
        /// A flip and/or rotation operation to be performed on the image. See Orientation for more info.
        /// </summary>
        public BitmapSourceOrientation Orientation
        {
            get
            {
                return GetEnumValue<BitmapSourceOrientation>((int)BitmapSourceProperties.Orientation);
            }
            set
            {
                SetEnumValue((int)BitmapSourceProperties.Orientation, value);
            }
        }
    }
}