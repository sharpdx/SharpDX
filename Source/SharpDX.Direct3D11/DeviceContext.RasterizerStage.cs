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

using System;

namespace SharpDX.Direct3D11
{
    public partial class RasterizerStage
    {
        /// <summary>	
        /// Get the array of {{viewports}} bound  to the {{rasterizer stage}} 	
        /// </summary>	
        /// <returns>An array of viewports (see <see cref="SharpDX.Direct3D11.Viewport"/>).</returns>
        /// <unmanaged>void RSGetViewports([InOut] int* NumViewports,[Out, Buffer, Optional] D3D10_VIEWPORT* pViewports)</unmanaged>
        public SharpDX.Direct3D11.Viewport[] GetViewports()
        {
            int numViewports = 0;

            GetViewports(ref numViewports, null);

            SharpDX.Direct3D11.Viewport[] viewports = new SharpDX.Direct3D11.Viewport[numViewports];
            GetViewports(ref numViewports, viewports);

            return viewports;
        }

        /// <summary>	
        /// Get the array of {{viewports}} bound  to the {{rasterizer stage}} 	
        /// </summary>	
        /// <returns>An array of viewports (see <see cref="SharpDX.Direct3D11.Viewport"/>).</returns>
        /// <unmanaged>void RSGetViewports([InOut] int* NumViewports,[Out, Buffer, Optional] D3D10_VIEWPORT* pViewports)</unmanaged>
        public void GetViewports(SharpDX.Direct3D11.Viewport[] viewports)
        {
            int numViewports = viewports.Length;
            GetViewports(ref numViewports, viewports);
        }

        /// <summary>	
        /// Get the array of {{scissor rectangles}} bound to the {{rasterizer stage}}.	
        /// </summary>	
        /// <returns>An array of scissor rectangles (see <see cref="SharpDX.Rectangle"/>).</returns>
        /// <unmanaged>void RSGetScissorRects([InOut] int* NumRects,[Out, Buffer, Optional] D3D10_RECT* pRects)</unmanaged>
        public SharpDX.Rectangle[] GetScissorRectangles()
        {
            int numRects = 0;
            GetScissorRects(ref numRects, null);

            SharpDX.Rectangle[] scissorRectangles = new Rectangle[numRects];
            GetScissorRects(ref numRects, scissorRectangles);

            return scissorRectangles;
        }

        /// <summary>	
        /// Get the array of {{scissor rectangles}} bound to the {{rasterizer stage}}.	
        /// </summary>	
        /// <returns>An array of scissor rectangles (see <see cref="SharpDX.Rectangle"/>).</returns>
        /// <unmanaged>void RSGetScissorRects([InOut] int* NumRects,[Out, Buffer, Optional] D3D10_RECT* pRects)</unmanaged>
        public void GetScissorRectangles(SharpDX.Rectangle[] scissorRectangles)
        {
            int numRects = scissorRectangles.Length;
            GetScissorRects(ref numRects, scissorRectangles);
        }

        /// <summary>
        /// Binds a single scissor rectangle to the rasterizer stage.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="top">The top.</param>
        /// <param name="right">The right.</param>
        /// <param name="bottom">The bottom.</param>
        public void SetScissorRectangle(int left, int top, int right, int bottom)
        {
            var rect = new Rectangle(left, top, right, bottom);
            unsafe
            {
                SetScissorRects(1, new IntPtr(&rect));
            }
        }

        /// <summary>
        ///   Binds a set of scissor rectangles to the rasterizer stage.
        /// </summary>
        /// <param name = "scissorRectangles">The set of scissor rectangles to bind.</param>
        public void SetScissorRectangles(params Rectangle[] scissorRectangles)
        {
            unsafe
            {
                fixed (void* pBuffer = scissorRectangles)
                    SetScissorRects(scissorRectangles == null ? 0 : scissorRectangles.Length, (IntPtr)pBuffer);
            }
        }

        /// <summary>
        /// Binds a single viewport to the rasterizer stage.
        /// </summary>
        /// <param name="x">The x coord of the viewport.</param>
        /// <param name="y">The x coord of the viewport.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="minZ">The min Z.</param>
        /// <param name="maxZ">The max Z.</param>
        public void SetViewport(float x, float y, float width, float height, float minZ = 0.0f, float maxZ = 1.0f)
        {
            var viewport = new Viewport(x, y, width, height, minZ, maxZ);
            unsafe
            {
                SetViewports(1, new IntPtr(&viewport));
            }
        }

        /// <summary>
        /// Binds a single viewport to the rasterizer stage.
        /// </summary>
        /// <param name="viewport">The viewport.</param>
        public void SetViewports(Viewport viewport)
        {
            unsafe
            {
                SetViewports(1, new IntPtr(&viewport));
            }
        }

        /// <summary>
        ///   Binds a set of viewports to the rasterizer stage.
        /// </summary>
        /// <param name = "viewports">The set of viewports to bind.</param>
        public void SetViewports(params Viewport[] viewports)
        {
            unsafe
            {
                fixed (void* pBuffer = viewports)
                    SetViewports(viewports == null ? 0 : viewports.Length, (IntPtr)pBuffer);
            }
        }
    }
}