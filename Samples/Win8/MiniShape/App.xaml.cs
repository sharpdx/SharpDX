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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommonDX;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227

namespace MiniShape
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        DeviceManager deviceManager;
        SwapChainBackgroundPanelTarget target;
        ShapeRenderer renderer;

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
       public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
            {
                //TODO: Load state from previously suspended application
            }

            // Place the frame in the current Window and ensure that it is active
            var swapChainPanel = new MainPage();
            Window.Current.Content = swapChainPanel;
            Window.Current.Activate();

            // Safely dispose any previous instance
            // Creates a new DeviceManager (Direct3D, Direct2D, DirectWrite, WIC)
            deviceManager = new DeviceManager();

            // New CubeRenderer
            renderer = new ShapeRenderer();
            var fpsRenderer = new FpsRenderer();

            // Use CoreWindowTarget as the rendering target (Initialize SwapChain, RenderTargetView, DepthStencilView, BitmapTarget)
            target = new SwapChainBackgroundPanelTarget(swapChainPanel);

            // Add Initializer to device manager
            deviceManager.OnInitialize += target.Initialize;
            deviceManager.OnInitialize += renderer.Initialize;
            deviceManager.OnInitialize += fpsRenderer.Initialize;

            // Render the cube within the CoreWindow
            target.OnRender += renderer.Render;
            target.OnRender += fpsRenderer.Render;

            // Initialize the device manager and all registered deviceManager.OnInitialize 
            deviceManager.Initialize(DisplayProperties.LogicalDpi);

            // Setup rendering callback
            CompositionTarget.Rendering += CompositionTarget_Rendering;

            // Callback on DpiChanged
            DisplayProperties.LogicalDpiChanged += DisplayProperties_LogicalDpiChanged;
        }

        void DisplayProperties_LogicalDpiChanged(object sender)
        {
            deviceManager.Dpi = DisplayProperties.LogicalDpi;
        }

        void CompositionTarget_Rendering(object sender, object e)
        {
            target.RenderAll();
            target.Present();
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        void OnSuspending(object sender, SuspendingEventArgs e)
        {
            //TODO: Save application state and stop any background activity
        }
    }
}
