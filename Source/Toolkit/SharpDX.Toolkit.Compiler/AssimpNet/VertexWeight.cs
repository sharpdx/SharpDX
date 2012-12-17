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
using System.Globalization;
using System.Runtime.InteropServices;

namespace Assimp {
    /// <summary>
    /// Represents a single influence of a bone on a vertex.
    /// </summary>
    [Serializable]
    [StructLayoutAttribute(LayoutKind.Sequential)]
    internal struct VertexWeight {
        /// <summary>
        /// Index of the vertex which is influenced by the bone.
        /// </summary>
        public uint VertexID;

        /// <summary>
        /// Strength of the influence in range of (0...1). All influences
        /// from all bones at one vertex amounts to 1.
        /// </summary>
        public float Weight;

        /// <summary>
        /// Constructs a new VertexWeight.
        /// </summary>
        /// <param name="vertID">Index of the vertex.</param>
        /// <param name="weight">Weight of the influence.</param>
        public VertexWeight(uint vertID, float weight) {
            VertexID = vertID;
            Weight = weight;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString() {
            CultureInfo info = CultureInfo.CurrentCulture;
            return String.Format(info, "{{VertexID:{0} Weight:{1}}}",
                new Object[] { VertexID.ToString(info), Weight.ToString(info) });
        }
    }
}
