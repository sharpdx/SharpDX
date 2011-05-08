namespace WPFHost
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using SharpDX;
    using SharpDX.Direct3D10;
    using SharpDX.DXGI;
    using Device = SharpDX.Direct3D10.Device1;

    public partial class DPFCanvas : Image, ISceneHost
    {
        private Device Device;
        private Texture2D RenderTarget;
        private Texture2D DepthStencil;
        private RenderTargetView RenderTargetView;
        private DepthStencilView DepthStencilView;
        private DX10ImageSource D3DSurface;
        private Stopwatch RenderTimer;
        private IScene RenderScene;
        private bool SceneAttached;

        public Color4 ClearColor = new Color4(1.0f, 0.0f, 0.0f, 0.0f);

        public DPFCanvas()
        {
            this.RenderTimer = new Stopwatch();
            this.Loaded += this.Window_Loaded;
            this.Unloaded += this.Window_Closing;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (DPFCanvas.IsInDesignMode)
                return;

            this.StartD3D();
            this.StartRendering();
        }

        private void Window_Closing(object sender, RoutedEventArgs e)
        {
            if (DPFCanvas.IsInDesignMode)
                return;

            this.StopRendering();
            this.EndD3D();
        }

        private void StartD3D()
        {
            this.Device = new Device(DriverType.Hardware, DeviceCreationFlags.BgraSupport, FeatureLevel.Level_10_0);

            this.D3DSurface = new DX10ImageSource();
            this.D3DSurface.IsFrontBufferAvailableChanged += OnIsFrontBufferAvailableChanged;

            this.CreateAndBindTargets();

            this.Source = this.D3DSurface;
        }

        private void EndD3D()
        {
            if (this.RenderScene != null)
            {
                this.RenderScene.Detach();
                this.SceneAttached = false;
            }

            this.D3DSurface.IsFrontBufferAvailableChanged -= OnIsFrontBufferAvailableChanged;
            this.Source = null;

            Disposer.SafeDispose(ref this.D3DSurface);
            Disposer.SafeDispose(ref this.RenderTargetView);
            Disposer.SafeDispose(ref this.DepthStencilView);
            Disposer.SafeDispose(ref this.RenderTarget);
            Disposer.SafeDispose(ref this.DepthStencil);
            Disposer.SafeDispose(ref this.Device);
        }

        private void CreateAndBindTargets()
        {
            this.D3DSurface.SetRenderTargetDX10(null);

            Disposer.SafeDispose(ref this.RenderTargetView);
            Disposer.SafeDispose(ref this.DepthStencilView);
            Disposer.SafeDispose(ref this.RenderTarget);
            Disposer.SafeDispose(ref this.DepthStencil);

            int width = Math.Max((int)base.ActualWidth, 100);
            int height = Math.Max((int)base.ActualHeight, 100);

            Texture2DDescription colordesc = new Texture2DDescription
            {
                BindFlags = BindFlags.RenderTarget | BindFlags.ShaderResource,
                Format = Format.B8G8R8A8_UNorm,
                Width = width,
                Height = height,
                MipLevels = 1,
                SampleDescription = new SampleDescription(1, 0),
                Usage = ResourceUsage.Default,
                OptionFlags = ResourceOptionFlags.Shared,
                CpuAccessFlags = CpuAccessFlags.None,
                ArraySize = 1
            };

            Texture2DDescription depthdesc = new Texture2DDescription
            {
                BindFlags = BindFlags.DepthStencil,
                Format = Format.D32_Float_S8X24_UInt,
                Width = width,
                Height = height,
                MipLevels = 1,
                SampleDescription = new SampleDescription(1, 0),
                Usage = ResourceUsage.Default,
                OptionFlags = ResourceOptionFlags.None,
                CpuAccessFlags = CpuAccessFlags.None,
                ArraySize = 1,
            };

            this.RenderTarget = new Texture2D(this.Device, colordesc);
            this.DepthStencil = new Texture2D(this.Device, depthdesc);
            this.RenderTargetView = new RenderTargetView(this.Device, this.RenderTarget);
            this.DepthStencilView = new DepthStencilView(this.Device, this.DepthStencil);

            this.D3DSurface.SetRenderTargetDX10(this.RenderTarget);
        }

        private void StartRendering()
        {
            if (this.RenderTimer.IsRunning)
                return;

            CompositionTarget.Rendering += OnRendering;
            this.RenderTimer.Start();
        }

        private void StopRendering()
        {
            if (!this.RenderTimer.IsRunning)
                return;

            CompositionTarget.Rendering -= OnRendering;
            this.RenderTimer.Stop();
        }

        private void OnRendering(object sender, EventArgs e)
        {
            if (!this.RenderTimer.IsRunning)
                return;

            this.Render(this.RenderTimer.Elapsed);
            this.D3DSurface.InvalidateD3DImage();
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            this.CreateAndBindTargets();
            base.OnRenderSizeChanged(sizeInfo);
        }

        void Render(TimeSpan sceneTime)
        {
            SharpDX.Direct3D10.Device device = this.Device;
            if (device == null)
                return;

            Texture2D renderTarget = this.RenderTarget;
            if (renderTarget == null)
                return;

            int targetWidth = renderTarget.Description.Width;
            int targetHeight = renderTarget.Description.Height;

            device.OutputMerger.SetTargets(this.DepthStencilView, this.RenderTargetView);
            device.Rasterizer.SetViewports(new Viewport(0, 0, targetWidth, targetHeight, 0.0f, 1.0f));

            device.ClearRenderTargetView(this.RenderTargetView, this.ClearColor);
            device.ClearDepthStencilView(this.DepthStencilView, DepthStencilClearFlags.Depth | DepthStencilClearFlags.Stencil, 1.0f, 0);

            if (this.Scene != null)
            {
                if (!this.SceneAttached)
                {
                    this.SceneAttached = true;
                    this.RenderScene.Attach(this);
                }

                this.Scene.Update(this.RenderTimer.Elapsed);
                this.Scene.Render();
            }

            device.Flush();
        }

        private void OnIsFrontBufferAvailableChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            // this fires when the screensaver kicks in, the machine goes into sleep or hibernate
            // and any other catastrophic losses of the d3d device from WPF's point of view
            if (this.D3DSurface.IsFrontBufferAvailable)
                this.StartRendering();
            else
                this.StopRendering();
        }

        /// <summary>
        /// Gets a value indicating whether the control is in design mode
        /// (running in Blend or Visual Studio).
        /// </summary>
        public static bool IsInDesignMode
        {
            get
            {
                DependencyProperty prop = DesignerProperties.IsInDesignModeProperty;
                bool isDesignMode = (bool)DependencyPropertyDescriptor.FromProperty(prop, typeof(FrameworkElement)).Metadata.DefaultValue;
                return isDesignMode;
            }
        }

        public IScene Scene
        {
            get { return this.RenderScene; }
            set
            {
                if (ReferenceEquals(this.RenderScene, value))
                    return;

                if (this.RenderScene != null)
                    this.RenderScene.Detach();

                this.SceneAttached = false;
                this.RenderScene = value;
            }
        }

        SharpDX.Direct3D10.Device ISceneHost.Device
        {
            get { return this.Device; }
        }
    }
}
