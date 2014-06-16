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

namespace SharpDX.Direct3D9
{
    /// <summary>
    /// Helper methods to create special <see cref="VertexFormat"/>.
    /// </summary>
    public class VertexFormatHelper
    {
        /// <summary>
        /// Calculates a specific TEXCOORDSIZEN <see cref="VertexFormat"/>.
        /// </summary>
        /// <param name="size">The size of the texcoord. The value must be in the range [1,4] (Number of floating point values)</param>
        /// <param name="coordIndex">Index of the coord.</param>
        /// <returns>The <see cref="VertexFormat"/></returns>
        /// <exception cref="System.ArgumentException">If size is not in the range [1,4]</exception>
        /// <remarks>
        /// </remarks>
        public static VertexFormat TexCoordSize(int size, int coordIndex)
        {
            //// Macros to set texture coordinate format bits in the FVF id
            //#define D3DFVF_TEXTUREFORMAT2 0         // Two floating point values
            //#define D3DFVF_TEXTUREFORMAT1 3         // One floating point value
            //#define D3DFVF_TEXTUREFORMAT3 1         // Three floating point values
            //#define D3DFVF_TEXTUREFORMAT4 2         // Four floating point values
            //#define D3DFVF_TEXCOORDSIZE3(CoordIndex) (D3DFVF_TEXTUREFORMAT3 << (CoordIndex*2 + 16))
            //#define D3DFVF_TEXCOORDSIZE2(CoordIndex) (D3DFVF_TEXTUREFORMAT2)
            //#define D3DFVF_TEXCOORDSIZE4(CoordIndex) (D3DFVF_TEXTUREFORMAT4 << (CoordIndex*2 + 16))
            //#define D3DFVF_TEXCOORDSIZE1(CoordIndex) (D3DFVF_TEXTUREFORMAT1 << (CoordIndex*2 + 16))
            int textureFormat;
            switch (size)
            {
                case 1:
                    textureFormat = 3;
                    break;
                case 2:
                    textureFormat = 0;
                    break;
                case 3:
                    textureFormat = 1;
                    break;
                case 4:
                    textureFormat = 2;
                    break;
                default:
                    throw new ArgumentException("Size must be in the range [1,4]", "size");
            }

            return (VertexFormat)(textureFormat == 0 ? textureFormat : (textureFormat << (coordIndex * 2 + 16)));
        }
    }
}