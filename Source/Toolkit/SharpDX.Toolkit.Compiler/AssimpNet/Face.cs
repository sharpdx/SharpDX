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
    /// A single face in a mesh, referring to multiple vertices. This can be a triangle
    /// if the index count is equal to three, or a polygon if the count is greater than three.
    /// 
    /// Since multiple primitive types can be contained in a single mesh, this approach
    /// allows you to better examine how the mesh is constructed. If you use the <see cref="PostProcessSteps.SortByPrimitiveType"/>
    /// post process step flag during import, then each mesh will be homogenous where primitive type is concerned.
    /// </summary>
    internal sealed class Face {
        private uint _numIndices;
        private uint[] _indices;

        /// <summary>
        /// Gets the number of indices defined in the face.
        /// </summary>
        public uint IndexCount {
            get {
                return _numIndices;
            }
        }

        /// <summary>
        /// Gets the indices that refer to positions of vertex data in the mesh's vertex 
        /// arrays.
        /// </summary>
        public uint[] Indices {
            get {
                return _indices;
            }
        }

        /// <summary>
        /// Constructs a new Face.
        /// </summary>
        /// <param name="face">Unmanaged AiFace structure</param>
        internal Face(AiFace face) {
            _numIndices = face.NumIndices;

            if(_numIndices > 0 && face.Indices != IntPtr.Zero) {
                _indices = MemoryHelper.MarshalArray<uint>(face.Indices, (int)_numIndices);
            }
        }
    }
}
