﻿// Copyright (c) 2010-2013 SharpDX - Alexandre Mutel
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
#if DIRECTX11_1
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpDX.Direct2D1.Effects
{
    /// <summary>
    /// Built in Flood effect.
    /// </summary>
    public class Flood : Effect
    {
        /// <summary>
        /// Initializes a new instance of <see cref="Flood"/> effect.
        /// </summary>
        /// <param name="context"></param>
        public Flood(DeviceContext context) : base(context, Effect.Flood)
        {
        }

        /// <summary>
        /// The color and opacity of the bitmap. This property is a <see cref="Color4"/>. 
        /// The individual values for each channel are of type FLOAT, unbounded and unitless. 
        /// The effect doesn't modify the values for the channels.
        /// The RGBA values for each channel range from 0 to 1.
        /// </summary>
        public Color4 Color
        {
            get
            {
                return GetColor4Value((int)FloodProperties.Color);
            }
            set
            {
                SetValue((int)FloodProperties.Color, value);
            }
        }
    }
}
#endif