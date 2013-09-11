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

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// Describes a stack of <see cref="MaterialTexture"/>.
    /// </summary>
    public class MaterialTextureStack : List<MaterialTexture>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialTextureStack"/> class.
        /// </summary>
        public MaterialTextureStack()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Collections.Generic.List`1" /> class that is empty and has the specified initial capacity.
        /// </summary>
        /// <param name="capacity">The number of elements that the new list can initially store.</param>
        public MaterialTextureStack(int capacity)
            : base(capacity)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialTextureStack"/> class.
        /// </summary>
        /// <param name="collection">The collection.</param>
        public MaterialTextureStack(IEnumerable<MaterialTexture> collection)
            : base(collection)
        {
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns>A new instance of MaterialTextureStack.</returns>
        public virtual MaterialTextureStack Clone()
        {
            var materialStack = (MaterialTextureStack)MemberwiseClone();
            for (int i = 0; i < this.Count; i++)
            {
                materialStack[i] = this[i].Clone();
            }
            return materialStack;
        }
    }
}