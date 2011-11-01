// Copyright (c) 2010-2011 SharpDX - Alexandre Mutel
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
using System.Reflection;
using System.Runtime.InteropServices;
using SharpDX;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.IO;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.Infrastructure;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Device = SharpDX.Direct3D11.Device;
using Device1 = SharpDX.Direct3D11.Device1;

namespace Win8MiniCube
{
    /// <summary>
    ///   SharpDX port of SharpDX-MiniCube Direct3D 11 Sample
    /// </summary>
    internal static class App
    {
        class SharpDXMiniCubeViewProvider : IViewProvider
        {
            CoreWindow window;
            Device1 device;
            DeviceContext1 context;
            RenderTargetView renderView;
            SwapChain swapChain;
            Texture2D depthBuffer;
            DepthStencilView depthView;

            int width;
            int height;

            /// <inheritdoc/>
            public void Initialize(CoreWindow window, CoreApplicationView applicationView)
            {
                this.window = window;
            }

            /// <inheritdoc/>
            public void Load(string entryPoint)
            {

            }

            /// <inheritdoc/>
            public void Run()
            {
                var path = Windows.ApplicationModel.Package.Current.InstalledLocation.Path;

                // Enable compatibility with Direct2D
                var defaultDevice = new Device(DriverType.Hardware, DeviceCreationFlags.BgraSupport);

                // Retrieve the Direct3D 11.1 device amd device context
                device = defaultDevice.QueryInterface<Device1>();

                context = defaultDevice.ImmediateContext.QueryInterface<DeviceContext1>();

                // Compile Vertex and Pixel shaders
                // Because d3dcompiler_44.dll is not in the path, use precompiled fx files
                // var vertexShaderByteCode = ShaderBytecode.CompileFromFile("MiniCube.fx", "VS", "vs_4_0", ShaderFlags.None, EffectFlags.None);
                // vertexShaderByteCode.Save("MiniCube_VS.fxo");
                ShaderBytecode vertexShaderByteCode;
                using (var stream = new NativeFileStream(path + "\\MiniCube_VS.fxo", NativeFileMode.Open, NativeFileAccess.Read))
                    vertexShaderByteCode = ShaderBytecode.Load(stream);
                var vertexShader = new VertexShader(device, vertexShaderByteCode);

                // Because d3dcompiler_44.dll is not in the path, use precompiled fx files
                // var pixelShaderByteCode = ShaderBytecode.CompileFromFile("MiniCube.fx", "PS", "ps_4_0", ShaderFlags.None, EffectFlags.None);
                // pixelShaderByteCode.Save("MiniCube_PS.fxo");
                ShaderBytecode pixelShaderByteCode;
                using (var stream = new NativeFileStream(path + "\\MiniCube_PS.fxo", NativeFileMode.Open, NativeFileAccess.Read))
                    pixelShaderByteCode = ShaderBytecode.Load(stream);
                var pixelShader = new PixelShader(device, pixelShaderByteCode); 

                // Layout from VertexShader input signature
                var layout = new InputLayout(device, vertexShaderByteCode, new[]
                    {
                        new InputElement("POSITION", 0, Format.R32G32B32A32_Float, 0, 0),
                        new InputElement("COLOR", 0, Format.R32G32B32A32_Float, 16, 0)
                    });

                // After the D3D device is created, create additional application resources.
                CreateWindowSizeDependentResources();

                // Instantiate Vertex buffer from vertex data
                var vertices = Buffer.Create(device, BindFlags.VertexBuffer, new[]
                                  {
                                      new Vector4(-1.0f, -1.0f, -1.0f, 1.0f), new Vector4(1.0f, 0.0f, 0.0f, 1.0f), // Front
                                      new Vector4(-1.0f,  1.0f, -1.0f, 1.0f), new Vector4(1.0f, 0.0f, 0.0f, 1.0f),
                                      new Vector4( 1.0f,  1.0f, -1.0f, 1.0f), new Vector4(1.0f, 0.0f, 0.0f, 1.0f),
                                      new Vector4(-1.0f, -1.0f, -1.0f, 1.0f), new Vector4(1.0f, 0.0f, 0.0f, 1.0f),
                                      new Vector4( 1.0f,  1.0f, -1.0f, 1.0f), new Vector4(1.0f, 0.0f, 0.0f, 1.0f),
                                      new Vector4( 1.0f, -1.0f, -1.0f, 1.0f), new Vector4(1.0f, 0.0f, 0.0f, 1.0f),

                                      new Vector4(-1.0f, -1.0f,  1.0f, 1.0f), new Vector4(0.0f, 1.0f, 0.0f, 1.0f), // BACK
                                      new Vector4( 1.0f,  1.0f,  1.0f, 1.0f), new Vector4(0.0f, 1.0f, 0.0f, 1.0f),
                                      new Vector4(-1.0f,  1.0f,  1.0f, 1.0f), new Vector4(0.0f, 1.0f, 0.0f, 1.0f),
                                      new Vector4(-1.0f, -1.0f,  1.0f, 1.0f), new Vector4(0.0f, 1.0f, 0.0f, 1.0f),
                                      new Vector4( 1.0f, -1.0f,  1.0f, 1.0f), new Vector4(0.0f, 1.0f, 0.0f, 1.0f),
                                      new Vector4( 1.0f,  1.0f,  1.0f, 1.0f), new Vector4(0.0f, 1.0f, 0.0f, 1.0f),

                                      new Vector4(-1.0f, 1.0f, -1.0f,  1.0f), new Vector4(0.0f, 0.0f, 1.0f, 1.0f), // Top
                                      new Vector4(-1.0f, 1.0f,  1.0f,  1.0f), new Vector4(0.0f, 0.0f, 1.0f, 1.0f),
                                      new Vector4( 1.0f, 1.0f,  1.0f,  1.0f), new Vector4(0.0f, 0.0f, 1.0f, 1.0f),
                                      new Vector4(-1.0f, 1.0f, -1.0f,  1.0f), new Vector4(0.0f, 0.0f, 1.0f, 1.0f),
                                      new Vector4( 1.0f, 1.0f,  1.0f,  1.0f), new Vector4(0.0f, 0.0f, 1.0f, 1.0f),
                                      new Vector4( 1.0f, 1.0f, -1.0f,  1.0f), new Vector4(0.0f, 0.0f, 1.0f, 1.0f),

                                      new Vector4(-1.0f,-1.0f, -1.0f,  1.0f), new Vector4(1.0f, 1.0f, 0.0f, 1.0f), // Bottom
                                      new Vector4( 1.0f,-1.0f,  1.0f,  1.0f), new Vector4(1.0f, 1.0f, 0.0f, 1.0f),
                                      new Vector4(-1.0f,-1.0f,  1.0f,  1.0f), new Vector4(1.0f, 1.0f, 0.0f, 1.0f),
                                      new Vector4(-1.0f,-1.0f, -1.0f,  1.0f), new Vector4(1.0f, 1.0f, 0.0f, 1.0f),
                                      new Vector4( 1.0f,-1.0f, -1.0f,  1.0f), new Vector4(1.0f, 1.0f, 0.0f, 1.0f),
                                      new Vector4( 1.0f,-1.0f,  1.0f,  1.0f), new Vector4(1.0f, 1.0f, 0.0f, 1.0f),

                                      new Vector4(-1.0f, -1.0f, -1.0f, 1.0f), new Vector4(1.0f, 0.0f, 1.0f, 1.0f), // Left
                                      new Vector4(-1.0f, -1.0f,  1.0f, 1.0f), new Vector4(1.0f, 0.0f, 1.0f, 1.0f),
                                      new Vector4(-1.0f,  1.0f,  1.0f, 1.0f), new Vector4(1.0f, 0.0f, 1.0f, 1.0f),
                                      new Vector4(-1.0f, -1.0f, -1.0f, 1.0f), new Vector4(1.0f, 0.0f, 1.0f, 1.0f),
                                      new Vector4(-1.0f,  1.0f,  1.0f, 1.0f), new Vector4(1.0f, 0.0f, 1.0f, 1.0f),
                                      new Vector4(-1.0f,  1.0f, -1.0f, 1.0f), new Vector4(1.0f, 0.0f, 1.0f, 1.0f),

                                      new Vector4( 1.0f, -1.0f, -1.0f, 1.0f), new Vector4(0.0f, 1.0f, 1.0f, 1.0f), // Right
                                      new Vector4( 1.0f,  1.0f,  1.0f, 1.0f), new Vector4(0.0f, 1.0f, 1.0f, 1.0f),
                                      new Vector4( 1.0f, -1.0f,  1.0f, 1.0f), new Vector4(0.0f, 1.0f, 1.0f, 1.0f),
                                      new Vector4( 1.0f, -1.0f, -1.0f, 1.0f), new Vector4(0.0f, 1.0f, 1.0f, 1.0f),
                                      new Vector4( 1.0f,  1.0f, -1.0f, 1.0f), new Vector4(0.0f, 1.0f, 1.0f, 1.0f),
                                      new Vector4( 1.0f,  1.0f,  1.0f, 1.0f), new Vector4(0.0f, 1.0f, 1.0f, 1.0f),
                            });

                // Create Constant Buffer
                var contantBuffer = new Buffer(device, Utilities.SizeOf<Matrix>(), ResourceUsage.Default, BindFlags.ConstantBuffer, CpuAccessFlags.None, ResourceOptionFlags.None, 0);

                // Setup all stages that are constant for this scene
                context.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(vertices, Utilities.SizeOf<Vector4>() * 2, 0));
                context.InputAssembler.InputLayout = layout;
                context.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
                context.VertexShader.SetConstantBuffer(0, contantBuffer);
                context.VertexShader.Set(vertexShader);
                context.PixelShader.Set(pixelShader);

                // Prepare matrices
                var view = Matrix.LookAtLH(new Vector3(0, 0, -5), new Vector3(0, 0, 0), Vector3.UnitY);
                var proj = Matrix.PerspectiveFovLH((float)Math.PI / 4.0f, width / (float)height, 0.1f, 100.0f);
                var viewProj = Matrix.Multiply(view, proj);

                // Once all D3D resources are created, configure the application window.

                // Allow the application to respond when the window size changes.
                window.SizeChanged += OnWindowSizeChanged;

                // Specify the cursor type as the standard arrow cursor.
                window.PointerCursor = new CoreCursor(CoreCursorType.Arrow, 0);

                // Activate the application window, making it visible and enabling it to receive events.
                window.Activate();

                // Use clock
                var clock = new Stopwatch();
                clock.Start();

                // Enter the render loop.  Note that Metro style apps should never exit.
                while (true)
                {
                    // Process events incoming to the window.
                    window.Dispatcher.ProcessEvents(CoreProcessEventsOption.ProcessAllIfPresent);

                    if (window.GetAsyncKeyState(Windows.System.VirtualKey.Escape) == CoreVirtualKeyStates.Down)
                        break;

                    var time = clock.ElapsedMilliseconds / 1000.0f;

                    // Set targets (This is mandatory in the loop)
                    context.OutputMerger.SetTargets(depthView, renderView);

                    // Clear the views
                    context.ClearRenderTargetView(renderView, new Color4(1.0f, 0.0f, 0.0f, 0.0f));
                    context.ClearDepthStencilView(depthView, DepthStencilClearFlags.Depth, 1.0f, 0);

                    // Calculate WorldViewProj
                    var worldViewProj = Matrix.RotationX(time) * Matrix.RotationY(time * 2.0f) * Matrix.RotationZ(time * .7f) * viewProj;
                    worldViewProj.Transpose();

                    // Update Constant Buffer
                    context.UpdateSubresource(ref worldViewProj, contantBuffer, 0);

                    // Draw the cube
                    context.Draw(36, 0);

                    // Present the rendered image to the window.  Because the maximum frame latency is set to 1,
                    // the render loop will generally be throttled to the screen refresh rate, typically around
                    // 60Hz, by sleeping the application on Present until the screen is refreshed.
                    swapChain.Present(1, PresentFlags.None);
                }          
            }

            /// <inheritdoc/>
            public void Uninitialize()
            {

            }

            // This method is called whenever the application window size changes.
            void OnWindowSizeChanged(
                CoreWindow sender,
                WindowSizeChangedEventArgs args
                )
            {
                renderView = null;
                CreateWindowSizeDependentResources();
            }

            // This method creates all application resources that depend on
            // the application window size.  It is called at app initialization,
            // and whenever the application window size changes.
            void CreateWindowSizeDependentResources()
            {
                if (swapChain != null)
                {
                    swapChain.ResizeBuffers(2, 0, 0, Format.B8G8R8A8_UNorm, 0);
                }
                else
                {
                    // SwapChain description
                    var desc = new SwapChainDescription1()
                    {
                        // Automatic sizing
                        Width = 0,
                        Height = 0,
                        Format = SharpDX.DXGI.Format.B8G8R8A8_UNorm,
                        Stereo = false,
                        // Use two buffers to enable flip effect.
                        SampleDescription = new SampleDescription(1, 0),
                        Usage = Usage.RenderTargetOutput,
                        BufferCount = 2,
                        SwapEffect = SwapEffect.FlipSequential,
                    };

                    var dxgiDevice2 = device.QueryInterface<Device2>();

                    dxgiDevice2.MaximumFrameLatency = 1;

                    var dxgiAdapter = dxgiDevice2.Adapter;
                    var dxgiFactory2 = dxgiAdapter.GetParent<Factory2>();

                    var pCom = new ComObject(Marshal.GetIUnknownForObject(window));
                    
                    // Creates the swap chain
                    swapChain = dxgiFactory2.CreateSwapChainForImmersiveWindow(device, pCom, ref desc, null);
                }

                // New RenderTargetView from the backbuffer
                var backBuffer = Texture2D.FromSwapChain<Texture2D>(swapChain, 0);
                renderView = new RenderTargetView(device, backBuffer);

                var backBufferDesc = backBuffer.Description;

                // Create Depth Buffer & View
                depthBuffer = new Texture2D(device, new Texture2DDescription()
                {
                    Format = Format.D24_UNorm_S8_UInt,
                    ArraySize = 1,
                    MipLevels = 1,
                    Width = backBufferDesc.Width,
                    Height = backBufferDesc.Height,
                    SampleDescription = new SampleDescription(1, 0),
                    BindFlags = BindFlags.DepthStencil,
                });

                depthView = new DepthStencilView(device, depthBuffer, new DepthStencilViewDescription() { Dimension = DepthStencilViewDimension.Texture2D });

                context.Rasterizer.SetViewports(new Viewport(0, 0, backBufferDesc.Width, backBufferDesc.Height, 0.0f, 1.0f));

                width = backBufferDesc.Width;
                height = backBufferDesc.Height;
            }
        }

        class SharpDXMiniCubeViewProviderFactory : IViewProviderFactory
        {
            public IViewProvider CreateViewProvider()
            {
                return new SharpDXMiniCubeViewProvider();
            }
        }

        [MTAThread]
        private static void Main()
        {
            var viewFactory = new SharpDXMiniCubeViewProviderFactory();
            Windows.ApplicationModel.Core.CoreApplication.Run(viewFactory);
        }
    }
}