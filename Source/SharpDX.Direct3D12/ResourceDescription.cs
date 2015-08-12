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

using SharpDX.DXGI;

namespace SharpDX.Direct3D12
{
    public partial struct ResourceDescription
    {
        public ResourceDescription(ResourceDimension dimension, long alignment, long width, int height, short depthOrArraySize, short mipLevels, Format format, int sampleCount, int sampleQuality, TextureLayout layout, ResourceFlags optionFlags)
        {
            Dimension = dimension;
            Alignment = alignment;
            Width = width;
            Height = height;
            DepthOrArraySize = depthOrArraySize;
            MipLevels = mipLevels;
            Format = format;
            SampleDescription = new SampleDescription(sampleCount, sampleQuality);
            Layout = layout;
            Flags = optionFlags;
        }

        public static ResourceDescription Buffer(ResourceAllocationInformation resourceAllocInfowidth, ResourceFlags flags = ResourceFlags.None)
        {
            return new ResourceDescription(ResourceDimension.Buffer, resourceAllocInfowidth.Alignment, resourceAllocInfowidth.SizeInBytes,
                1, 1, 1, DXGI.Format.Unknown, 1, 0, TextureLayout.RowMajor, flags);
        }

        public static ResourceDescription Buffer(long width, ResourceFlags flags = ResourceFlags.None, long alignment = 0)
        {
            return new ResourceDescription(ResourceDimension.Buffer, alignment, width, 1, 1, 1, DXGI.Format.Unknown, 1, 0, TextureLayout.RowMajor, flags);
        }

        public static ResourceDescription Texture1D(DXGI.Format format,
            long width,
            short arraySize = 1,
            short mipLevels = 0,
            ResourceFlags flags = ResourceFlags.None,
            TextureLayout layout = TextureLayout.Unknown,
            long alignment = 0)
        {
            return new ResourceDescription(ResourceDimension.Texture1D, alignment, width, 1, arraySize, mipLevels, format, 1, 0, layout, flags);
        }

        public static ResourceDescription Texture2D(DXGI.Format format,
            long width,
            int height,
            short arraySize = 1,
            short mipLevels = 0,
            int sampleCount = 1,
            int sampleQuality = 0,
            ResourceFlags flags = ResourceFlags.None,
            TextureLayout layout = TextureLayout.Unknown,
            long alignment = 0)
        {
            return new ResourceDescription(ResourceDimension.Texture2D, alignment, width, height, arraySize, mipLevels, format, sampleCount, sampleQuality, layout, flags);
        }

        public static ResourceDescription Texture3D(DXGI.Format format,
            long width,
            int height,
            short depth,
            short mipLevels = 0,
            ResourceFlags flags = ResourceFlags.None,
            TextureLayout layout = TextureLayout.Unknown,
            long alignment = 0)
        {
            return new ResourceDescription(ResourceDimension.Texture3D, alignment, width, height, depth, mipLevels, format, 1, 0, layout, flags);
        }
    }
}
