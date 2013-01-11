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

using System;
using System.IO;
using NUnit.Framework;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;

namespace SharpDX.Toolkit.Graphics.Tests
{

    /// <summary>
    /// Tests for all Textures
    /// </summary>
    [TestFixture]
    [Description("Tests SharpDX.Toolkit.Graphics")]
    public class TestEffectTextureCube
    {
        [Test]
        public void Test()
        {
            var device = GraphicsDevice.New();

            // Compile a toolkit effect from a file
            var result = EffectCompiler.CompileFromFile("toto.fx");

            // Check that we don't have any errors
            Assert.False(result.HasErrors);

            var bytecode = result.EffectData;

            var effect = new Effect(device, bytecode);

            var tex1 = TextureCube.New(device, 64, PixelFormat.R8.UNorm);
            var samplerState = device.SamplerStates.PointWrap;

            effect.Parameters["SkyBoxTexture"].SetResource(tex1);
            effect.Parameters["SkyBoxSampler"].SetResource(samplerState);

            effect.Techniques[0].Passes[0].Apply();

            Console.WriteLine(effect.Parameters.Count);

            effect.Dispose();
            device.Dispose();
        }
    }
}