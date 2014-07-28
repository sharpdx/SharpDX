// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
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

namespace SharpDX.DirectInput
{
    /// <summary>
    /// Flags for a data format.
    /// </summary>
    [Flags]
    public enum ObjectDataFormatFlags : int
    {
        /// <summary>
        /// The data format doesn't report any specific information.
        /// </summary>
        None = 0,
        /// <summary>
        /// The data format must report acceleration information.
        /// </summary>
        Acceleration = ObjectAspect.Acceleration,
        /// <summary>
        /// The data format must report force information.
        /// </summary>
        Force = ObjectAspect.Force,
        /// <summary>
        /// The data format must report position information.
        /// </summary>
        Position = ObjectAspect.Position,
        /// <summary>
        /// The data format must report velocity information.
        /// </summary>
        Velocity = ObjectAspect.Velocity   
    }
}