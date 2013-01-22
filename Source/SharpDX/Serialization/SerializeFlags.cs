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


using System;

namespace SharpDX.Serialization
{
    /// <summary>
    /// Flags used when serializing a value with a <see cref="BinarySerializer"/>.
    /// </summary>
    [Flags]
    public enum SerializeFlags
    {
        /// <summary>
        /// Normal serialize (not dynamic, not nullable).
        /// </summary>
        Normal = 0,

        /// <summary>
        /// Serialize a value as a dynamic value (the output stream will contain a magic-code for each encoded object). 
        /// </summary>
        Dynamic = 1,

        /// <summary>
        /// Serialize a value that can be null.
        /// </summary>
        Nullable = 2,
    }
}