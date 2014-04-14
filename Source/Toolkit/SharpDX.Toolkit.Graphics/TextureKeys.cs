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
    /// Default keys optionally stored in <see cref="Material.Properties"/>.
    /// </summary>
    public static class TextureKeys
    {
        /// <summary>
        /// The texture is combined with the result of the diffuse
        /// lighting equation.
        /// </summary>
        public static readonly PropertyKey<MaterialTextureStack> DiffuseTexture = MaterialKeys.RegisterKey(new PropertyKey<MaterialTextureStack>("DiffuseTexture"));

        /// <summary>
        /// The texture is combined with the result of the specular
        /// lighting equation.
        /// </summary>
        public static readonly PropertyKey<MaterialTextureStack> SpecularTexture = MaterialKeys.RegisterKey(new PropertyKey<MaterialTextureStack>("SpecularTexture"));

        /// <summary>
        /// The texture is combined with the result of the ambient
        /// lighting equation.
        /// </summary>
        public static readonly PropertyKey<MaterialTextureStack> AmbientTexture = MaterialKeys.RegisterKey(new PropertyKey<MaterialTextureStack>("AmbientTexture"));

        /// <summary>
        /// The texture is added to the result of the lighting
        /// calculation. It isn't influenced by incoming light.
        /// </summary>
        public static readonly PropertyKey<MaterialTextureStack> EmissiveTexture = MaterialKeys.RegisterKey(new PropertyKey<MaterialTextureStack>("EmissiveTexture"));

        /// <summary>
        /// The texture is a height map.
        /// </summary>
        /// <remarks>
        /// By convention, higher gray-scale values stand for
        /// higher elevations from the base height.
        /// </remarks>
        public static readonly PropertyKey<MaterialTextureStack> HeightTexture = MaterialKeys.RegisterKey(new PropertyKey<MaterialTextureStack>("HeightTexture"));

        /// <summary>
        /// The texture is a (tangent space) normal-map.
        /// </summary>
        /// <remarks>
        /// Again, there are several conventions for tangent-space
        /// normal maps. Assimp does (intentionally) not 
        /// distinguish here.
        /// </remarks>
        public static readonly PropertyKey<MaterialTextureStack> NormalsTexture = MaterialKeys.RegisterKey(new PropertyKey<MaterialTextureStack>("NormalsTexture"));

        /// <summary>
        /// The texture defines the glossiness of the material.
        /// </summary>
        /// <remarks>
        /// The glossiness is in fact the exponent of the specular
        /// (Phong) lighting equation. Usually there is a conversion
        /// function defined to map the linear color values in the
        /// texture to a suitable exponent. Have fun.
        /// </remarks>
        public static readonly PropertyKey<MaterialTextureStack> ShininessTexture = MaterialKeys.RegisterKey(new PropertyKey<MaterialTextureStack>("ShininessTexture"));

        /// <summary>
        /// The texture defines per-pixel opacity.
        /// </summary>
        /// <remarks>
        /// Usually 'white' means opaque and 'black' means 
        /// 'transparency'. Or quite the opposite. Have fun.
        /// </remarks>
        public static readonly PropertyKey<MaterialTextureStack> OpacityTexture = MaterialKeys.RegisterKey(new PropertyKey<MaterialTextureStack>("OpacityTexture"));

        /// <summary>
        /// Displacement texture
        /// </summary>
        /// <remarks>
        /// The exact purpose and format is application-dependent.
        /// Higher color values stand for higher vertex displacements.
        /// </remarks>
        public static readonly PropertyKey<MaterialTextureStack> DisplacementTexture = MaterialKeys.RegisterKey(new PropertyKey<MaterialTextureStack>("DisplacementTexture"));

        /// <summary>
        /// Lightmap texture (aka Ambient Occlusion)
        /// </summary>
        /// <remarks>
        /// Both 'Lightmaps' and dedicated 'ambient occlusion maps' are
        /// covered by this material property. The texture contains a
        /// scaling value for the final color value of a pixel. Its
        /// intensity is not affected by incoming light.
        /// </remarks>
        public static readonly PropertyKey<MaterialTextureStack> LightmapTexture = MaterialKeys.RegisterKey(new PropertyKey<MaterialTextureStack>("LightmapTexture"));

        /// <summary>
        /// Reflection texture
        /// </summary>
        /// <remarks>
        /// Contains the color of a perfect mirror reflection.
        /// Rarely used, almost never for real-time applications.
        /// </remarks>
        public static readonly PropertyKey<MaterialTextureStack> ReflectionTexture = MaterialKeys.RegisterKey(new PropertyKey<MaterialTextureStack>("ReflectionTexture"));

        /// <summary>
        /// Unknown texture
        /// </summary>
        /// <remarks>
        /// A texture reference that does not match any of the definitions 
        /// above is considered to be 'unknown'. It is still imported,
        /// but is excluded from any further post processing.
        /// </remarks>
        public static readonly PropertyKey<MaterialTextureStack> UnknownTexture = MaterialKeys.RegisterKey(new PropertyKey<MaterialTextureStack>("UnknownTexture"));
    }
}