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

namespace SharpDX.Toolkit
{
    internal class GameWindowPhoneBackgroundXaml : GameWindow, IDrawingSurfaceBackgroundContentProviderNative,
                                                          IInspectable, ICustomQueryInterface
    {
        internal RenderTarget2D BackBuffer;
        internal  GraphicsDevice GraphicsDevice;

        private bool isInitialized;

        private IGraphicsDeviceManager graphicsDeviceManager;
        private readonly IntPtr thisComObjectPtr;

        private DrawingSurfaceBackgroundGrid drawingSurfaceBackgroundGrid;
        private DrawingSurfaceRuntimeHost host;

        internal GameWindowPhoneBackgroundXaml()
        {
            thisComObjectPtr = CppObjectShadow.ToIntPtr<IDrawingSurfaceBackgroundContentProviderNative>(this);
        }

        public override object NativeWindow
        {
            get { return drawingSurfaceBackgroundGrid; }
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
                if (BackBuffer != null)
                {
                    return new DrawingRectangle(0, 0, BackBuffer.Width, BackBuffer.Height);
                }
                return DrawingRectangle.Empty;
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

        #region Implementation of IDrawingSurfaceBackgroundContentProviderNative

        private readonly RenderTargetLocal[] renderTargets = new RenderTargetLocal[3];
        private Exception drawException;
        private RenderTargetGraphicsPresenter graphicsPresenter;
        private int nextRenderTarget;

        void IDrawingSurfaceBackgroundContentProviderNative.Connect(DrawingSurfaceRuntimeHost host, Device device)
        {
            this.host = host;
        }

        void IDrawingSurfaceBackgroundContentProviderNative.Disconnect()
        {
            for (int i = 0; i < renderTargets.Length; i++)
            {
                renderTargets[i].Dispose();
            }

            Utilities.Dispose(ref GraphicsDevice);
            Utilities.Dispose(ref BackBuffer);
        }

        void IDrawingSurfaceBackgroundContentProviderNative.PrepareResources(DateTime presentTargetTime,
                                                                             ref DrawingSizeF desiredRenderTargetSize)
        {
        }

        private void CreateDevice(Device device, DeviceContext context, RenderTargetView renderTargetView)
        {
            GraphicsDevice = GraphicsDevice.New(device);

            renderTargets[0].NativePointer = renderTargetView.NativePointer;
            renderTargets[0].RenderTarget = RenderTarget2D.New(GraphicsDevice, renderTargetView, true);
            BackBuffer = renderTargets[0].RenderTarget;
            nextRenderTarget++;

            graphicsPresenter = new RenderTargetGraphicsPresenter(GraphicsDevice, BackBuffer);
            GraphicsDevice.Presenter = graphicsPresenter;
        }

        void IDrawingSurfaceBackgroundContentProviderNative.Draw(Device device, DeviceContext context,
                                                                 RenderTargetView renderTargetView)
        {
            try
            {
                RenderTarget2D localBackBuffer = null;
                if (!Exiting)
                {
                    if (!isInitialized)
                    {
                        CreateDevice(device, context, renderTargetView);
                        InitCallback();
                        isInitialized = true;
                    }
                    else
                    {
                        if (GraphicsDevice == null || (((Device) GraphicsDevice).NativePointer != device.NativePointer ||
                            ((DeviceContext) GraphicsDevice).NativePointer != context.NativePointer))
                        {
                            Utilities.Dispose(ref GraphicsDevice);
                            Utilities.Dispose(ref BackBuffer);

                            CreateDevice(device, context, renderTargetView);

                            // Force the graphics device to reload the content manager
                            graphicsDeviceManager.CreateDevice();
                        }

                        // Find any previous render target that was already alocated.
                        bool foundRenderTarget = false;
                        foreach (RenderTargetLocal renderTargetLocal in renderTargets)
                        {
                            if (renderTargetLocal.NativePointer == renderTargetView.NativePointer)
                            {
                                BackBuffer = renderTargetLocal.RenderTarget;
                                graphicsPresenter.SetBackBuffer(BackBuffer);
                                foundRenderTarget = true;
                                break;
                            }
                        }

                        if (!foundRenderTarget)
                        {
                            // Dispose any previous render target.
                            renderTargets[nextRenderTarget].Dispose();

                            // Creates the new BackBuffer and associated it to the GraphicsPresenter
                            BackBuffer = RenderTarget2D.New(GraphicsDevice, renderTargetView, true);
                            graphicsPresenter.SetBackBuffer(BackBuffer);

                            renderTargets[nextRenderTarget].NativePointer = renderTargetView.NativePointer;
                            renderTargets[nextRenderTarget].RenderTarget = BackBuffer;
                            nextRenderTarget = (nextRenderTarget + 1)%renderTargets.Length;
                        }

                        // TODO: Check that new device/devicecontext/renderTargetView are the same
                        // else we need to handle DeviceReset/Remove...etc.
                    }

                    RunCallback();

                    // Aks the host for additional frame
                    host.RequestAdditionalFrame();
                }
            }
            catch (Exception ex)
            {
                // TODO: As we are in a callback from a native code, we cannot pass back this exception,
                // so how to pass back this exception to the user at an appropriate time?
                drawException = ex;
                Debug.WriteLine(drawException);
            }
        }

        private struct RenderTargetLocal
        {
            public IntPtr NativePointer;
            public RenderTarget2D RenderTarget;

            public void Dispose()
            {
                Utilities.Dispose(ref RenderTarget);
                NativePointer = IntPtr.Zero;
            }
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
            return gameContext.ContextType == GameContextType.WindowsPhoneBackgroundXaml;
        }

        internal override void Initialize(GameContext gameContext)
        {
            drawingSurfaceBackgroundGrid = (DrawingSurfaceBackgroundGrid) gameContext.Control;
            graphicsDeviceManager = (IGraphicsDeviceManager)Services.GetService(typeof(IGraphicsDeviceManager));
        }

        internal override void Run()
        {
            drawingSurfaceBackgroundGrid.Loaded += DrawingSurfaceBackgroundGridOnLoaded;

            // Never called??
            drawingSurfaceBackgroundGrid.Unloaded += drawingSurfaceBackgroundGrid_Unloaded;
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
            drawingSurfaceBackgroundGrid.SetBackgroundContentProvider(null);
            
            base.Dispose(disposeManagedResources);
        }

        private void DrawingSurfaceBackgroundGridOnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            // Set the background to run this instance 
            drawingSurfaceBackgroundGrid.SetBackgroundContentProvider(this);
        }

        private void drawingSurfaceBackgroundGrid_Unloaded(object sender, RoutedEventArgs e)
        {
            // Never called??
            Exiting = true;
            // Set the background to run this instance 
            drawingSurfaceBackgroundGrid.SetBackgroundContentProvider(null);
        }
    }
}

#endif