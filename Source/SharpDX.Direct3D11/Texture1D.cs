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
using System.Diagnostics;

namespace SharpDX.Direct3D11
{
    public partial class Texture1D
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SharpDX.Direct3D11.Texture1D" /> class.
        /// </summary>
        /// <param name = "device">The device with which to associate the texture.</param>
        /// <param name = "description">The description of the texture.</param>
        public Texture1D(Device device, Texture1DDescription description)
            : base(IntPtr.Zero)
        {
            Texture1D temp;
            device.CreateTexture1D(ref description, null, out temp);
            NativePointer = temp.NativePointer;
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SharpDX.Direct3D11.Texture1D" /> class.
        /// </summary>
        /// <param name = "device">The device with which to associate the texture.</param>
        /// <param name = "description">The description of the texture.</param>
        /// <param name = "data">The initial texture data.</param>
        public Texture1D(Device device, Texture1DDescription description, DataStream data)
            : this(device, description, new[] {data})
        {
            System.Diagnostics.Debug.Assert(data != null);
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:SharpDX.Direct3D11.Texture1D" /> class.
        /// </summary>
        /// <param name = "device">The device with which to associate the texture.</param>
        /// <param name = "description">The description of the texture.</param>
        /// <param name = "data">An array of initial texture data for each subresource.</param>
        public Texture1D(Device device, Texture1DDescription description, DataStream[] data)
            : base(IntPtr.Zero)
        {
            System.Diagnostics.Debug.Assert(data != null);

            Texture1D temp;

            SubResourceData[] subResourceDatas = new SubResourceData[data.Length];
            for (int i = 0; i < subResourceDatas.Length; i++)
                subResourceDatas[i].DataPointer = data[i].DataPointer;

            device.CreateTexture1D(ref description, subResourceDatas, out temp);
            NativePointer = temp.NativePointer;
        }
    }
}