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

        public override string GetDefaultAppDirectory()
        {
            return Package.Current.InstalledLocation.Path;
        }

        public override GameWindow Window
        {
            get
            {
                return gameWindowWinRT;
            }
        }

        public override void Run(object windowContext, VoidAction initCallback, VoidAction tickCallback)
        {
            // If window context is null under WinRT, then this is a non-XAML application
            IsBlockingRun = windowContext == null;

            // Initialize the window
            Window.Initialize(windowContext);

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
            }
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