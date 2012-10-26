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
#if WIN8METRO
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using SharpDX.DXGI;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.Toolkit.Graphics;
using Windows.ApplicationModel;

namespace SharpDX.Toolkit
{
    internal class GamePlatformWinRT : GamePlatform, IGraphicsDeviceFactory
    {
        private readonly GameWindowWinRT gameWindowWinRT;

        private bool isMouseVisible;

        public GamePlatformWinRT(IServiceRegistry services) : base(services)
        {
            gameWindowWinRT = new GameWindowWinRT();
            services.AddService(typeof(IGraphicsDeviceFactory), this);
        }

        public override string GetDefaultAppDirectory()
        {
            return Path.GetDirectoryName(Package.Current.InstalledLocation.Path);
        }

        public override GameWindow Window
        {
            get
            {
                return gameWindowWinRT;
            }
        }

        public override bool IsMouseVisible
        {
            get
            {
                return isMouseVisible;
            }

            set
            {
                isMouseVisible = value;
            }
        }

        public override void Run(object windowContext, VoidAction initCallback, VoidAction tickCallback)
        {
            // If window context is null under WinRT, then this is a non-XAML application
            IsBlockingRun = windowContext == null;

            // Initialize the window
            Window.Initialize(windowContext);

            if (windowContext == null)
            {
                gameWindowWinRT.RunCoreWindow(initCallback, tickCallback);
            }
        }

        public override void Exit()
        {
            if (gameWindowWinRT.CoreWindow != null)
            {
                gameWindowWinRT.CoreWindow.Close();
            }

            base.Exit();
        }

        public List<GraphicsDeviceInformation> FindBestDevices()
        {
            var graphicsDeviceInfos = new List<GraphicsDeviceInformation>();
            foreach (var graphicsAdapter in GraphicsAdapter.Adapters)
            {
                // Get display mode for the particular width, height, pixelformat
                foreach (var displayMode in graphicsAdapter.SupportedDisplayModes)
                {
                    var deviceInfo = new GraphicsDeviceInformation
                        {
                            Adapter = graphicsAdapter,
                            GraphicsProfile = FeatureLevel.Level_11_0,
                            PresentationParameters =
                                {
                                    BackBufferWidth = displayMode.Width,
                                    BackBufferHeight = displayMode.Height,
                                    BackBufferFormat = displayMode.Format,
                                    RefreshRate = displayMode.RefreshRate,
                                    PresentationInterval = PresentInterval.Default,
                                    RenderTargetUsage = Usage.BackBuffer | Usage.RenderTargetOutput,
                                    DeviceWindowHandle = gameWindowWinRT.CoreWindow,
                                    IsFullScreen = true,
                                }
                        };

                    graphicsDeviceInfos.Add(deviceInfo);
                }
            }
            return graphicsDeviceInfos;
        }

        public GraphicsDevice CreateDevice(GraphicsDeviceInformation deviceInformation)
        {
            var device = GraphicsDevice.New(deviceInformation.Adapter);
            device.Presenter = new SwapChainGraphicsPresenter(device, deviceInformation.PresentationParameters);
            return device;
        }
    }
}
#endif