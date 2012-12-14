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
namespace SharpDX.Toolkit
{ 
    /// <summary>
    /// Provides high-level geometry, logic and mathematical helper functions.
    /// </summary>
    public static class HlUtil
    { 
        /// <summary>
        /// Calculates a world space <see cref="SharpDX.Ray"/> from 2d screen coordinates.
        /// </summary>
        /// <param name="x">X coordinate on 2d screen.</param>
        /// <param name="y">Y coordinate on 2d screen.</param>
        /// <param name="viewport"><see cref="SharpDX.Direct3D11.Viewport"/>.</param>
        /// <param name="worldViewProjection">Transformation <see cref="SharpDX.Matrix"/>.</param>
        /// <returns>Resulting <see cref="SharpDX.Ray"/>.</returns>
        public static Ray GetPickRay(int x, int y, Viewport viewport, Matrix worldViewProjection)
        {
            var nearPoint = new Vector3(x, y, 0);
            var farPoint = new Vector3(x, y, 1); 
          
            nearPoint = Vector3.Unproject(nearPoint, viewport.TopLeftX, viewport.TopLeftY, viewport.Width, viewport.Height, viewport.MinDepth,
                                        viewport.MaxDepth, worldViewProjection);
            farPoint = Vector3.Unproject(farPoint, viewport.TopLeftX, viewport.TopLeftY, viewport.Width, viewport.Height, viewport.MinDepth,
                                        viewport.MaxDepth, worldViewProjection); 

            Vector3 direction = farPoint - nearPoint; 
            direction.Normalize();

            return new Ray(nearPoint, direction);   
        }
    }
}
