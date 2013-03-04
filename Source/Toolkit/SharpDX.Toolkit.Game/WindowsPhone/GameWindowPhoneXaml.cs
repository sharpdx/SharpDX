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
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using SharpDX.Direct3D11;
using SharpDX.Toolkit.Graphics;
using Texture2D = SharpDX.Direct3D11.Texture2D;

namespace SharpDX.Toolkit
{
    internal class GameWindowPhoneXaml : GameWindow, IDrawingSurfaceContentProviderNative,
                                                IInspectable, ICustomQueryInterface
    {
        private DrawingSizeF currentSize;
        private readonly IntPtr thisComObjectPtr;

        private bool isInitialized;

        private DrawingSurface drawingSurface;
        private DrawingSurfaceRuntimeHost host;

        private IGraphicsDeviceService graphicsDeviceService;

        private IGraphicsDeviceManager graphicsDeviceManager;

        internal GameWindowPhoneXaml()
        {
            thisComObjectPtr = CppObjectShadow.ToIntPtr<IDrawingSurfaceContentProviderNative>(this);
        }

        public override object NativeWindow
        {
            get { return drawingSurface; }
        }

        public override bool IsMouseVisible { get; set; }

        public override bool AllowUserResizing
        {
            get { return false; }
            set { }
        }

        public override DrawingRectangle ClientBounds
        {
            get
            {
                return new DrawingRectangle(0, 0, (int)currentSize.Width, (int)currentSize.Height);
            }
        }

        public override DisplayOrientation CurrentOrientation
        {
            get { return DisplayOrientation.Default; }
        }

        public override bool IsMinimized
        {
            get { return false; }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="GameWindow" /> is visible.
        /// </summary>
        /// <value>
        ///     <c>true</c> if visible; otherwise, <c>false</c>.
        /// </value>
        public override bool Visible
        {
            get { return true; }
            set { }
        }

        #region Implementation of ICallbackable

        IDisposable ICallbackable.Shadow { get; set; }

        #endregion

        #region Implementation of IDrawingSurfaceContentProviderNative

        private Exception drawException;
        private RenderTargetGraphicsPresenter graphicsPresenter;
        private DrawingSurfaceSynchronizedTexture synchronizedTexture;

        public void CreateSynchronizedTexture(Graphics.Texture2D renderTarget2D)
        {
            Utilities.Dispose(ref synchronizedTexture);
            synchronizedTexture = host.CreateSynchronizedTexture((Texture2D)renderTarget2D);
        }

        void IDrawingSurfaceContentProviderNative.Connect(DrawingSurfaceRuntimeHost host)
        {
            this.host = host;
        }

        void IDrawingSurfaceContentProviderNative.Disconnect()
        {
            // Dispose the graphics device
            if (graphicsDeviceService.GraphicsDevice != null)
            {
                graphicsDeviceService.GraphicsDevice.Dispose();
            }

            Utilities.Dispose(ref synchronizedTexture);
        }

        void IDrawingSurfaceContentProviderNative.PrepareResources(DateTime presentTargetTime, out Bool isContentDirty)
        {
            isContentDirty = true;
        }

        void IDrawingSurfaceContentProviderNative.GetTexture(DrawingSizeF surfaceSize, out DrawingSurfaceSynchronizedTexture synchronizedTexture, out RectangleF textureSubRectangle)
        {
            try
            {
                if (!Exiting)
                {
                    // TODO Check if surfaceSize changed to reinitialize the buffers?
                    currentSize = surfaceSize;

                    if (!isInitialized)
                    {
                        InitCallback();
                        isInitialized = true;
                    }
                    else if (this.synchronizedTexture == null)
                    {
                        // Make sure that the graphics device is created
                        graphicsDeviceManager.CreateDevice();
                    }

                    this.synchronizedTexture.BeginDraw();

                    RunCallback();

                    host.RequestAdditionalFrame();

                    this.synchronizedTexture.EndDraw();
                }
            }
            catch (Exception ex)
            {
                // TODO: As we are in a callback from a native code, we cannot pass back this exception,
                // so how to pass back this exception to the user at an appropriate time?
                drawException = ex;
                Debug.WriteLine(drawException);
            }

            // Set output parameters.
            textureSubRectangle = new RectangleF(0f, 0f, surfaceSize.Width, surfaceSize.Height);
            synchronizedTexture = this.synchronizedTexture;
        }

        #endregion

        #region Implementation of ICustomQueryInterface

        CustomQueryInterfaceResult ICustomQueryInterface.GetInterface(ref Guid iid, out IntPtr ppv)
        {
            ppv = thisComObjectPtr;
            return CustomQueryInterfaceResult.Handled;
        }

        #endregion

        public override void BeginScreenDeviceChange(bool willBeFullScreen)
        {
        }

        public override void EndScreenDeviceChange(int clientWidth, int clientHeight)
        {
        }

        internal override bool CanHandle(GameContext gameContext)
        {
            return gameContext.ContextType == GameContextType.WindowsPhoneXaml;
        }

        internal override void Initialize(GameContext gameContext)
        {
            drawingSurface = (DrawingSurface)gameContext.Control;
            graphicsDeviceService = (IGraphicsDeviceService)Services.GetService(typeof(IGraphicsDeviceService));
            graphicsDeviceManager = (IGraphicsDeviceManager)Services.GetService(typeof(IGraphicsDeviceManager));
        }

        internal override void Run()
        {
            drawingSurface.Loaded += DrawingSurfaceOnLoaded;

            // Never called??
            drawingSurface.Unloaded += DrawingSurfaceUnloaded;
        }

        internal override void Resize(int width, int height)
        {
        }

        protected internal override void SetSupportedOrientations(DisplayOrientation orientations)
        {
        }

        protected override void SetTitle(string title)
        {
        }

        protected override void Dispose(bool disposeManagedResources)
        {
            drawingSurface.SetContentProvider(null);

            var callbackable = (ICallbackable)this;
            if (callbackable.Shadow != null)
            {
                callbackable.Shadow.Dispose();
                callbackable.Shadow = null;
            }

            base.Dispose(disposeManagedResources);
        }

        private void DrawingSurfaceOnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            // Set the background to run this instance 
            drawingSurface.SetContentProvider(this);
        }

        private void DrawingSurfaceUnloaded(object sender, RoutedEventArgs e)
        {
            // Never called??
            Exiting = true;
            // Set the background to run this instance 
            drawingSurface.SetContentProvider(null);
        }
    }
}

#endif