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
using SharpDX.Mathematics.Interop;

namespace SharpDX.Direct3D10
{
    public partial class RasterizerStage
    {
        /// <summary>	
        /// Get the array of {{viewports}} bound  to the {{rasterizer stage}} 	
        /// </summary>	
        /// <returns>An array of viewports (see <see cref="RawViewport"/>).</returns>
        /// <unmanaged>void RSGetViewports([InOut] int* NumViewports,[Out, Buffer, Optional] D3D10_VIEWPORT* pViewports)</unmanaged>
        public RawViewport[] GetViewports()
        {
            int numViewports = 0;

            GetViewports(ref numViewports, null);

            RawViewport[] viewports = new RawViewport[numViewports];
            GetViewports(ref numViewports, viewports);

            return viewports;
        }

        /// <summary>	
        /// Get the array of {{viewports}} bound  to the {{rasterizer stage}} 	
        /// </summary>	
        /// <returns>An array of viewports (see <see cref="RawViewport"/>).</returns>
        /// <unmanaged>void RSGetViewports([InOut] int* NumViewports,[Out, Buffer, Optional] D3D10_VIEWPORT* pViewports)</unmanaged>
        public void GetViewports(RawViewport[] viewports)
        {
            int numViewports = viewports.Length;
            GetViewports(ref numViewports, viewports);
        }

        /// <summary>	
        /// Get the array of {{scissor rectangles}} bound to the {{rasterizer stage}}.	
        /// </summary>	
        /// <returns>An array of scissor rectangles (see <see cref="RawRectangle"/>).</returns>
        /// <unmanaged>void RSGetScissorRects([InOut] int* NumRects,[Out, Buffer, Optional] D3D10_RECT* pRects)</unmanaged>
        public RawRectangle[] GetScissorRectangles()
        {
            int numRects = 0;
            GetScissorRects(ref numRects, null);

            RawRectangle[] scissorRectangles = new RawRectangle[numRects];
            GetScissorRects(ref numRects, scissorRectangles);

            return scissorRectangles;
        }

        /// <summary>	
        /// Get the array of {{scissor rectangles}} bound to the {{rasterizer stage}}.	
        /// </summary>	
        /// <returns>An array of scissor rectangles (see <see cref="RawRectangle"/>).</returns>
        /// <unmanaged>void RSGetScissorRects([InOut] int* NumRects,[Out, Buffer, Optional] D3D10_RECT* pRects)</unmanaged>
        public void GetScissorRectangles(RawRectangle[] scissorRectangles)
        {
            int numRects = scissorRectangles.Length;
            GetScissorRects(ref numRects, scissorRectangles);
        }

        /// <summary>
        ///   Binds a single scissor rectangle to the rasterizer stage.
        /// </summary>
        /// <param name = "scissorRectangle">The scissor rectangle to bind.</param>
        public void SetScissorRectangles(RawRectangle scissorRectangle)
        {
            SetScissorRectangles(new RawRectangle[] { scissorRectangle });
        }

        /// <summary>
        ///   Binds a set of scissor rectangles to the rasterizer stage.
        /// </summary>
        /// <param name = "scissorRectangles">The set of scissor rectangles to bind.</param>
        public void SetScissorRectangles(params RawRectangle[] scissorRectangles)
        {
            SetScissorRects(scissorRectangles.Length, scissorRectangles);
        }

        /// <summary>
        ///   Binds a set of viewports to the rasterizer stage.
        /// </summary>
        /// <param name = "viewports">The set of viewports to bind.</param>
        public void SetViewports(params RawViewport[] viewports)
        {
            SetViewports(viewports.Length, viewports);
        }

        /// <summary>
        ///   Binds a single viewport to the rasterizer stage.
        /// </summary>
        /// <param name = "viewport">The viewport to bind.</param>
        public void SetViewports(RawViewport viewport)
        {
            SetViewports(new RawViewport[] { viewport });
        }
    }
}