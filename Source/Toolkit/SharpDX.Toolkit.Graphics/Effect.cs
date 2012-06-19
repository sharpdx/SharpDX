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
    public class Effect : Component
    {
        internal readonly VertexShader VertexShader;
        internal readonly DomainShader DomainShader;
        internal readonly HullShader HullShader;
        internal readonly GeometryShader GeometryShader;
        internal readonly PixelShader PixelShader;
        internal readonly ComputeShader ComputeShader;

        public Effect(EffectBytecode effectBytecode)
        {
            var device = GraphicsDevice.Current;

            if (effectBytecode.Bytecodes[0] != null)
            {
                VertexShader = new VertexShader(device, effectBytecode.Bytecodes[0]);
            }
            if (effectBytecode.Bytecodes[1] != null)
            {
                DomainShader = new DomainShader(device, effectBytecode.Bytecodes[1]);
            }
            if (effectBytecode.Bytecodes[2] != null)
            {
                HullShader = new HullShader(device, effectBytecode.Bytecodes[2]);
            }
            if (effectBytecode.Bytecodes[3] != null)
            {
                GeometryShader = new GeometryShader(device, effectBytecode.Bytecodes[3]);
            }
            if (effectBytecode.Bytecodes[4] != null)
            {
                PixelShader = new PixelShader(device, effectBytecode.Bytecodes[4]);
            }
            if (effectBytecode.Bytecodes[5] != null)
            {
                ComputeShader = new ComputeShader(device, effectBytecode.Bytecodes[5]);
            }
        }

        public void Begin(GraphicsDeviceContext context)
        {

        }

        public void End(GraphicsDeviceContext context)
        {
            
        }
    }
}