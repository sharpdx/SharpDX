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

using SharpDX.Serialization;

namespace SharpDX.Toolkit.Graphics
{
    public sealed partial class ModelData
    {
        /// <summary>
        /// Class Bone
        /// </summary>
        public struct Bone : IDataSerializable
        {
            /// <summary>
            /// The index of this bone into the bone collection of the model.
            /// </summary>
            public int Index;

            /// <summary>
            /// Gets parent bone index.
            /// </summary>
            public int ParentIndex;

            /// <summary>
            /// The transform this bone relative to its parent bone.
            /// </summary>
            public Matrix Transform;

            /// <summary>
            /// The name of this bone.
            /// </summary>
            public string Name;

            /// <summary>
            /// The children bone indices.
            /// </summary>
            public int[] Children;

            void IDataSerializable.Serialize(BinarySerializer serializer)
            {
                serializer.Serialize(ref Index);
                serializer.Serialize(ref ParentIndex);
                serializer.Serialize(ref Transform);
                serializer.Serialize(ref Name, false, SerializeFlags.Nullable);
                serializer.Serialize(ref Children, serializer.Serialize, SerializeFlags.Nullable);
            }
        }
    }
}