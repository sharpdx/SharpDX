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
using SharpDX.Toolkit.Serialization;

namespace SharpDX.Toolkit.Graphics
{

    public partial class ModelData
    {
        /// <summary>
        /// Class Mesh
        /// </summary>
        public sealed class Material : CommonData, IDataSerializable
        {
            public Material()
            {
                Textures = new Dictionary<string, List<MaterialTexture>>();
                Properties = new PropertyCollection();
            }

            /// <summary>
            /// The textures
            /// </summary>
            public Dictionary<string, List<MaterialTexture>> Textures;

            /// <summary>
            /// Gets attributes attached to this material.
            /// </summary>
            public PropertyCollection Properties;

            void IDataSerializable.Serialize(BinarySerializer serializer)
            {
                if (serializer.Mode == SerializerMode.Write)
                {
                    serializer.Writer.Write(Textures.Count);
                    foreach (var texture in Textures)
                    {
                        var name = texture.Key;
                        var list = texture.Value;
                        serializer.Serialize(ref name);
                        serializer.Serialize(ref list);
                    }
                }
                else
                {
                    var count = serializer.Reader.ReadInt32();
                    Textures = new Dictionary<string, List<MaterialTexture>>(count);
                    for(int i = 0; i < count; i++)
                    {
                        string name = null;
                        List<MaterialTexture> list = null;
                        serializer.Serialize(ref name);
                        serializer.Serialize(ref list);
                        Textures.Add(name, list);
                    }
                }

                serializer.Serialize(ref Properties);
            }
        }
    }
}