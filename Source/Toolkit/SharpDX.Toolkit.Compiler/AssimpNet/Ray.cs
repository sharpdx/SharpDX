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
using System.Runtime.InteropServices;

namespace Assimp {

    /// <summary>
    /// Defines a 3D ray with a point of origin and a direction.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    internal struct Ray {

        /// <summary>
        /// Origin of the ray in space.
        /// </summary>
        public Vector3D Position;

        /// <summary>
        /// Direction of the ray.
        /// </summary>
        public Vector3D Direction;

        /// <summary>
        /// Constructs a new Ray.
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="dir"></param>
        public Ray(Vector3D pos, Vector3D dir) {
            Position = pos;
            Direction = dir;
        }
    }
}
