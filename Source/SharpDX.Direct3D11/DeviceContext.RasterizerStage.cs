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

using System;

namespace SharpDX.Direct3D11
{
    public partial class RasterizerStage
    {
        /// <summary>	
        /// Get the array of {{viewports}} bound  to the {{rasterizer stage}} 	
        /// </summary>	
        /// <returns>An array of viewports (see <see cref="SharpDX.ViewportF"/>).</returns>
        /// <unmanaged>void RSGetViewports([InOut] int* NumViewports,[Out, Buffer, Optional] D3D10_VIEWPORT* pViewports)</unmanaged>
        /// <msdn-id>ff476477</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::RSGetViewports([InOut] unsigned int* pNumViewports,[Out, Buffer, Optional] D3D11_VIEWPORT* pViewports)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::RSGetViewports</unmanaged-short>	
        public SharpDX.ViewportF[] GetViewports()
        {
            int numViewports = 0;

            GetViewports(ref numViewports, null);

            var viewports = new SharpDX.ViewportF[numViewports];
            GetViewports(ref numViewports, viewports);

            return viewports;
        }

        /// <summary>	
        /// Get the array of {{viewports}} bound  to the {{rasterizer stage}} 	
        /// </summary>	
        /// <returns>An array of viewports (see <see cref="SharpDX.ViewportF"/>).</returns>
        /// <unmanaged>void RSGetViewports([InOut] int* NumViewports,[Out, Buffer, Optional] D3D10_VIEWPORT* pViewports)</unmanaged>
        /// <msdn-id>ff476477</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::RSGetViewports([InOut] unsigned int* pNumViewports,[Out, Buffer, Optional] D3D11_VIEWPORT* pViewports)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::RSGetViewports</unmanaged-short>	
        public void GetViewports(SharpDX.ViewportF[] viewports)
        {
            int numViewports = viewports.Length;
            GetViewports(ref numViewports, viewports);
        }

        /// <summary>	
        /// Get the array of {{scissor rectangles}} bound to the {{rasterizer stage}}.	
        /// </summary>	
        /// <returns>An array of scissor rectangles (see <see cref="SharpDX.Rectangle"/>).</returns>
        /// <unmanaged>void RSGetScissorRects([InOut] int* NumRects,[Out, Buffer, Optional] D3D10_RECT* pRects)</unmanaged>
        /// <msdn-id>ff476475</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::RSGetScissorRects([InOut] unsigned int* pNumRects,[Out, Buffer, Optional] RECT* pRects)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::RSGetScissorRects</unmanaged-short>	
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
        /// <msdn-id>ff476475</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::RSGetScissorRects([InOut] unsigned int* pNumRects,[Out, Buffer, Optional] RECT* pRects)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::RSGetScissorRects</unmanaged-short>	
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
        /// <remarks>	
        /// <p>All scissor rects must be set atomically as one operation. Any scissor rects not defined by the call are disabled.</p><p>The scissor rectangles will only be used if ScissorEnable is set to true in the rasterizer state (see <strong><see cref="SharpDX.Direct3D11.RasterizerStateDescription"/></strong>).</p><p>Which scissor rectangle to use is determined by the SV_ViewportArrayIndex semantic output by a geometry shader (see shader semantic syntax). If a geometry shader does not make use of the SV_ViewportArrayIndex semantic then Direct3D will use the first scissor rectangle in the array.</p><p>Each scissor rectangle in the array corresponds to a viewport in an array of viewports (see <strong><see cref="SharpDX.Direct3D11.RasterizerStage.SetViewports"/></strong>).</p>	
        /// </remarks>	
        /// <msdn-id>ff476478</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::RSSetScissorRects([In] unsigned int NumRects,[In, Buffer, Optional] const void* pRects)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::RSSetScissorRects</unmanaged-short>	
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
        /// <remarks>	
        /// <p>All scissor rects must be set atomically as one operation. Any scissor rects not defined by the call are disabled.</p><p>The scissor rectangles will only be used if ScissorEnable is set to true in the rasterizer state (see <strong><see cref="SharpDX.Direct3D11.RasterizerStateDescription"/></strong>).</p><p>Which scissor rectangle to use is determined by the SV_ViewportArrayIndex semantic output by a geometry shader (see shader semantic syntax). If a geometry shader does not make use of the SV_ViewportArrayIndex semantic then Direct3D will use the first scissor rectangle in the array.</p><p>Each scissor rectangle in the array corresponds to a viewport in an array of viewports (see <strong><see cref="SharpDX.Direct3D11.RasterizerStage.SetViewports"/></strong>).</p>	
        /// </remarks>	
        /// <msdn-id>ff476478</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::RSSetScissorRects([In] unsigned int NumRects,[In, Buffer, Optional] const void* pRects)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::RSSetScissorRects</unmanaged-short>	
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
        /// <remarks>	
        /// <p></p><p>All viewports must be set atomically as one operation. Any viewports not defined by the call are disabled.</p><p>Which viewport to use is determined by the SV_ViewportArrayIndex semantic output by a geometry shader; if a geometry shader does not specify the semantic, Direct3D will use the first viewport in the array.</p>	
        /// </remarks>	
        /// <msdn-id>ff476480</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::RSSetViewports([In] unsigned int NumViewports,[In, Buffer, Optional] const void* pViewports)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::RSSetViewports</unmanaged-short>	
        public void SetViewport(float x, float y, float width, float height, float minZ = 0.0f, float maxZ = 1.0f)
        {
            var viewport = new ViewportF(x, y, width, height, minZ, maxZ);
            unsafe
            {
                SetViewports(1, new IntPtr(&viewport));
            }
        }

        /// <summary>
        /// Binds a single viewport to the rasterizer stage.
        /// </summary>
        /// <param name="viewport">The viewport.</param>
        /// <remarks>	
        /// <p></p><p>All viewports must be set atomically as one operation. Any viewports not defined by the call are disabled.</p><p>Which viewport to use is determined by the SV_ViewportArrayIndex semantic output by a geometry shader; if a geometry shader does not specify the semantic, Direct3D will use the first viewport in the array.</p>	
        /// </remarks>	
        /// <msdn-id>ff476480</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::RSSetViewports([In] unsigned int NumViewports,[In, Buffer, Optional] const void* pViewports)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::RSSetViewports</unmanaged-short>	
        public void SetViewports(ViewportF viewport)
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
        /// <remarks>	
        /// <p></p><p>All viewports must be set atomically as one operation. Any viewports not defined by the call are disabled.</p><p>Which viewport to use is determined by the SV_ViewportArrayIndex semantic output by a geometry shader; if a geometry shader does not specify the semantic, Direct3D will use the first viewport in the array.</p>	
        /// </remarks>	
        /// <msdn-id>ff476480</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::RSSetViewports([In] unsigned int NumViewports,[In, Buffer, Optional] const void* pViewports)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::RSSetViewports</unmanaged-short>	
        public void SetViewports(params ViewportF[] viewports)
        {
            unsafe
            {
                fixed (void* pBuffer = viewports)
                    SetViewports(viewports == null ? 0 : viewports.Length, (IntPtr)pBuffer);
            }
        }
    }
}