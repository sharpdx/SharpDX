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

using System.Collections.Generic;

using SharpDX.Serialization;

namespace SharpDX.Toolkit.Graphics
{
    public sealed partial class ModelData
    {
        /// <summary>
        /// Class Mesh
        /// </summary>
        public sealed class TextureSlot : IDataSerializable
        {
            /// <summary>
            /// The file path
            /// </summary>
            public string FilePath;

            /// <summary>
            /// The type of this texture.
            /// </summary>
            public TextureType Type;

            /// <summary>
            /// The index of this texture.
            /// </summary>
            public int Index;

            /// <summary>
            /// The UV index.
            /// </summary>
            public int UVIndex;

            /// <summary>
            /// The blend factor
            /// </summary>
            public float BlendFactor;

            /// <summary>
            /// The texture operation.
            /// </summary>
            public TextureOperation Operation;

            /// <summary>
            /// The wrap mode
            /// </summary>
            public TextureWrapMode WrapMode;

            public void Serialize(BinarySerializer serializer)
            {
                serializer.Serialize(ref FilePath);
                serializer.SerializeEnum(ref Type);
                serializer.Serialize(ref Index);
                serializer.Serialize(ref UVIndex);
                serializer.Serialize(ref BlendFactor);
                serializer.SerializeEnum(ref Operation);
                serializer.SerializeEnum(ref WrapMode);
            }
        }

        public enum TextureType : uint
        {
            None,
            Diffuse,
            Specular,
            Ambient,
            Emissive,
            Height,
            Normals,
            Shininess,
            Opacity,
            Displacement,
            Lightmap,
            Reflection,
            Unknown,
        }

        public enum TextureOperation
        {
            Multiply,
            Add,
            Subtract,
            Divide,
            SmoothAdd,
            SignedAdd,
        }

        public enum TextureWrapMode
        {
            Wrap,
            Clamp,
            Mirror,
            Decal,
        }
    }
}