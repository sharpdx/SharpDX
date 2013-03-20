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
    internal class GameWindowWinRTBackgroundXaml : GameWindow
    {
        #region Fields

        private SwapChainBackgroundPanel swapChainBackgroundPanel;

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
                return new Rectangle(0, 0, (int)(this.swapChainBackgroundPanel.ActualWidth * DisplayProperties.LogicalDpi / 96.0), (int)(this.swapChainBackgroundPanel.ActualHeight * DisplayProperties.LogicalDpi / 96.0));
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

        public override bool IsMouseVisible { get; set; }

        public override object NativeWindow
        {
            get
            {
                return swapChainBackgroundPanel;
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
            return windowContext.ContextType == GameContextType.WinRTBackgroundXaml;
        }

        internal override void Initialize(GameContext windowContext)
        {
            if (windowContext != null)
            {
                swapChainBackgroundPanel = windowContext.Control as SwapChainBackgroundPanel;
                if (swapChainBackgroundPanel == null)
                {
                    throw new NotSupportedException(string.Format("Unsupported window context [{0}]. Only  SwapChainBackgroundPanel",  windowContext.Control.GetType().FullName));
                }

                //clientBounds = new DrawingRectangle(0, 0, (int)swapChainBackgroundPanel.ActualWidth, (int)swapChainBackgroundPanel.ActualHeight);
                swapChainBackgroundPanel.SizeChanged += swapChainBackgroundPanel_SizeChanged;

            }
        }

        private void swapChainBackgroundPanel_SizeChanged(object sender, SizeChangedEventArgs e)
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