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
using SharpDX;

namespace MiniTri
{
    // Use Toolkit namespace inside your namepsace in order to make a priority over Direct3D11/DXGI namespaces.
    using SharpDX.Toolkit.Graphics;

    /// <summary>
    /// Sample application of MiniTri using SharpDX.Toolkit.
    /// </summary>
    class Program
    {
        [STAThread]
        static void Main()
        {
            // Instantiate an abstract Window (on desktop: a Windows Form)
            var window = GraphicsWindow.New("SharpDX Toolkit - MiniTri Sample");

            // Instantiate the graphics device (A Direct3D11 device)
            var device = GraphicsDevice.New();

            // Instantiate a graphics presenter (equivalent of a swap chain).
            var presenter = GraphicsPresenter.New(device, new PresentationParameters(1024, 768, window));

            // Set the presenter on the graphics device, in order to use device.BackBuffer, device.Present.
            device.Presenter = presenter;

            // Create a XNA like BasicEffect
            var effect = new BasicEffect(device)
            {
                VertexColorEnabled = true,
                View = Matrix.Identity,
                Projection = Matrix.Identity,
                World = Matrix.Identity
            };

            // Instantiate the vertex buffer
            var vertexBuffer = Buffer.Vertex.New(device, new[]
                           {
                               new VertexPositionColor(new Vector3(-0.5f, -0.5f, 0.5f), Color.Red),
                               new VertexPositionColor(new Vector3( 0.0f,  0.5f, 0.5f), Color.Green),
                               new VertexPositionColor(new Vector3( 0.5f, -0.5f, 0.5f), Color.Blue),
                           });
            var inputLayout = VertexInputLayout.FromBuffer(0, vertexBuffer);

            // Setup our main loop
            window.OnRender += () =>
            {
                // Clear the back buffer
                device.Clear(device.BackBuffer, Color.CornflowerBlue);

                // Set the vertex input layout
                device.SetVertexInputLayout(inputLayout);

                // Set the vertex buffer
                device.SetVertexBuffer(0, vertexBuffer);

                // Set the viewport
                device.SetViewports(0, 0, device.BackBuffer.Description.Width, device.BackBuffer.Description.Height);

                // Set the render target
                device.SetRenderTargets(device.BackBuffer);

                // Apply the pass
                effect.Techniques[0].Passes[0].Apply();

                // Draw the triangle
                device.Draw(PrimitiveType.TriangleList, 3);

                // Present to the current presenter
                device.Present();
            };

            // Run this window and block until it is closed.
            window.Run();
        }
    }
}
