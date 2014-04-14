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

namespace SharpDX.XACT3
{
    public partial class SoundProperties
    {
        public TrackProperties[] TrackProperties;

        // Internal native struct used for marshalling
        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        internal partial struct __Native
        {
            public short Category;
            public byte Priority;
            public short Pitch;
            public float Volume;
            public short NumTracks;
            public SharpDX.XACT3.TrackProperties TrackPropertiesPointer;
            // Method to free unmanaged allocation
            internal unsafe void __MarshalFree()
            {
            }
        }

        // Method to free unmanaged allocation
        internal unsafe void __MarshalFree(ref __Native @ref)
        {
            @ref.__MarshalFree();
        }

        // Method to marshal from native to managed struct
        internal unsafe void __MarshalFrom(ref __Native @ref)
        {
            this.Category = @ref.Category;
            this.Priority = @ref.Priority;
            this.Pitch = @ref.Pitch;
            this.Volume = @ref.Volume;
            this.TrackProperties = new TrackProperties[@ref.NumTracks];

            // if there are no tracks - don't read the properties, otherwise a IndexOutOfRangeException is thrown.
            if (@ref.NumTracks > 0)
            {
                fixed (void* ptr = &@ref.TrackPropertiesPointer)
                    Utilities.Read((IntPtr)ptr, this.TrackProperties, 0, @ref.NumTracks);
            }
        }
        // Method to marshal from managed struct to native
        internal unsafe void __MarshalTo(ref __Native @ref)
        {
            throw new NotImplementedException();
        }
    }
}