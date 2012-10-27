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

using SharpDX.DXGI;
using SharpDX.Toolkit.Graphics;

using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.Graphics.Display;

namespace SharpDX.Toolkit
{
    /// <summary>
    /// An abstract window.
    /// </summary>
    internal class GameWindowWinRT : GameWindow, IFrameworkViewSource, IFrameworkView
    {
        private object nativeWindow;

        public CoreWindow CoreWindow;

        public bool IsSwapChainBackgroundPanel;

        private VoidAction initCallback;
        private VoidAction tickCallback;

        internal GameWindowWinRT()
        {
        }

        public override object NativeWindow
        {
            get
            {
                return nativeWindow;
            }
        }

        public override void BeginScreenDeviceChange(bool willBeFullScreen)
        {
            
        }

        public override void EndScreenDeviceChange(int clientWidth, int clientHeight)
        {
            
        }

        public override bool IsFullScreenMandatory
        {
            get
            {
                return true;
            }
        }

        protected internal override void SetSupportedOrientations(DisplayOrientation orientations)
        {
            // Desktop doesn't have orientation (unless on Windows 8?)
        }

        internal override void Initialize(object windowContext)
        {
            nativeWindow = windowContext;

            if (windowContext != null)
            {
                try
                {
                    // Just check that the window context is implementing ISwapChainBackgroundPanelNative
                    var nativePanel = ComObject.As<ISwapChainBackgroundPanelNative>(windowContext);
                    IsSwapChainBackgroundPanel = true;
                    nativePanel.Dispose();
                } catch (SharpDXException ex)
                {
                    throw new NotSupportedException(string.Format("Unsupported window context [{0}]. Only null or SwapChainBackgroundPanel", nativeWindow.GetType().FullName));
                }
            }
        }

        IFrameworkView IFrameworkViewSource.CreateView()
        {
            return this;
        }

        public override bool AllowUserResizing
        {
            get
            {
                return false;
            }
            set
            {
            }
        }

        public override DrawingRectangle ClientBounds
        {
            get
            {
                if (CoreWindow != null)
                {
                    return new DrawingRectangle(0, 0, (int)(CoreWindow.Bounds.Width * DisplayProperties.LogicalDpi / 96.0), (int)(CoreWindow.Bounds.Height * DisplayProperties.LogicalDpi / 96.0));
                }

                return DrawingRectangle.Empty;
            }
        }

        public override DisplayOrientation CurrentOrientation
        {
            get
            {
                return DisplayOrientation.Default;
            }
        }

        public override bool IsMinimized
        {
            get
            {
                return false;
            }
        }

        public void RunCoreWindow(VoidAction initCallback, VoidAction tickCallback)
        {
            this.initCallback = initCallback;
            this.tickCallback = tickCallback;

            CoreApplication.Run(this);
        }

        void IFrameworkView.Initialize(CoreApplicationView applicationView)
        {
            
        }

        void IFrameworkView.SetWindow(CoreWindow window)
        {
            nativeWindow = window;
            CoreWindow = window;

            // Call the init callback once the window is activated
            initCallback();
        }

        void IFrameworkView.Load(string entryPoint)
        {
        }

        void IFrameworkView.Run()
        {
            // Specify the cursor type as the standard arrow cursor.
            // CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Arrow, 0);

            // Activate the application window, making it visible and enabling it to receive events.
            CoreWindow.Activate();

            bool windowIsClosed = false;

            CoreWindow.Closed += (sender, args) => windowIsClosed = true;

            // Enter the render loop.  Note that Metro style apps should never exit.
            while (!windowIsClosed)
            {
                // Process events incoming to the window.
                CoreWindow.Dispatcher.ProcessEvents(CoreProcessEventsOption.ProcessAllIfPresent);

                tickCallback();
            }
        }

        void IFrameworkView.Uninitialize()
        {
        }
    }
}
#endif