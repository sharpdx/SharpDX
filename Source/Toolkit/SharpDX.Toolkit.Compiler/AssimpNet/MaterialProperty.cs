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
using System.Runtime.InteropServices;
using Assimp.Unmanaged;

namespace Assimp {
    /// <summary>
    /// A key-value pairing that represents some material property.
    /// </summary>
    internal sealed class MaterialProperty {
        private String _name;
        private PropertyType _type;
        private byte[] _value;
        private TextureType _texType;
        private int _texIndex;
        private String _stringValue;
        private String _fullyQualifiedName;

        /// <summary>
        /// Gets the property key name. E.g. $tex.file. This corresponds to the
        /// "AiMatKeys" base name constants.
        /// </summary>
        public String Name {
            get {
                return _name;
            }
        }

        /// <summary>
        /// Gets the type of property.
        /// </summary>
        public PropertyType PropertyType {
            get {
                return _type;
            }
        }

        /// <summary>
        /// Gets the raw byte data count.
        /// </summary>
        public int ByteCount {
            get {
                return (_value == null) ? 0 : _value.Length;
            }
        }

        /// <summary>
        /// Checks if the property has data.
        /// </summary>
        public bool HasRawData {
            get {
                return _value != null;
            }
        }

        /// <summary>
        /// Gets the raw byte data.
        /// </summary>
        public byte[] RawData {
            get {
                return _value;
            }
        }

        /// <summary>
        /// Gets the texture type semantic, for non-texture properties this is always <see cref="Assimp.TextureType.None"/>.
        /// </summary>
        public TextureType TextureType {
            get {
                return _texType;
            }
        }

        /// <summary>
        /// Gets the texture index, for non-texture properties this is always zero.
        /// </summary>
        public int TextureIndex {
            get {
                return _texIndex;
            }
        }

        /// <summary>
        /// Gets the property's fully qualified name. Format: "{base name},{texture type semantic},{texture index}". E.g. "$clr.diffuse,0,0". This
        /// is the key that is used to index the property in the material property map.
        /// </summary>
        public String FullyQualifiedName {
            get {
                return _fullyQualifiedName;
            }
        }

        /// <summary>
        /// Constructs a new MaterialProperty.
        /// </summary>
        /// <param name="property">Umananaged AiMaterialProperty struct</param>
        internal MaterialProperty(AiMaterialProperty property) {
            _name = property.Key.GetString();
            _type = property.Type;
            _texIndex = (int) property.Index;
            _texType = property.Semantic;
            
            if(property.DataLength > 0 && property.Data != IntPtr.Zero) {
                if (_type == Assimp.PropertyType.String) {
                    _stringValue = Marshal.PtrToStringAnsi(property.Data, (int)property.DataLength);
                } else {
                    _value = MemoryHelper.MarshalArray<byte>(property.Data, (int)property.DataLength);
                }
            }

            _fullyQualifiedName = String.Format("{0},{1},{2}", _name, ((uint)_texType).ToString(), _texIndex.ToString());
        }

        /// <summary>
        /// Returns the property raw data as a float.
        /// </summary>
        /// <returns>Float</returns>
        public float AsFloat() {
            if(_type == PropertyType.Float) {
                return BitConverter.ToSingle(_value, 0);
            }
            return 0;
        }

        /// <summary>
        /// Returns the property raw data as an integer.
        /// </summary>
        /// <returns>Integer</returns>
        public int AsInteger() {
            if(_type == PropertyType.Integer) {
                return BitConverter.ToInt32(_value, 0);
            }
            return 0;
        }

        /// <summary>
        /// Returns the property raw data as a string.
        /// </summary>
        /// <returns>String</returns>
        public String AsString() {
            if(_type == PropertyType.String) {
                return _stringValue;
            }
            return null;
        }

        /// <summary>
        /// Returns the property raw data as a float array.
        /// </summary>
        /// <returns>Float array</returns>
        public float[] AsFloatArray() {
            if(_type == Assimp.PropertyType.Float) {
                return ValueAsArray<float>();
            }
            return null;
        }

        /// <summary>
        /// Returns the property raw data as an integer array.
        /// </summary>
        /// <returns>Integer array</returns>
        public int[] AsIntegerArray() {
            if(_type == Assimp.PropertyType.Integer) {
                return ValueAsArray<int>();
            }
            return null;
        }

        /// <summary>
        /// Returns the property raw data as a boolean.
        /// </summary>
        /// <returns>Boolean</returns>
        public bool AsBoolean() {
            return (AsInteger() == 0) ? false : true;
        }

        /// <summary>
        /// Returns the property raw data as a Color3D.
        /// </summary>
        /// <returns>Color3D</returns>
        public Color3D AsColor3D() {
            if(_type == Assimp.PropertyType.Float) {
                return ValueAs<Color3D>();
            }
            return new Color3D();
        }

        /// <summary>
        /// Returns the property raw data as a Color4D.
        /// </summary>
        /// <returns>Color4D</returns>
        public Color4D AsColor4D() {
            if(_type == Assimp.PropertyType.Float) {
                return ValueAs<Color4D>();
            }
            return new Color4D();
        }

        /// <summary>
        /// Returns the property raw data as a ShadingMode enum value.
        /// </summary>
        /// <returns>Shading mode</returns>
        public ShadingMode AsShadingMode() {
            return (ShadingMode) AsInteger();
        }

        /// <summary>
        /// Returns the property raw data as a BlendMode enum value.
        /// </summary>
        /// <returns>Blend mode</returns>
        public BlendMode AsBlendMode() {
            return (BlendMode) AsInteger();
        }

        private unsafe T ValueAs<T>() where T : struct {
            T value = default(T);
            if(HasRawData) {
                try {
                    fixed(byte* ptr = _value) {
                        value = MemoryHelper.MarshalStructure<T>(new IntPtr(ptr));
                    }
                } catch(Exception) {

                }
            }
            return value;
        }

        //Probably shouldn't use for anything other than float/int types.
        private unsafe T[] ValueAsArray<T>() where T : struct {
            T[] value = null;
            if(HasRawData) {
                try {
                    int size = Marshal.SizeOf(typeof(T));
                    fixed(byte* ptr = _value) {
                        value = MemoryHelper.MarshalArray<T>(new IntPtr(ptr), ByteCount / size);
                    }
                } catch(Exception) {

                }
            }
            return value;
        }
    }
}
