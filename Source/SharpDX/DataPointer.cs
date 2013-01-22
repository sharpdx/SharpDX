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
using System.Runtime.InteropServices;

namespace SharpDX
{
    /// <summary>
    /// Pointer to a native buffer with a specific size.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct DataPointer : IEquatable<DataPointer>
    {
        /// <summary>
        /// Gets an Empty Data Pointer.
        /// </summary>
        public static readonly DataPointer Zero = new DataPointer(IntPtr.Zero, 0);

        /// <summary>
        /// Initializes a new instance of the <see cref="DataPointer" /> struct.
        /// </summary>
        /// <param name="pointer">The pointer.</param>
        /// <param name="size">The size.</param>
        public DataPointer(IntPtr pointer, int size)
        {
            Pointer = pointer;
            Size = size;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataPointer" /> struct.
        /// </summary>
        /// <param name="pointer">The pointer.</param>
        /// <param name="size">The size.</param>
        public unsafe DataPointer(void* pointer, int size)
        {
            Pointer = (IntPtr)pointer;
            Size = size;
        }

        /// <summary>
        /// Pointer to the buffer.
        /// </summary>
        public IntPtr Pointer;

        /// <summary>
        /// Size in bytes of the buffer.
        /// </summary>
        public int Size;

        public bool Equals(DataPointer other)
        {
            return Pointer.Equals(other.Pointer) && Size == other.Size;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is DataPointer && Equals((DataPointer) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Pointer.GetHashCode() * 397) ^ Size;
            }
        }

        /// <summary>
        /// Implements the ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(DataPointer left, DataPointer right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Implements the !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(DataPointer left, DataPointer right)
        {
            return !left.Equals(right);
        }
    }
}