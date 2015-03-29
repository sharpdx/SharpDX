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
using System.Collections.Generic;
using System.Text;

namespace SharpDX.Direct2D1.Effects
{
    /// <summary>
    /// Built in DisplacementMap effect.
    /// </summary>
    public class DisplacementMap : Effect
    {
        /// <summary>
        /// Initializes a new instance of <see cref="DisplacementMap"/> effect.
        /// </summary>
        /// <param name="context"></param>
        public DisplacementMap(DeviceContext context) : base(context, Effect.DisplacementMap)
        {
        }

        /// <summary>
        /// Multiplies the intensity of the selected channel from the displacement image. The higher you set this property, the more the effect displaces the pixels
        /// </summary>
        public float Scale
        {
            get
            {
                return GetFloatValue((int)DisplacementMapProperties.Scale);
            }
            set
            {
                SetValue((int)DisplacementMapProperties.Scale, value);
            }
        }

        /// <summary>
        /// The effect extracts the intensity from this color channel and uses it to spatially displace the image in the X direction. See Color channels for more info.
        /// </summary>
        public ChannelSelector XChannelSelect
        {
            get
            {
                return GetEnumValue<ChannelSelector>((int)DisplacementMapProperties.XChannelSelect);
            }
            set
            {
                SetEnumValue((int)DisplacementMapProperties.XChannelSelect, value);
            }
        }

        /// <summary>
        /// The effect extracts the intensity from this color channel and uses it to spatially displace the image in the Y direction. See Color channels for more info.
        /// </summary>
        public ChannelSelector YChannelSelect
        {
            get
            {
                return GetEnumValue<ChannelSelector>((int)DisplacementMapProperties.YChannelSelect);
            }
            set
            {
                SetEnumValue((int)DisplacementMapProperties.YChannelSelect, value);
            }
        }
    }
}