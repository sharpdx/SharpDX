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
using System.Threading;
using System.Threading.Tasks;
using CommonDX;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.IO;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Core;
using Windows.Graphics.Display;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.System.Threading;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace MiniCubeTexture
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
            CubeTextureRenderer cubeRenderer;
            CoreWindow window;
            private bool IsInitialized = false;

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
                cubeRenderer = ToDispose(new CubeTextureRenderer());
            }

            /// <inheritdoc/>
            public void Load(string entryPoint)
            {
            }

            private async void RunAsync()
            {
                var fileOpenPicker = new FileOpenPicker
                {
                    SuggestedStartLocation = PickerLocationId.VideosLibrary,
                    ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail
                };
                fileOpenPicker.CommitButtonText = "Open the Selected Video";

                fileOpenPicker.FileTypeFilter.Clear();
                fileOpenPicker.FileTypeFilter.Add(".mp4");
                fileOpenPicker.FileTypeFilter.Add(".m4v");
                fileOpenPicker.FileTypeFilter.Add(".mts");
                fileOpenPicker.FileTypeFilter.Add(".mov");
                fileOpenPicker.FileTypeFilter.Add(".wmv");
                fileOpenPicker.FileTypeFilter.Add(".avi");
                fileOpenPicker.FileTypeFilter.Add(".asf");

                var file = await fileOpenPicker.PickSingleFileAsync();

                cubeRenderer.VideoStream = await file.OpenAsync(FileAccessMode.Read);

                // Add Initializer to device manager
                deviceManager.OnInitialize += target.Initialize;
                deviceManager.OnInitialize += cubeRenderer.Initialize;

                // Render the cube within the CoreWindow
                target.OnRender += cubeRenderer.Render;

                // Initialize the device manager and all registered deviceManager.OnInitialize 
                deviceManager.Initialize(DisplayProperties.LogicalDpi);

                // Notifies the main loop that initialization is done
                lock (this)
                {
                    IsInitialized = true;
                }
            }

            /// <inheritdoc/>
            public void Run()
            {
                DisplayProperties.LogicalDpiChanged += DisplayProperties_LogicalDpiChanged;

                // Specify the cursor type as the standard arrow cursor.
                window.PointerCursor = new CoreCursor(CoreCursorType.Arrow, 0);

                // Activate the application window, making it visible and enabling it to receive events.
                window.Activate();

                // Run this method async
                // But because It was not possible to use correctly async here
                // we have to use a lock approach, which is far from ideal, but it is at least working.
                // The problem is described here: http://social.msdn.microsoft.com/Forums/mr-IN/winappswithcsharp/thread/d09dd944-f92b-484d-b2ef-d1850c4a587f
                RunAsync();

                // Enter the render loop.  Note that Metro style apps should never exit.
                while (true)
                {
                    // Process events incoming to the window.
                    window.Dispatcher.ProcessEvents(CoreProcessEventsOption.ProcessAllIfPresent);

                    if (window.GetAsyncKeyState(Windows.System.VirtualKey.Escape) == CoreVirtualKeyStates.Down)
                        break;

                    bool isInitOk = false;

                    // Check if initialization is done
                    lock (this)
                    {
                        isInitOk = IsInitialized;
                    }

                    // If done, perform rendering
                    if (isInitOk)
                    {
                        // Render the cube
                        target.RenderAll();

                        // Present the cube
                        target.Present();
                    }
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