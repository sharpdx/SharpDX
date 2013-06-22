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

namespace SharpDX.Direct2D1
{
    public partial class BorderTransform
    {
        /// <summary>
        /// Initializes a new instance of <see cref="BorderTransform"/> class
        /// </summary>
        /// <param name="context">The effect context</param>
        /// <param name="extendModeX">The extend mode for X coordinates</param>
        /// <param name="extendModeY">The extend mode for Y coordinates</param>
        /// <unmanaged>HRESULT ID2D1EffectContext::CreateBorderTransform([In] D2D1_EXTEND_MODE extendModeX,[In] D2D1_EXTEND_MODE extendModeY,[Out, Fast] ID2D1BorderTransform** transform)</unmanaged>	
        public BorderTransform(EffectContext context, SharpDX.Direct2D1.ExtendMode extendModeX, SharpDX.Direct2D1.ExtendMode extendModeY) : base(IntPtr.Zero)
        {
            context.CreateBorderTransform(extendModeX, ExtendModeX, this);
        }
    }
}
#endif