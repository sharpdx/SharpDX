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

using SharpDX.Mathematics;

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// Provides a set of default parameters that could be defined in an effect. See remarks for usage.
    /// </summary>
    /// <remarks>>
    /// An effect can have several default optional parameters that are accessible when using <see cref="Model.Draw"/>.
    /// These parameters are stored in this structure. Each of them can be null.
    /// </remarks>
    public class EffectDefaultParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EffectDefaultParameters"/> struct.
        /// </summary>
        /// <param name="effect">The effect.</param>
        public EffectDefaultParameters(Effect effect)
        {
            // Initialize predefined parameters used by Model.Draw (to speedup things internally)
            WorldParameter = effect.Parameters["World"];
            ViewParameter = effect.Parameters["View"];
            ViewInverseParameter = effect.Parameters["ViewInverse"];
            ProjectionParameter = effect.Parameters["Projection"];
            WorldViewParameter = effect.Parameters["WorldView"];
            ViewProjectionParameter = effect.Parameters["ViewProjection"] ?? effect.Parameters["ViewProj"];
            WorldInverseTransposeParameter = effect.Parameters["WorldInverseTranspose"];
            WorldInverseTransposeViewParameter = effect.Parameters["WorldInverseTransposeView"];
            WorldViewProjectionParameter = effect.Parameters["WorldViewProj"] ?? effect.Parameters["WorldViewProjection"];
        }

        /// <summary>
        /// The world parameter defined as "float4x4 World" in an effect. See remarks.
        /// </summary>
        /// <remarks>
        /// When applying values with <see cref="Apply(ref EffectDefaultParametersContext, ref Matrix,ref Matrix,ref Matrix)"/>, this parameter will receive the matrix <c>world</c>.
        /// This parameter can be null if not present in the effect.
        /// </remarks>
        public readonly EffectParameter WorldParameter;

        /// <summary>
        /// The view parameter defined as "float4x4 View" in an effect.
        /// </summary>
        /// <remarks>
        /// When applying values with <see cref="Apply(ref EffectDefaultParametersContext, ref Matrix,ref Matrix,ref Matrix)"/>, this parameter will receive the matrix <c>view</c>.
        /// This parameter can be null if not present in the effect.
        /// </remarks>
        public readonly EffectParameter ViewParameter;

        /// <summary>
        /// The view inverse parameter defined as "float4x4 ViewInverse" in an effect.
        /// </summary>
        /// <remarks>
        /// When applying values with <see cref="Apply(ref EffectDefaultParametersContext, ref Matrix,ref Matrix,ref Matrix)"/>, this parameter will receive the matrix <c>view</c>.
        /// This parameter can be null if not present in the effect.
        /// </remarks>
        public readonly EffectParameter ViewInverseParameter;

        /// <summary>
        /// The projection parameter defined as "float4x4 Projection" in an effect.
        /// </summary>
        /// <remarks>
        /// When applying values with <see cref="Apply(ref EffectDefaultParametersContext, ref Matrix,ref Matrix,ref Matrix)"/>, this parameter will receive the matrix <c>projection</c>.
        /// This parameter can be null if not present in the effect.
        /// </remarks>
        public readonly EffectParameter ProjectionParameter;

        /// <summary>
        /// The world view parameter defined as "float4x4 WorldView" in an effect.
        /// </summary>
        /// <remarks>
        /// When applying values with <see cref="Apply(ref EffectDefaultParametersContext, ref Matrix,ref Matrix,ref Matrix)"/>, this parameter will receive the matrix <c>world * view</c>.
        /// This parameter can be null if not present in the effect.
        /// </remarks>
        public readonly EffectParameter WorldViewParameter;

        /// <summary>
        /// The view projection parameter defined as "float4x4 ViewProjection" or "float4x4 ViewProj" in an effect.
        /// </summary>
        /// <remarks>
        /// When applying values with <see cref="Apply(ref EffectDefaultParametersContext, ref Matrix,ref Matrix,ref Matrix)"/>, this parameter will receive the matrix <c>view * projection</c>.
        /// This parameter can be null if not present in the effect.
        /// </remarks>
        public readonly EffectParameter ViewProjectionParameter;

        /// <summary>
        /// The world inverse transpose parameter defined as "float4x4 WorldInverseTranspose"
        /// </summary>
        /// <remarks>
        /// When applying values with <see cref="Apply(ref EffectDefaultParametersContext, ref Matrix,ref Matrix,ref Matrix)"/>, this parameter will receive the matrix <c>world.Invert().Transpose()</c>.
        /// This parameter can be null if not present in the effect.
        /// </remarks>
        public readonly EffectParameter WorldInverseTransposeParameter;

        /// <summary>
        /// The world inverse transpose * view parameter defined as "float4x4 WorldInverseTransposeView"
        /// </summary>
        /// <remarks>
        /// When applying values with <see cref="Apply(ref EffectDefaultParametersContext, ref Matrix,ref Matrix,ref Matrix)"/>, this parameter will receive the matrix <c>world.Invert().Transpose()</c>.
        /// This parameter can be null if not present in the effect.
        /// </remarks>
        public readonly EffectParameter WorldInverseTransposeViewParameter;

        /// <summary>
        /// The world view projection parameter defined as "float4x4 WorldViewProjection" or "float4x4 WorldViewProj" in an effect.
        /// </summary>
        /// <remarks>
        /// When applying values with <see cref="Apply(ref EffectDefaultParametersContext, ref Matrix,ref Matrix,ref Matrix)"/>, this parameter will receive the matrix <c>world * view * projection</c>.
        /// This parameter can be null if not present in the effect.
        /// </remarks>
        public readonly EffectParameter WorldViewProjectionParameter;

        /// <summary>
        /// Applies the specified parameters to the effect including all dependent parameters (<see cref="ViewProjectionParameter"/>, <see cref="WorldViewParameter"/>...etc.) if they are defined in the effect.
        /// </summary>
        /// <param name="world">The world.</param>
        /// <param name="view">The view.</param>
        /// <param name="projection">The projection.</param>
        public void Apply(ref Matrix world, ref Matrix view, ref Matrix projection)
        {
            var context = new EffectDefaultParametersContext();
            Apply(ref context, ref world, ref view, ref projection);
        }

        /// <summary>
        /// Applies the specified parameters to the effect including all dependent parameters (<see cref="ViewProjectionParameter"/>, <see cref="WorldViewParameter"/>...etc.) if they are defined in the effect.
        /// </summary>
        /// <param name="context">The context caching precompute value (like ViewProjection).</param>
        /// <param name="world">The world.</param>
        /// <param name="view">The view.</param>
        /// <param name="projection">The projection.</param>
        public void Apply(ref EffectDefaultParametersContext context, ref Matrix world, ref Matrix view, ref Matrix projection)
        {
            // If effect doesn't implement IEffectMatrices, we can still use
            // directly standard dynamic parameters.
            if (WorldParameter != null)
                WorldParameter.SetValue(ref world);

            if (ViewParameter != null)
                ViewParameter.SetValue(ref view);

            if (ProjectionParameter != null)
                ProjectionParameter.SetValue(ref projection);

            if(ViewInverseParameter != null)
            {
                if(!context.IsViewInverseCalculated)
                {
                    Matrix.Invert(ref view, out context.ViewInverse);
                    context.IsViewInverseCalculated = true;
                }
                ViewInverseParameter.SetValue(ref context.ViewInverse);
            }

            if (WorldViewParameter != null)
            {
                Matrix worldView;
                Matrix.Multiply(ref world, ref view, out worldView);
                WorldViewParameter.SetValue(ref worldView);
            }

            if (ViewProjectionParameter != null)
            {
                if (!context.IsViewProjectionCalculated)
                {
                    Matrix.Multiply(ref view, ref projection, out context.ViewProjection);
                    context.IsViewProjectionCalculated = true;
                }
                ViewProjectionParameter.SetValue(ref context.ViewProjection);
            }

            if (WorldInverseTransposeParameter != null || WorldInverseTransposeViewParameter != null)
            {
                Matrix worldTranspose;
                Matrix worldInverseTranspose;
                Matrix.Invert(ref world, out worldTranspose);
                Matrix.Transpose(ref worldTranspose, out worldInverseTranspose);

                if(WorldInverseTransposeParameter != null)
                {
                    WorldInverseTransposeParameter.SetValue(ref worldInverseTranspose);
                }

                if(WorldInverseTransposeViewParameter != null)
                {
                    Matrix worldInverseViewTranspose;
                    Matrix.Multiply(ref worldInverseTranspose, ref view, out worldInverseViewTranspose);
                    WorldInverseTransposeViewParameter.SetValue(ref worldInverseViewTranspose);
                }
            }

            if (WorldViewProjectionParameter != null)
            {
                if (!context.IsViewProjectionCalculated)
                {
                    Matrix.Multiply(ref view, ref projection, out context.ViewProjection);
                    context.IsViewProjectionCalculated = true;
                }

                Matrix worldViewProjection;
                Matrix.Multiply(ref world, ref context.ViewProjection, out worldViewProjection);

                WorldViewProjectionParameter.SetValue(ref worldViewProjection);
            }
        }

    }
}