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
#if WP8
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
    internal class GamePlatformWP8 : GamePlatform, IGraphicsDeviceFactory
    {
        private readonly GameWindowWP8 gameWindowWP8;

        private VoidAction tickCallback;

        public GamePlatformWP8(IServiceRegistry services) : base(services)
        {
            gameWindowWP8 = new GameWindowWP8();
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
                return gameWindowWP8;
            }
        }

        public override GameWindow CreateWindow(object windowContext = null, int width = 0, int height = 0)
        {
            throw new NotSupportedException();
        }

        public override void Run(GameContext context)
        {
            // If window context is null under WinRT, then this is a non-XAML application
            IsBlockingRun = false;

            // Initialize the window
            MainWindow.Initialize(context);

            MainWindow.Run();

            // Rendering to CoreWindow
            gameWindowWP8.RunDrawingSurfaceBackground(initCallback, tickCallback);
        }

        public override void Exit()
        {
            // Notify the WP8 window to shutdown
            gameWindowWP8.Exiting = true;
            base.Exit();
        }

        public override List<GraphicsDeviceInformation> FindBestDevices(GameGraphicsParameters prefferedParameters)
        {
            // Unlike Desktop and WinRT, the list of best devices are completely fixed in WP8 XAML
            // So we return a single element
            var deviceInfo = new GraphicsDeviceInformation
                {
                    Adapter = gameWindowWP8.GraphicsDevice.Adapter,
                    GraphicsProfile = gameWindowWP8.GraphicsDevice.Features.Level,
                    PresentationParameters = gameWindowWP8.GraphicsDevice.Presenter.Description
                };

            return new List<GraphicsDeviceInformation>() { deviceInfo };
        }

        public override GraphicsDevice CreateDevice(GraphicsDeviceInformation deviceInformation)
        {
            // We don't have anything else than the GraphicsDevice created for the XAML so return it directly.
            return gameWindowWP8.GraphicsDevice;
        }
    }
}
#endif