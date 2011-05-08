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
    ///   A DataRectangle provides supporting information for a <see cref = "T:SharpDX.DataStream" /> whose
    ///   data is organized within two dimensions (a rectangle).
    /// </summary>
    /// <unmanaged>None</unmanaged>
    public class DataRectangle
    {
        private DataStream _data;
        private int _pitch;

        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SharpDX.DataRectangle" /> class.
        /// </summary>
        /// <param name = "pitch">The row pitch, in bytes.</param>
        /// <param name = "data">The data.</param>
        public DataRectangle(int pitch, DataStream data)
        {
            System.Diagnostics.Debug.Assert(data != null);
            System.Diagnostics.Debug.Assert(pitch >= 0);
            this._pitch = pitch;
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
        public int Pitch
        {
            get { return this._pitch; }
            set
            {
                System.Diagnostics.Debug.Assert(value > 0);
                this._pitch = value;
            }
        }
    }
}