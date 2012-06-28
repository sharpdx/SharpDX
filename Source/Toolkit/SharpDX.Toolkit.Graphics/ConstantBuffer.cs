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
    public class ConstantBuffer : BufferBase
    {
        protected ConstantBuffer(BufferDescription description)
            : this(GraphicsDevice.Current, description)
        {
        }

        protected ConstantBuffer(GraphicsDevice deviceLocal, BufferDescription description)
            : base(deviceLocal, description)
        {
        }

        protected ConstantBuffer(GraphicsDevice deviceLocal, Direct3D11.Buffer nativeBuffer)
            : base(deviceLocal, nativeBuffer)
        {
        }

        public ConstantBuffer Clone()
        {
            return new ConstantBuffer(GraphicsDevice, Description);
        }

        public static ConstantBuffer New(BufferDescription description)
        {
            return new ConstantBuffer(description);
        }

        public static ConstantBuffer New(int sizeInBytes)
        {
            return new ConstantBuffer(NewDescription(sizeInBytes, BindFlags.ConstantBuffer, false, 0, ResourceUsage.Dynamic));
        }

        public static ConstantBuffer New<T>() where T : struct
        {
            return New(Utilities.SizeOf<T>());
        }

        protected override void InitializeViews()
        {
        }

        public override BufferBase ToStaging()
        {
            var stagingDesc = Description;
            stagingDesc.BindFlags = BindFlags.None;
            stagingDesc.CpuAccessFlags = CpuAccessFlags.Read;
            stagingDesc.Usage = ResourceUsage.Staging;
            stagingDesc.OptionFlags = ResourceOptionFlags.None;
            return new ConstantBuffer(this.GraphicsDevice, stagingDesc);
        }
    }
}