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

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// Describes the material data attached to a <see cref="ModelMeshPart"/>. The material data is not bound to a particular effect.
    /// </summary>
    public class Material : ComponentBase
    {
        /// <summary>
        /// The texture is combined with the result of the diffuse
        /// lighting equation.
        /// </summary>
        public MaterialTextureStack Diffuse;

        /// <summary>
        /// The texture is combined with the result of the specular
        /// lighting equation.
        /// </summary>
        public MaterialTextureStack Specular;

        /// <summary>
        /// The texture is combined with the result of the ambient
        /// lighting equation.
        /// </summary>
        public MaterialTextureStack Ambient;

        /// <summary>
        /// The texture is added to the result of the lighting
        /// calculation. It isn't influenced by incoming light.
        /// </summary>
        public MaterialTextureStack Emissive;

        /// <summary>
        /// The texture is a height map.
        /// </summary>
        /// <remarks>
        /// By convention, higher gray-scale values stand for
        /// higher elevations from the base height.
        /// </remarks>
        public MaterialTextureStack Height;

        /// <summary>
        /// The texture is a (tangent space) normal-map.
        /// </summary>
        /// <remarks>
        /// Again, there are several conventions for tangent-space
        /// normal maps. Assimp does (intentionally) not 
        /// distinguish here.
        /// </remarks>
        public MaterialTextureStack Normals;

        /// <summary>
        /// The texture defines the glossiness of the material.
        /// </summary>
        /// <remarks>
        /// The glossiness is in fact the exponent of the specular
        /// (phong) lighting equation. Usually there is a conversion
        /// function defined to map the linear color values in the
        /// texture to a suitable exponent. Have fun.
        /// </remarks>
        public MaterialTextureStack Shininess;

        /// <summary>
        /// The texture defines per-pixel opacity.
        /// </summary>
        /// <remarks>
        /// Usually 'white' means opaque and 'black' means 
        /// 'transparency'. Or quite the opposite. Have fun.
        /// </remarks>
        public MaterialTextureStack Opacity;

        /// <summary>
        /// Displacement texture
        /// </summary>
        /// <remarks>
        /// The exact purpose and format is application-dependent.
        /// Higher color values stand for higher vertex displacements.
        /// </remarks>
        public MaterialTextureStack Displacement;

        /// <summary>
        /// Lightmap texture (aka Ambient Occlusion)
        /// </summary>
        /// <remarks>
        /// Both 'Lightmaps' and dedicated 'ambient occlusion maps' are
        /// covered by this material property. The texture contains a
        /// scaling value for the final color value of a pixel. Its
        /// intensity is not affected by incoming light.
        /// </remarks>
        public MaterialTextureStack Lightmap;

        /// <summary>
        /// Reflection texture
        /// </summary>
        /// <remarks>
        /// Contains the color of a perfect mirror reflection.
        /// Rarely used, almost never for real-time applications.
        /// </remarks>
        public MaterialTextureStack Reflection;

        /// <summary>
        /// Unknown texture
        /// </summary>
        /// <remarks>
        /// A texture reference that does not match any of the definitions 
        /// above is considered to be 'unknown'. It is still imported,
        /// but is excluded from any further postprocessing.
        /// </remarks>
        public MaterialTextureStack Unknown;

        /// <summary>
        /// Gets the properties attached to this material. A list of standard keys are accessible from <see cref="MaterialKeys"/>.
        /// </summary>
        public MaterialPropertyCollection Properties;

        /// <summary>
        /// Sets a property attached to this material. A list of standard keys are accessible from <see cref="MaterialKeys"/>.
        /// </summary>
        /// <typeparam name="T">Type of the property.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void SetProperty<T>(MaterialKey<T> key, T value)
        {
            Properties.SetProperty(key, value);
        }

        /// <summary>
        /// Determines whether the specified key has property value. A list of standard keys are accessible from <see cref="MaterialKeys"/>.
        /// </summary>
        /// <typeparam name="T">Type of the property.</typeparam>
        /// <param name="key">The key.</param>
        /// <returns><c>true</c> if the specified key has property; otherwise, <c>false</c>.</returns>
        public bool HasProperty<T>(MaterialKey<T> key)
        {
            return Properties.ContainsKey(key);
        }

        /// <summary>
        /// Gets the property value for the specified key. A list of standard keys are accessible from <see cref="MaterialKeys"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <returns>``0.</returns>
        public T GetProperty<T>(MaterialKey<T> key)
        {
            return Properties.GetProperty(key);
        }

        /// <summary>
        /// Deep clone of this instance.
        /// </summary>
        /// <returns>A new copy of this Material.</returns>
        public virtual Material Clone()
        {
            var material = (Material)MemberwiseClone();

            CloneStack(ref material.Diffuse);
            CloneStack(ref material.Specular);
            CloneStack(ref material.Ambient);
            CloneStack(ref material.Emissive);
            CloneStack(ref material.Height);
            CloneStack(ref material.Normals);
            CloneStack(ref material.Shininess);
            CloneStack(ref material.Opacity);
            CloneStack(ref material.Displacement);
            CloneStack(ref material.Lightmap);
            CloneStack(ref material.Reflection);
            CloneStack(ref material.Unknown);

            if (Properties != null)
            {
                material.Properties = Properties.Clone();
            }

            return material;
        }

        private static void CloneStack(ref MaterialTextureStack stack)
        {
            if (stack != null) stack = stack.Clone();
        }
    }
}