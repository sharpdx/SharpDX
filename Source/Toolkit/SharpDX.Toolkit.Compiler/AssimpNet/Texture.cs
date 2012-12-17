/*
* Copyright (c) 2012 Nicholas Woodfield
* 
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
* 
* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
* 
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
* THE SOFTWARE.
*/

using System;
using Assimp.Unmanaged;

namespace Assimp {
    /// <summary>
    /// Represents an embedded texture. Some file formats directly embed texture assets.
    /// Embedded textures may be uncompressed, where the data is given in an uncompressed format.
    /// Or it may be compressed in a format like png or jpg. In the latter case, the raw
    /// file bytes are given so the application must utilize an image decoder (e.g. DevIL) to
    /// get access to the actual color data.
    /// </summary>
    internal abstract class Texture {

        /// <summary>
        /// Gets if the texture is compressed or not.
        /// </summary>
        public abstract bool IsCompressed {
            get;
        }

        /// <summary>
        /// Creates a new texture based on the unmanaged struct. A height of zero
        /// indicates a compressed texture.
        /// </summary>
        /// <param name="texture">Unmanaged AiTexture struct</param>
        /// <returns>The embededded texture</returns>
        internal static Texture CreateTexture(AiTexture texture) {
            if(texture.Height == 0) {
                return new CompressedTexture(texture);
            } else {
                return new UncompressedTexture(texture);
            }
        }
    }

    /// <summary>
    /// Represents a compressed embedded texture. See <see cref="Texture"/> for a complete
    /// description.
    /// </summary>
    internal sealed class CompressedTexture : Texture {
        private byte[] _data;
        private String _formatHint;

        /// <summary>
        /// Gets if the texture data is present - this should always be true.
        /// </summary>
        public bool HasData {
            get {
                return _data != null;
            }
        }

        /// <summary>
        /// Gets the number of bytes in the buffer.
        /// </summary>
        public int ByteCount {
            get {
                return (_data == null) ? 0 : _data.Length;
            }
        }

        /// <summary>
        /// Gets the raw byte data representing the compressed texture.
        /// </summary>
        public byte[] Data {
            get {
                return _data;
            }
        }

        /// <summary>
        /// Gets the format hint to determine the type of compressed data. This hint
        /// will always be a three-character hint like "dds", "jpg", "png".
        /// </summary>
        public String FormatHint {
            get {
                return _formatHint;
            }
        }

        /// <summary>
        /// Gets if the texture is compressed or not.
        /// </summary>
        public override bool IsCompressed {
            get {
                return true;
            }
        }

        internal CompressedTexture(AiTexture texture) {
            _formatHint = texture.FormatHint;

            if(texture.Width > 0 && texture.Data != IntPtr.Zero) {
                _data = MemoryHelper.MarshalArray<byte>(texture.Data, (int) texture.Width);
            }
        }
    }

    /// <summary>
    /// Represents an uncompressed embedded texture. See <see cref="Texture"/> for a complete
    /// description.
    /// </summary>
    internal sealed class UncompressedTexture : Texture {
        private int _width;
        private int _height;
        private Texel[] _data;

        /// <summary>
        /// Gets the width of the texture in pixels.
        /// </summary>
        public int Width {
            get {
                return _width;
            }
        }

        /// <summary>
        /// Gets the height of the texture in pixels.
        /// </summary>
        public int Height {
            get {
                return _height;
            }
        }

        /// <summary>
        /// Gets if the texel data is present - should always be true.
        /// </summary>
        public bool HasData {
            get {
                return _data != null;
            }
        }

        /// <summary>
        /// Gets the texel data, the array is of size Width * Height.
        /// </summary>
        public Texel[] Data {
            get {
                return _data;
            }
        }

        /// <summary>
        /// Gets if the texture is compressed or not.
        /// </summary>
        public override bool IsCompressed {
            get {
                return false;
            }
        }

        /// <summary>
        /// Constructs a new UnCompressedTexture.
        /// </summary>
        /// <param name="texture">Unmanaged AiTexture struct.</param>
        internal UncompressedTexture(AiTexture texture) {
            _width = (int) texture.Width;
            _height = (int) texture.Height;

            int size = _width * _height;

            if(size > 0 && texture.Data != IntPtr.Zero) {
                _data = MemoryHelper.MarshalArray<Texel>(texture.Data, size);
            }
        }
    }
}
