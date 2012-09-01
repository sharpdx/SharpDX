// Copyright (c) 2010-2012 SharpDX - Alexandre Mutel
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
// THE SOFTWARE.using System;

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using SharpDX.Serialization;

namespace SharpDX
{
    /// <summary>
    /// Direct2D Matrix 3x2. Use <see cref="SharpDX.Matrix"/> and implicit cast to <see cref="Matrix3x2"/>.
    /// </summary>
#if !WIN8METRO
    [Serializable]
#endif
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct Matrix3x2 : IDataSerializable
    {
        /// <summary>
        /// Element (1,1)
        /// </summary>
        public float M11;

        /// <summary>
        /// Element (1,2)
        /// </summary>
        public float M12;

        /// <summary>
        /// Element (2,1)
        /// </summary>
        public float M21;

        /// <summary>
        /// Element (2,2)
        /// </summary>
        public float M22;

        /// <summary>
        /// Element (3,1)
        /// </summary>
        public float M31;

        /// <summary>
        /// Element (3,2)
        /// </summary>
        public float M32;

        /// <summary>
        /// Performs an implicit conversion from <see cref="SharpDX.Matrix"/> to <see cref="SharpDX.Matrix3x2"/>.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Matrix3x2(Matrix matrix)
        {
            return new Matrix3x2
                                   {
                                       M11 = matrix.M11,
                                       M12 = matrix.M12,
                                       M21 = matrix.M21,
                                       M22 = matrix.M22,
                                       M31 = matrix.M41,
                                       M32 = matrix.M42
                                   };
        }

        /// <summary>
        /// Gets the identity matrix.
        /// </summary>
        /// <value>The identity matrix.</value>
        public static Matrix3x2 Identity
        {
            get
            {
                return new Matrix3x2 {M11 = 1f, M22 = 1f};
            }
        }

        /// <inheritdoc/>
        void IDataSerializable.Serialize(BinarySerializer serializer)
        {
            // Write optimized version without using Serialize methods
            if (serializer.Mode == SerializerMode.Write)
            {
                serializer.Writer.Write(M11);
                serializer.Writer.Write(M12);
                serializer.Writer.Write(M21);
                serializer.Writer.Write(M22);
                serializer.Writer.Write(M31);
                serializer.Writer.Write(M32);
            }
            else
            {
                M11 = serializer.Reader.ReadSingle();
                M12 = serializer.Reader.ReadSingle();
                M21 = serializer.Reader.ReadSingle();
                M22 = serializer.Reader.ReadSingle();
                M31 = serializer.Reader.ReadSingle();
                M32 = serializer.Reader.ReadSingle();
            }
        }
    }
 
}
