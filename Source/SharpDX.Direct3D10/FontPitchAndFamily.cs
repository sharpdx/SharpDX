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
    /// Defines pitch and family settings for fonts.
    /// </summary>
    [Flags]
    public enum FontPitchAndFamily : byte
    {
        /// <summary>
        /// Use the Decorative family.
        /// </summary>
        Decorative = 80,
        /// <summary>
        /// Default pitch.
        /// </summary>
        Default = 0,
        /// <summary>
        /// The font family doesn't matter.
        /// </summary>
        DontCare = 0,
        /// <summary>
        /// Fixed pitch.
        /// </summary>
        Fixed = 1,
        /// <summary>
        /// Use the Modern family.
        /// </summary>
        Modern = 0x30,
        /// <summary>
        /// Mono pitch.
        /// </summary>
        Mono = 8,
        /// <summary>
        /// Use the Roman family.
        /// </summary>
        Roman = 0x10,
        /// <summary>
        /// Use the Script family.
        /// </summary>
        Script = 0x40,
        /// <summary>
        /// Use the Swiss family.
        /// </summary>
        Swiss = 0x20,
        /// <summary>
        /// Variable pitch.
        /// </summary>
        Variable = 2
    }
}