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
using SharpDX.Mathematics.Interop;

namespace SharpDX.Direct3D9
{
    public partial class Line
    {
        /// <summary>	
        /// Instantiates a left-handed coordinate system to create a <see cref="SharpDX.Direct3D9.Line"/>.	
        /// </summary>	
        /// <param name="device"><para>Pointer to an <see cref="SharpDX.Direct3D9.Device"/> interface, representing the device associated with the created box mesh.</para></param>	
        /// <remarks>	
        /// This function creates a mesh with the <see cref="SharpDX.Direct3D9.MeshFlags.Managed"/> creation option and <see cref="SharpDX.Direct3D9.VertexFormat.Position"/> | <see cref="SharpDX.Direct3D9.VertexFormat.Normal"/> Flexible Vertex Format (FVF).	
        /// </remarks>	
        /// <unmanaged>HRESULT D3DXCreateLine([In] IDirect3DDevice9* pDevice,[Out, Fast] ID3DXLine** ppLine)</unmanaged>	
        public Line(Device device)
        {
            D3DX9.CreateLine(device, this);
        }

        /// <summary>	
        /// Draws a line strip in screen space. Input is in the form of an array that defines points (of <see cref="RawVector2"/>) on the line strip.	
        /// </summary>	
        /// <param name="vertices">No documentation.</param>	
        /// <param name="color">No documentation.</param>	
        /// <unmanaged>HRESULT ID3DXLine::Draw([In] const void* pVertexList,[In] unsigned int dwVertexListCount,[In] D3DCOLOR Color)</unmanaged>	
        public void Draw(RawVector2[] vertices, RawColorBGRA color)
        {
            unsafe
            {
                fixed (void* pVertexListRef = vertices)
                    Draw((IntPtr)pVertexListRef, vertices.Length, color);
            }
        }

        /// <summary>	
        /// Draws a line strip in screen space. Input is in the form of an array that defines points (of <see cref="RawVector2"/>) on the line strip.	
        /// </summary>	
        /// <param name="vertices">No documentation.</param>	
        /// <param name="color">No documentation.</param>	
        /// <unmanaged>HRESULT ID3DXLine::Draw([In] const void* pVertexList,[In] unsigned int dwVertexListCount,[In] D3DCOLOR Color)</unmanaged>	
        public void Draw<T>(T[] vertices, RawColorBGRA color) where T : struct
        {
            unsafe
            {
                if (Utilities.SizeOf<T>() != sizeof(RawVector2))
                    throw new ArgumentException("Invalid size for T. Must be 2 floats (8 bytes)");

                Draw((IntPtr)Interop.Fixed(vertices), vertices.Length, color);
            }
        }

        /// <summary>	
        /// Draws a line strip in screen space with a specified input transformation matrix.	
        /// </summary>	
        /// <param name="vertices"><para>Array of vertices that make up the line. See <see cref="RawVector3"/>.</para></param>	
        /// <param name="transform"><para>A scale, rotate, and translate (SRT) matrix for transforming the points. See <see cref="RawMatrix"/>. If this matrix is a projection matrix, any stippled lines will be drawn with a perspective-correct stippling pattern. Or, you can transform the vertices and use <see cref="SharpDX.Direct3D9.Line.Draw"/> to draw the line with a nonperspective-correct stipple pattern.</para></param>	
        /// <param name="color"><para>Color of the line. See <see cref="RawColor4"/>.</para></param>	
        /// <returns>If the method succeeds, the return value is <see cref="SharpDX.Direct3D9.ResultCode.Success"/>. If the method fails, the return value can be one of the following: <see cref="SharpDX.Direct3D9.ResultCode.InvalidCall"/>, D3DXERR_INVALIDDATA.</returns>	
        /// <unmanaged>HRESULT ID3DXLine::DrawTransform([In] const void* pVertexList,[In] unsigned int dwVertexListCount,[In] const D3DXMATRIX* pTransform,[In] D3DCOLOR Color)</unmanaged>	
        public void DrawTransform(RawVector3[] vertices, RawMatrix transform, RawColorBGRA color)
        {
            unsafe
            {
                fixed (void* pVertexListRef = vertices)
                    DrawTransform((IntPtr)pVertexListRef, vertices.Length, ref transform, color);
            }
        }

        /// <summary>	
        /// Draws a line strip in screen space with a specified input transformation matrix.	
        /// </summary>	
        /// <param name="vertices"><para>Array of vertices that make up the line. See <see cref="RawVector3"/>.</para></param>	
        /// <param name="transform"><para>A scale, rotate, and translate (SRT) matrix for transforming the points. See <see cref="RawMatrix"/>. If this matrix is a projection matrix, any stippled lines will be drawn with a perspective-correct stippling pattern. Or, you can transform the vertices and use <see cref="SharpDX.Direct3D9.Line.Draw"/> to draw the line with a nonperspective-correct stipple pattern.</para></param>	
        /// <param name="color"><para>Color of the line. See <see cref="RawColor4"/>.</para></param>	
        /// <returns>If the method succeeds, the return value is <see cref="SharpDX.Direct3D9.ResultCode.Success"/>. If the method fails, the return value can be one of the following: <see cref="SharpDX.Direct3D9.ResultCode.InvalidCall"/>, D3DXERR_INVALIDDATA.</returns>	
        /// <unmanaged>HRESULT ID3DXLine::DrawTransform([In] const void* pVertexList,[In] unsigned int dwVertexListCount,[In] const D3DXMATRIX* pTransform,[In] D3DCOLOR Color)</unmanaged>	
        public void DrawTransform<T>(T[] vertices, RawMatrix transform, RawColorBGRA color) where T : struct
        {
            unsafe
            {
                if (Utilities.SizeOf<T>() != sizeof(RawVector3))
                    throw new ArgumentException("Invalid size for T. Must be 3 floats (12 bytes)");

                DrawTransform((IntPtr)Interop.Fixed(vertices), vertices.Length, ref transform, color);
            }
        }
    }
}
