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

namespace SharpDX.Toolkit.Graphics
{
    public partial class GeometricPrimitive
    {
        /// <summary>
        /// A plane primitive.
        /// </summary>
        public struct Plane
        {
            /// <summary>
            /// Creates a Plane primitive on the X/Y plane with a normal equal to -<see cref="Vector3.UnitZ"/>.
            /// </summary>
            /// <param name="device">The device.</param>
            /// <param name="sizeX">The size X.</param>
            /// <param name="sizeY">The size Y.</param>
            /// <param name="tessellation">The tessellation, as the number of quads per axis.</param>
            /// <param name="toLeftHanded">if set to <c>true</c> vertices and indices will be transformed to left handed. Default is true.</param>
            /// <returns>A Plane primitive.</returns>
            /// <exception cref="System.ArgumentOutOfRangeException">tessellation;tessellation must be > 0</exception>
            public static GeometricPrimitive New(GraphicsDevice device, float sizeX = 1.0f, float sizeY = 1.0f, int tessellation = 1, bool toLeftHanded = false)
            {
                return New(device, sizeX, sizeY, tessellation, Vector2.One, toLeftHanded);


            }

            /// <summary>
            /// Creates a Plane primitive on the X/Y plane with a normal equal to -<see cref="Vector3.UnitZ" />.
            /// </summary>
            /// <param name="device">The device.</param>
            /// <param name="sizeX">The size X.</param>
            /// <param name="sizeY">The size Y.</param>
            /// <param name="tessellation">The tessellation, as the number of quads per axis.</param>
            /// <param name="uvFactor">The uv factor.</param>
            /// <param name="toLeftHanded">if set to <c>true</c> vertices and indices will be transformed to left handed. Default is true.</param>
            /// <returns>A Plane primitive.</returns>
            /// <exception cref="System.ArgumentOutOfRangeException">tessellation;tessellation must be &gt; 0</exception>
            public static GeometricPrimitive New(GraphicsDevice device, float sizeX, float sizeY, int tessellation, Vector2 uvFactor, bool toLeftHanded = false)
            {
                if (tessellation < 1)
                {
                    throw new ArgumentOutOfRangeException("tessellation", "tessellation must be > 0");
                }

                var lineWidth = tessellation + 1;
                var vertices = new VertexPositionNormalTexture[lineWidth * lineWidth];
                var indices = new int[tessellation * tessellation * 6];

                var deltaX = sizeX/tessellation;
                var deltaY = sizeY/tessellation;

                sizeX /= 2.0f;
                sizeY /= 2.0f;

                int vertexCount = 0;
                int indexCount = 0;
                var normal = Vector3.UnitZ;

                // Create vertices
                for (int y = 0; y < (tessellation+1); y++)
                {
                    for (int x = 0; x < (tessellation+1); x++)
                    {
                        var position = new Vector3(-sizeX + deltaX * x, sizeY - deltaY * y, 0);
                        var texCoord = new Vector2(1.0f * x / tessellation * uvFactor.X, 1.0f * y / tessellation * uvFactor.Y);
                        vertices[vertexCount++] = new VertexPositionNormalTexture(position, normal, texCoord);
                    }
                }

                // Create indices
                for (int y = 0; y < tessellation; y++)
                {
                    for (int x = 0; x < tessellation; x++)
                    {
                        // Six indices (two triangles) per face.
                        int vbase = lineWidth * y + x;
                        indices[indexCount++] = (vbase + 1);
                        indices[indexCount++] = (vbase + 1 + lineWidth);
                        indices[indexCount++] = (vbase + lineWidth);

                        indices[indexCount++] = (vbase + 1);
                        indices[indexCount++] = (vbase + lineWidth);
                        indices[indexCount++] = (vbase );
                    }
                }

                // Create the primitive object.
                return new GeometricPrimitive(device, vertices, indices, toLeftHanded) { Name = "Plane"};
            }
        }
   }
}