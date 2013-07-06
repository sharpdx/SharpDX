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
#if !W8CORE && NET35Plus && !DIRECTX11_1

using SharpDX.Toolkit.Graphics;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace SharpDX.Toolkit
{
    using System.Windows.Threading;
    using Direct3D11;

    internal class GameWindowDesktopWpf : GameWindow
    {
        private SharpDXElement element;
        private DispatcherOperation previousRenderCall;
        private readonly Action renderDelegate; //delegate cache to avoid garbage generation

        private bool isMouseVisible;
        private bool isMouseCurrentlyHidden;
        private bool isVisible;
        private Cursor oldCursor;
        private RenderTargetGraphicsPresenter presenter;

        public override bool AllowUserResizing { get { return false; } set { /* ignore, WPF will resize everything itself */ } }
        public override Rectangle ClientBounds { get { return new Rectangle(0, 0, (int)element.ActualWidth, (int)element.ActualHeight); } }
        public override DisplayOrientation CurrentOrientation { get { return DisplayOrientation.Default; } }
        public override bool IsMinimized { get { return false; } }
        public override object NativeWindow { get { return element; } }

        public GameWindowDesktopWpf()
        {
            renderDelegate = RenderLoopCallback;
        }

        public override bool IsMouseVisible
        {
            get { return isMouseVisible; }
            set
            {
                if (isMouseVisible == value) return;

                isMouseVisible = value;
                if (isMouseVisible)
                    TryShowMouse();
                else
                    TryHideMouse();
            }
        }

        public override bool Visible
        {
            get { return isVisible; }
            set
            {
                if (isVisible == value) return;
                isVisible = value;
                element.Visibility = isVisible ? Visibility.Visible : Visibility.Hidden;
            }
        }

        public override void BeginScreenDeviceChange(bool willBeFullScreen)
        {
        }

        public override void EndScreenDeviceChange(int clientWidth, int clientHeight)
        {
            if (element != null)
            {
                element.TrySetSize(clientWidth, clientHeight);
                element.SetBackbuffer(presenter.BackBuffer);
            }
        }

        internal override bool CanHandle(GameContext gameContext)
        {
            if (gameContext == null) throw new ArgumentNullException("gameContext");

            return gameContext.ContextType == GameContextType.DesktopWpf;
        }

        internal override GraphicsPresenter CreateGraphicsPresenter(GraphicsDevice device, PresentationParameters parameters)
        {
            var backbufferDesc = RenderTarget2D.CreateDescription(device,
                                                              parameters.BackBufferWidth,
                                                              parameters.BackBufferHeight,
                                                              Graphics.PixelFormat.B8G8R8A8.UNorm);

            // mandatory to share the surface with D3D9
            backbufferDesc.OptionFlags |= ResourceOptionFlags.Shared;

            if (presenter != null)
                RemoveAndDispose(ref presenter);

            presenter = ToDispose(new RenderTargetGraphicsPresenter(device, backbufferDesc, parameters.DepthStencilFormat, false, true));
            element.SetBackbuffer(presenter.BackBuffer);
            return presenter;
        }

        internal override void Initialize(GameContext gameContext)
        {
            if (gameContext == null) throw new ArgumentNullException("gameContext");

            element = gameContext.Control as SharpDXElement;
            if (element == null) throw new ArgumentException("Only SharpDXElement is supported at this time", "gameContext");

            var width = gameContext.RequestedWidth;
            if (width <= 0)
                width = GraphicsDeviceManager.DefaultBackBufferWidth;

            var height = gameContext.RequestedHeight;
            if (height <= 0)
                height = GraphicsDeviceManager.DefaultBackBufferHeight;

            element.TrySetSize(width, height);

            element.ResizeCompleted += OnClientSizeChanged;
            element.MouseEnter += OnMouseEnter;
            element.MouseLeave += OnMouseLeave;
        }

        internal override void Run()
        {
            Debug.Assert(InitCallback != null);
            Debug.Assert(RunCallback != null);

            InitCallback();

            CompositionTarget.Rendering += OnCompositionTargetRendering;
        }

        internal override void Resize(int width, int height)
        {
            element.TrySetSize(width, height);
        }

        protected internal override void SetSupportedOrientations(DisplayOrientation orientations)
        {
            // orientations are not supported on Desktop platform
        }

        protected override void SetTitle(string title)
        {
            // ignore. SharpDXElement doesn't have title
        }

        private void OnCompositionTargetRendering(object sender, EventArgs e)
        {
            if (element.LowPriorityRendering)
            {
                // if we called render previously...
                if (previousRenderCall != null)
                {
                    var previousStatus = previousRenderCall.Status;

                    // ... and the operation didn't finish yet - then skip the current call
                    if (previousStatus == DispatcherOperationStatus.Pending
                        || previousStatus == DispatcherOperationStatus.Executing)
                    {
                        return;
                    }
                }

                previousRenderCall = element.Dispatcher.BeginInvoke(renderDelegate, DispatcherPriority.Input);
            }
            else
            {
                RenderLoopCallback();
            }
        }

        private void RenderLoopCallback()
        {
            if (Exiting)
            {
                CompositionTarget.Rendering -= OnCompositionTargetRendering;
                if (element != null)
                {
                    element.Dispose();
                    element = null;
                }
                return;
            }

            RunCallback();

            element.InvalidateRendering();
        }

        private void OnMouseEnter(object sender, MouseEventArgs e)
        {
            TryHideMouse();
        }

        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            TryShowMouse();
        }

        private void TryHideMouse()
        {
            if (isMouseVisible || isMouseCurrentlyHidden) return;

            oldCursor = element.Cursor;
            isMouseCurrentlyHidden = true;
        }

        private void TryShowMouse()
        {
            if (!isMouseVisible || !isMouseCurrentlyHidden) return;

            element.Cursor = oldCursor;
            isMouseCurrentlyHidden = false;
        }
    }
}
#endif