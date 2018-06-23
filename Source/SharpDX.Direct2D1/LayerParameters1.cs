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
using SharpDX.Mathematics.Interop;

namespace SharpDX.Direct2D1
{
    public partial struct LayerParameters1
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LayerParameters1"/> struct.
        /// </summary>
        /// <param name="contentBounds">The content bounds.</param>
        /// <param name="geometryMask">The geometry mask.</param>
        /// <param name="maskAntialiasMode">The mask antialias mode.</param>
        /// <param name="maskTransform">The mask transform.</param>
        /// <param name="opacity">The opacity.</param>
        /// <param name="opacityBrush">The opacity brush.</param>
        /// <param name="layerOptions">The layer options.</param>
        public LayerParameters1(RawRectangleF contentBounds, Geometry geometryMask, AntialiasMode maskAntialiasMode, RawMatrix3x2 maskTransform, float opacity, Brush opacityBrush, LayerOptions1 layerOptions)
            : this()
        {
            ContentBounds = contentBounds;
            GeometricMask = geometryMask;
            MaskAntialiasMode = maskAntialiasMode;
            MaskTransform = maskTransform;
            Opacity = opacity;
            OpacityBrush = opacityBrush;
            LayerOptions = layerOptions;
        }
    }
}