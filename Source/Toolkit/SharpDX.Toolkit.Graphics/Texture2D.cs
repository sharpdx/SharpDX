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
// THE SOFTWARE.

using System.Runtime.InteropServices;
using SharpDX.Direct3D11;

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// This class is a frontend to <see cref="SharpDX.Direct3D11.Texture2D"/>.
    /// </summary>
    public class Texture2D : Texture<Direct3D11.Texture2D>
    {
        /// <summary>
        /// Specialised constructor for use only by derived classes.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <param name="dataRectangles">A variable-length parameters list containing data rectangles.</param>
        protected Texture2D(Texture2DDescription description, params DataRectangle[] dataRectangles)
            : this(GraphicsDevice.Current, description, dataRectangles)
        {
        }

        /// <summary>
        /// Specialised constructor for use only by derived classes.
        /// </summary>
        /// <param name="device">The device local.</param>
        /// <param name="description">The description.</param>
        /// <param name="dataRectangles">A variable-length parameters list containing data rectangles.</param>
        protected Texture2D(GraphicsDevice device, Texture2DDescription description, params DataRectangle[] dataRectangles)
        {
            Description = description;
            Initialize(device, new Direct3D11.Texture2D(device, description, dataRectangles), Description.BindFlags);
        }

        /// <summary>
        /// Specialised constructor for use only by derived classes.
        /// </summary>
        /// <param name="texture">The texture.</param>
        protected Texture2D(Direct3D11.Texture2D texture)
            : this(GraphicsDevice.Current, texture)
        {
        }

        /// <summary>
        /// Specialised constructor for use only by derived classes.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="texture">The texture.</param>
        protected Texture2D(GraphicsDevice device, Direct3D11.Texture2D texture)
        {
            Description = texture.Description;
            Initialize(device, texture, Description.BindFlags);
        }

        /// <summary>
        /// The description.
        /// </summary>
        public Texture2DDescription Description { get; private set; }

        /// <summary>
        /// Makes a copy of this texture.
        /// </summary>
        /// <remarks>
        /// This method doesn't copy the content of the texture.
        /// </remarks>
        /// <returns>
        /// A copy of this texture.
        /// </returns>
        public Texture2D Clone()
        {
            return new Texture2D(GraphicsDevice, Description);
        }

        public override GraphicsResource ToStaging()
        {
            var stagingDesc = Description;
            stagingDesc.BindFlags = BindFlags.None;
            stagingDesc.CpuAccessFlags = CpuAccessFlags.Read;
            stagingDesc.Usage = ResourceUsage.Staging;
            stagingDesc.OptionFlags = ResourceOptionFlags.None;
            return new Texture2D(this.GraphicsDevice, stagingDesc);
        }

        /// <summary>
        /// Creates a new texture from a <see cref="Texture2DDescription"/>.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <returns>
        /// A new instance of <see cref="Texture2D"/> class.
        /// </returns>
        public static Texture2D New(Texture2DDescription description)
        {
            return new Texture2D(description);
        }

        /// <summary>
        /// Creates a new texture from a <see cref="Direct3D11.Texture2D"/>.
        /// </summary>
        /// <param name="texture">The native texture <see cref="Direct3D11.Texture2D"/>.</param>
        /// <returns>
        /// A new instance of <see cref="Texture2D"/> class.
        /// </returns>
        public static Texture2D New(Direct3D11.Texture2D texture)
        {
            return new Texture2D(texture);
        }

        public static Texture2D New(int width, int height, PixelFormat format, int mipCount = 1)
        {
            return New(width, height, format, ResourceUsage.Default, false, mipCount);
        }

        public static Texture2D New(int width, int height, PixelFormat format, bool isReadWrite, int mipCount = 1)
        {
            return New(width, height, format, ResourceUsage.Default, isReadWrite, mipCount);
        }

        public static Texture2D New<T>(int width, int height, PixelFormat format, params T[][] initialTextures)
        {
            return New(width, height, format, ResourceUsage.Immutable, false, initialTextures);
        }

        public static Texture2D New<T>(int width, int height, PixelFormat format, bool isReadWrite, params T[][] initialTextures)
        {
            return New(width, height, format, isReadWrite ? ResourceUsage.Default : ResourceUsage.Immutable, isReadWrite, initialTextures);
        }

        public static Texture2D New(int width, int height, PixelFormat format, ResourceUsage usage, bool isReadWrite = false, int mipCount = 1)
        {
            return new Texture2D(NewDescription(width, height, format, isReadWrite, mipCount));
        }

        public static Texture2D New<T>(int width, int height, PixelFormat format, ResourceUsage usage, bool isReadWrite = false, params T[][] initialTextures)
        {
            GCHandle[] handles;
            var dataRectangles = Pin(width, format, initialTextures, out handles);
            var texture = new Texture2D(NewDescription(width, height, format, isReadWrite, initialTextures.Length, usage), dataRectangles);
            UnPin(handles);
            return texture;
        }

        protected static Texture2DDescription NewDescription(int width, int height, PixelFormat format, bool isReadWrite = false, int mipCount = 1, ResourceUsage usage = ResourceUsage.Default)
        {
            var desc = new Texture2DDescription()
                           {
                               Width = width,
                               Height = height,
                               ArraySize = 1,
                               SampleDescription = new DXGI.SampleDescription(1, 0),
                               BindFlags = BindFlags.ShaderResource,
                               Format = format,
                               MipLevels = mipCount,
                               Usage = usage,
                               CpuAccessFlags = GetCputAccessFlagsFromUsage(usage)
                           };

            if (isReadWrite)
            {
                desc.BindFlags |= BindFlags.UnorderedAccess;
            }
            return desc;
        }
    }
}