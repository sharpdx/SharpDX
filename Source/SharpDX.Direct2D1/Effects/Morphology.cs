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
using SharpDX.WIC;

namespace SharpDX.Direct2D1.Effects
{
    /// <summary>
    /// Built in Morphology effect.
    /// </summary>
    public class Morphology : Effect
    {
        /// <summary>
        /// Initializes a new instance of <see cref="MorphologyEffect"/> effect.
        /// </summary>
        /// <param name="context"></param>
        public Morphology(DeviceContext context)
            : base(context, Effect.Morphology)
        {
        }

        /// <summary>
        /// The morphology mode. The available modes are erode (flatten) and dilate (thicken).
        /// See Morphology modes for more info.
        /// </summary>
        public MorphologyMode Mode
        {
            get
            {
                return GetEnumValue<MorphologyMode>((int)MorphologyProperties.Mode);
            }
            set
            {
                SetEnumValue((int)MorphologyProperties.Mode, value);
            }
        }

        /// <summary>
        /// Size of the kernel in the X direction. The units are in DIPs.
        /// </summary>
        public int Width
        {
            get
            {
                return (int)GetUIntValue((int)MorphologyProperties.Width);
            }
            set
            {
                SetValue((int)MorphologyProperties.Width, (uint)value);
            }
        }

        /// <summary>
        /// Size of the kernel in the Y direction. The units are in DIPs.
        /// </summary>
        public int Height
        {
            get
            {
                return (int)GetUIntValue((int)MorphologyProperties.Height);
            }
            set
            {
                SetValue((int)MorphologyProperties.Height, (uint)value);
            }
        }
    }
}