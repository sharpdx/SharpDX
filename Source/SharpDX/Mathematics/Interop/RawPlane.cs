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

using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SharpDX.Mathematics.Interop
{
    /// <summary>
    /// Interop type for a Plane (4 floats).
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    [DebuggerDisplay("Normal: {Normal}, D: {D}")]
    public struct RawPlane
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RawPlane"/> struct.
        /// </summary>
        /// <param name="normal">The plane normal.</param>
        /// <param name="d">The plance distance.</param>
        public RawPlane(RawVector3 normal, float d)
        {
            Normal = normal;
            D = d;
        }

        /// <summary>
        /// The normal vector of the plane.
        /// </summary>
        public RawVector3 Normal;

        /// <summary>
        /// The distance of the plane along its normal from the origin.
        /// </summary>
        public float D;
    }
}
