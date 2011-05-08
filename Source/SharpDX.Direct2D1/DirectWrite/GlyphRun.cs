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
using System;
using System.Runtime.InteropServices;

namespace SharpDX.DirectWrite
{
    public partial class GlyphRun
    {
        /// <summary>
        /// Gets or sets the <see cref="FontFace"/> associated with this GlypRun.
        /// </summary>
        /// <value>The font face.</value>
        public FontFace FontFace { get; set; }

        /// <summary>
        /// Gets or sets an array containing glyph advance widths for the glyph run. 
        /// </summary>
        /// <value>The glyph advances.</value>
        public GlyphRunItem[] Items { get; set; }


        /// <summary>
        /// Trannsform this GlyphRun to individual indices, advances and offsets arrays.
        /// </summary>
        /// <param name="advances">The advances array.</param>
        /// <param name="offsets">The offsets array.</param>
        /// <returns>the indices array.</returns>
        public short[] ToArrays(out float[] advances, out GlyphOffset[] offsets)
        {
            var indices = new short[Items.Length];
            advances = new float[Items.Length];
            offsets = new GlyphOffset[Items.Length];
            for (int i = 0; i < Items.Length; i++)
            {
                indices[i] = Items[i].Index;
                advances[i] = Items[i].Advance;
                offsets[i] = Items[i].Offset;
            }
            return indices;
        }

        // Internal native struct used for marshalling
        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        internal unsafe partial struct __Native
        {
            public IntPtr FontFace;
            public float FontEmSize;
            public int GlyphCount;
            public IntPtr GlyphIndices;
            public IntPtr GlyphAdvances;
            public IntPtr GlyphOffsets;
            public int _IsSideways;
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
            this.FontSize= @ref.FontEmSize;
            this.Items = new GlyphRunItem[@ref.GlyphCount];
            for (int i = 0; i < Items.Length; i++)
            {
                Items[i].Index = ((short*) @ref.GlyphIndices)[i];
                Items[i].Advance = ((float*)@ref.GlyphAdvances)[i];
                Items[i].Offset = ((GlyphOffset*)@ref.GlyphOffsets)[i];
            }
            this._IsSideways = @ref._IsSideways;
            this.BidiLevel = @ref.BidiLevel;
        }
        // Method to marshal from managed struct tot native
        internal unsafe void __MarshalTo(ref __Native @ref)
        {
            @ref.FontFace = this.FontFace == null?IntPtr.Zero:this.FontFace.NativePointer;
            @ref.FontEmSize = this.FontSize;
            @ref.GlyphCount = Items == null ? 0 : Items.Length;
            @ref.GlyphIndices = IntPtr.Zero;
            @ref.GlyphAdvances = IntPtr.Zero;
            @ref.GlyphOffsets = IntPtr.Zero;
            if (Items != null && Items.Length > 0)
            {
                // TODO: improve performance by making a single AllocHGlobal call
                @ref.GlyphIndices = Marshal.AllocHGlobal(this.Items.Length * sizeof(short));
                @ref.GlyphAdvances = Marshal.AllocHGlobal(this.Items.Length * sizeof(float));
                @ref.GlyphOffsets = Marshal.AllocHGlobal(this.Items.Length * sizeof(GlyphOffset));

                for (int i = 0; i < Items.Length; i++)
                {
                    ((short*) @ref.GlyphIndices)[i] = Items[i].Index;
                    ((float*) @ref.GlyphAdvances)[i] = Items[i].Advance;
                    ((GlyphOffset*) @ref.GlyphOffsets)[i] = Items[i].Offset;
                }
            }
            @ref._IsSideways = this._IsSideways;
            @ref.BidiLevel = this.BidiLevel;
        }
    }
}