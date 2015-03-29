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
    /// Built in DpiCompensation effect.
    /// </summary>
    public class DpiCompensation : Effect
    {
        /// <summary>
        /// Initializes a new instance of <see cref="DpiCompensation"/> effect.
        /// </summary>
        /// <param name="context"></param>
        public DpiCompensation(DeviceContext context) : base(context, Effect.DpiCompensation)
        {
        }

        /// <summary>
        /// The Dpi interpolation mode.
        /// </summary>
        public DpiCompensationInterpolationMode InterpolationMode
        {
            get
            {
                return GetEnumValue<DpiCompensationInterpolationMode>((int)DpiCompensationProperties.InterpolationMode);
            }
            set
            {
                SetEnumValue((int)DpiCompensationProperties.InterpolationMode, value);
            }
        }

        /// <summary>
        /// The mode used to calculate the border of the image, soft or hard. See <see cref="BorderMode"/> modes for more info.
        /// </summary>
        public BorderMode BorderMode
        {
            get
            {
                return GetEnumValue<BorderMode>((int)DpiCompensationProperties.BorderMode);
            }
            set
            {
                SetEnumValue((int)DpiCompensationProperties.BorderMode, value);
            }
        }

        /// <summary>
        /// The input dpi.
        /// </summary>
        public float InputDpi
        {
            get
            {
                return GetFloatValue((int)DpiCompensationProperties.InputDpi);
            }
            set
            {
                SetValue((int)DpiCompensationProperties.InputDpi, value);
            }
        }
    }
}