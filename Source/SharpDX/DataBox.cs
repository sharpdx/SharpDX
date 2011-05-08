// Copyright (c) 2010-2011 SharpDX - Alexandre Mutel
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
// -----------------------------------------------------------------------------
// Original code from SlimDX project.
// Greetings to SlimDX Group. Original code published with the following license:
// -----------------------------------------------------------------------------
/*
* Copyright (c) 2007-2011 SlimDX Group
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

namespace SharpDX
{
    /// <summary>
    ///   A DataBox provides supporting information for a <see cref = "T:SharpDX.DataStream" /> whose
    ///   data is organized within three dimensions (a box).
    /// </summary>
    /// <unmanaged>None</unmanaged>
    public class DataBox
    {
        private DataStream _data;
        private int rowPitch;
        private int slicePitch;

        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SharpDX.DataBox" /> class.
        /// </summary>
        /// <param name = "rowPitch">The row pitch, in bytes.</param>
        /// <param name = "slicePitch">The slice pitch, in bytes.</param>
        /// <param name = "data">The data.</param>
        public DataBox(int rowPitch, int slicePitch, DataStream data)
        {
            System.Diagnostics.Debug.Assert(data != null);
            System.Diagnostics.Debug.Assert(rowPitch >= 0);
            System.Diagnostics.Debug.Assert(slicePitch >= 0);

            this.rowPitch = rowPitch;
            this.slicePitch = slicePitch;
            this._data = data;
        }

        /// <summary>
        ///   Gets the <see cref = "T:SharpDX.DataStream" /> containing the actual data bytes.
        /// </summary>
        public DataStream Data
        {
            get { return this._data; }
        }

        /// <summary>
        ///   Gets or sets the number of bytes of data between two consecutive (1D) rows of data.
        /// </summary>
        public int RowPitch
        {
            get { return this.rowPitch; }
            set
            {
                System.Diagnostics.Debug.Assert(value >= 0);
                this.rowPitch = value;
            }
        }

        /// <summary>
        ///   Gets or sets the number of bytes of data between two consecutive (2D) slices of data.
        /// </summary>
        public int SlicePitch
        {
            get { return this.slicePitch; }
            set
            {
                System.Diagnostics.Debug.Assert(value >= 0);
                this.slicePitch = value;
            }
        }
    }
}