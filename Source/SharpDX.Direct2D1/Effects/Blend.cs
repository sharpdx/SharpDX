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
    /// Built in Blend effect.
    /// </summary>
    public class Blend : Effect
    {
        /// <summary>
        /// Initializes a new instance of <see cref="Blend"/> effect.
        /// </summary>
        /// <param name="context"></param>
        public Blend(DeviceContext context) : base(context, Effect.Blend)
        {
        }

        /// <summary>
        /// The blend mode used for the effect. See Blend modes for more info.
        /// </summary>
        public BlendMode Mode
        {
            get
            {
                return GetEnumValue<BlendMode>((int)BlendProperties.Mode);
            }
            set
            {
                SetEnumValue((int)BlendProperties.Mode, value);
            }
        }
    }
}
#endif