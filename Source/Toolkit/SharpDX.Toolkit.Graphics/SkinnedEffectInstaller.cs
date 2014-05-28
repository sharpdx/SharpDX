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
    /// Implementation of the <see cref="IEffectInstaller"/> for the <see cref="SkinnedEffect"/>.
    /// </summary>
    /// <remarks>
    /// This effect installer uses <see cref="SkinnedEffect"/> if the model supports skeletal animation and <see cref="BasicEffect"/> otherwise.
    /// </remarks>
    public class SkinnedEffectInstaller : BasicEffectInstaller
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SkinnedEffectInstaller" /> class.
        /// </summary>
        /// <param name="services">The services.</param>
        public SkinnedEffectInstaller(IServiceRegistry services)
            : base(services)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SkinnedEffectInstaller" /> class.
        /// </summary>
        /// <param name="graphicsDevice">The graphics device.</param>
        public SkinnedEffectInstaller(GraphicsDevice graphicsDevice)
            : base(graphicsDevice)
        {
        }

        /// <summary>
        /// Creates an instance of <see cref="SkinnedEffect"/>, overridable in case a user would want to subclass <see cref="SkinnedEffect"/>.
        /// </summary>
        /// <returns>A new instance of BasicEffect.</returns>
        protected virtual SkinnedEffect CreateSkinnedEffect()
        {
            return new SkinnedEffect(GraphicsDevice);
        }

        protected override Effect Process(Model model, ModelMeshPart meshPart)
        {
            if (!meshPart.IsSkinned)
            {
                return base.Process(model, meshPart);
            }

            var effect = CreateSkinnedEffect();

            var material = meshPart.Material;

            // Setup ColorDiffuse
            if (material.HasProperty(MaterialKeys.ColorDiffuse))
            {
                effect.DiffuseColor = material.GetProperty(MaterialKeys.ColorDiffuse);
            }

            // Setup ColorSpecular
            if (material.HasProperty(MaterialKeys.ColorSpecular))
            {
                effect.SpecularColor = material.GetProperty(MaterialKeys.ColorSpecular);
            }

            // Setup ColorEmissive
            if (material.HasProperty(MaterialKeys.ColorEmissive))
            {
                effect.EmissiveColor = material.GetProperty(MaterialKeys.ColorEmissive);
            }

            if (material.HasProperty(TextureKeys.DiffuseTexture))
            {
                var diffuseTextureStack = material.GetProperty(TextureKeys.DiffuseTexture);
                if (diffuseTextureStack.Count > 0)
                {
                    var diffuseTexture = diffuseTextureStack[0];
                    effect.Texture = (Texture2D)diffuseTexture.Texture;
                }
            }

            return effect;
        }
    }
}