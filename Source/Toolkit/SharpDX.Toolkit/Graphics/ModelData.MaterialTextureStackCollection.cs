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
    public partial class ModelData
    {
        public class MaterialTextureStackCollection : Dictionary<MaterialTextureType, MaterialTextureStack>, IDataSerializable
        {
            public void Add(MaterialTexture texture)
            {
                MaterialTextureStack textures;
                if (!TryGetValue(texture.Type, out textures))
                {
                    Add(texture.Type, textures = new MaterialTextureStack());
                }
                textures.Add(texture);
            }

            void IDataSerializable.Serialize(BinarySerializer serializer)
            {
                if (serializer.Mode == SerializerMode.Write)
                {
                    serializer.Writer.Write(Count);
                    foreach (var value in this)
                    {
                        var localKey = value.Key;
                        var localValue = value.Value;
                        serializer.SerializeEnum(ref localKey);
                        serializer.Serialize(ref localValue);
                    }
                }
                else
                {
                    var count = serializer.Reader.ReadInt32();
                    for (int i = 0; i < count; i++)
                    {
                        var type = MaterialTextureType.None;
                        MaterialTextureStack localValue = null;
                        serializer.SerializeEnum(ref type);
                        serializer.Serialize(ref localValue);
                        Add(type, localValue);
                    }
                }
            }
        }
    }
}