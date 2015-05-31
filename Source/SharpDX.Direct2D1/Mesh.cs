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

namespace SharpDX.Direct2D1
{
    public partial class Mesh
    {
        /// <summary>	
        /// Create a mesh that uses triangles to describe a shape.	
        /// </summary>	
        /// <remarks>	
        /// To populate a mesh, use its {{Open}} method to obtain an <see cref="SharpDX.Direct2D1.TessellationSink"/>. To draw the mesh, use the render target's {{FillMesh}} method.	
        /// </remarks>	
        /// <param name="renderTarget">an instance of <see cref = "SharpDX.Direct2D1.RenderTarget" /></param>
        /// <unmanaged>HRESULT CreateMesh([Out] ID2D1Mesh** mesh)</unmanaged>
        public Mesh(RenderTarget renderTarget) : base(IntPtr.Zero)
        {
            renderTarget.CreateMesh(this);
        }

        /// <summary>	
        /// Create a mesh that uses triangles to describe a shape and populates it with triangles.
        /// </summary>	
        /// <param name="renderTarget">an instance of <see cref = "SharpDX.Direct2D1.RenderTarget" /></param>
        /// <param name="triangles">An array of <see cref="SharpDX.Direct2D1.Triangle"/> structures that describe the triangles to add to this mesh.</param>
        /// <unmanaged>HRESULT CreateMesh([Out] ID2D1Mesh** mesh)</unmanaged>
        public Mesh(RenderTarget renderTarget, SharpDX.Direct2D1.Triangle[] triangles) : this(renderTarget)
        {
            using(var sink = Open())
            {
                sink.AddTriangles(triangles);
                sink.Close();
            }
        }

        /// <summary>	
        /// Opens the mesh for population.	
        /// </summary>	
        /// <returns>When this method returns, contains a pointer to a pointer to an <see cref="SharpDX.Direct2D1.TessellationSink"/> that is used to populate the mesh. This parameter is passed uninitialized.</returns>
        /// <unmanaged>HRESULT Open([Out] ID2D1TessellationSink** tessellationSink)</unmanaged>
        public SharpDX.Direct2D1.TessellationSink Open()
        {
            SharpDX.Direct2D1.TessellationSink temp;
            Open_(out temp);
            return temp;           
        }
    }
}
