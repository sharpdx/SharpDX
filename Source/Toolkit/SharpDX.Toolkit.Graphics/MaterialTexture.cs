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

using SharpDX.Direct3D11;

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// Describes a texture used by a material.
    /// </summary>
    public class MaterialTexture : ComponentBase
    {
        /// <summary>
        /// The Texture associated to this material.
        /// </summary>
        public Texture Texture;

        /// <summary>
        /// The type of this texture.
        /// </summary>
        public MaterialTextureType Type;

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
        /// The texture operation to perform on the stack.
        /// </summary>
        public MaterialTextureOperator Operation;

        /// <summary>
        /// The wrap mode
        /// </summary>
        public TextureAddressMode WrapMode;

        /// <summary>
        /// Texture flags.
        /// </summary>
        public MaterialTextureFlags Flags;

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns>A new MaterialTexture.</returns>
        public virtual MaterialTexture Clone()
        {
            return (MaterialTexture)MemberwiseClone();
        }
    }
}