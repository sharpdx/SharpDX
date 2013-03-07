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

using System;

using SharpDX.Serialization;

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// A Name describing a material attribute.
    /// </summary>
    public class MaterialKey : IDataSerializable, IEquatable<MaterialKey>
    {
        private string key;
        private MaterialTextureType type;
        private int index;
        private int hashCode;

        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialKey"/> class.
        /// </summary>
        public MaterialKey()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialKey"/> class.
        /// </summary>
        /// <param name="fullName">The full name.</param>
        /// <exception cref="System.ArgumentNullException">fullName</exception>
        /// <exception cref="System.ArgumentException">Invalid format for fullName: must be \xxx,0,0\;fullName</exception>
        public MaterialKey(string fullName)
        {
            if (string.IsNullOrEmpty(fullName))
                throw new ArgumentNullException("fullName");

            var values = fullName.Split(',');
            if (values.Length != 3)
                throw new ArgumentException("Invalid format for fullName: must be \"xxx,0,0\"", "fullName");

            key = values[0];
            type = (MaterialTextureType)int.Parse(values[1]);
            index = int.Parse(values[2]);

            CalculateHashcode();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialKey"/> class.
        /// </summary>
        /// <param name="key">The Name.</param>
        /// <param name="type">The type.</param>
        /// <param name="index">The index.</param>
        public MaterialKey(string key, MaterialTextureType type, int index)
        {
            this.key = key;
            this.type = type;
            this.index = index;

            CalculateHashcode();
        }

        /// <summary>
        /// Gets the Name.
        /// </summary>
        /// <value>The Name.</value>
        public string Key
        {
            get
            {
                return key;
            }
        }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>The type.</value>
        public MaterialTextureType Type
        {
            get
            {
                return type;
            }
        }

        /// <summary>
        /// Gets the index.
        /// </summary>
        /// <value>The index.</value>
        public int Index
        {
            get
            {
                return index;
            }
        }

        public bool Equals(MaterialKey other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return string.Equals(key, other.key) && type == other.type && index == other.index;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            var materialKey = obj as MaterialKey;
            if (materialKey == null)
            {
                return false;
            }
            return Equals(materialKey);
        }

        public override int GetHashCode()
        {
            return hashCode;
        }

        private void CalculateHashcode()
        {
            unchecked
            {
                hashCode = key.GetHashCode();
                hashCode = (hashCode * 397) ^ (int)type;
                hashCode = (hashCode * 397) ^ index;
            }
        }

        public static bool operator ==(MaterialKey left, MaterialKey right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(MaterialKey left, MaterialKey right)
        {
            return !Equals(left, right);
        }

        void IDataSerializable.Serialize(BinarySerializer serializer)
        {
            serializer.Serialize(ref key);

            if (serializer.Mode == SerializerMode.Write)
            {
                serializer.Writer.Write((byte)type);
                serializer.Writer.Write((byte)index);
            }
            else
            {
                type = (MaterialTextureType)serializer.Reader.ReadByte();
                index = serializer.Reader.ReadByte();
                CalculateHashcode();
            }
        }

        public override string ToString()
        {
            return string.Format("{0},{1},{2}", key, (byte)type, (byte)index);
        }
    }

    public class MaterialKey<T> : MaterialKey
    {
        public MaterialKey(string key, MaterialTextureType type, int index) : base(key, type, index)
        {
        }
    }

    public class MaterialKeys
    {
        public readonly static MaterialKey<string> Name = new MaterialKey<string>("?mat.name", 0, 0);
        public readonly static MaterialKey<bool> TwoSided = new MaterialKey<bool>("$mat.twosided", 0, 0);
        public readonly static MaterialKey<MaterialShadingMode> ShadingMode = new MaterialKey<MaterialShadingMode>("$mat.shadingm", 0, 0);
        public readonly static MaterialKey<bool> Wireframe = new MaterialKey<bool>("$mat.wireframe", 0, 0);
        public readonly static MaterialKey<MaterialBlendMode> BlendMode = new MaterialKey<MaterialBlendMode>("$mat.blend", 0, 0);
        public readonly static MaterialKey<float> Opacity = new MaterialKey<float>("$mat.opacity", 0, 0);
        public readonly static MaterialKey<float> BumpScaling = new MaterialKey<float>("$mat.bumpscaling", 0, 0);
        public readonly static MaterialKey<float> Shininess = new MaterialKey<float>("$mat.shininess", 0, 0);
        public readonly static MaterialKey<float> Reflectivity = new MaterialKey<float>("$mat.reflectivity", 0, 0);
        public readonly static MaterialKey<float> ShininessStrength = new MaterialKey<float>("$mat.shinpercent", 0, 0);
        public readonly static MaterialKey<float> Refractivity = new MaterialKey<float>("$mat.refracti", 0, 0);
        public readonly static MaterialKey<Color4> ColorDiffuse = new MaterialKey<Color4>("$clr.diffuse", 0, 0);
        public readonly static MaterialKey<Color4> ColorAmbient = new MaterialKey<Color4>("$clr.ambient", 0, 0);
        public readonly static MaterialKey<Color4> ColorSpecular = new MaterialKey<Color4>("$clr.specular", 0, 0);
        public readonly static MaterialKey<Color4> ColorEmissive = new MaterialKey<Color4>("$clr.emissive", 0, 0);
        public readonly static MaterialKey<Color4> ColorTransparent = new MaterialKey<Color4>("$clr.transparent", 0, 0);
        public readonly static MaterialKey<Color4> ColorReflective = new MaterialKey<Color4>("$clr.reflective", 0, 0);
        public readonly static MaterialKey GlobalBacgroundImage = new MaterialKey("?bg.global", 0, 0);
    }

    public enum MaterialBlendMode
    {
        Default,

        Additive,
    }
}