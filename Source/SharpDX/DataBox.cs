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
using System.Runtime.InteropServices;

namespace SharpDX
{
    /// <summary>
    /// Provides access to data organized in 3D.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct DataBox
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataBox"/> struct.
        /// </summary>
        /// <param name="datapointer">The datapointer.</param>
        /// <param name="rowPitch">The row pitch.</param>
        /// <param name="slicePitch">The slice pitch.</param>
        public DataBox(IntPtr datapointer, int rowPitch, int slicePitch)
        {
            DataPointer = datapointer;
            RowPitch = rowPitch;
            SlicePitch = slicePitch;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataBox"/> struct.
        /// </summary>
        /// <param name="dataPointer">The data pointer.</param>
        public DataBox(IntPtr dataPointer)
        {
            DataPointer = dataPointer;
            RowPitch = 0;
            SlicePitch = 0;
        }

        /// <summary>
        /// Pointer to the data.
        /// </summary>
        public IntPtr DataPointer;

        /// <summary>
        /// Gets the number of bytes per row.
        /// </summary>
        public int RowPitch;

        /// <summary>
        /// Gets the number of bytes per slice (for a 3D texture, a slice is a 2D image)
        /// </summary>
        public int SlicePitch;

        /// <summary>
        /// Gets a value indicating whether this instance is empty.
        /// </summary>
        /// <value><c>true</c> if this instance is empty; otherwise, <c>false</c>.</value>
        public bool IsEmpty
        {
            get
            {
                return DataPointer == IntPtr.Zero && RowPitch == 0 && SlicePitch == 0;
            }
        }
    }
}