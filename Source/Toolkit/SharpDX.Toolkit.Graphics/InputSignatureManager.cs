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

using System.Collections.Generic;
using SharpDX.Direct3D11;

namespace SharpDX.Toolkit.Graphics
{
    internal struct InputLayoutPair
    {
        public VertexInputLayout VertexInputLayout;

        public InputLayout InputLayout;
    }

    internal class InputSignatureManager
    {
        private readonly Direct3D11.Device device;

        public readonly byte[] Bytecode;

        public Dictionary<VertexInputLayout, InputLayoutPair> ContextCache;

        public Dictionary<VertexInputLayout, InputLayoutPair> DeviceCache;

        public InputSignatureManager(GraphicsDevice device, byte[] byteCode, Dictionary<VertexInputLayout, InputLayoutPair> contextCache, Dictionary<VertexInputLayout, InputLayoutPair> deviceCache)
        {
            this.device = device;
            Bytecode = byteCode;
            ContextCache = contextCache;
            DeviceCache = deviceCache;
        }

        public void GetOrCreate(VertexInputLayout layout, out InputLayoutPair currentPassPreviousPair)
        {
            if (!ContextCache.TryGetValue(layout, out currentPassPreviousPair))
            {
                lock (DeviceCache)
                {
                    if (!DeviceCache.TryGetValue(layout, out currentPassPreviousPair))
                    {

                        currentPassPreviousPair.InputLayout = new InputLayout(device, Bytecode, layout.InputElements);
                        currentPassPreviousPair.VertexInputLayout = layout;
                        DeviceCache.Add(layout, currentPassPreviousPair);
                    }
                }

                ContextCache.Add(layout, currentPassPreviousPair);
            }
        }
    }
}