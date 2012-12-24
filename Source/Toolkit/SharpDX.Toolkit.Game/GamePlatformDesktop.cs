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
#if !W8CORE
using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

using SharpDX.Toolkit.Graphics;
using SharpDX.Windows;

namespace SharpDX.Toolkit
{
    internal class GamePlatformDesktop : GamePlatform
    {
        private readonly GameWindowDesktop gameMainWindowDesktop;

        public GamePlatformDesktop(IServiceRegistry services) : base(services)
        {
            IsBlockingRun = true;
            gameMainWindowDesktop = new GameWindowDesktop();
            services.AddService(typeof(IGraphicsDeviceFactory), this);
        }

        public override string DefaultAppDirectory
        {
            get
            {
                var assemblyUri = new Uri(Assembly.GetEntryAssembly().CodeBase);
                return Path.GetDirectoryName(assemblyUri.LocalPath);
            }
        }

        public override GameWindow MainWindow
        {
            get
            {
                return gameMainWindowDesktop;
            }
        }

        public override GameWindow CreateWindow(object windowContext = null, int width = 0, int height = 0)
        {
            var window = new GameWindowDesktop();
            window.Initialize(windowContext);

            width = width == 0 ? window.IsForm ? GraphicsDeviceManager.DefaultBackBufferWidth : window.Control.ClientSize.Width : width;
            height = height == 0 ? window.IsForm ? GraphicsDeviceManager.DefaultBackBufferHeight : window.Control.ClientSize.Height : height;
            window.Control.ClientSize = new System.Drawing.Size(width, height);
            return window;
        }

        public override void Run(object windowContext, VoidAction initCallback, VoidAction tickCallback)
        {
            // Initialize the window
            MainWindow.Initialize(windowContext);

            // Register on Activated 
            MainWindow.Activated += OnActivated;
            MainWindow.Deactivated += OnDeactivated;

            // Initialize the init callback
            initCallback();

            // Run the rendering loop
            try
            {
                RenderLoop.Run((Control)MainWindow.NativeWindow, new RenderLoop.RenderCallback(tickCallback));
            } 
            finally
            {
                OnExiting(this, EventArgs.Empty);
            }
        }

        public override void Exit()
        {
            gameMainWindowDesktop.Control.Dispose();

            base.Exit();
        }

        public override GraphicsDevice CreateDevice(GraphicsDeviceInformation deviceInformation)
        {
            gameMainWindowDesktop.Control.ClientSize = new System.Drawing.Size(deviceInformation.PresentationParameters.BackBufferWidth, deviceInformation.PresentationParameters.BackBufferHeight);
            return base.CreateDevice(deviceInformation);
        }
    }
}
#endif