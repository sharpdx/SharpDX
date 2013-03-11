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

using System.Collections.Generic;

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// Default keys optionnaly stored in <see cref="Material.Properties"/>.
    /// </summary>
    public class MaterialKeys
    {
        private static readonly Dictionary<string, PropertyKey> registeredKeys = new Dictionary<string, PropertyKey>(64);

        /// <summary>
        /// The name of the material, if available. 
        /// </summary>
        public readonly static PropertyKey<string> Name = RegisterKey(new PropertyKey<string>("Name"));

        /// <summary>
        /// Specifies whether meshes using this material must be rendered without backface culling. 0 for false, !0 for true. 
        /// </summary>
        public readonly static PropertyKey<bool> TwoSided = RegisterKey(new PropertyKey<bool>("TwoSided"));

        /// <summary>
        /// One of the <see cref="MaterialShadingMode"/> enumerated values. Defines the library shading model to use for (real time) rendering to approximate the original look of the material as closely as possible. 
        /// </summary>
        public readonly static PropertyKey<MaterialShadingMode> ShadingMode = RegisterKey(new PropertyKey<MaterialShadingMode>("ShadingMode"));

        /// <summary>
        /// Specifies whether wireframe rendering must be turned on for the material. 0 for false, !0 for true. 
        /// </summary>
        public readonly static PropertyKey<bool> Wireframe = RegisterKey(new PropertyKey<bool>("Wireframe"));

        /// <summary>
        /// Defines the blending mode used when rendering this material.
        /// </summary>
        public readonly static PropertyKey<MaterialBlendMode> BlendMode = RegisterKey(new PropertyKey<MaterialBlendMode>("BlendMode"));

        /// <summary>
        /// Defines the opacity of the material in a range between 0..1. 
        /// </summary>
        public readonly static PropertyKey<float> Opacity = RegisterKey(new PropertyKey<float>("Opacity"));

        /// <summary>
        /// Defines the bump normal scaling.
        /// </summary>
        public readonly static PropertyKey<float> BumpScaling = RegisterKey(new PropertyKey<float>("BumpScaling"));

        /// <summary>
        /// Defines the shininess of a phong-shaded material. This is actually the exponent of the phong specular equation 
        /// </summary>
        public readonly static PropertyKey<float> Shininess = RegisterKey(new PropertyKey<float>("Shininess"));

        /// <summary>
        /// Defines the reflectivity of the material.
        /// </summary>
        public readonly static PropertyKey<float> Reflectivity = RegisterKey(new PropertyKey<float>("Reflectivity"));

        /// <summary>
        /// Scales the specular color of the material. 
        /// </summary>
        public readonly static PropertyKey<float> ShininessStrength = RegisterKey(new PropertyKey<float>("ShininessStrength"));

        /// <summary>
        /// Defines the Index Of Refraction for the material. That's not supported by most file formats. 
        /// </summary>
        public readonly static PropertyKey<float> Refraction = RegisterKey(new PropertyKey<float>("Refraction"));

        /// <summary>
        /// Diffuse color of the material. This is typically scaled by the amount of incoming diffuse light (e.g. using gouraud shading).
        /// </summary>
        public readonly static PropertyKey<Color4> ColorDiffuse = RegisterKey(new PropertyKey<Color4>("ColorDiffuse"));

        /// <summary>
        /// Ambient color of the material. This is typically scaled by the amount of ambient light 
        /// </summary>
        public readonly static PropertyKey<Color4> ColorAmbient = RegisterKey(new PropertyKey<Color4>("ColorAmbient"));

        /// <summary>
        /// Specular color of the material. This is typically scaled by the amount of incoming specular light (e.g. using phong shading) 
        /// </summary>
        public readonly static PropertyKey<Color4> ColorSpecular = RegisterKey(new PropertyKey<Color4>("ColorSpecular"));

        /// <summary>
        /// Emissive color of the material. This is the amount of light emitted by the object. In real time applications it will usually not affect surrounding objects, but raytracing applications may wish to treat emissive objects as light sources. 
        /// </summary>
        public readonly static PropertyKey<Color4> ColorEmissive = RegisterKey(new PropertyKey<Color4>("ColorEmissive"));

        /// <summary>
        /// Defines the transparent color of the material, this is the color to be multiplied with the color of translucent light to construct the final 'destination color' for a particular position in the screen buffer. T 
        /// </summary>
        public readonly static PropertyKey<Color4> ColorTransparent = RegisterKey(new PropertyKey<Color4>("ColorTransparent"));

        /// <summary>
        /// Defines the reflective color of the material.
        /// </summary>
        public readonly static PropertyKey<Color4> ColorReflective = RegisterKey(new PropertyKey<Color4>("ColorReflective"));

        /// <summary>
        /// The texture is combined with the result of the diffuse
        /// lighting equation.
        /// </summary>
        public static readonly PropertyKey<MaterialTextureStack> DiffuseTexture = RegisterKey(new PropertyKey<MaterialTextureStack>("DiffuseTexture"));

        /// <summary>
        /// The texture is combined with the result of the specular
        /// lighting equation.
        /// </summary>
        public static readonly PropertyKey<MaterialTextureStack> SpecularTexture = RegisterKey(new PropertyKey<MaterialTextureStack>("SpecularTexture"));

        /// <summary>
        /// The texture is combined with the result of the ambient
        /// lighting equation.
        /// </summary>
        public static readonly PropertyKey<MaterialTextureStack> AmbientTexture = RegisterKey(new PropertyKey<MaterialTextureStack>("AmbientTexture"));

        /// <summary>
        /// The texture is added to the result of the lighting
        /// calculation. It isn't influenced by incoming light.
        /// </summary>
        public static readonly PropertyKey<MaterialTextureStack> EmissiveTexture = RegisterKey(new PropertyKey<MaterialTextureStack>("EmissiveTexture"));

        /// <summary>
        /// The texture is a height map.
        /// </summary>
        /// <remarks>
        /// By convention, higher gray-scale values stand for
        /// higher elevations from the base height.
        /// </remarks>
        public static readonly PropertyKey<MaterialTextureStack> HeightTexture = RegisterKey(new PropertyKey<MaterialTextureStack>("HeightTexture"));

        /// <summary>
        /// The texture is a (tangent space) normal-map.
        /// </summary>
        /// <remarks>
        /// Again, there are several conventions for tangent-space
        /// normal maps. Assimp does (intentionally) not 
        /// distinguish here.
        /// </remarks>
        public static readonly PropertyKey<MaterialTextureStack> NormalsTexture = RegisterKey(new PropertyKey<MaterialTextureStack>("NormalsTexture"));

        /// <summary>
        /// The texture defines the glossiness of the material.
        /// </summary>
        /// <remarks>
        /// The glossiness is in fact the exponent of the specular
        /// (phong) lighting equation. Usually there is a conversion
        /// function defined to map the linear color values in the
        /// texture to a suitable exponent. Have fun.
        /// </remarks>
        public static readonly PropertyKey<MaterialTextureStack> ShininessTexture = RegisterKey(new PropertyKey<MaterialTextureStack>("ShininessTexture"));

        /// <summary>
        /// The texture defines per-pixel opacity.
        /// </summary>
        /// <remarks>
        /// Usually 'white' means opaque and 'black' means 
        /// 'transparency'. Or quite the opposite. Have fun.
        /// </remarks>
        public static readonly PropertyKey<MaterialTextureStack> OpacityTexture = RegisterKey(new PropertyKey<MaterialTextureStack>("OpacityTexture"));

        /// <summary>
        /// Displacement texture
        /// </summary>
        /// <remarks>
        /// The exact purpose and format is application-dependent.
        /// Higher color values stand for higher vertex displacements.
        /// </remarks>
        public static readonly PropertyKey<MaterialTextureStack> DisplacementTexture = RegisterKey(new PropertyKey<MaterialTextureStack>("DisplacementTexture"));

        /// <summary>
        /// Lightmap texture (aka Ambient Occlusion)
        /// </summary>
        /// <remarks>
        /// Both 'Lightmaps' and dedicated 'ambient occlusion maps' are
        /// covered by this material property. The texture contains a
        /// scaling value for the final color value of a pixel. Its
        /// intensity is not affected by incoming light.
        /// </remarks>
        public static readonly PropertyKey<MaterialTextureStack> LightmapTexture = RegisterKey(new PropertyKey<MaterialTextureStack>("LightmapTexture"));

        /// <summary>
        /// Reflection texture
        /// </summary>
        /// <remarks>
        /// Contains the color of a perfect mirror reflection.
        /// Rarely used, almost never for real-time applications.
        /// </remarks>
        public static readonly PropertyKey<MaterialTextureStack> ReflectionTexture = RegisterKey(new PropertyKey<MaterialTextureStack>("ReflectionTexture"));

        /// <summary>
        /// Unknown texture
        /// </summary>
        /// <remarks>
        /// A texture reference that does not match any of the definitions 
        /// above is considered to be 'unknown'. It is still imported,
        /// but is excluded from any further postprocessing.
        /// </remarks>
        public static readonly PropertyKey<MaterialTextureStack> UnknownTexture = RegisterKey(new PropertyKey<MaterialTextureStack>("UnknownTexture"));

        /// <summary>
        /// Gets the registered keys.
        /// </summary>
        /// <value>The registered keys.</value>
        public static IEnumerator<PropertyKey> RegisteredKeys
        {
            get
            {
                return registeredKeys.Values.GetEnumerator();
            }
        }

        /// <summary>
        /// Finds the name of the key by.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>PropertyKey.</returns>
        public static PropertyKey FindKeyByName(string name)
        {
            PropertyKey key;
            registeredKeys.TryGetValue(name, out key);
            return key;
        }

        /// <summary>
        /// Registers the specified key.
        /// </summary>
        /// <typeparam name="T">Type of the property</typeparam>
        /// <param name="key">The key.</param>
        /// <returns>The key.</returns>
        public static PropertyKey<T> RegisterKey<T>(PropertyKey<T> key)
        {
            registeredKeys[key.Name] = key;
            return key;
        }
    }
}