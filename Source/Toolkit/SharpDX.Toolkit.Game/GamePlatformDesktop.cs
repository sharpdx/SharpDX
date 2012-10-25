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
#if !WIN8METRO
using System.Collections.Generic;
using System.Windows.Forms;

using SharpDX.DXGI;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.Toolkit.Graphics;
using SharpDX.Windows;

namespace SharpDX.Toolkit
{
    internal class GamePlatformDesktop : GamePlatform, IGraphicsDeviceFactory
    {
        private readonly GameWindowDesktop gameWindowDesktop;

        private bool isMouseVisible;

        public GamePlatformDesktop(IServiceRegistry services) : base(services)
        {
            IsBlockingRun = true;
            gameWindowDesktop = new GameWindowDesktop();
            services.AddService(typeof(IGraphicsDeviceFactory), this);
        }

        public override GameWindow Window
        {
            get
            {
                return gameWindowDesktop;
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

        public override void Run()
        {
            RenderLoop.Run((Control)Window.NativeWindow, OnTick);
        }

        public override void Exit()
        {
            gameWindowDesktop.Control.Dispose();

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
                                    DeviceWindowHandle = gameWindowDesktop.Control
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
            device.Presenter = GraphicsPresenter.New(device, deviceInformation.PresentationParameters);
            gameWindowDesktop.Control.ClientSize = new System.Drawing.Size(deviceInformation.PresentationParameters.BackBufferWidth, deviceInformation.PresentationParameters.BackBufferHeight);
            return device;
        }
    }
}
#endif