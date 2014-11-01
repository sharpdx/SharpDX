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
using SharpDX.Direct3D11;

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// Primitive batch base class.
    /// </summary>
    public class PrimitiveBatchBase : Component
    {
        #region Fields

        /// <summary>
        /// Size in bytes of a vertex.
        /// </summary>
        protected readonly int VertexSize;

        private readonly GraphicsDevice graphicsDevice;

        private readonly Buffer indexBuffer;

        private readonly int maxIndices;

        private readonly int maxVertices;

        private readonly Buffer vertexBuffer;

        private int baseIndex;

        private int baseVertex;

        private int currentIndex;

        private PrimitiveTopology currentTopology;

        private int currentVertex;

        private bool currentlyIndexed;

        private bool inBeginEndPair;

        private DataBox mappedIndices;

        private DataBox mappedVertices;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PrimitiveBatchBase" /> class.
        /// </summary>
        /// <param name="graphicsDevice">The device.</param>
        /// <param name="maxIndices">The max indices.</param>
        /// <param name="maxVertices">The max vertices.</param>
        /// <param name="vertexSize">Size of the vertex.</param>
        protected PrimitiveBatchBase(GraphicsDevice graphicsDevice, int maxIndices, int maxVertices, int vertexSize)
        {
            this.graphicsDevice = graphicsDevice;
            this.maxIndices = maxIndices;
            this.maxVertices = maxVertices;
            this.VertexSize = vertexSize;

            // If you only intend to draw non-indexed geometry, specify maxIndices = 0 to skip creating the index buffer.
            if (maxIndices > 0)
            {
                indexBuffer = ToDispose(Buffer.New(graphicsDevice, maxIndices * sizeof(short), sizeof(short), BufferFlags.IndexBuffer, ResourceUsage.Dynamic));
            }

            vertexBuffer = ToDispose(Buffer.New(graphicsDevice, maxVertices * vertexSize, vertexSize, BufferFlags.VertexBuffer, ResourceUsage.Dynamic));
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets the graphics device.
        /// </summary>
        /// <value>The graphics device.</value>
        public GraphicsDevice GraphicsDevice
        {
            get
            {
                return graphicsDevice;
            }
        }

        /// <summary>
        /// Begin a batch of primitive drawing operations.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Cannot nest Begin calls</exception>
        public virtual void Begin()
        {
            if (inBeginEndPair)
            {
                throw new InvalidOperationException("Cannot nest Begin calls");
            }

            // Bind the index buffer.
            if (maxIndices > 0)
            {
                graphicsDevice.SetIndexBuffer(indexBuffer, false);
            }

            // Binds the vertex buffer
            graphicsDevice.SetVertexBuffer(0, vertexBuffer, VertexSize);

            // If this is a deferred D3D context, reset position so the first Map calls will use D3D11_MAP_WRITE_DISCARD.
            if (graphicsDevice.IsDeferred)
            {
                currentIndex = 0;
                currentVertex = 0;
            }

            inBeginEndPair = true;
        }

        /// <summary>
        /// Ends batch of primitive drawing operations.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Begin must be called before End</exception>
        public virtual void End()
        {
            if (!inBeginEndPair)
            {
                throw new InvalidOperationException("Begin must be called before End");
            }

            FlushBatch();

            inBeginEndPair = false;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Draws the specified topology.
        /// </summary>
        /// <param name="topology">The topology.</param>
        /// <param name="isIndexed">if set to <c>true</c> [is indexed].</param>
        /// <param name="indices">The indices.</param>
        /// <param name="indexCount">The index count.</param>
        /// <param name="vertexCount">The vertex count.</param>
        /// <returns>IntPtr.</returns>
        /// <exception cref="System.ArgumentNullException">Indices cannot be null</exception>
        /// <exception cref="System.ArgumentException">Too many indices;indexCount</exception>
        /// <exception cref="System.InvalidOperationException">Begin must be called before Draw</exception>
        protected unsafe IntPtr Draw(PrimitiveTopology topology, bool isIndexed, IntPtr indices, int indexCount, int vertexCount)
        {
            if (isIndexed && indices == IntPtr.Zero)
            {
                throw new ArgumentNullException("indices", "Indices cannot be null.");
            }

            if (indexCount > maxIndices)
            {
                throw new ArgumentException("Too many indices", "indexCount");
            }

            if (vertexCount > maxVertices)
            {
                throw new ArgumentException("Too many vertices");
            }

            if (!inBeginEndPair)
            {
                throw new InvalidOperationException("Begin must be called before Draw");
            }

            // Can we merge this primitive in with an existing batch, or must we flush first?
            bool wrapIndexBuffer = currentIndex + indexCount > maxIndices;
            bool wrapVertexBuffer = currentVertex + vertexCount > maxVertices;

            if ((topology != currentTopology) ||
                (isIndexed != currentlyIndexed) ||
                !CanBatchPrimitives(topology) ||
                wrapIndexBuffer || wrapVertexBuffer)
            {
                FlushBatch();
            }

            if (wrapIndexBuffer)
            {
                currentIndex = 0;
            }

            if (wrapVertexBuffer)
            {
                currentVertex = 0;
            }

            // If we are not already in a batch, lock the buffers.
            if (currentTopology == PrimitiveType.Undefined)
            {
                if (isIndexed)
                {
                    mappedIndices = LockBuffer(indexBuffer, currentIndex);
                    baseIndex = currentIndex;
                }

                mappedVertices = LockBuffer(vertexBuffer, currentVertex);
                baseVertex = currentVertex;

                currentTopology = topology;
                currentlyIndexed = isIndexed;
            }

            // Copy over the index data.
            if (isIndexed)
            {
                short* outputIndices = (short*)mappedIndices.DataPointer + currentIndex;

                for (int i = 0; i < indexCount; i++)
                {
                    outputIndices[i] = (short)(((short*)indices)[i] + currentVertex - baseVertex);
                }

                currentIndex += indexCount;
            }

            // Return the output vertex data location.
            var result = (IntPtr)((byte*)mappedVertices.DataPointer + (currentVertex * VertexSize));
            currentVertex += vertexCount;
            return result;
        }

        /// <summary>
        /// Flushes the batch.
        /// </summary>
        protected void FlushBatch()
        {
            // Early out if there is nothing to flush.
            if (currentTopology == PrimitiveTopology.Undefined)
            {
                return;
            }

            ((DeviceContext)graphicsDevice).UnmapSubresource(vertexBuffer, 0);

            if (currentlyIndexed)
            {
                // Draw indexed geometry.
                ((DeviceContext)graphicsDevice).UnmapSubresource(indexBuffer, 0);

                graphicsDevice.DrawIndexed(currentTopology, currentIndex - baseIndex, baseIndex, baseVertex);
            }
            else
            {
                // Draw non-indexed geometry.
                graphicsDevice.Draw(currentTopology, currentVertex - baseVertex, baseVertex);
            }

            currentTopology = PrimitiveTopology.Undefined;
        }

        private static bool CanBatchPrimitives(PrimitiveTopology topology)
        {
            switch (topology)
            {
                case PrimitiveTopology.PointList:
                case PrimitiveTopology.LineList:
                case PrimitiveTopology.TriangleList:

                    // Lists can easily be merged.
                    return true;
            }

            return false;

            // We could also merge indexed strips by inserting degenerates,
            // but that's not always a perf win, so let's keep things simple.
        }

        /// <summary>
        /// Locks a vertex or index buffer.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="currentPosition">The current position.</param>
        /// <returns>A DataBox.</returns>
        private DataBox LockBuffer(Buffer buffer, int currentPosition)
        {
            var mapType = (currentPosition == 0) ? MapMode.WriteDiscard : MapMode.WriteNoOverwrite;
            return ((DeviceContext)graphicsDevice).MapSubresource(buffer, 0, mapType, MapFlags.None);
        }

        #endregion
    }

}