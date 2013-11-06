// Copyright (c) 2010-2011 SharpDX - Alexandre Mutel
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

using System.Runtime.InteropServices;

namespace SharpDX.Win32
{
    /// <summary>The bitmap information header struct.</summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct BitmapInfoHeader
    {
        /// <summary>The size information bytes.</summary>
        public int SizeInBytes;
        /// <summary>The width.</summary>
        public int Width;
        /// <summary>The height.</summary>
        public int Height;
        /// <summary>The plane count.</summary>
        public short PlaneCount;
        /// <summary>The bit count.</summary>
        public short BitCount;
        /// <summary>The compression.</summary>
        public int Compression;
        /// <summary>The size image.</summary>
        public int SizeImage;
        /// <summary>The executable pixels per meter.</summary>
        public int XPixelsPerMeter;
        /// <summary>The asynchronous pixels per meter.</summary>
        public int YPixelsPerMeter;
        /// <summary>The color used count.</summary>
        public int ColorUsedCount;
        /// <summary>The color important count.</summary>
        public int ColorImportantCount;
    }
}