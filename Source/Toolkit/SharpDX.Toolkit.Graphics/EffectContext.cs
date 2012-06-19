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
    public class EffectContext : Component
    {
        private readonly GraphicsDeviceContext context;
        private readonly DeviceContext nativeContext;

        public EffectContext(Effect effect, GraphicsDeviceContext context)
        {
            this.Effect = effect;
            this.context = context;
            this.nativeContext = context;
        }

        public readonly Effect Effect;
    
        public void Begin()
        {
            // Setup the shaders to the pipeline
            // but don't setup it if it is already fine (avoid managed/unmanaged transitions)

            if (!ReferenceEquals(context.CurrentStage.VertexShader, Effect.VertexShader))
                nativeContext.VertexShader.Set(Effect.VertexShader);

            if (!ReferenceEquals(context.CurrentStage.DomainShader, Effect.DomainShader))
                nativeContext.DomainShader.Set(Effect.DomainShader);

            if (!ReferenceEquals(context.CurrentStage.HullShader, Effect.HullShader))
                nativeContext.HullShader.Set(Effect.HullShader);

            if (!ReferenceEquals(context.CurrentStage.GeometryShader, Effect.GeometryShader))
                nativeContext.GeometryShader.Set(Effect.GeometryShader);

            if (!ReferenceEquals(context.CurrentStage.PixelShader, Effect.PixelShader))
                nativeContext.PixelShader.Set(Effect.PixelShader);

            if (!ReferenceEquals(context.CurrentStage.ComputeShader, Effect.ComputeShader))
                nativeContext.ComputeShader.Set(Effect.ComputeShader);
        }

        public void End()
        {
            
        }
    }
}