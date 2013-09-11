// Copyright (c) 2010-2013 SharpDX - Alexandre Mutel
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

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// Represents a batch of geometry information to submit to the graphics device during rendering. 
    /// Each <see cref="ModelMeshPart"/> is a subdivision of a <see cref="ModelMesh"/> object. The <see cref="ModelMesh"/> class is split into multiple <see cref="ModelMeshPart"/> objects, typically based on material information. 
    /// See remarks for differences with XNA.
    /// </summary>
    /// <remarks>
    /// Unlike XNA, a <see cref="ModelMeshPart"/> is not bound to a specific Effect. The effect must have been setup prior calling the <see cref="Draw"/> method on this instance.
    /// The <see cref="Draw"/> method is only responsible to setup the VertexBuffer, IndexBuffer and call the appropriate <see cref="GraphicsDevice.DrawIndexed"/> method on the <see cref="GraphicsDevice"/>.
    /// </remarks>
    public class ModelMeshPart : ComponentBase
    {
        /// <summary>
        /// The parent mesh.
        /// </summary>
        public ModelMesh ParentMesh;

        /// <summary>
        /// The material used by this mesh part.
        /// </summary>
        public Material Material;

        /// <summary>
        /// The index buffer range for this mesh part.
        /// </summary>
        public ModelBufferRange<Buffer> IndexBuffer;

        /// <summary>
        /// The vertex buffer range for this mesh part.
        /// </summary>
        public ModelBufferRange<VertexBufferBinding> VertexBuffer;

        /// <summary>
        /// The attributes for this mesh part.
        /// </summary>
        public PropertyCollection Properties;

        private Effect effect;

        /// <summary>Gets or sets the material Effect for this mesh part.  Reference page contains code sample.</summary>
        public Effect Effect
        {
            get
            {
                return effect;
            }
            set
            {
                if (value != effect)
                {
                    bool isPreviousEffectUsedByAnotherMeshPart = false;
                    bool isEffectUsedByAnotherMeshPart = false;

                    var effects = ParentMesh.Effects;
                    var meshPartRenderers = ParentMesh.MeshParts;

                    // Check that 
                    for (int i = 0; i < meshPartRenderers.Count; i++)
                    {
                        ModelMeshPart objA = meshPartRenderers[i];
                        if (!ReferenceEquals(objA, this))
                        {
                            var partEffect = meshPartRenderers[i].Effect;
                            if (ReferenceEquals(partEffect, effect))
                            {
                                isPreviousEffectUsedByAnotherMeshPart = true;
                            }
                            else if (ReferenceEquals(partEffect, value))
                            {
                                isEffectUsedByAnotherMeshPart = true;
                            }
                        }
                    }

                    if (!isPreviousEffectUsedByAnotherMeshPart && (effect != null))
                    {
                        effects.Remove(effect);
                    }
                    if (!isEffectUsedByAnotherMeshPart && (value != null))
                    {
                        effects.Add(value);
                    }
                    effect = value;
                }
            }
        }

        /// <summary>
        /// Draws this <see cref="ModelMeshPart"/>. See remarks for difference with XNA.
        /// </summary>
        /// <param name="graphicsDevice">The graphics device.</param>
        /// <remarks>
        /// Unlike XNA, a <see cref="ModelMeshPart"/> is not bound to a specific Effect. The effect must have been setup prior calling this method.
        /// This method is only responsible to setup the VertexBuffer, IndexBuffer and call the appropriate <see cref="GraphicsDevice.DrawIndexed"/> method on the <see cref="GraphicsDevice"/>.
        /// </remarks>
        public void Draw(GraphicsDevice graphicsDevice)
        {
            // Setup the Vertex Buffer
            var vertexBuffer = VertexBuffer.Resource.Buffer;
            var elementSize = vertexBuffer.ElementSize;
            graphicsDevice.SetVertexBuffer(0, vertexBuffer, elementSize, VertexBuffer.Start == 0 ? 0 : VertexBuffer.Start * elementSize);

            // Setup the Vertex Buffer Input layout
            graphicsDevice.SetVertexInputLayout(VertexBuffer.Resource.Layout);

            // Setup the index Buffer
            var indexBuffer = IndexBuffer.Resource;
            graphicsDevice.SetIndexBuffer(indexBuffer, indexBuffer.ElementSize == 4, IndexBuffer.Start == 0 ? 0 : IndexBuffer.Start * indexBuffer.ElementSize);

            // Finally Draw this mesh
            graphicsDevice.DrawIndexed(PrimitiveType.TriangleList, IndexBuffer.Count);
        }
    }
}