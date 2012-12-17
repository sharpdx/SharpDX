/*
* Copyright (c) 2012 Nicholas Woodfield
* 
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
* 
* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
* 
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
* THE SOFTWARE.
*/

using System;
using System.Collections.Generic;
using Assimp.Unmanaged;

namespace Assimp {
    /// <summary>
    /// A material contains all the information that describes how to render a mesh. E.g. textures, colors, and render states. Internally
    /// all this information is stored as key-value pair properties. The class contains many convienence methods and properties for
    /// accessing non-texture/texture properties without having to know the Assimp material key names. Not all properties may be present,
    /// and if they aren't a default value will be returned.
    /// </summary>
    internal sealed class Material {
        private Dictionary<String, MaterialProperty> _properties;
        private Dictionary<int, List<TextureSlot>> _textures;

        /// <summary>
        /// Gets the number of properties contained in the material.
        /// </summary>
        public int PropertyCount {
            get {
                return _properties.Count;
            }
        }

        /// <summary>
        /// Checks if the material has a name property.
        /// </summary>
        public bool HasName {
            get {
                return HasProperty(AiMatKeys.NAME);
            }
        }

        /// <summary>
        /// Gets the material name value, if any. Default value is an empty string.
        /// </summary>
        public String Name {
            get {
                MaterialProperty prop = GetProperty(AiMatKeys.NAME);
                if(prop != null) {
                    return prop.AsString();
                }
                return String.Empty;
            }
        }

        /// <summary>
        /// Checks if the material has a two-sided property.
        /// </summary>
        public bool HasTwoSided {
            get {
                return HasProperty(AiMatKeys.TWOSIDED);
            }
        }

        /// <summary>
        /// Gets if the material should be rendered as two-sided. Default value is false.
        /// </summary>
        public bool IsTwoSided {
            get {
                MaterialProperty prop = GetProperty(AiMatKeys.TWOSIDED);
                if(prop != null) {
                    return prop.AsBoolean();
                }
                return false;
            }
        }

        /// <summary>
        /// Checks if the material has a shading-mode property.
        /// </summary>
        public bool HasShadingMode {
            get {
                return HasProperty(AiMatKeys.SHADING_MODEL);
            }
        }

        /// <summary>
        /// Gets the shading mode. Default value is <see cref="Assimp.ShadingMode.None"/>, meaning it is not defined.
        /// </summary>
        public ShadingMode ShadingMode {
            get {
                MaterialProperty prop = GetProperty(AiMatKeys.SHADING_MODEL);
                if(prop != null) {
                    return prop.AsShadingMode();
                }
                return ShadingMode.None;
            }
        }

        /// <summary>
        /// Checks if the material has a wireframe property.
        /// </summary>
        public bool HasWireFrame {
            get {
                return HasProperty(AiMatKeys.ENABLE_WIREFRAME);
            }
        }

        /// <summary>
        /// Gets if wireframe should be enabled. Default value is false.
        /// </summary>
        public bool IsWireFrameEnabled {
            get {
                MaterialProperty prop = GetProperty(AiMatKeys.ENABLE_WIREFRAME);
                if(prop != null) {
                    return prop.AsBoolean();
                }
                return false;
            }
        }

        /// <summary>
        /// Checks if the material has a blend mode property.
        /// </summary>
        public bool HasBlendMode {
            get {
                return HasProperty(AiMatKeys.BLEND_FUNC);
            }
        }

        /// <summary>
        /// Gets the blending mode. Default value is <see cref="Assimp.BlendMode.Default"/>.
        /// </summary>
        public BlendMode BlendMode {
            get {
                MaterialProperty prop = GetProperty(AiMatKeys.BLEND_FUNC);
                if(prop != null) {
                    return prop.AsBlendMode();
                }
                return BlendMode.Default;
            }
        }

        /// <summary>
        /// Checks if the material has an opacity property.
        /// </summary>
        public bool HasOpacity {
            get {
                return HasProperty(AiMatKeys.OPACITY);
            }
        }

        /// <summary>
        /// Gets the opacity. Default value is 1.0f.
        /// </summary>
        public float Opacity {
            get {
                MaterialProperty prop = GetProperty(AiMatKeys.OPACITY);
                if(prop != null) {
                    return prop.AsFloat();
                }
                return 1.0f;
            }
        }

        /// <summary>
        /// Checks if the material has a bump scaling property.
        /// </summary>
        public bool HasBumpScaling {
            get {
                return HasProperty(AiMatKeys.BUMPSCALING);
            }
        }

        /// <summary>
        /// Gets the bump scaling. Default value is 0.0f;
        /// </summary>
        public float BumpScaling {
            get {
                MaterialProperty prop = GetProperty(AiMatKeys.BUMPSCALING);
                if(prop != null) {
                    return prop.AsFloat();
                }
                return 0.0f;
            }
        }

        /// <summary>
        /// Checks if the material has a shininess property.
        /// </summary>
        public bool HasShininess {
            get {
                return HasProperty(AiMatKeys.SHININESS);
            }
        }

        /// <summary>
        /// Gets the shininess. Default value is 0.0f;
        /// </summary>
        public float Shininess {
            get {
                MaterialProperty prop = GetProperty(AiMatKeys.SHININESS);
                if(prop != null) {
                    return prop.AsFloat();
                }
                return 0.0f;
            }
        }

        /// <summary>
        /// Checks if the material has a shininess strength property.
        /// </summary>
        public bool HasShininessStrength {
            get {
                return HasProperty(AiMatKeys.SHININESS_STRENGTH);
            }
        }

        /// <summary>
        /// Gets the shininess strength. Default vaulue is 1.0f.
        /// </summary>
        public float ShininessStrength {
            get {
                MaterialProperty prop = GetProperty(AiMatKeys.SHININESS_STRENGTH);
                if(prop != null) {
                    return prop.AsFloat();
                }
                return 1.0f;
            }
        }

        /// <summary>
        /// Checks if the material has a reflectivty property.
        /// </summary>
        public bool HasReflectivity {
            get {
                return HasProperty(AiMatKeys.REFLECTIVITY);
            }
        }


        /// <summary>
        /// Gets the reflectivity. Default value is 0.0f;
        /// </summary>
        public float Reflectivity {
            get {
                MaterialProperty prop = GetProperty(AiMatKeys.REFLECTIVITY);
                if(prop != null) {
                    return prop.AsFloat();
                }
                return 0.0f;
            }
        }

        /// <summary>
        /// Checks if the material has a color diffuse property.
        /// </summary>
        public bool HasColorDiffuse {
            get {
                return HasProperty(AiMatKeys.COLOR_DIFFUSE);
            }
        }

        /// <summary>
        /// Gets the color diffuse. Default value is white.
        /// </summary>
        public Color4D ColorDiffuse {
            get {
                MaterialProperty prop = GetProperty(AiMatKeys.COLOR_DIFFUSE);
                if(prop != null) {
                    return prop.AsColor4D();
                }
                return new Color4D(1.0f, 1.0f, 1.0f, 1.0f);
            }
        }

        /// <summary>
        /// Checks if the material has a color ambient property.
        /// </summary>
        public bool HasColorAmbient {
            get {
                return HasProperty(AiMatKeys.COLOR_AMBIENT);
            }
        }

        /// <summary>
        /// Gets the color ambient. Default value is (.2f, .2f, .2f, 1.0f).
        /// </summary>
        public Color4D ColorAmbient {
            get {
                MaterialProperty prop = GetProperty(AiMatKeys.COLOR_AMBIENT);
                if(prop != null) {
                    return prop.AsColor4D();
                }
                return new Color4D(.2f, .2f, .2f, 1.0f);
            }
        }

        /// <summary>
        /// Checks if the material has a color specular property.
        /// </summary>
        public bool HasColorSpecular {
            get {
                return HasProperty(AiMatKeys.COLOR_SPECULAR);
            }
        }

        /// <summary>
        /// Gets the color specular. Default value is black.
        /// </summary>
        public Color4D ColorSpecular {
            get {
                MaterialProperty prop = GetProperty(AiMatKeys.COLOR_SPECULAR);
                if(prop != null) {
                    return prop.AsColor4D();
                }
                return new Color4D(0, 0, 0, 1.0f);
            }
        }

        /// <summary>
        /// Checks if the material has a color emissive property.
        /// </summary>
        public bool HasColorEmissive {
            get {
                return HasProperty(AiMatKeys.COLOR_EMISSIVE);
            }
        }

        /// <summary>
        /// Gets the color emissive. Default value is black.
        /// </summary>
        public Color4D ColorEmissive {
            get {
                MaterialProperty prop = GetProperty(AiMatKeys.COLOR_EMISSIVE);
                if(prop != null) {
                    return prop.AsColor4D();
                }
                return new Color4D(0, 0, 0, 1.0f);
            }
        }

        /// <summary>
        /// Checks if the material has a color transparent property.
        /// </summary>
        public bool HasColorTransparent {
            get {
                return HasProperty(AiMatKeys.COLOR_TRANSPARENT);
            }
        }

        /// <summary>
        /// Gets the color transparent. Default value is black.
        /// </summary>
        public Color4D ColorTransparent {
            get {
                MaterialProperty prop = GetProperty(AiMatKeys.COLOR_TRANSPARENT);
                if(prop != null) {
                    return prop.AsColor4D();
                }
                return new Color4D(0, 0, 0, 1.0f);
            }
        }

        /// <summary>
        /// Checks if the material has a color reflective property.
        /// </summary>
        public bool HasColorReflective {
            get {
                return HasProperty(AiMatKeys.COLOR_REFLECTIVE);
            }
        }

        /// <summary>
        /// Gets the color reflective. Default value is black.
        /// </summary>
        public Color4D ColorReflective {
            get {
                MaterialProperty prop = GetProperty(AiMatKeys.COLOR_REFLECTIVE);
                if(prop != null) {
                    return prop.AsColor4D();
                }
                return new Color4D(0, 0, 0, 1.0f);
            }
        }

        /// <summary>
        /// Constructs a new Material.
        /// </summary>
        /// <param name="material">Unmanaged AiMaterial struct.</param>
        internal Material(AiMaterial material) {
            _properties = new Dictionary<String, MaterialProperty>();
            _textures = new Dictionary<int, List<TextureSlot>>();

            if(material.NumProperties > 0 && material.Properties != IntPtr.Zero) {
                AiMaterialProperty[] properties = MemoryHelper.MarshalArray<AiMaterialProperty>(material.Properties, (int) material.NumProperties, true);
                for(int i = 0; i < properties.Length; i++) {
                    MaterialProperty prop = new MaterialProperty(properties[i]);
                    _properties.Add(prop.FullyQualifiedName, prop);
                }
            }
            //Idea is to look at each texture type, and get the "TextureSlot" struct of each one. They're essentially stored in a dictionary where each type contains a bucket
            //of textures. It seems just looping over properties will yield duplicates (no idea what the non $tex.file properties are, but they all seem to contain the same texture info).
            //So hopefully doing it this way will give a nice and concise list of textures that can easily be retrieved, and all pertinent info (file path, wrap mode, etc) will be available to
            //the user.
            foreach(var texType in Enum.GetValues(typeof(TextureType))) {
                TextureType type = (TextureType) texType;
                if(type != TextureType.None) {
                    uint count = AssimpMethods.GetMaterialTextureCount(ref material, type);
                    for(uint i = 0; i < count; i++) {
                        List<TextureSlot> slots;
                        if(!_textures.TryGetValue((int) type, out slots)) {
                            slots = new List<TextureSlot>();
                            _textures.Add((int) type, slots);
                        }
                        slots.Add(AssimpMethods.GetMaterialTexture(ref material, type, i));
                    }
                }
            }
        }

        /// <summary>
        /// Helper method to construct a fully qualified name from the input parameters. All the input parameters are combined into the fully qualified name: {baseName},{texType},{texIndex}. E.g.
        /// "$clr.diffuse,0,0" or "$tex.file,1,0". This is the name that is used as the material dictionary key.
        /// </summary>
        /// <param name="baseName">Key basename, this must not be null or empty</param>
        /// <param name="texType">Texture type; non-texture properties should leave this <see cref="TextureType.None"/></param>
        /// <param name="texIndex">Texture index; non-texture properties should leave this zero.</param>
        /// <returns>The fully qualified name</returns>
        public static String CreateFullyQualifiedName(String baseName, TextureType texType, int texIndex) {
            if(String.IsNullOrEmpty(baseName)) {
                return null;
            }
            return String.Format("{0},{1},{2}", baseName, 0, 0);
        }

        /// <summary>
        /// Gets the non-texture properties contained in this Material. The name should be
        /// the "base name", as in it should not contain texture type/texture index information. E.g. "$clr.diffuse" rather than "$clr.diffuse,0,0". The extra
        /// data will be filled in automatically.
        /// </summary>
        /// <param name="baseName">Key basename</param>
        /// <returns>The material property, if it exists</returns>
        public MaterialProperty GetNonTextureProperty(String baseName) {
            if(String.IsNullOrEmpty(baseName)) {
                return null;
            }
            String fullyQualifiedName = CreateFullyQualifiedName(baseName, TextureType.None, 0);
            return GetProperty(fullyQualifiedName);
        }

        /// <summary>
        /// Gets the material property. All the input parameters are combined into the fully qualified name: {baseName},{texType},{texIndex}. E.g.
        /// "$clr.diffuse,0,0" or "$tex.file,1,0".
        /// </summary>
        /// <param name="baseName">Key basename</param>
        /// <param name="texType">Texture type; non-texture properties should leave this <see cref="TextureType.None"/></param>
        /// <param name="texIndex">Texture index; non-texture properties should leave this zero.</param>
        /// <returns>The material property, if it exists</returns>
        public MaterialProperty GetProperty(String baseName, TextureType texType, int texIndex) {
            if(String.IsNullOrEmpty(baseName)) {
                return null;
            }
            String fullyQualifiedName = CreateFullyQualifiedName(baseName, texType, texIndex);
            return GetProperty(fullyQualifiedName);
        }

        /// <summary>
        /// Gets the material property by its fully qualified name. The format is: {baseName},{texType},{texIndex}. E.g.
        /// "$clr.diffuse,0,0" or "$tex.file,1,0".
        /// </summary>
        /// <param name="fullyQualifiedName">Fully qualified name of the property</param>
        /// <returns>The material property, if it exists</returns>
        public MaterialProperty GetProperty(String fullyQualifiedName) {
            if(String.IsNullOrEmpty(fullyQualifiedName)) {
                return null;
            }
            MaterialProperty prop;
            if(!_properties.TryGetValue(fullyQualifiedName, out prop)) {
                return null;
            }
            return prop;
        }

        /// <summary>
        /// Checks if the material has the specified non-texture property. The name should be
        /// the "base name", as in it should not contain texture type/texture index information. E.g. "$clr.diffuse" rather than "$clr.diffuse,0,0". The extra
        /// data will be filled in automatically.
        /// </summary>
        /// <param name="baseName"></param>
        /// <returns></returns>
        public bool HasNonTextureProperty(String baseName) {
            if(String.IsNullOrEmpty(baseName)) {
                return false;
            }
            String fullyQualifiedName = CreateFullyQualifiedName(baseName, TextureType.None, 0);
            return HasProperty(fullyQualifiedName);
        }

        /// <summary>
        /// Checks if the material has the specified property. All the input parameters are combined into the fully qualified name: {baseName},{texType},{texIndex}. E.g.
        /// "$clr.diffuse,0,0" or "$tex.file,1,0".
        /// </summary>
        /// <param name="baseName">Key basename</param>
        /// <param name="texType">Texture type; non-texture properties should leave this <see cref="TextureType.None"/></param>
        /// <param name="texIndex">Texture index; non-texture properties should leave this zero.</param>
        /// <returns>True if the property exists, false otherwise.</returns>
        public bool HasProperty(String baseName, TextureType texType, int texIndex) {
            if(String.IsNullOrEmpty(baseName)) {
                return false;
            }
            String fullyQualifiedName = CreateFullyQualifiedName(baseName, texType, texIndex);
            return HasProperty(fullyQualifiedName);
        }

        /// <summary>
        /// Checks if the material has the specified property by looking up its fully qualified name. The format is: {baseName},{texType},{texIndex}. E.g.
        /// "$clr.diffuse,0,0" or "$tex.file,1,0".
        /// </summary>
        /// <param name="fullyQualifiedName">Fully qualified name of the property</param>
        /// <returns>True if the property exists, false otherwise.</returns>
        public bool HasProperty(String fullyQualifiedName) {
            if(String.IsNullOrEmpty(fullyQualifiedName)) {
                return false;
            }
            return _properties.ContainsKey(fullyQualifiedName);
        }

        /// <summary>
        /// Gets -all- properties contained in the Material.
        /// </summary>
        /// <returns>All properties in the material property map.</returns>
        public MaterialProperty[] GetAllProperties()
        {
            var localProperties = new MaterialProperty[_properties.Count];
            int i = 0; 
            foreach (var materialProperty in _properties.Values)
            {
                localProperties[i] = materialProperty;
                i++;
            }
            return localProperties;
        }

        /// <summary>
        /// Gets all the textures that are of the specified texture type.
        /// </summary>
        /// <param name="texType">Texture type</param>
        /// <returns>Texture count</returns>
        public int GetTextureCount(TextureType texType) {
            if(texType != TextureType.None) {
                List<TextureSlot> slot;
                if(_textures.TryGetValue((int) texType, out slot)) {
                    return slot.Count;
                }
            }
            return 0;
        }

        /// <summary>
        /// Gets the specific texture information for the texture type and texture index.
        /// </summary>
        /// <param name="texType">Texture type</param>
        /// <param name="index">Texture index</param>
        /// <returns>Texture information struct</returns>
        public TextureSlot GetTexture(TextureType texType, int index) {
            TextureSlot texSlot = new TextureSlot();
            if(texType != TextureType.None) {
                List<TextureSlot> slotList;
                if(_textures.TryGetValue((int) texType, out slotList)) {
                    //Note: Unsure if the textures will be in the proper index order in our list, so to play it safe we're looking at all of them until
                    //we find the index
                    foreach(TextureSlot slot in slotList) {
                        if(slot.TextureIndex == index) {
                            return slot;
                        }
                    }
                }
            }
            return texSlot;
        }

        /// <summary>
        /// Gets all texture infos for the specific texture type.
        /// </summary>
        /// <param name="texType">Texture type</param>
        /// <returns>All textures that correspond to the texture type.</returns>
        public TextureSlot[] GetTextures(TextureType texType) {
            if(texType != TextureType.None) {
                List<TextureSlot> slotList;
                if(_textures.TryGetValue((int) texType, out slotList)) {
                    return slotList.ToArray();
                }
            }
            return null;
        }

        /// <summary>
        /// Get all textures contained in this material.
        /// </summary>
        /// <returns>All texture information structs</returns>
        public TextureSlot[] GetAllTextures() {
            List<TextureSlot> textures = new List<TextureSlot>();
            foreach(KeyValuePair<int, List<TextureSlot>> kv in _textures) {
                textures.AddRange(kv.Value);
            }
            return textures.ToArray();
        }
    }
}
