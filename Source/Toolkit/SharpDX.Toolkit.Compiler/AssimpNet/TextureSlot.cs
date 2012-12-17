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

namespace Assimp {

    /// <summary>
    /// Describes all the values pertaining to a particular texture slot in a material.
    /// </summary>
    [Serializable]
    internal struct TextureSlot {
        private String _filePath;
        private TextureType _type;
        private uint _index;
        private TextureMapping _mapping;
        private uint _uvIndex;
        private float _blendFactor;
        private TextureOperation _texOp;
        private TextureWrapMode _wrapMode;
        private uint _flags;

        /// <summary>
        /// Gets the texture file path.
        /// </summary>
        public String FilePath {
            get {
                return _filePath;
            }
        }

        /// <summary>
        /// Gets the texture type semantic.
        /// </summary>
        public TextureType TextureType {
            get {
                return _type;
            }
        }

        /// <summary>
        /// Gets the texture index in the material.
        /// </summary>
        public uint TextureIndex {
            get {
                return _index;
            }
        }

        /// <summary>
        /// Gets the texture mapping.
        /// </summary>
        public TextureMapping Mapping {
            get {
                return _mapping;
            }
        }

        /// <summary>
        /// Gets the UV channel index that corresponds to this texture from the mesh.
        /// </summary>
        public uint UVIndex {
            get {
                return _uvIndex;
            }
        }

        /// <summary>
        /// Gets the blend factor.
        /// </summary>
        public float BlendFactor {
            get {
                return _blendFactor;
            }
        }

        /// <summary>
        /// Gets the texture operation.
        /// </summary>
        public TextureOperation Operation {
            get {
                return _texOp;
            }
        }

        /// <summary>
        /// Gets the texture wrap mode.
        /// </summary>
        public TextureWrapMode WrapMode {
            get {
                return _wrapMode;
            }
        }

        /// <summary>
        /// Gets misc flags.
        /// </summary>
        public uint Flags {
            get {
                return _flags;
            }
        }

        /// <summary>
        /// Constructs a new TextureSlot.
        /// </summary>
        /// <param name="filePath">Texture filepath</param>
        /// <param name="typeSemantic">Texture type semantic</param>
        /// <param name="texIndex">Texture index in the material</param>
        /// <param name="mapping">Texture mapping</param>
        /// <param name="uvIndex">UV channel in mesh that corresponds to this texture</param>
        /// <param name="blendFactor">Blend factor</param>
        /// <param name="texOp">Texture operation</param>
        /// <param name="wrapMode">Texture wrap mode</param>
        /// <param name="flags">Misc flags</param>
        public TextureSlot(String filePath, TextureType typeSemantic, uint texIndex, TextureMapping mapping, uint uvIndex, float blendFactor,
            TextureOperation texOp, TextureWrapMode wrapMode, uint flags) {
                _filePath = (filePath == null) ? String.Empty : filePath;
                _type = typeSemantic;
                _index = texIndex;
                _mapping = mapping;
                _uvIndex = uvIndex;
                _blendFactor = blendFactor;
                _texOp = texOp;
                _wrapMode = wrapMode;
                _flags = flags;
        }
    }
}
