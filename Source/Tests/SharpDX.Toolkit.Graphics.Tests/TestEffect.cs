// Copyright (c) 2010-2013 SharpDX - Alexandre Mutel
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
using NUnit.Framework;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.Mathematics;

namespace SharpDX.Toolkit.Graphics.Tests
{
    /// <summary>
    /// Tests for all Textures
    /// </summary>
    [TestFixture]
    [Description("Tests SharpDX.Toolkit.Graphics")]
    public class TestEffect
    {
        [Test]
        public void TestCompiler()
        {
            var device = GraphicsDevice.New();

            // Compile a toolkit effect from a file
            var result = new EffectCompiler().CompileFromFile("TestEffect.fx");

            // Check that we don't have any errors
            Assert.False(result.HasErrors);

            var bytecode = result.EffectData;

            // Check that the name of this effect is the file name
            Assert.AreEqual(bytecode.Description.Name, "TestEffect");

            // We have 3 shaders compiled: VS, VS2, PS
            Assert.AreEqual(bytecode.Shaders.Count, 3);

            // Check that this is the profile 10.0
            Assert.AreEqual(bytecode.Shaders[0].Level, FeatureLevel.Level_10_0);

            // Create a EffectData pool from a single EffectData
            var effectGroup = EffectPool.New(device);

            //var effect = effectGroup.New<BasicEffect>();
            var effect = new Effect(device, bytecode, effectGroup);

            //Texture2D tex : register(t0);
            //Texture2D tex1 : register(t2);
            //Texture2D tex2 : register(t3);
            //Texture2D tex3 : register(t10);
            //SamplerState samp;
            var tex = Texture2D.New(device, 256, 256, PixelFormat.R8.UNorm);
            var tex1 = Texture2D.New(device, 256, 256, PixelFormat.R8.UNorm);
            var tex2 = Texture2D.New(device, 256, 256, PixelFormat.R8.UNorm);
            var tex3 = Texture2D.New(device, 256, 256, PixelFormat.R8.UNorm);
            var samplerState = device.SamplerStates.PointWrap;

            effect.Parameters["tex"].SetResource(tex);
            effect.Parameters["tex1"].SetResource(tex1);
            effect.Parameters["tex2"].SetResource(tex2);
            effect.Parameters["tex3"].SetResource(tex3);
            effect.Parameters["worldViewProj"].SetValue(Matrix.Identity);
            effect.Parameters["samp"].SetResource(samplerState);

            //effect.Parameters["World"].SetValue(Vector3.Zero);
            //effect.Parameters["Tata"].SetResource(texture);

            effect.Techniques[0].Passes[0].Apply();

            Console.WriteLine(effect.Parameters.Count);

            effect.Dispose();
            device.Dispose();
        }


        [Test]
        public void TestMatrix()
        {
            // Compile a toolkit effect from a file
            var device = GraphicsDevice.New(DeviceCreationFlags.Debug);
            var result = new EffectCompiler().CompileFromFile("TestEffect.fx");

            var effect = new Effect(device, result.EffectData);

            var worldViewProj = effect.Parameters["worldViewProj"];
            var worlViewProjRowMajor = effect.Parameters["worlViewProjRowMajor"];
            var worldViewProj3x3 = effect.Parameters["worldViewProj3x3"];
            var matrix4x3 = effect.Parameters["matrix4x3"];
            var matrix3x4RowMajor = effect.Parameters["matrix3x4RowMajor"];


            var constantBuffer = effect.ConstantBuffers["$Globals"];
            var sourceMatrix = Matrix.RotationX(0.5f);

            // Test column_major float4x4 (the matrix is transposed automatically by the effect)
            worldViewProj.SetValue(sourceMatrix);
            var destMatrix = worldViewProj.GetMatrix();
            Assert.AreEqual(sourceMatrix, destMatrix);

            var destMatrix2 = constantBuffer.BackingBuffer.Get<Matrix>(worldViewProj.Offset);
            destMatrix2.Transpose();
            Assert.AreEqual(sourceMatrix, destMatrix2);

            // Test row_major float4x4 (the matrix is transferred as is)
            worlViewProjRowMajor.SetValue(sourceMatrix);
            destMatrix = worlViewProjRowMajor.GetMatrix();
            Assert.AreEqual(sourceMatrix, destMatrix);

            destMatrix2 = constantBuffer.BackingBuffer.Get<Matrix>(worlViewProjRowMajor.Offset);
            Assert.AreEqual(sourceMatrix, destMatrix2);

            // Test column_major float3x3 (the matrix is transposed automatically by the effect and only the 3x3 is transferred)
            var sourceMatrix3x3 = sourceMatrix;
            sourceMatrix3x3.Row4 = Vector4.Zero;
            sourceMatrix3x3.Column4 = Vector4.Zero;

            worldViewProj3x3.SetValue(sourceMatrix3x3);
            destMatrix = worldViewProj3x3.GetMatrix();
            Assert.AreEqual(sourceMatrix3x3, destMatrix);

            var destMatrixFloats = constantBuffer.BackingBuffer.GetRange<float>(worldViewProj3x3.Offset, 3 * 3);
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Assert.AreEqual(destMatrixFloats[j + i*3], sourceMatrix3x3[j, i]);
                }
            }

            effect.Dispose();
            device.Dispose();
        }
    }
}