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
    /// Built in Crop effect.
    /// </summary>
    public class Crop : Effect
    {
        /// <summary>
        /// Initializes a new instance of <see cref="Crop"/> effect.
        /// </summary>
        /// <param name="context"></param>
        public Crop(DeviceContext context) : base(context, Effect.Crop)
        {
        }

        /// <summary>
        /// The region to be cropped specified as a vector in the form (left, top, width, height). The units are in DIPs.
        /// </summary>
        /// <remarks>
        /// The rectangle will be truncated if it overlaps the edge boundaries of the input image.
        /// </remarks>
        public Vector4 Rectangle
        {
            get
            {
                return GetVector4Value((int)CropProperties.Rectangle);
            }
            set
            {
                SetValue((int)CropProperties.Rectangle, value);
            }
        }
    }
}
#endif