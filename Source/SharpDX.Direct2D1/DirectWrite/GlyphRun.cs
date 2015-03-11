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
using SharpDX.Mathematics.Interop;

namespace SharpDX.DirectWrite
{
    public partial class GlyphRun : IDisposable
    {
        /// <summary>
        /// Gets or sets the <see cref="FontFace"/> associated with this GlypRun.
        /// </summary>
        /// <value>The font face.</value>
        public FontFace FontFace { get; set; }

        /// <summary>
        /// An array of glyph indices. This array contains <see cref="GlyphCount"/> elements.
        /// </summary>
        public short[] Indices { get; set; }

        /// <summary>
        /// An optional array of glyph advances. This array could be null or contains <see cref="GlyphCount"/> elements.
        /// </summary>
        public float[] Advances { get; set; }

        /// <summary>
        /// An optional array of glyph offsets. This array could be null or contains <see cref="GlyphCount"/> elements.
        /// </summary>
        public GlyphOffset[] Offsets { get; set; }

        // Internal native struct used for marshalling
        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        internal partial struct __Native
        {
            public IntPtr FontFace;
            public float FontEmSize;
            public int GlyphCount;
            public IntPtr GlyphIndices;
            public IntPtr GlyphAdvances;
            public IntPtr GlyphOffsets;
            public RawBool IsSideways;
            public int BidiLevel;
            // Method to free native struct
            internal unsafe void __MarshalFree()
            {
                if (GlyphIndices != IntPtr.Zero)
                    Marshal.FreeHGlobal(GlyphIndices);
                if (GlyphAdvances != IntPtr.Zero)
                    Marshal.FreeHGlobal(GlyphAdvances);
                if (GlyphOffsets != IntPtr.Zero)
                    Marshal.FreeHGlobal(GlyphOffsets);
            }
        }

        internal unsafe void __MarshalFree(ref __Native @ref)
        {
            @ref.__MarshalFree();
        }

        // Method to marshal from native to managed struct
        internal unsafe void __MarshalFrom(ref __Native @ref)
        {
            this.FontFace = (@ref.FontFace == IntPtr.Zero) ? null : new FontFace(@ref.FontFace);
            // If FontFace != null, adds a reference to it
            if (FontFace != null)
                ((IUnknown) this.FontFace).AddReference();
            this.FontSize= @ref.FontEmSize;
            this.GlyphCount = @ref.GlyphCount;
            if (@ref.GlyphIndices != IntPtr.Zero)
            {
                Indices = new short[GlyphCount];
                if (GlyphCount > 0)
                    Utilities.Read(@ref.GlyphIndices, Indices, 0, GlyphCount);
            }

            if (@ref.GlyphAdvances != IntPtr.Zero)
            {
                Advances = new float[GlyphCount];
                if (GlyphCount > 0)
                    Utilities.Read(@ref.GlyphAdvances, Advances, 0, GlyphCount);
            }

            if (@ref.GlyphOffsets != IntPtr.Zero)
            {
                Offsets = new GlyphOffset[GlyphCount];
                if (GlyphCount > 0)
                    Utilities.Read(@ref.GlyphOffsets, Offsets, 0, GlyphCount);
            }
            this.IsSideways = @ref.IsSideways;
            this.BidiLevel = @ref.BidiLevel;
        }
        // Method to marshal from managed struct tot native
        internal unsafe void __MarshalTo(ref __Native @ref)
        {
            @ref.FontFace = this.FontFace == null?IntPtr.Zero:this.FontFace.NativePointer;
            @ref.FontEmSize = this.FontSize;
            @ref.GlyphCount = -1;
            @ref.GlyphIndices = IntPtr.Zero;
            @ref.GlyphAdvances = IntPtr.Zero;
            @ref.GlyphOffsets = IntPtr.Zero;

            if (this.Indices != null)
            {
                @ref.GlyphCount = this.Indices.Length;
                @ref.GlyphIndices = Marshal.AllocHGlobal(this.Indices.Length*sizeof (short));
                if (this.Indices.Length > 0)
                    Utilities.Write(@ref.GlyphIndices, Indices, 0, this.Indices.Length);
            }

            if (this.Advances != null)
            {
                if (@ref.GlyphCount >= 0 && @ref.GlyphCount != this.Advances.Length)
                    throw new InvalidOperationException(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Invalid length for array Advances [{0}] and Indices [{1}]. Indices, Advances and Offsets array must have same size - or may be null", this.Advances.Length, @ref.GlyphCount));
                @ref.GlyphCount = this.Advances.Length;
                @ref.GlyphAdvances = Marshal.AllocHGlobal(this.Advances.Length * sizeof(float));
                if (this.Advances.Length > 0)
                    Utilities.Write(@ref.GlyphAdvances, Advances, 0, this.Advances.Length);
            } 

            if (this.Offsets != null)
            {
                if (@ref.GlyphCount >= 0 && @ref.GlyphCount != this.Offsets.Length)
                    throw new InvalidOperationException( string.Format(System.Globalization.CultureInfo.InvariantCulture, "Invalid length for array Offsets [{0}]. Indices, Advances and Offsets array must have same size (Current is [{1}]- or may be null", this.Offsets.Length, @ref.GlyphCount));
                @ref.GlyphCount = this.Offsets.Length;
                @ref.GlyphOffsets = Marshal.AllocHGlobal(this.Offsets.Length * sizeof(GlyphOffset));
                if (this.Offsets.Length > 0)
                    Utilities.Write(@ref.GlyphOffsets, Offsets, 0, this.Offsets.Length);
            }

            if (@ref.GlyphCount < 0)
                @ref.GlyphCount = 0;

            // Update GlyphCount only for debug purpose
            this.GlyphCount = @ref.GlyphCount;

            @ref.IsSideways = this.IsSideways;
            @ref.BidiLevel = this.BidiLevel;
        }

        public void Dispose()
        {
            if (FontFace != null)
            {
                FontFace.Dispose();
                FontFace = null;
            }
        }
    }
}