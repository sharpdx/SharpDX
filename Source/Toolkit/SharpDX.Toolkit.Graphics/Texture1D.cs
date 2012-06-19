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
using SharpDX.Direct3D11;

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// This class is a frontend to <see cref="SharpDX.Direct3D11.Texture1D"/>.
    /// </summary>
    public class Texture1D : Texture<Direct3D11.Texture1D>
    {
        /// <summary>
        /// Specialised constructor for use only by derived classes.
        /// </summary>
        /// <param name="description">The description.</param>
        protected Texture1D(Texture1DDescription description) : this(GraphicsDevice.Current, description)
        {
        }

        /// <summary>
        /// Specialised constructor for use only by derived classes.
        /// </summary>
        /// <param name="device">The graphics device.</param>
        /// <param name="description">The description.</param>
        protected Texture1D(GraphicsDevice device, Texture1DDescription  description)
        {
            Description = description;
            Initialize(device, new Direct3D11.Texture1D(device, description), Description.BindFlags);
        }

        /// <summary>
        /// Specialised constructor for use only by derived classes.
        /// </summary>
        /// <param name="texture">The texture.</param>
        protected Texture1D(Direct3D11.Texture1D texture)
            : this(GraphicsDevice.Current, texture)
        {
        }

        /// <summary>
        /// Specialised constructor for use only by derived classes.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="texture">The texture.</param>
        protected Texture1D(GraphicsDevice device, Direct3D11.Texture1D texture)
        {
            Description = texture.Description;
            Initialize(device, texture, Description.BindFlags);
        }

        /// <summary>The description.</summary>
        public Texture1DDescription Description { get; private set; }

        /// <summary>
        /// Makes a copy of this texture.
        /// </summary>
        /// <remarks>
        /// This method doesn't copy the content of the texture.
        /// </remarks>
        /// <returns>
        /// A copy of this texture.
        /// </returns>
        public Texture1D Clone()
        {
            return new Texture1D(GraphicsDevice, Description);
        }

        public override GraphicsResource ToStaging()
        {
            var stagingDesc = Description;
            stagingDesc.BindFlags = BindFlags.None;
            stagingDesc.CpuAccessFlags = CpuAccessFlags.Read;
            stagingDesc.Usage = ResourceUsage.Staging;
            stagingDesc.OptionFlags = ResourceOptionFlags.None;
            return new Texture1D(this.GraphicsDevice, stagingDesc);
        }

        /// <summary>
        /// Creates a new <see cref="Texture1D"/> with the given description.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <returns>
        /// A new instance of <see cref="Texture1D"/>.
        /// </returns>
        public static Texture1D New(Texture1DDescription description)
        {
            return new Texture1D(description);
        }

        public static Texture1D New(Direct3D11.Texture1D nativeTexture)
        {
            return new Texture1D(nativeTexture);
        }

        /// <summary>
        /// Creates a new <see cref="Texture1D"/> with the given description.
        /// </summary>
        /// <param name="width">The width of the texture.</param>
        /// <param name="format">Describes the format to use.</param>
        /// <param name="isReadWrite">(optional) If texture should be RW.</param>
        /// <param name="mipCount">(optional)Number of mips.</param>
        /// <returns>
        /// A new instance of <see cref="Texture1D"/>.
        /// </returns>
        public static Texture1D New(int width, PixelFormat format, bool isReadWrite = false, int mipCount = 1)
        {
            return new Texture1D(NewDescription(width, format, isReadWrite, mipCount));
        }

        protected static Texture1DDescription NewDescription(int width, PixelFormat format, bool isReadWrite = false, int mipCount = 1)
        {
            var desc = new Texture1DDescription()
                           {
                               Width = width,
                               ArraySize = 1,
                               BindFlags = BindFlags.ShaderResource,
                               Format = format,
                               MipLevels = mipCount,
                           };

            if (isReadWrite)
            {
                desc.BindFlags |= BindFlags.UnorderedAccess;
            }
            return desc;
        }
    }
}