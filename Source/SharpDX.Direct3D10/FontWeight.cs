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

namespace SharpDX.Direct3D10
{
    /// <summary>
    /// Specifies weights for font rendering.
    /// </summary>
    /// <unmanaged>FW</unmanaged>
    public enum FontWeight
    {
        /// <summary>
        /// Use a black weight.
        /// </summary>
        Black = 900,
        /// <summary>
        /// Use a bold weight.
        /// </summary>
        Bold = 700,
        /// <summary>
        /// Use a demi-bold weight.
        /// </summary>
        DemiBold = 600,
        /// <summary>
        /// The font weight doesn't matter.
        /// </summary>
        DoNotCare = 0,
        /// <summary>
        /// Use an extra bold weight.
        /// </summary>
        ExtraBold = 800,
        /// <summary>
        /// Make the font extra light.
        /// </summary>
        ExtraLight = 200,
        /// <summary>
        /// Use a heavy weight.
        /// </summary>
        Heavy = 900,
        /// <summary>
        /// Make the font light.
        /// </summary>
        Light = 300,
        /// <summary>
        /// Use a medium weight.
        /// </summary>
        Medium = 500,
        /// <summary>
        /// Use a normal weight.
        /// </summary>
        Normal = 400,
        /// <summary>
        /// Use a regular weight.
        /// </summary>
        Regular = 400,
        /// <summary>
        /// Use a semi-bold weight.
        /// </summary>
        SemiBold = 600,
        /// <summary>
        /// Make the font thin.
        /// </summary>
        Thin = 100,
        /// <summary>
        /// Use an ultra bold weight.
        /// </summary>
        UltraBold = 800,
        /// <summary>
        /// Make the font ultra light.
        /// </summary>
        UltraLight = 200
    }
}