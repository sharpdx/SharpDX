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
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using CommonDX;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.IO;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Core;
using Windows.Graphics.Display;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace MiniCube
{
    /// <summary>
    ///   SharpDX port of SharpDX-MiniCube Direct3D 11 Sample
    /// </summary>
    internal static class App
    {
        class SharpDXMiniCubeViewProvider : Component, IFrameworkView
        {
            DeviceManager deviceManager;
            CoreWindowTarget target;
            CubeRenderer cubeRenderer;
            CoreWindow window;

            public SharpDXMiniCubeViewProvider()
            {
            }

            /// <inheritdoc/>
            public void Initialize(CoreApplicationView applicationView)
            {
            }

            /// <inheritdoc/>
            public void SetWindow(CoreWindow window)
            {
                this.window = window;

                // Safely dispose any previous instance
                RemoveAndDispose(ref deviceManager);
                RemoveAndDispose(ref target);
                RemoveAndDispose(ref cubeRenderer);

                // Creates a new DeviceManager (Direct3D, Direct2D, DirectWrite, WIC)
                deviceManager = ToDispose(new DeviceManager());

                // Use CoreWindowTarget as the rendering target (Initialize SwapChain, RenderTargetView, DepthStencilView, BitmapTarget)
                target = ToDispose(new CoreWindowTarget(window));

                // New CubeRenderer
                cubeRenderer = ToDispose(new CubeRenderer());
                var fpsRenderer = new FpsRenderer();

                // Add Initializer to device manager
                deviceManager.OnInitialize += target.Initialize;
                deviceManager.OnInitialize += cubeRenderer.Initialize;
                deviceManager.OnInitialize += fpsRenderer.Initialize;

                // Render the cube within the CoreWindow
                target.OnRender += cubeRenderer.Render;
                target.OnRender += fpsRenderer.Render;

                // Initialize the device manager and all registered deviceManager.OnInitialize 
                deviceManager.Initialize(DisplayProperties.LogicalDpi);
            }

            /// <inheritdoc/>
            public void Load(string entryPoint)
            {
            }

            /// <inheritdoc/>
            public void Run()
            {
                DisplayProperties.LogicalDpiChanged += DisplayProperties_LogicalDpiChanged;

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

                    // Render the cube
                    target.RenderAll();

                    // Present the cube
                    target.Present();
                }          
            }

            void DisplayProperties_LogicalDpiChanged(object sender)
            {
                deviceManager.Dpi = DisplayProperties.LogicalDpi;
            }

            public void Uninitialize()
            {
            }
        }

        class SharpDXMiniCubeViewProviderFactory : IFrameworkViewSource
        {
            public IFrameworkView CreateView()
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