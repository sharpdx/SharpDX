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
using SharpDX.Direct3D;

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// A geometric primitive used to draw a simple model built from a set of vertices and indices.
    /// </summary>
    public class GeometricPrimitive<T> : Component where T : struct
    {
        /// <summary>
        /// The index buffer used by this geometric primitive.
        /// </summary>
        private readonly Buffer indexBuffer;

        /// <summary>
        /// The vertex buffer used by this geometric primitive.
        /// </summary>
        private readonly Buffer<T> vertexBuffer;

        /// <summary>
        /// The default graphics device.
        /// </summary>
        protected readonly GraphicsDevice GraphicsDevice;

        /// <summary>
        /// The input layout used by this geometric primitive (shared for all geometric primitive).
        /// </summary>
        private readonly static VertexInputLayout InputLayout = VertexInputLayout.New<T>(0);

        /// <summary>
        /// True if the index buffer is a 32 bit index buffer.
        /// </summary>
        private readonly bool isIndex32Bits;

        /// <summary>
        /// Initializes a new instance of the <see cref="GeometricPrimitive" /> class.
        /// </summary>
        /// <param name="graphicsDevice">The graphics device.</param>
        /// <param name="vertices">The vertices described in right handed form.</param>
        /// <param name="indices">The indices described in right handed form.</param>
        /// <param name="toLeftHanded">if set to <c>true</c> vertices and indices will be transformed to left handed. Default is true.</param>
        /// <exception cref="System.InvalidOperationException">Cannot generate more than 65535 indices on feature level HW &lt;= 9.3</exception>
        public GeometricPrimitive(GraphicsDevice graphicsDevice, T[] vertices, short[] indices, bool toLeftHanded = false)
        {
            GraphicsDevice = graphicsDevice;

            if (indices.Length >= 0xFFFF)
            {
                throw new InvalidOperationException("Cannot generate more than 65535 indices on feature level HW <= 9.3");
            }

            if (toLeftHanded)
                ReverseWinding(vertices, indices);

            indexBuffer = ToDispose(Buffer.Index.New(graphicsDevice, indices));
            vertexBuffer = ToDispose(Buffer.Vertex.New(graphicsDevice, vertices));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeometricPrimitive" /> class.
        /// </summary>
        /// <param name="graphicsDevice">The graphics device.</param>
        /// <param name="vertices">The vertices.</param>
        /// <param name="indices">The indices.</param>
        /// <param name="toLeftHanded">if set to <c>true</c> vertices and indices will be transformed to left handed. Default is true.</param>
        /// <exception cref="System.InvalidOperationException">Cannot generate more than 65535 indices on feature level HW <= 9.3</exception>
        public GeometricPrimitive(GraphicsDevice graphicsDevice, T[] vertices, int[] indices, bool toLeftHanded = false)
        {
            GraphicsDevice = graphicsDevice;

            if (toLeftHanded)
                ReverseWinding(vertices, indices);

            if (indices.Length < 0xFFFF)
            {
                var indicesShort = new ushort[indices.Length];
                for (int i = 0; i < indicesShort.Length; i++)
                {
                    indicesShort[i] = (ushort)indices[i];
                }
                indexBuffer = ToDispose(Buffer.Index.New(graphicsDevice, indicesShort));
            }
            else
            {
                if (graphicsDevice.Features.Level <= FeatureLevel.Level_9_3)
                {
                    throw new InvalidOperationException("Cannot generate more than 65535 indices on feature level HW <= 9.3");
                }

                indexBuffer = ToDispose(Buffer.Index.New(graphicsDevice, indices));
                isIndex32Bits = true;
            }

            vertexBuffer = ToDispose(Buffer.Vertex.New(graphicsDevice, vertices));
        }

        /// <summary>
        /// Draws this <see cref="GeometricPrimitive" /> using an optional effect. See remarks.
        /// </summary>
        /// <param name="effect">The effect. Null by default.</param>
        /// <remarks>If an effect is passed to this method, a draw will be issued for each passes defined in the current technique of the effect. 
        /// If no effect are passed, it is expected that an <see cref="EffectPass"/> was previously applied.
        /// </remarks>
        public void Draw(Effect effect = null)
        {
            Draw(GraphicsDevice, effect);
        }

        /// <summary>
        /// Draws this <see cref="GeometricPrimitive" /> using an optional effect. See remarks.
        /// </summary>
        /// <param name="graphicsDevice">The graphics device.</param>
        /// <param name="effect">The effect. Null by default.</param>
        /// <remarks>If an effect is passed to this method, a draw will be issued for each passes defined in the current technique of the effect. 
        /// If no effect are passed, an <see cref="EffectPass"/> must have been applied previously.
        /// </remarks>
        public void Draw(GraphicsDevice graphicsDevice, Effect effect = null)
        {
            if (effect != null)
            {
                var passes = effect.CurrentTechnique.Passes;
                var passesCount = passes.Count;
                for(var i = 0; i < passesCount; i++)
                {
                    Draw(graphicsDevice, passes[i]);
                }
            }
            else
            {
                Draw(graphicsDevice, (EffectPass)null);
            }
        }

        /// <summary>
        /// Draws this <see cref="GeometricPrimitive" /> using an optional specific pass, and
        /// a <see cref="PrimitiveType"/> of TriangleList.
        /// </summary>
        /// <param name="graphicsDevice">The graphics device.</param>
        /// <param name="pass">The effect pass, null by default.</param>
        /// <remarks>If an effect pass is passed to this method, the effect pass will be applied before drawing the geometry. 
        /// If no effect pass is passed, an <see cref="EffectPass"/> must have been applied previously.
        /// </remarks>
        public void Draw(GraphicsDevice graphicsDevice, EffectPass pass = null)
        {
            Draw(graphicsDevice, PrimitiveType.TriangleList, pass);
        }

        /// <summary>
        /// Draws this <see cref="GeometricPrimitive" /> using an optional specific pass
        /// </summary>
        /// <param name="graphicsDevice">The graphics device.</param>
        /// <param name="primitiveType">The primitive type to use when drawing.</param>
        /// <param name="pass">The effect pass, null by default.</param>
        /// <remarks>If an effect pass is passed to this method, the effect pass will be applied before drawing the geometry. 
        /// If no effect pass is passed, an <see cref="EffectPass"/> must have been applied previously.
        /// </remarks>
        public void Draw(GraphicsDevice graphicsDevice, PrimitiveType primitiveType, EffectPass pass)
        {
            if (pass != null)
                pass.Apply();

            // Setup the Vertex Buffer
            graphicsDevice.SetVertexBuffer(0, vertexBuffer);

            // Setup the Vertex Buffer Input layout
            graphicsDevice.SetVertexInputLayout(InputLayout);

            // Setup the index Buffer
            graphicsDevice.SetIndexBuffer(indexBuffer, isIndex32Bits);

            // Finally Draw this mesh
            graphicsDevice.DrawIndexed(primitiveType, indexBuffer.ElementCount);
        }

        /// <summary>
        /// Helper for flipping winding of geometric primitives for LH vs. RH coordinates
        /// </summary>
        /// <typeparam name="TIndex">The type of the T index.</typeparam>
        /// <param name="vertices">The vertices.</param>
        /// <param name="indices">The indices.</param>
        protected virtual void ReverseWinding<TIndex>(T[] vertices, TIndex[] indices)
        {
            for (int i = 0; i < indices.Length; i += 3)
            {
                Utilities.Swap(ref indices[i], ref indices[i + 2]);
            }
        }
    }

    /// <summary>
    /// A geometric primitive helper that provides several drawable basic 3D models <see cref="Cube.New"/>, <see cref="Cylinder.New"/>, <see cref="Sphere.New"/>, <see cref="GeoSphere.New"/>, <see cref="Teapot.New"/>, <see cref="Torus.New"/>.
    /// </summary>
    /// <remarks>The vertex format used is <see cref="VertexPositionNormalTexture"/>.</remarks>
    public partial class GeometricPrimitive : GeometricPrimitive<VertexPositionNormalTexture>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GeometricPrimitive" /> class.
        /// </summary>
        /// <param name="graphicsDevice">The graphics device.</param>
        /// <param name="vertices">The vertices described in right handed form.</param>
        /// <param name="indices">The indices described in right handed form.</param>
        /// <param name="toLeftHanded">if set to <c>true</c> vertices and indices will be transformed to left handed. Default is true.</param>
        /// <exception cref="System.InvalidOperationException">Cannot generate more than 65535 indices on feature level HW &lt;= 9.3</exception>
        public GeometricPrimitive(GraphicsDevice graphicsDevice, VertexPositionNormalTexture[] vertices, short[] indices, bool toLeftHanded = false)
            : base(graphicsDevice, vertices, indices, toLeftHanded)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeometricPrimitive" /> class.
        /// </summary>
        /// <param name="graphicsDevice">The graphics device.</param>
        /// <param name="vertices">The vertices.</param>
        /// <param name="indices">The indices.</param>
        /// <param name="toLeftHanded">if set to <c>true</c> vertices and indices will be transformed to left handed. Default is true.</param>
        /// <exception cref="System.InvalidOperationException">Cannot generate more than 65535 indices on feature level HW &lt;= 9.3</exception>
        public GeometricPrimitive(GraphicsDevice graphicsDevice, VertexPositionNormalTexture[] vertices, int[] indices, bool toLeftHanded = false)
            : base(graphicsDevice, vertices, indices, toLeftHanded)
        {
        }

        protected override void ReverseWinding<TIndex>(VertexPositionNormalTexture[] vertices, TIndex[] indices)
        {
            base.ReverseWinding(vertices, indices);
            for( int i = 0; i < vertices.Length; i++)
            {
                vertices[i].TextureCoordinate.X = ( 1.0f - vertices[i].TextureCoordinate.X );
            }
        }
    }
}