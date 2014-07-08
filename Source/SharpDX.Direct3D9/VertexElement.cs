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

namespace SharpDX.Direct3D9
{
    public partial struct VertexElement
    {
        /// <summary>
        /// Used for closing a VertexElement declaration.
        /// </summary>
        public static readonly VertexElement VertexDeclarationEnd;

        /// <summary>
        /// Initializes the <see cref="VertexElement"/> struct.
        /// </summary>
        static VertexElement()
        {
            VertexDeclarationEnd = new VertexElement(0xff, 0, DeclarationType.Unused, DeclarationMethod.Default, DeclarationUsage.Position, 0);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VertexElement"/> struct.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="type">The type.</param>
        /// <param name="method">The method.</param>
        /// <param name="usage">The usage.</param>
        /// <param name="usageIndex">Index of the usage.</param>
        public VertexElement(short stream, short offset, DeclarationType type, DeclarationMethod method, DeclarationUsage usage, byte usageIndex)
        {
            Stream = stream;
            Offset = offset;
            Type = type;
            Method = method;
            Usage = usage;
            UsageIndex = usageIndex;
        }
    }
}
