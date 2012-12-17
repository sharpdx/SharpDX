using System;
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
using System.Runtime.InteropServices;

namespace Assimp {
    /// <summary>
    /// Represents a plane in three-dimensional euclidean space where
    /// A, B, C are components of the plane normal and D is the distance along the
    /// normal from the origin to the plane.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    internal struct Plane {

        /// <summary>
        /// X component of the normal vector.
        /// </summary>
        public float A;

        /// <summary>
        /// Y component of the normal vector.
        /// </summary>
        public float B;

        /// <summary>
        /// Z component of the normal vector.
        /// </summary>
        public float C;

        /// <summary>
        /// Distance from the origin to the plane along the normal vector.
        /// </summary>
        public float D;

        /// <summary>
        /// Constructs a new Plane.
        /// </summary>
        /// <param name="a">X component of the normal vector.</param>
        /// <param name="b">Y component of the normal vector.</param>
        /// <param name="c">Z component of the normal vector.</param>
        /// <param name="d">Distance from the origin to the plane along the normal vector.</param>
        public Plane(float a, float b, float c, float d) {
            A = a;
            B = b;
            C = c;
            D = d;
        }
    }
}
