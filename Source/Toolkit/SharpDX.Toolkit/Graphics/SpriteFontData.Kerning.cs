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
    public partial class SpriteFontData
    {
        /// <summary>
        /// Describes kerning information.
        /// </summary>
        public struct Kerning : IDataSerializable
        {
            /// <summary>
            /// Unicode for the 1st character.
            /// </summary>
            public int First;

            /// <summary>
            /// Unicode for the 2nd character.
            /// </summary>
            public int Second;

            /// <summary>
            /// X Offsets in pixels to apply between the 1st and 2nd character.
            /// </summary>
            public float Offset;

            public void Serialize(BinarySerializer serializer)
            {
                serializer.Serialize(ref First);
                serializer.Serialize(ref Second);
                serializer.Serialize(ref Offset);
            }
        }
    }
}