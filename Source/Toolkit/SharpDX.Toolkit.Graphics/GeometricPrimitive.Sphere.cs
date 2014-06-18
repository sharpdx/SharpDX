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
// -----------------------------------------------------------------------------
// The following code is a port of DirectXTk http://directxtk.codeplex.com
// -----------------------------------------------------------------------------
// Microsoft Public License (Ms-PL)
//
// This license governs use of the accompanying software. If you use the 
// software, you accept this license. If you do not accept the license, do not
// use the software.
//
// 1. Definitions
// The terms "reproduce," "reproduction," "derivative works," and 
// "distribution" have the same meaning here as under U.S. copyright law.
// A "contribution" is the original software, or any additions or changes to 
// the software.
// A "contributor" is any person that distributes its contribution under this 
// license.
// "Licensed patents" are a contributor's patent claims that read directly on 
// its contribution.
//
// 2. Grant of Rights
// (A) Copyright Grant- Subject to the terms of this license, including the 
// license conditions and limitations in section 3, each contributor grants 
// you a non-exclusive, worldwide, royalty-free copyright license to reproduce
// its contribution, prepare derivative works of its contribution, and 
// distribute its contribution or any derivative works that you create.
// (B) Patent Grant- Subject to the terms of this license, including the license
// conditions and limitations in section 3, each contributor grants you a 
// non-exclusive, worldwide, royalty-free license under its licensed patents to
// make, have made, use, sell, offer for sale, import, and/or otherwise dispose
// of its contribution in the software or derivative works of the contribution 
// in the software.
//
// 3. Conditions and Limitations
// (A) No Trademark License- This license does not grant you rights to use any 
// contributors' name, logo, or trademarks.
// (B) If you bring a patent claim against any contributor over patents that 
// you claim are infringed by the software, your patent license from such 
// contributor to the software ends automatically.
// (C) If you distribute any portion of the software, you must retain all 
// copyright, patent, trademark, and attribution notices that are present in the
// software.
// (D) If you distribute any portion of the software in source code form, you 
// may do so only under this license by including a complete copy of this 
// license with your distribution. If you distribute any portion of the software
// in compiled or object code form, you may only do so under a license that 
// complies with this license.
// (E) The software is licensed "as-is." You bear the risk of using it. The
// contributors give no express warranties, guarantees or conditions. You may
// have additional consumer rights under your local laws which this license 
// cannot change. To the extent permitted under your local laws, the 
// contributors exclude the implied warranties of merchantability, fitness for a
// particular purpose and non-infringement.

using System;
using SharpDX.Mathematics;

namespace SharpDX.Toolkit.Graphics
{
    public partial class GeometricPrimitive
    {
        /// <summary>
        /// A sphere primitive.
        /// </summary>
        public struct Sphere
        {
            /// <summary>
            /// Creates a sphere primitive.
            /// </summary>
            /// <param name="device">The device.</param>
            /// <param name="diameter">The diameter.</param>
            /// <param name="tessellation">The tessellation.</param>
            /// <param name="toLeftHanded">if set to <c>true</c> vertices and indices will be transformed to left handed. Default is true.</param>
            /// <returns>A sphere primitive.</returns>
            /// <exception cref="System.ArgumentOutOfRangeException">tessellation;Must be >= 3</exception>
            public static GeometricPrimitive New(GraphicsDevice device, float diameter = 1.0f, int tessellation = 16, bool toLeftHanded = false)
            {
                if (tessellation < 3) throw new ArgumentOutOfRangeException("tessellation", "Must be >= 3");

                int verticalSegments = tessellation;
                int horizontalSegments = tessellation * 2;

                var vertices = new VertexPositionNormalTexture[(verticalSegments + 1) * (horizontalSegments + 1)];
                var indices = new int[(verticalSegments) * (horizontalSegments + 1) * 6];

                float radius = diameter / 2;

                int vertexCount = 0;
                // Create rings of vertices at progressively higher latitudes.
                for (int i = 0; i <= verticalSegments; i++)
                {
                    float v = 1.0f - (float)i / verticalSegments;

                    var latitude = (float)((i * Math.PI / verticalSegments) - Math.PI / 2.0);
                    var dy = (float)Math.Sin(latitude);
                    var dxz = (float)Math.Cos(latitude);

                    // Create a single ring of vertices at this latitude.
                    for (int j = 0; j <= horizontalSegments; j++)
                    {
                        float u = (float)j / horizontalSegments;

                        var longitude = (float)(j * 2.0 * Math.PI / horizontalSegments);
                        var dx = (float)Math.Sin(longitude);
                        var dz = (float)Math.Cos(longitude);

                        dx *= dxz;
                        dz *= dxz;

                        var normal = new Vector3(dx, dy, dz);
                        var textureCoordinate = new Vector2(u, v);

                        vertices[vertexCount++] = new VertexPositionNormalTexture(normal * radius, normal, textureCoordinate);
                    }
                }

                // Fill the index buffer with triangles joining each pair of latitude rings.
                int stride = horizontalSegments + 1;

                int indexCount = 0;
                for (int i = 0; i < verticalSegments; i++)
                {
                    for (int j = 0; j <= horizontalSegments; j++)
                    {
                        int nextI = i + 1;
                        int nextJ = (j + 1) % stride;

                        indices[indexCount++] = (i * stride + j);
                        indices[indexCount++] = (nextI * stride + j);
                        indices[indexCount++] = (i * stride + nextJ);

                        indices[indexCount++] = (i * stride + nextJ);
                        indices[indexCount++] = (nextI * stride + j);
                        indices[indexCount++] = (nextI * stride + nextJ);
                    }
                }

                // Create the primitive object.
                return new GeometricPrimitive(device, vertices, indices, toLeftHanded) { Name = "Sphere" };
            }
        }
   }
}