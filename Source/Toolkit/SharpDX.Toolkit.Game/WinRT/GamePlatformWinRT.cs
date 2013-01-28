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
#if WIN8METRO
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;

using SharpDX.DXGI;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.IO;
using SharpDX.Toolkit.Graphics;
using Windows.ApplicationModel;
using Windows.UI.Xaml.Media;

namespace SharpDX.Toolkit
{
    internal class GamePlatformWinRT : GamePlatform, IGraphicsDeviceFactory
    {
        private readonly GameWindowWinRT gameWindowWinRT;

        private VoidAction tickCallback;

        public GamePlatformWinRT(IServiceRegistry services) : base(services)
        {
            gameWindowWinRT = new GameWindowWinRT();
            services.AddService(typeof(IGraphicsDeviceFactory), this);
        }

        public override string DefaultAppDirectory
        {
            get
            {
                return Package.Current.InstalledLocation.Path;
            }
        }

        public override GameWindow MainWindow
        {
            get
            {
                return gameWindowWinRT;
            }
        }

        public override GameWindow CreateWindow(object windowContext = null, int width = 0, int height = 0)
        {
            throw new NotSupportedException();
        }

        public override void Run(object windowContext, VoidAction initCallback, VoidAction tickCallback)
        {
            // If window context is null under WinRT, then this is a non-XAML application
            IsBlockingRun = windowContext == null;

            // Initialize the window
            MainWindow.Initialize(windowContext);

            this.tickCallback = tickCallback;

            if (windowContext == null)
            {
                // Rendering to CoreWindow
                gameWindowWinRT.RunCoreWindow(initCallback, tickCallback);
            }
            else
            {
                // Rendering to SwapChainBackgroundPanel
                initCallback();
                CompositionTarget.Rendering += CompositionTarget_Rendering;
                gameWindowWinRT.ClientSizeChanged += gameWindowWinRT_ClientSizeChanged;
            }
        }

        void gameWindowWinRT_ClientSizeChanged(object sender, EventArgs e)
        {
            IGraphicsDeviceService deviceManager = (IGraphicsDeviceService)this.Services.GetService(typeof(IGraphicsDeviceService));

            if (deviceManager != null)
            {
                int newWidth = gameWindowWinRT.ClientBounds.Width;
                int newHeight = gameWindowWinRT.ClientBounds.Height;
                SharpDX.DXGI.Format newFormat = deviceManager.GraphicsDevice.Presenter.Description.BackBufferFormat;

                deviceManager.GraphicsDevice.Presenter.Resize(newWidth, newHeight, newFormat);
            }
        }

        public override List<GraphicsDeviceInformation> FindBestDevices(GameGraphicsParameters prefferedParameters)
        {
            var graphicsDeviceInfos = base.FindBestDevices(prefferedParameters);

            // Special case where the default FindBestDevices is not working
            if (graphicsDeviceInfos.Count == 0)
            {
                var graphicsAdapter = GraphicsAdapter.Adapters[0];

                // Iterate on each preferred graphics profile
                foreach (var featureLevel in prefferedParameters.PreferredGraphicsProfile)
                {
                    // Check if this profile is supported.
                    if (graphicsAdapter.IsProfileSupported(featureLevel))
                    {
                        var deviceInfo = new GraphicsDeviceInformation
                                             {
                                                 Adapter = graphicsAdapter,
                                                 GraphicsProfile = featureLevel,
                                                 PresentationParameters =
                                                     {
                                                         MultiSampleCount = MSAALevel.None,
                                                         IsFullScreen = prefferedParameters.IsFullScreen,
                                                         PresentationInterval = prefferedParameters.SynchronizeWithVerticalRetrace ? PresentInterval.One : PresentInterval.Immediate,
                                                         DeviceWindowHandle = MainWindow.NativeWindow,
                                                         RenderTargetUsage = Usage.BackBuffer | Usage.RenderTargetOutput
                                                     }
                                             };

                        // Hardcoded format and refresh rate...
                        // This is a workaround to allow this code to work inside the emulator
                        // but this is not really robust
                        // TODO: Check how to handle this case properly
                        var displayMode = new DisplayMode(DXGI.Format.B8G8R8A8_UNorm, gameWindowWinRT.ClientBounds.Width, gameWindowWinRT.ClientBounds.Height, new Rational(60, 1));
                        AddDevice(graphicsAdapter, displayMode, deviceInfo, prefferedParameters, graphicsDeviceInfos);

                        // If the profile is supported, we are just using the first best one
                        break;
                    }
                }
            }

            return graphicsDeviceInfos;
        }



        void CompositionTarget_Rendering(object sender, object e)
        {
            tickCallback();
        }

        public override void Exit()
        {
            if (gameWindowWinRT.CoreWindow != null)
            {
                gameWindowWinRT.CoreWindow.Close();
            }
            else if (gameWindowWinRT.IsSwapChainBackgroundPanel)
            {
                CompositionTarget.Rendering -= CompositionTarget_Rendering;
            }

            base.Exit();
        }
    }
}
#endif