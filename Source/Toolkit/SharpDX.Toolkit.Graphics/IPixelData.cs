// Copyright (c) 2010-2012 SharpDX - Alexandre Mutel
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

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// Interface used to write to an arbitrary pixel data structure.
    /// </summary>
    public interface IPixelData
    {
        /// <summary>
        /// Gets the associated <see cref="PixelFormat"/>.
        /// </summary>
        PixelFormat Format { get; }

        /// <summary>
        /// Writes LDR pixel.
        /// </summary>
        /// <param name="red"The red component></param>
        /// <param name="green">The green component</param>
        /// <param name="blue">The blue component</param>
        /// <param name="alpha">The alpha component</param>
        void Write(byte red, byte green, byte blue, byte alpha);

        /// <summary>
        /// Writes LDR alpha pixel.
        /// </summary>
        /// <param name="alpha">The alpha component.</param>
        void Write(byte alpha);


        /// <summary>
        /// Writes HDR pixel
        /// </summary>
        /// <param name="color">RGBA color as <see cref="Color4"/></param>
        void Write(Color4 color);

        /// <summary>
        /// Writes HDR alpha pixel
        /// </summary>
        /// <param name="alpha">The alpha component</param>
        void Write(float alpha);

        /// <summary>
        /// Gets the red component.
        /// </summary>
        float Red { get; }

        /// <summary>
        /// Gets the green component.
        /// </summary>
        float Green { get; }

        /// <summary>
        /// Gets the blue component.
        /// </summary>
        float Blue { get; }

        /// <summary>
        /// Gets the alpha component.
        /// </summary>
        float Alpha { get; }
    }
}