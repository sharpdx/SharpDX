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

using SharpDX.Direct3D11;

namespace SharpDX.Direct3D12
{
    public partial struct ResourceDescription
    {
        public ResourceDescription(BufferDescription o)
        {
            Dimension = ResourceDimension.Buffer;
            Width = o.SizeInBytes;
            Height = 1;
            Depth = 1;
            MipLevels = 1;
            ArraySize = 1;
            Format = DXGI.Format.Unknown;
            SampleDescription.Count = 1;
            SampleDescription.Quality = 0;
            Usage = o.Usage;
            BindFlags = o.BindFlags;
            CpuAccessFlags = o.CpuAccessFlags;
            OptionFlags = o.OptionFlags;
            StructureByteStride = o.StructureByteStride;
        }

        public ResourceDescription(Texture1DDescription o)
        {
            Dimension = ResourceDimension.Texture1D;
            Width = o.Width;
            Height = 1;
            Depth = 1;
            MipLevels = o.MipLevels;
            ArraySize = o.ArraySize;
            Format = o.Format;
            SampleDescription.Count = 1;
            SampleDescription.Quality = 0;
            Usage = o.Usage;
            BindFlags = o.BindFlags;
            CpuAccessFlags = o.CpuAccessFlags;
            OptionFlags = o.OptionFlags;
            StructureByteStride = 0;
        }

        public ResourceDescription(Texture2DDescription o)
        {
            Dimension = ResourceDimension.Texture2D;
            Width = o.Width;
            Height = o.Height;
            Depth = 1;
            MipLevels = o.MipLevels;
            ArraySize = o.ArraySize;
            Format = o.Format;
            SampleDescription = o.SampleDescription;
            Usage = o.Usage;
            BindFlags = o.BindFlags;
            CpuAccessFlags = o.CpuAccessFlags;
            OptionFlags = o.OptionFlags;
            StructureByteStride = 0;
        }

        public ResourceDescription(Texture3DDescription o)
        {
            Dimension = ResourceDimension.Texture3D;
            Width = o.Width;
            Height = o.Height;
            Depth = o.Depth;
            MipLevels = o.MipLevels;
            ArraySize = 1;
            Format = o.Format;
            SampleDescription.Count = 1;
            SampleDescription.Quality = 0;
            Usage = o.Usage;
            BindFlags = o.BindFlags;
            CpuAccessFlags = o.CpuAccessFlags;
            OptionFlags = o.OptionFlags;
            StructureByteStride = 0;
        }

        public static implicit operator ResourceDescription(BufferDescription description)
        {
            return new ResourceDescription(description);
        }
        public static implicit operator ResourceDescription(Texture1DDescription description)
        {
            return new ResourceDescription(description);
        }
        public static implicit operator ResourceDescription(Texture2DDescription description)
        {
            return new ResourceDescription(description);
        }
        public static implicit operator ResourceDescription(Texture3DDescription description)
        {
            return new ResourceDescription(description);
        }
    }
}