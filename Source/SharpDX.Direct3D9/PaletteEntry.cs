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
using System.Runtime.InteropServices;

namespace SharpDX.Direct3D9  {

    /// <summary>The PaletteEntry struct contains the color and usage of an entry in a logical palette. </summary>
    /// <unmanaged>PALETTEENTRY</unmanaged>
    [StructLayout(LayoutKind.Sequential, Pack = 0)]
    public struct PaletteEntry
    {
        /// <summary>
        /// The red intensity value for the palette entry.
        /// </summary>
        public byte Red;

        /// <summary>
        /// The green intensity value for the palette entry.
        /// </summary>
        public byte Green;

        /// <summary>
        /// The blue intensity value for the palette entry.
        /// </summary>
        public byte Blue;

        /// <summary>
        /// Indicates how the palette entry is to be used. 
        /// TODO define an enum for flags
        /// </summary>
        public byte Flags;
    }
}