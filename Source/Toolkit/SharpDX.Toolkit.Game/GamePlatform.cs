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
using System.Collections.Generic;

using SharpDX.DXGI;
using SharpDX.Direct3D;
using SharpDX.Toolkit.Graphics;

namespace SharpDX.Toolkit
{
    internal abstract class GamePlatform : IGraphicsDeviceFactory
    {
        protected IServiceRegistry Services;

        protected GamePlatform(IServiceRegistry services)
        {
            Services = services;
        }

        public static GamePlatform Create(IServiceRegistry services)
        {
#if WIN8METRO
            return new GamePlatformWinRT(services);
#elif WP8
            return new GamePlatformWP8(services);
#else
            return new GamePlatformDesktop(services);
#endif
        }

        public abstract string GetDefaultAppDirectory();

        public object WindowContext { get; set; }

        public event EventHandler<EventArgs> Activated;

        public event EventHandler<EventArgs> Deactivated;

        public event EventHandler<EventArgs> Exiting;

        public event EventHandler<EventArgs> Idle;

        public event EventHandler<EventArgs> Resume;

        public event EventHandler<EventArgs> Suspend;

        public abstract GameWindow Window { get; }

        public bool IsBlockingRun { get; protected set; }

        public abstract bool IsMouseVisible { get; set; }

        public abstract void Run(object windowContext, VoidAction initCallback, VoidAction tickCallback);

        public virtual void Exit()
        {
            Activated = null;
            Deactivated = null;
            Exiting = null;
            Idle = null;
            Resume = null;
            Suspend = null;
        }
        
        protected void OnActivated(EventArgs e)
        {
            EventHandler<EventArgs> handler = Activated;
            if (handler != null) handler(this, e);
        }

        protected void OnDeactivated(EventArgs e)
        {
            EventHandler<EventArgs> handler = Deactivated;
            if (handler != null) handler(this, e);
        }

        protected void OnExiting(EventArgs e)
        {
            EventHandler<EventArgs> handler = Exiting;
            if (handler != null) handler(this, e);
        }

        protected void OnIdle(EventArgs e)
        {
            EventHandler<EventArgs> handler = Idle;
            if (handler != null) handler(this, e);
        }

        protected void OnResume(EventArgs e)
        {
            EventHandler<EventArgs> handler = Resume;
            if (handler != null) handler(this, e);
        }

        protected void OnSuspend(EventArgs e)
        {
            EventHandler<EventArgs> handler = Suspend;
            if (handler != null) handler(this, e);
        }

        public virtual List<GraphicsDeviceInformation> FindBestDevices()
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
                            DeviceWindowHandle = Window.NativeWindow
                        }
                    };

                    graphicsDeviceInfos.Add(deviceInfo);
                }
            }
            return graphicsDeviceInfos;
        }

        public virtual GraphicsDevice CreateDevice(GraphicsDeviceInformation deviceInformation)
        {
            var device = GraphicsDevice.New(deviceInformation.Adapter);
            device.Presenter = new SwapChainGraphicsPresenter(device, deviceInformation.PresentationParameters);
            return device;
        }
    }
}