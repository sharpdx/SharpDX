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
#if WP8

using System;
using System.Runtime.InteropServices;

using SharpDX.Direct3D11;
using SharpDX.Toolkit.Graphics;
using System.Windows.Controls;

namespace SharpDX.Toolkit
{
    /// <summary>
    /// An abstract window.
    /// </summary>
    internal class GameWindowWP8 : GameWindow, ICallbackable, IDrawingSurfaceBackgroundContentProviderNative, IInspectable, ICustomQueryInterface
    {
        private object nativeWindow;
        private IntPtr thisComObjectPtr;

        private DrawingSurfaceBackgroundGrid drawingSurfaceBackgroundGrid;
        private DrawingSurfaceRuntimeHost host;

        private VoidAction initCallback;
        private VoidAction tickCallback;

        internal GameWindowWP8()
        {
            thisComObjectPtr = CppObjectShadow.ToIntPtr<IDrawingSurfaceBackgroundContentProviderNative>(this);
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

        internal override bool IsMouseVisible {get; set;}

        protected internal override void SetSupportedOrientations(DisplayOrientation orientations)
        {
            // Desktop doesn't have orientation (unless on Windows 8?)
        }

        internal override void Initialize(object windowContext)
        {
            nativeWindow = windowContext;
            drawingSurfaceBackgroundGrid = windowContext as DrawingSurfaceBackgroundGrid;
            if (windowContext == null)
            {
                throw new ArgumentNullException("windowContext cannot be null under WP8", "windowContext");
            }
            if (drawingSurfaceBackgroundGrid == null)
            {
                throw new ArgumentException("Expecting a DrawingSurfaceBackgroundGrid as a window context under WP8", "windowContext");
            }
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
                if (BackBuffer != null)
                {
                    return new DrawingRectangle(0, 0, BackBuffer.Width, BackBuffer.Height);
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

        public void RunDrawingSurfaceBackground(VoidAction initCallback, VoidAction tickCallback)
        {
            this.initCallback = initCallback;
            this.tickCallback = tickCallback;

            // Set the background to this 
            drawingSurfaceBackgroundGrid.SetBackgroundContentProvider(this);
        }

        protected override void SetTitle(string title)
        {
        }

#region Implementation of ICallbackable

        IDisposable ICallbackable.Shadow { get; set; }


        public GraphicsDevice GraphicsDevice;

        public RenderTarget2D BackBuffer;

        #endregion

#region Implementation of IDrawingSurfaceBackgroundContentProviderNative

        public bool Exiting;

        void IDrawingSurfaceBackgroundContentProviderNative.Connect(DrawingSurfaceRuntimeHost host, Device device)
        {
            this.host = host;
        }

        void IDrawingSurfaceBackgroundContentProviderNative.Disconnect()
        {
            ComObject.Dispose(ref GraphicsDevice);
            ComObject.Dispose(ref BackBuffer);
        }

        void IDrawingSurfaceBackgroundContentProviderNative.PrepareResources(DateTime presentTargetTime, ref DrawingSizeF desiredRenderTargetSize)
        {
            
        }


        struct RenderTargetLocal
        {
            public IntPtr NativePointer;
            public RenderTarget2D RenderTarget;

            public void Dispose()
            {
                ComObject.Dispose(ref RenderTarget);
                NativePointer = IntPtr.Zero;
            }
        }

        // Only 3 buffers, seems ok on default WP8
        private readonly RenderTargetLocal[] renderTargets = new RenderTargetLocal[3];
        private int nextRenderTarget = 0;
        private RenderTargetGraphicsPresenter graphicsPresenter;

        private Exception drawException = null;

        void IDrawingSurfaceBackgroundContentProviderNative.Draw(Device device, DeviceContext context, RenderTargetView renderTargetView)
        {
            try
            {
                RenderTarget2D localBackBuffer = null;
                if (!Exiting)
                {
                    if (GraphicsDevice == null)
                    {
                        GraphicsDevice = GraphicsDevice.New(device);

                        renderTargets[0].NativePointer = renderTargetView.NativePointer;
                        renderTargets[0].RenderTarget = RenderTarget2D.New(GraphicsDevice, renderTargetView, true);
                        BackBuffer = renderTargets[0].RenderTarget;
                        nextRenderTarget++;

                        graphicsPresenter = new RenderTargetGraphicsPresenter(GraphicsDevice, BackBuffer);
                        GraphicsDevice.Presenter = graphicsPresenter;
                        initCallback();
                    }
                    else
                    {
                        if (((Direct3D11.Device)GraphicsDevice).NativePointer != device.NativePointer || ((Direct3D11.DeviceContext)GraphicsDevice).NativePointer != context.NativePointer)
                        {
                            System.Diagnostics.Debugger.Break();
                        }

                        // Find any previous render target that was already alocated.
                        bool foundRenderTarget = false;
                        foreach (var renderTargetLocal in renderTargets)
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
                            nextRenderTarget++;
                        }

                        // TODO: Check that new device/devicecontext/renderTargetView are the same
                        // else we need to handle DeviceReset/Remove...etc.
                    }

                    tickCallback();

                    // Aks the host for additional frame
                    host.RequestAdditionalFrame();
                }
            } catch (Exception ex)
            {
                // TODO: As we are in a callback from a native code, we cannot pass back this exception,
                // so how to pass back this exception to the user at an appropriate time?
                drawException = ex;
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
    }
}
#endif