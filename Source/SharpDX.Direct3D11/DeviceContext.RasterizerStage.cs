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

namespace SharpDX.Direct3D11
{
    public partial class RasterizerStage
    {
        /// <summary>	
        /// Get the array of {{viewports}} bound  to the {{rasterizer stage}} 	
        /// </summary>	
        /// <returns>An array of viewports (see <see cref="RawViewportF"/>).</returns>
        /// <unmanaged>void RSGetViewports([InOut] int* NumViewports,[Out, Buffer, Optional] D3D10_VIEWPORT* pViewports)</unmanaged>
        /// <msdn-id>ff476477</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::RSGetViewports([InOut] unsigned int* pNumViewports,[Out, Buffer, Optional] D3D11_VIEWPORT* pViewports)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::RSGetViewports</unmanaged-short>	
        public T[] GetViewports<T>() where T : struct
        {
            if (Utilities.SizeOf<T>() != Utilities.SizeOf<RawViewportF>())
                throw new ArgumentException("Type T must have same size and layout as RawViewPortF", "viewports");

            int numViewports = 0;

            GetViewports(ref numViewports, IntPtr.Zero);
            var viewports = new T[numViewports];
            GetViewports(viewports);
            return viewports;
        }

        /// <summary>	
        /// Get the array of {{viewports}} bound  to the {{rasterizer stage}} 	
        /// </summary>	
        /// <returns>An array of viewports (see <see cref="RawViewportF"/>).</returns>
        /// <unmanaged>void RSGetViewports([InOut] int* NumViewports,[Out, Buffer, Optional] D3D10_VIEWPORT* pViewports)</unmanaged>
        /// <msdn-id>ff476477</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::RSGetViewports([InOut] unsigned int* pNumViewports,[Out, Buffer, Optional] D3D11_VIEWPORT* pViewports)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::RSGetViewports</unmanaged-short>	
        public unsafe void GetViewports<T>(T[] viewports) where T : struct
        {
            if (Utilities.SizeOf<T>() != Utilities.SizeOf<RawViewportF>())
                throw new ArgumentException("Type T must have same size and layout as RawViewPortF", "viewports");

            int numViewports = viewports.Length;
            void* pBuffer = Interop.Fixed(viewports);
            GetViewports(ref numViewports, new IntPtr(pBuffer));
        }

        /// <summary>	
        /// Get the array of {{scissor rectangles}} bound to the {{rasterizer stage}}.	
        /// </summary>	
        /// <returns>An array of scissor rectangles (see <see cref="RawRectangle"/>).</returns>
        /// <unmanaged>void RSGetScissorRects([InOut] int* NumRects,[Out, Buffer, Optional] D3D10_RECT* pRects)</unmanaged>
        /// <msdn-id>ff476475</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::RSGetScissorRects([InOut] unsigned int* pNumRects,[Out, Buffer, Optional] RECT* pRects)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::RSGetScissorRects</unmanaged-short>	
        public T[] GetScissorRectangles<T>() where T : struct
        {
            int numRects = 0;
            GetScissorRects(ref numRects, IntPtr.Zero);

            var scissorRectangles = new T[numRects];
            GetScissorRectangles(scissorRectangles);

            return scissorRectangles;
        }

        /// <summary>	
        /// Get the array of {{scissor rectangles}} bound to the {{rasterizer stage}}.	
        /// </summary>	
        /// <returns>An array of scissor rectangles (see <see cref="RawRectangle"/>).</returns>
        /// <unmanaged>void RSGetScissorRects([InOut] int* NumRects,[Out, Buffer, Optional] D3D10_RECT* pRects)</unmanaged>
        /// <msdn-id>ff476475</msdn-id>	
        /// <unmanaged>void ID3D11DeviceContext::RSGetScissorRects([InOut] unsigned int* pNumRects,[Out, Buffer, Optional] RECT* pRects)</unmanaged>	
        /// <unmanaged-short>ID3D11DeviceContext::RSGetScissorRects</unmanaged-short>	
        public unsafe void GetScissorRectangles<T>(T[] scissorRectangles) where T : struct
        {
            if (Utilities.SizeOf<T>() != Utilities.SizeOf<RawRectangle>())
                throw new ArgumentException("Type T must have same size and layout as RawRectangle", "scissorRectangles");

            int numRects = scissorRectangles.Length;
            void* pBuffer = Interop.Fixed(scissorRectangles);
            GetScissorRects(ref numRects, new IntPtr(pBuffer));
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
            var rect = new RawRectangle() { Left = left, Top = top, Right = right, Bottom = bottom };
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
        public void SetScissorRectangles<T>(params T[] scissorRectangles) where T : struct
        {
            if (Utilities.SizeOf<T>() != Utilities.SizeOf<RawRectangle>())
                throw new ArgumentException("Type T must have same size and layout as RawRectangle", "viewports");

            unsafe
            {
                void* pBuffer = scissorRectangles == null ? (void*) null : Interop.Fixed(scissorRectangles);
                SetScissorRects(scissorRectangles == null ? 0 : scissorRectangles.Length, (IntPtr)pBuffer);
            }
        }

        /// <summary>
        /// Binds a single viewport to the rasterizer stage.
        /// </summary>
        /// <param name="x">The x coordinate of the viewport.</param>
        /// <param name="y">The y coordinate of the viewport.</param>
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
            var viewport = new RawViewportF() { X = x, Y = y, Width = width, Height = height, MinDepth= minZ, MaxDepth = maxZ};
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
        public void SetViewport(RawViewportF viewport)
        {
            unsafe
            {
                SetViewports(1, new IntPtr(Interop.Fixed(ref viewport)));
            }
        }

        /// <summary>
        /// Binds a set of viewports to the rasterizer stage.
        /// </summary>
        /// <param name="viewports">The set of viewports to bind.</param>
        /// <param name="count">The number of viewport to set.</param>
        /// <msdn-id>ff476480</msdn-id>
        ///   <unmanaged>void ID3D11DeviceContext::RSSetViewports([In] unsigned int NumViewports,[In, Buffer, Optional] const void* pViewports)</unmanaged>
        ///   <unmanaged-short>ID3D11DeviceContext::RSSetViewports</unmanaged-short>
        /// <remarks><p></p><p>All viewports must be set atomically as one operation. Any viewports not defined by the call are disabled.</p><p>Which viewport to use is determined by the SV_ViewportArrayIndex semantic output by a geometry shader; if a geometry shader does not specify the semantic, Direct3D will use the first viewport in the array.</p></remarks>
        public unsafe void SetViewports(RawViewportF[] viewports, int count = 0)
        {
            void* pBuffer = viewports == null ? (void*) null : Interop.Fixed(viewports);
            SetViewports(viewports == null ? 0 : count <= 0 ? viewports.Length : count, (IntPtr)pBuffer);
        }

        /// <summary>
        /// Binds a set of viewports to the rasterizer stage.
        /// </summary>
        /// <param name="viewports">The set of viewports to bind.</param>
        /// <param name="count">The number of viewport to set.</param>
        /// <msdn-id>ff476480</msdn-id>
        ///   <unmanaged>void ID3D11DeviceContext::RSSetViewports([In] unsigned int NumViewports,[In, Buffer, Optional] const void* pViewports)</unmanaged>
        ///   <unmanaged-short>ID3D11DeviceContext::RSSetViewports</unmanaged-short>
        /// <remarks><p></p><p>All viewports must be set atomically as one operation. Any viewports not defined by the call are disabled.</p><p>Which viewport to use is determined by the SV_ViewportArrayIndex semantic output by a geometry shader; if a geometry shader does not specify the semantic, Direct3D will use the first viewport in the array.</p></remarks>
        public unsafe void SetViewports(RawViewportF* viewports, int count = 0)
        {
            SetViewports(count, (IntPtr)viewports);
        }
    }
}