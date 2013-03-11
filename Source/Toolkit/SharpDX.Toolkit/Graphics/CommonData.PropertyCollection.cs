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
using System.Collections.Generic;

using SharpDX.Serialization;

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// Common data used by <see cref="EffectData"/>, <see cref="ModelData"/>.
    /// </summary>
    public partial class CommonData
    {
        public class PropertyCollection : Dictionary<string, object>, IDataSerializable
        {
            public void SetProperty<T>(string key, T value)
            {
                if (Utilities.IsEnum(typeof(T)))
                {
                    var intValue = Convert.ToInt32(value);
                    Add(key, intValue);
                }
                else
                {
                    Add(key, value);
                }
            }

            void IDataSerializable.Serialize(BinarySerializer serializer)
            {
                if (serializer.Mode == SerializerMode.Write)
                {
                    serializer.Writer.Write(Count);
                    foreach (var item in this)
                    {
                        var key = item.Key;
                        var value = item.Value;
                        serializer.Serialize(ref key);
                        serializer.SerializeDynamic(ref value, SerializeFlags.Nullable);
                    }
                }
                else
                {
                    var count = serializer.Reader.ReadInt32();
                    for (int i = 0; i < count; i++)
                    {
                        string name = null;
                        object value = null;
                        serializer.Serialize(ref name);
                        serializer.SerializeDynamic(ref value, SerializeFlags.Nullable);
                        Add(name, value);
                    }
                }
            }
        }
    }
}