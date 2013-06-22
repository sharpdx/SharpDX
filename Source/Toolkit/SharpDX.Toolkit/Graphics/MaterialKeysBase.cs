using System.Collections.Generic;

namespace SharpDX.Toolkit.Graphics
{
    public class MaterialKeysBase
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
        /// Defines the shininess of a Phong-shaded material. This is actually the exponent of the Phong specular equation 
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
        /// Diffuse color of the material. This is typically scaled by the amount of incoming diffuse light (e.g. using Gouraud shading).
        /// </summary>
        public readonly static PropertyKey<Color4> ColorDiffuse = RegisterKey(new PropertyKey<Color4>("ColorDiffuse"));

        /// <summary>
        /// Ambient color of the material. This is typically scaled by the amount of ambient light 
        /// </summary>
        public readonly static PropertyKey<Color4> ColorAmbient = RegisterKey(new PropertyKey<Color4>("ColorAmbient"));

        /// <summary>
        /// Specular color of the material. This is typically scaled by the amount of incoming specular light (e.g. using Phong shading) 
        /// </summary>
        public readonly static PropertyKey<Color3> ColorSpecular = RegisterKey(new PropertyKey<Color3>("ColorSpecular"));

        /// <summary>
        /// Emissive color of the material. This is the amount of light emitted by the object. In real time applications it will usually not affect surrounding objects, but ray tracing applications may wish to treat emissive objects as light sources. 
        /// </summary>
        public readonly static PropertyKey<Color3> ColorEmissive = RegisterKey(new PropertyKey<Color3>("ColorEmissive"));

        /// <summary>
        /// Defines the transparent color of the material, this is the color to be multiplied with the color of translucent light to construct the final 'destination color' for a particular position in the screen buffer. T 
        /// </summary>
        public readonly static PropertyKey<Color4> ColorTransparent = RegisterKey(new PropertyKey<Color4>("ColorTransparent"));

        /// <summary>
        /// Defines the reflective color of the material.
        /// </summary>
        public readonly static PropertyKey<Color4> ColorReflective = RegisterKey(new PropertyKey<Color4>("ColorReflective"));

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