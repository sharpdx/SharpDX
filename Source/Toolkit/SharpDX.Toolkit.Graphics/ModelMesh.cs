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

using System;

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>The model mesh class.</summary>
    public class ModelMesh : ComponentBase
    {
        /// <summary>The parent bone.</summary>
        public ModelBone ParentBone;

        /// <summary>The bounding sphere.</summary>
        public BoundingSphere BoundingSphere;

        /// <summary>The vertex buffers.</summary>
        internal VertexBufferBindingCollection VertexBuffers;

        /// <summary>The index buffers.</summary>
        internal BufferCollection IndexBuffers;

        /// <summary>The mesh parts.</summary>
        public ModelMeshPartCollection MeshParts;

        /// <summary>The properties.</summary>
        public PropertyCollection Properties;

        /// <summary>Initializes a new instance of the <see cref="ModelMesh"/> class.</summary>
        public ModelMesh()
        {
            Effects = new ModelEffectCollection();
        }

        /// <summary>Gets the effects.</summary>
        /// <value>The effects.</value>
        public ModelEffectCollection Effects { get; private set; }

        /// <summary>
        /// Iterator on each <see cref="ModelMeshPart"/>.
        /// </summary>
        /// <param name="meshPartFunction">The mesh part function.</param>
        public void ForEach(Action<ModelMeshPart> meshPartFunction)
        {
            int meshPartCount = MeshParts.Count;
            for (int i = 0; i < meshPartCount; i++)
            {
                meshPartFunction(MeshParts[i]);
            }
        }

        /// <summary>
        /// Draws all of the ModelMeshPart objects in this mesh, using their current Effect settings.
        /// </summary>
        /// <param name="context">The graphics context.</param>
        /// <param name="effectOverride">The effect to use instead of the effect attached to each mesh part. Default is null (use Effect in MeshPart)</param>
        /// <exception cref="System.InvalidOperationException">Model has no effect</exception>
        public void Draw(GraphicsDevice context, Effect effectOverride = null)
        {
            int count = this.MeshParts.Count;
            for (int i = 0; i < count; i++)
            {
                ModelMeshPart part = MeshParts[i];
                Effect effect = effectOverride ?? part.Effect;
                if (effect == null)
                {
                    throw new InvalidOperationException("ModelMeshPart has no effect and effectOverride is null");
                }
                int passCount = effect.CurrentTechnique.Passes.Count;
                for (int j = 0; j < passCount; j++)
                {
                    effect.CurrentTechnique.Passes[j].Apply();
                    part.Draw(context);
                }
            }
        }

    }
}