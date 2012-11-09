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
// -----------------------------------------------------------------------------
// The following code is a port of SpriteBatch from DirectXTk
// http://go.microsoft.com/fwlink/?LinkId=248929
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
//--------------------------------------------------------------------
using System;

using SharpDX.Direct3D;

namespace SharpDX.Toolkit.Graphics
{
    // Template makes the API typesafe, eg. PrimitiveBatch<VertexPositionColor>.
    public class PrimitiveBatch<T> : PrimitiveBatchBase where T : struct
    {
        const int DefaultBatchSize = 2048;

        public PrimitiveBatch(GraphicsDevice device, int maxIndices = DefaultBatchSize * 3, int maxVertices = DefaultBatchSize)
            : base(device, maxIndices, maxVertices, Utilities.SizeOf<T>())
        {
        }

        // Similar to the D3D9 API DrawPrimitiveUP.
        public unsafe void Draw(PrimitiveType topology, T[] vertices)
        {
            var mappedVertices = Draw(topology, false, IntPtr.Zero, 0, vertices.Length);
            Utilities.CopyMemory(mappedVertices, (IntPtr)Interop.Fixed(vertices), vertices.Length * VertexSize) ;
        }

        // Similar to the D3D9 API DrawIndexedPrimitiveUP.
        public unsafe void DrawIndexed(PrimitiveType topology, short[] indices, T[] vertices)
        {
            var mappedVertices = Draw(topology, true, (IntPtr)Interop.Fixed(indices), indices.Length, vertices.Length);
            Utilities.CopyMemory(mappedVertices, (IntPtr)Interop.Fixed(vertices), vertices.Length * VertexSize) ;
        }

        public unsafe void DrawLine(T v1, T v2)
        {
            var mappedVertices = Draw(PrimitiveTopology.LineList, false, IntPtr.Zero, 0, 2);
            Utilities.Write(mappedVertices, ref v1);
            Utilities.Write(new IntPtr((byte*)mappedVertices + VertexSize), ref v2);
        }

        public unsafe void DrawTriangle(T v1, T v2, T v3)
        {
            var mappedVertices = Draw(PrimitiveTopology.TriangleList, false, IntPtr.Zero, 0, 3);
            Utilities.Write(mappedVertices, ref v1);
            Utilities.Write(new IntPtr((byte*)mappedVertices + VertexSize), ref v2);
            Utilities.Write(new IntPtr((byte*)mappedVertices + VertexSize + VertexSize), ref v3);
        }

        private readonly static short[] quadIndices = new short[] { 0, 1, 2, 0, 2, 3 };

        public unsafe void DrawQuad(T v1, T v2, T v3, T v4)
        {

            var mappedVertices = (byte*)Draw(PrimitiveTopology.TriangleList, true, (IntPtr)Interop.Fixed(quadIndices), 6, 4);
            Utilities.Write((IntPtr)mappedVertices, ref v1);
            mappedVertices += VertexSize;
            Utilities.Write((IntPtr)mappedVertices, ref v2);
            mappedVertices += VertexSize;
            Utilities.Write((IntPtr)mappedVertices, ref v3);
            mappedVertices += VertexSize;
            Utilities.Write((IntPtr)mappedVertices, ref v4);
        }
    }
}