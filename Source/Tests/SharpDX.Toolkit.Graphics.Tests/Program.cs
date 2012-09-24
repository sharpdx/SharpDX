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
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.IO;
using SharpDX.WIC;
using SharpDX.Windows;

namespace SharpDX.Toolkit.Graphics.Tests
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //var test = new TestBuffer();
            //test.AllTest();

            //var testTexture2D = new TestTexture2D();
            //testTexture2D.TestConstructors();

            //// Test Image
            //var testImage = new TestImage();
            //testImage.Initialize();
            //testImage.TestLoadAndSave();

            // Test Image
            //var testImage = new TestTexture();
            //testImage.Initialize();
            //testImage.TestLoadSave();

            //var testTexture = new TestTexture();
            //testTexture.Initialize();
            //testTexture.Test1DArrayAndMipmaps();

            //var testCompiler = new TestEffectCompiler();
            //testCompiler.TestCompiler();

            // Compile a toolkit effect from a file

            var form = new RenderForm("SharpDX - MiniTri Direct3D 11 Sample");

            int width = form.ClientSize.Width;
            int height = form.ClientSize.Height;

            var device = GraphicsDevice.New(DeviceCreationFlags.Debug);
            device.CurrentPresenter = GraphicsPresenter.New(device, width, height, PixelFormat.R8G8B8A8.UNorm, form.Handle);

            var effect = new BasicEffect(device)
                             {
                                 VertexColorEnabled = true, 
                                 View = Matrix.Identity, 
                                 Projection = Matrix.Identity, 
                                 World = Matrix.Identity
                             };

            var bufferData = new []
                           {
                               new VertexPositionColor(new Vector3(-0.5f, -0.5f, 0.5f), Color.Red),
                               new VertexPositionColor(new Vector3( 0.0f,  0.5f, 0.5f), Color.Green),
                               new VertexPositionColor(new Vector3( 0.5f, -0.5f, 0.5f), Color.Blue),
                           };


            var vertexBuffer = Buffer.Vertex.New(device, bufferData);
            var inputLayout = VertexInputLayout.FromBuffer(0, vertexBuffer);

            device.SetRenderTargets(device.CurrentPresenter.BackBuffer);
            device.SetViewports(0, 0, width, height);
            device.SetVertexInputLayout(inputLayout);
            device.SetVertexBuffer(0, vertexBuffer);

            RenderLoop.Run(form, () =>
                                     {
                                         device.Clear(device.CurrentPresenter.BackBuffer, Color.CornflowerBlue);

                                         effect.Techniques[0].Passes[0].Apply();

                                         device.Draw(PrimitiveType.TriangleList, 3);

                                         device.Present();

                                     });
        }
    }
}