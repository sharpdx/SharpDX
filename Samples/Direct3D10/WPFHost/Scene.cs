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
// -----------------------------------------------------------------------------
// Original code from SlimMath project. http://code.google.com/p/slimmath/
// Greetings to SlimDX Group. Original code published with the following license:
// -----------------------------------------------------------------------------
/*
* Copyright (c) 2007-2011 SlimDX Group
* 
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
* 
* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
* 
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
* THE SOFTWARE.
*/
namespace WPFHost
{
    using System;
    using SharpDX;
    using SharpDX.D3DCompiler;
    using SharpDX.Direct3D10;
    using SharpDX.DXGI;
    using Buffer = SharpDX.Direct3D10.Buffer;
    using Device = SharpDX.Direct3D10.Device;

    public class Scene : IScene
    {
        private ISceneHost Host;
        private InputLayout VertexLayout;
        private DataStream VertexStream;
        private Buffer Vertices;
        private Effect SimpleEffect;
        private Color4 OverlayColor = new Color4(1.0f);

        void IScene.Attach(ISceneHost host)
        {
            this.Host = host;

            Device device = host.Device;
            if (device == null)
                throw new Exception("Scene host device is null");

            ShaderBytecode shaderBytes = ShaderBytecode.CompileFromFile("Simple.fx", "fx_4_0", ShaderFlags.None, EffectFlags.None, null, null);
            this.SimpleEffect = new Effect(device, shaderBytes);

            EffectTechnique technique = this.SimpleEffect.GetTechniqueByIndex(0); ;
            EffectPass pass = technique.GetPassByIndex(0);

            this.VertexLayout = new InputLayout(device, pass.Description.Signature, new[] {
                new InputElement("POSITION", 0, Format.R32G32B32A32_Float, 0, 0),
                new InputElement("COLOR", 0, Format.R32G32B32A32_Float, 16, 0) 
            });

            this.VertexStream = new DataStream(3 * 32, true, true);
            this.VertexStream.WriteRange(new[] {
                new Vector4(0.0f, 0.5f, 0.5f, 1.0f), new Vector4(1.0f, 0.0f, 0.0f, 1.0f),
                new Vector4(0.5f, -0.5f, 0.5f, 1.0f), new Vector4(0.0f, 1.0f, 0.0f, 1.0f),
                new Vector4(-0.5f, -0.5f, 0.5f, 1.0f), new Vector4(0.0f, 0.0f, 1.0f, 1.0f)
            });
            this.VertexStream.Position = 0;

            this.Vertices = new Buffer(device, this.VertexStream, new BufferDescription()
                {
                    BindFlags = BindFlags.VertexBuffer,
                    CpuAccessFlags = CpuAccessFlags.None,
                    OptionFlags = ResourceOptionFlags.None,
                    SizeInBytes = 3 * 32,
                    Usage = ResourceUsage.Default
                }
            );

            device.Flush();
        }

        void IScene.Detach()
        {
            Disposer.RemoveAndDispose(ref this.Vertices);
            Disposer.RemoveAndDispose(ref this.VertexLayout);
            Disposer.RemoveAndDispose(ref this.SimpleEffect);
            Disposer.RemoveAndDispose(ref this.VertexStream);
        }

        void IScene.Update(TimeSpan sceneTime)
        {
            float t = (float) sceneTime.Milliseconds * 0.001f;
            this.OverlayColor.Alpha = t;
        }

        void IScene.Render()
        {
            Device device = this.Host.Device;
            if (device == null)
                return;

            device.InputAssembler.InputLayout = this.VertexLayout;
            device.InputAssembler.PrimitiveTopology = SharpDX.Direct3D.PrimitiveTopology.TriangleList;
            device.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(this.Vertices, 32, 0));

            EffectTechnique technique = this.SimpleEffect.GetTechniqueByIndex(0);
            EffectPass pass = technique.GetPassByIndex(0);

            EffectVectorVariable overlayColor = this.SimpleEffect.GetVariableBySemantic("OverlayColor").AsVector();

            overlayColor.Set(this.OverlayColor);

            for (int i = 0; i < technique.Description.PassCount; ++i)
            {
                pass.Apply();
                device.Draw(3, 0);
            }
        }
    }
}
