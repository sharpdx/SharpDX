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

using SharpDX.Mathematics.Interop;

namespace SharpDX.Direct2D1.Effects
{
    /// <summary>
    /// Built in Tile effect.
    /// </summary>
    public class Tile : Effect
    {
        /// <summary>
        /// Initializes a new instance of <see cref="Tile"/> effect.
        /// </summary>
        /// <param name="context"></param>
        public Tile(DeviceContext context) : base(context, Effect.Tile)
        {
        }

        /// <summary>
        /// The region to be tiled specified as a vector in the form (left, top, width, height). The units are in DIPs.
        /// </summary>
        public RawVector4 Rectangle
        {
            get
            {
                return GetVector4Value((int)TileProperties.Rectangle);
            }
            set
            {
                SetValue((int)TileProperties.Rectangle, value);
            }
        }
    }
}