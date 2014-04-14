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
using SharpDX.Toolkit.Graphics;

using Windows.UI.Core;
using Windows.Graphics.Display;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace SharpDX.Toolkit
{
    /// <summary>
    /// An abstract window.
    /// </summary>
    internal class GameWindowWinRTXaml : GameWindow
    {
        #region Fields

        private FrameworkElement surfaceControl;

        #endregion

        #region Public Properties

        public override bool AllowUserResizing
        {
            get
            {
                return true;
            }
            set
            {
            }
        }

        public override Rectangle ClientBounds
        {
            get
            {
                var swapChainPanel = surfaceControl as SwapChainPanel;
                if (swapChainPanel != null)
                {
                    return new Rectangle(0, 0, (int)(this.surfaceControl.ActualWidth * swapChainPanel.CompositionScaleX + 0.5f), (int)(this.surfaceControl.ActualHeight * swapChainPanel.CompositionScaleY + 0.5f));
                }
                return new Rectangle(0, 0, (int)(this.surfaceControl.ActualWidth * DisplayProperties.LogicalDpi / 96.0 + 0.5f), (int)(this.surfaceControl.ActualHeight * DisplayProperties.LogicalDpi / 96.0 + 0.5f));
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

        internal override bool IsBlockingRun { get { return false; } }

        public override bool IsMouseVisible { get; set; }

        public override object NativeWindow
        {
            get
            {
                return surfaceControl;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="GameWindow" /> is visible.
        /// </summary>
        /// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
        public override bool Visible
        {
            get
            {
                return true;
            }
            set
            {
            }
        }

        #endregion

        #region Public Methods and Operators

        public override void BeginScreenDeviceChange(bool willBeFullScreen)
        {
        }

        public override void EndScreenDeviceChange(int clientWidth, int clientHeight)
        {
        }

        #endregion

        #region Methods

        internal override bool CanHandle(GameContext windowContext)
        {
            return windowContext.ContextType == GameContextType.WinRTXaml;
        }

        internal override void Initialize(GameContext windowContext)
        {
            if (windowContext != null)
            {
                BindSurfaceControl(windowContext);
            }
        }

        private void SurfaceControlSizeChanged(object sender, SizeChangedEventArgs e)
        {
            OnClientSizeChanged(sender, EventArgs.Empty);
        }

        internal override void Resize(int width, int height)
        {

        }

        internal override void Run()
        {
            // Initialize Game
            InitCallback();

            // Perform the rendering loop
            CompositionTarget.Rendering += CompositionTarget_Rendering;
        }

        internal override void Switch(GameContext context)
        {
            surfaceControl.SizeChanged -= SurfaceControlSizeChanged;

            var swapChainPanel = surfaceControl as SwapChainPanel;
            if (swapChainPanel != null)
                swapChainPanel.CompositionScaleChanged -= SwapChainPanelCompositionScaleChanged;

            BindSurfaceControl(context);
        }

        private void BindSurfaceControl(GameContext windowContext)
        {
            surfaceControl = windowContext.Control as FrameworkElement;
            if (surfaceControl == null)
                throw new ArgumentException("A FrameworkElement expected.");

            surfaceControl.SizeChanged += SurfaceControlSizeChanged;

            var swapChainPanel = surfaceControl as SwapChainPanel;
            if (swapChainPanel != null)
                swapChainPanel.CompositionScaleChanged += SwapChainPanelCompositionScaleChanged;
        }

        private void SwapChainPanelCompositionScaleChanged(SwapChainPanel sender, object args)
        {
            OnClientSizeChanged(sender, EventArgs.Empty);
        }

        void CompositionTarget_Rendering(object sender, object e)
        {
            RunCallback();
        }

        protected internal override void SetSupportedOrientations(DisplayOrientation orientations)
        {
            // Desktop doesn't have orientation (unless on Windows 8?)
        }

        protected override void SetTitle(string title)
        {

        }

        protected override void Dispose(bool disposeManagedResources)
        {
            CompositionTarget.Rendering -= CompositionTarget_Rendering;


            base.Dispose(disposeManagedResources);
        }


        #endregion
    }
}

#endif