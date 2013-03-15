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
using System.Diagnostics;
using System.Runtime.InteropServices;
using SharpDX;
using SharpDX.DXGI;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.IO;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Core;
using Windows.System;
using Windows.UI.Core;
using Buffer = SharpDX.Direct3D11.Buffer;
using Device = SharpDX.Direct3D11.Device;
using Device1 = SharpDX.Direct3D11.Device1;
using Resource = SharpDX.Direct3D11.Resource;

namespace SimpleMiniCube
{
    /// <summary>
    ///   Simple application displaying a 3D rotating cube using SharpDX.Direct3D11 API.
    /// </summary>
    internal static class Program
    {
        [MTAThread]
        private static void Main()
        {
            var viewFactory = new MiniCubeFactory();
            CoreApplication.Run(viewFactory);
        }

        /// <summary>
        /// Main class to render a cube to the current view.
        /// </summary>
        private class MiniCube : Component, IFrameworkView
        {
            private Texture2D backBuffer;
            private Stopwatch clock;
            private Buffer constantBuffer;
            private DepthStencilView depthStencilView;
            private Device1 graphicsDevice;
            private int height;
            private InputLayout layout;
            private PixelShader pixelShader;
            private RenderTargetView renderTargetView;
            private SwapChain1 swapChain;
            private VertexBufferBinding vertexBufferBinding;
            private VertexShader vertexShader;
            private int width;
            private CoreWindow window;

            [StructLayout(LayoutKind.Sequential)]
            private struct VertexPositionColor
            {
                public VertexPositionColor(Vector4 position, Color color)
                {
                    Position = position;
                    Color = color;
                }

                public Vector4 Position;
                public Color Color;
            }


            /// <inheritdoc/>
            public void Initialize(CoreApplicationView applicationView)
            {
            }

            /// <inheritdoc/>
            public void SetWindow(CoreWindow window)
            {
                this.window = window;

                // Starts the timer
                clock = Stopwatch.StartNew();

                // Creates Direct3D11 device
                using (var defaultDevice = new Device(DriverType.Hardware, DeviceCreationFlags.None))
                    graphicsDevice = defaultDevice.QueryInterface<Device1>();


                // Setup swapchain, render target, depth stencil buffer.
                SetupScreenBuffers();

                string path = Package.Current.InstalledLocation.Path;

                // Loads vertex shader bytecode
                var vertexShaderByteCode = NativeFile.ReadAllBytes(path + "\\MiniCube_VS.fxo");
                vertexShader = new VertexShader(graphicsDevice, vertexShaderByteCode);

                // Loads pixel shader bytecode
                pixelShader = new PixelShader(graphicsDevice, NativeFile.ReadAllBytes(path + "\\MiniCube_PS.fxo"));

                // Layout from VertexShader input signature
                layout = new InputLayout(graphicsDevice, vertexShaderByteCode, new[]
                                                                              {
                                                                                  new InputElement("POSITION", 0, Format.R32G32B32A32_Float, 0, 0),
                                                                                  new InputElement("COLOR", 0, Format.R8G8B8A8_UNorm, 16, 0)
                                                                              });

                // Instantiate Vertex buffer from vertex data
                Buffer vertices = Buffer.Create(graphicsDevice, BindFlags.VertexBuffer, new[]
                                                                                       {
                                                                                           new VertexPositionColor(new Vector4(-1.0f, -1.0f, -1.0f, 1.0f), Color.Orange), // Front
                                                                                           new VertexPositionColor(new Vector4(-1.0f, 1.0f, -1.0f, 1.0f),  Color.Orange),
                                                                                           new VertexPositionColor(new Vector4(1.0f, 1.0f, -1.0f, 1.0f),   Color.Orange),
                                                                                           new VertexPositionColor(new Vector4(-1.0f, -1.0f, -1.0f, 1.0f), Color.Orange),
                                                                                           new VertexPositionColor(new Vector4(1.0f, 1.0f, -1.0f, 1.0f),   Color.Orange),
                                                                                           new VertexPositionColor(new Vector4(1.0f, -1.0f, -1.0f, 1.0f),  Color.Orange),
                                                                                           new VertexPositionColor(new Vector4(-1.0f, -1.0f, 1.0f, 1.0f),  Color.Orange), // BACK
                                                                                           new VertexPositionColor(new Vector4(1.0f, 1.0f, 1.0f, 1.0f),    Color.Orange),
                                                                                           new VertexPositionColor(new Vector4(-1.0f, 1.0f, 1.0f, 1.0f),   Color.Orange),
                                                                                           new VertexPositionColor(new Vector4(-1.0f, -1.0f, 1.0f, 1.0f),  Color.Orange),
                                                                                           new VertexPositionColor(new Vector4(1.0f, -1.0f, 1.0f, 1.0f),   Color.Orange),
                                                                                           new VertexPositionColor(new Vector4(1.0f, 1.0f, 1.0f, 1.0f),    Color.Orange),
                                                                                           new VertexPositionColor(new Vector4(-1.0f, 1.0f, -1.0f, 1.0f),  Color.OrangeRed), // Top
                                                                                           new VertexPositionColor(new Vector4(-1.0f, 1.0f, 1.0f, 1.0f),   Color.OrangeRed),
                                                                                           new VertexPositionColor(new Vector4(1.0f, 1.0f, 1.0f, 1.0f),    Color.OrangeRed),
                                                                                           new VertexPositionColor(new Vector4(-1.0f, 1.0f, -1.0f, 1.0f),  Color.OrangeRed),
                                                                                           new VertexPositionColor(new Vector4(1.0f, 1.0f, 1.0f, 1.0f),    Color.OrangeRed),
                                                                                           new VertexPositionColor(new Vector4(1.0f, 1.0f, -1.0f, 1.0f),   Color.OrangeRed),
                                                                                           new VertexPositionColor(new Vector4(-1.0f, -1.0f, -1.0f, 1.0f), Color.OrangeRed), // Bottom
                                                                                           new VertexPositionColor(new Vector4(1.0f, -1.0f, 1.0f, 1.0f),   Color.OrangeRed),
                                                                                           new VertexPositionColor(new Vector4(-1.0f, -1.0f, 1.0f, 1.0f),  Color.OrangeRed),
                                                                                           new VertexPositionColor(new Vector4(-1.0f, -1.0f, -1.0f, 1.0f), Color.OrangeRed),
                                                                                           new VertexPositionColor(new Vector4(1.0f, -1.0f, -1.0f, 1.0f),  Color.OrangeRed),
                                                                                           new VertexPositionColor(new Vector4(1.0f, -1.0f, 1.0f, 1.0f),   Color.OrangeRed),
                                                                                           new VertexPositionColor(new Vector4(-1.0f, -1.0f, -1.0f, 1.0f), Color.DarkOrange), // Left
                                                                                           new VertexPositionColor(new Vector4(-1.0f, -1.0f, 1.0f, 1.0f),  Color.DarkOrange),
                                                                                           new VertexPositionColor(new Vector4(-1.0f, 1.0f, 1.0f, 1.0f),   Color.DarkOrange),
                                                                                           new VertexPositionColor(new Vector4(-1.0f, -1.0f, -1.0f, 1.0f), Color.DarkOrange),
                                                                                           new VertexPositionColor(new Vector4(-1.0f, 1.0f, 1.0f, 1.0f),   Color.DarkOrange),
                                                                                           new VertexPositionColor(new Vector4(-1.0f, 1.0f, -1.0f, 1.0f),  Color.DarkOrange),
                                                                                           new VertexPositionColor(new Vector4(1.0f, -1.0f, -1.0f, 1.0f),  Color.DarkOrange), // Right
                                                                                           new VertexPositionColor(new Vector4(1.0f, 1.0f, 1.0f, 1.0f),    Color.DarkOrange),
                                                                                           new VertexPositionColor(new Vector4(1.0f, -1.0f, 1.0f, 1.0f),   Color.DarkOrange),
                                                                                           new VertexPositionColor(new Vector4(1.0f, -1.0f, -1.0f, 1.0f),  Color.DarkOrange),
                                                                                           new VertexPositionColor(new Vector4(1.0f, 1.0f, -1.0f, 1.0f),   Color.DarkOrange),
                                                                                           new VertexPositionColor(new Vector4(1.0f, 1.0f, 1.0f, 1.0f),    Color.DarkOrange),
                                                                                       });

                vertexBufferBinding = new VertexBufferBinding(vertices, Utilities.SizeOf<VertexPositionColor>(), 0);

                // Create Constant Buffer
                constantBuffer = ToDispose(new Buffer(graphicsDevice, Utilities.SizeOf<Matrix>(), ResourceUsage.Default, BindFlags.ConstantBuffer, CpuAccessFlags.None, ResourceOptionFlags.None, 0));
            }


            /// <inheritdoc/>
            public void Load(string entryPoint)
            {
            }

            /// <inheritdoc/>
            public void Run()
            {
                // Activate the application window, making it visible and enabling it to receive events.
                window.Activate();

                // Prepare matrices
                var view = Matrix.LookAtLH(new Vector3(0, 0, -5), new Vector3(0, 0, 0), Vector3.UnitY);
                var proj = Matrix.PerspectiveFovLH((float)Math.PI / 4.0f, width / (float)height, 0.1f, 100.0f);
                var viewProj = Matrix.Multiply(view, proj);

                // Enter the render loop.  Note that Metro style apps should never exit.
                while (true)
                {
                    // Process events incoming to the window.
                    window.Dispatcher.ProcessEvents(CoreProcessEventsOption.ProcessAllIfPresent);

                    if (window.GetAsyncKeyState(VirtualKey.Escape) == CoreVirtualKeyStates.Down)
                        break;

                    // Gets the current graphics context
                    var graphicsContext = graphicsDevice.ImmediateContext;

                    var time = (float) (clock.ElapsedMilliseconds/1000.0);

                    // Set targets (This is mandatory in the loop)
                    graphicsContext.OutputMerger.SetTargets(renderTargetView);

                    // Clear the views
                    graphicsContext.ClearRenderTargetView(renderTargetView, Color.CornflowerBlue);

                    // Calculate WorldViewProj
                    Matrix worldViewProj = Matrix.RotationX(time)*Matrix.RotationY(time*2.0f)*Matrix.RotationZ(time*.7f)*viewProj;
                    worldViewProj.Transpose();

                    // Setup the pipeline
                    graphicsContext.InputAssembler.SetVertexBuffers(0, vertexBufferBinding);
                    graphicsContext.InputAssembler.InputLayout = layout;
                    graphicsContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
                    graphicsContext.VertexShader.SetConstantBuffer(0, constantBuffer);
                    graphicsContext.VertexShader.Set(vertexShader);
                    graphicsContext.PixelShader.Set(pixelShader);

                    // Update Constant Buffer
                    graphicsContext.UpdateSubresource(ref worldViewProj, constantBuffer, 0);

                    // Draw the cube
                    graphicsContext.Draw(36, 0);

                    // Present the backbuffer
                    swapChain.Present(1, PresentFlags.None, new PresentParameters());
                }
            }

            public void Uninitialize()
            {
            }

            private void SetupScreenBuffers()
            {
                width = (int) window.Bounds.Width;
                height = (int) window.Bounds.Height;

                // If the swap chain already exists, resize it.
                if (swapChain != null)
                {
                    swapChain.ResizeBuffers(2, width, height, Format.B8G8R8A8_UNorm, SwapChainFlags.None);
                }
                    // Otherwise, create a new one.
                else
                {
                    // SwapChain description
                    var desc = new SwapChainDescription1
                                   {
                                       // Automatic sizing
                                       Width = width,
                                       Height = height,
                                       Format = Format.B8G8R8A8_UNorm,
                                       Stereo = false,
                                       SampleDescription = new SampleDescription(1, 0),
                                       Usage = Usage.BackBuffer | Usage.RenderTargetOutput,
                                       // Use two buffers to enable flip effect.
                                       BufferCount = 2,
                                       Scaling = Scaling.None,
                                       SwapEffect = SwapEffect.FlipSequential,
                                   };

                    // Once the desired swap chain description is configured, it must be created on the same adapter as our D3D Device

                    // First, retrieve the underlying DXGI Device from the D3D Device.
                    // Creates the swap chain 
                    using (var dxgiDevice2 = graphicsDevice.QueryInterface<Device2>())
                    using (Adapter dxgiAdapter = dxgiDevice2.Adapter)
                    using (var dxgiFactory2 = dxgiAdapter.GetParent<Factory2>())
                    {
                        // Creates a SwapChain from a CoreWindow pointer
                        using (var comWindow = new ComObject(window))
                            swapChain = dxgiFactory2.CreateSwapChainForCoreWindow(graphicsDevice, comWindow, ref desc, null);

                        // Ensure that DXGI does not queue more than one frame at a time. This both reduces 
                        // latency and ensures that the application will only render after each VSync, minimizing 
                        // power consumption.
                        dxgiDevice2.MaximumFrameLatency = 1;
                    }
                }

                // Obtain the backbuffer for this window which will be the final 3D rendertarget.
                backBuffer = ToDispose(Resource.FromSwapChain<Texture2D>(swapChain, 0));
                {
                    // Create a view interface on the rendertarget to use on bind.
                    renderTargetView = ToDispose(new RenderTargetView(graphicsDevice, backBuffer));
                }

                // Create a viewport descriptor of the full window size.
                var viewport = new Viewport(0, 0, width, height, 0.0f, 1.0f);

                // Set the current viewport using the descriptor.
                graphicsDevice.ImmediateContext.Rasterizer.SetViewport(viewport);
            }
        }
 
        private class MiniCubeFactory : IFrameworkViewSource
        {
            public IFrameworkView CreateView()
            {
                return new MiniCube();
            }
        }
    }
}