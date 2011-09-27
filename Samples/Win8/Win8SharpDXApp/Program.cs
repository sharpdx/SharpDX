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
using System.Runtime.InteropServices;
using SharpDX;
using SharpDX.Direct3D;
//using SharpDX.D3DCompiler;
//using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.Infrastructure;
using Windows.UI.Core;
using Windows.UI.Xaml;
//using Buffer = SharpDX.Direct3D11.Buffer;
using Device = SharpDX.Direct3D11.Device;
using Device1 = SharpDX.Direct3D11.Device1;

namespace Win8SharpDXApp
{
    /// <summary>
    ///   SharpDX port of SharpDX-MiniTri Direct3D 11 Sample
    /// </summary>
    internal static class App
    {
        class SharpDXTutorialViewProvider  : IViewProvider
        {
            CoreWindow window;
            Device1 device;
            DeviceContext1 context;
            RenderTargetView renderView;
            SwapChain swapChain;

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
                // Enable compatibility with Direct2D
                var defaultDevice = new Device(DriverType.Hardware, DeviceCreationFlags.None);

                // Retrieve the Direct3D 11.1 device amd device context
                device = defaultDevice.QueryInterface<Device1>();

                context = defaultDevice.ImmediateContext.QueryInterface<DeviceContext1>();

                // After the D3D device is created, create additional application resources.
                CreateWindowSizeDependentResources();

                // Once all D3D resources are created, configure the application window.

                // Allow the application to respond when the window size changes.
                window.SizeChanged += OnWindowSizeChanged;

                // Specify the cursor type as the standard arrow cursor.
                window.PointerCursor = new CoreCursor(CoreCursorType.Arrow, 0);

                // Activate the application window, making it visible and enabling it to receive events.
                window.Activate();

                // Enter the render loop.  Note that Metro style apps should never exit.
                while (true)
                {
                    // Process events incoming to the window.
                    window.Dispatcher.ProcessEvents(CoreProcessEventsOption.ProcessAllIfPresent);

                    if (window.GetAsyncKeyState(Windows.System.VirtualKey.Escape) == CoreVirtualKeyStates.Down)
                        break;

                    // Specify the render target we created as the output target.
                    context.OutputMerger.SetTargets(renderView);

                    // Clear the render target to a solid color.
                    context.ClearRenderTargetView(renderView, new Color4(1.0f, 0.0f, 0.0f, 1.0f));

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
                    swapChain.ResizeBuffers(2, 0, 0, Format.R8G8B8A8_UNorm, 0);
                }
                else
                {
                    // SwapChain description
                    var desc = new SwapChainDescription1()
                    {
                        Width = 0,
                        // Automatic sizing
                        Height = 0,
                        BufferCount = 2,
                        // Use two buffers to enable flip effect.
                        SampleDescription = new SampleDescription(1, 0),
                        // Don't use multi-sampling.
                        Format = SharpDX.DXGI.Format.R8G8B8A8_UNorm,
                        SwapEffect = SwapEffect.FlipSequential,
                        Scaling = Scaling.None,
                        Usage = Usage.RenderTargetOutput
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
                context.Rasterizer.SetViewports(new Viewport(0, 0, backBufferDesc.Width, backBufferDesc.Height, 0.0f, 1.0f));
            }
        }

        class SharpDXTutorialViewProviderFactory : IViewProviderFactory
        {
            public IViewProvider CreateViewProvider()
            {
                return new SharpDXTutorialViewProvider();
            }
        }

        [MTAThread]
        private static void Main()
        {
            var viewFactory = new SharpDXTutorialViewProviderFactory();
            Windows.ApplicationModel.Core.CoreApplication.Run(viewFactory);
        }
    }
}