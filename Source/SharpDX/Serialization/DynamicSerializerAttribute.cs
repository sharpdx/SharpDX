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
using SharpDX.Multimedia;

namespace SharpDX.Serialization
{
    /// <summary>
    /// Use this attribute to specify the id of a dynamic type with <see cref="BinarySerializer"/>.
    /// </summary>
    public class DynamicSerializerAttribute : Attribute
    {
        private readonly FourCC id;

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicSerializerAttribute" /> class.
        /// </summary>
        /// <param name="id">The id to register as a dynamic type.</param>
        public DynamicSerializerAttribute(int id)
        {
            this.id = id;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicSerializerAttribute" /> class.
        /// </summary>
        /// <param name="id">The id to register as a dynamic type.</param>
        public DynamicSerializerAttribute(string id)
        {
            this.id = id;
        }

        /// <summary>
        /// Gets the id.
        /// </summary>
        /// <value>The id.</value>
        public FourCC Id { get { return id; } }
    }
}