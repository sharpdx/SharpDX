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
        public sealed class Material : IDataSerializable
        {
            public Material()
            {
                TextureSlots = new List<TextureSlot>();
                Attributes = new List<AttributeData>();
            }

            /// <summary>
            /// The name of this material.
            /// </summary>
            public string Name;

            /// <summary>
            /// The textures
            /// </summary>
            public List<TextureSlot> TextureSlots;

            /// <summary>
            /// Gets attributes attached to this material.
            /// </summary>
            public List<AttributeData> Attributes;

            public void Serialize(BinarySerializer serializer)
            {
                serializer.Serialize(ref Name);
                serializer.Serialize(ref TextureSlots);
                serializer.Serialize(ref Attributes);
            }
        }
    }
}