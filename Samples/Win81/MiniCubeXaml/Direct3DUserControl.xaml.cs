using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using CommonDX;
using MiniCube;
using MiniShape;
using Windows.Graphics.Display;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace MiniCubeXaml
{
    public sealed partial class Direct3DUserControl : SwapChainPanel
    {
        DeviceManager deviceManager;
        SwapChainPanelTarget target;
        CubeRenderer cubeRenderer;
        ShapeRenderer shapeRenderer;

        public static readonly DependencyProperty DesignModeD3DRenderingProperty =
            DependencyProperty.Register("DesignModeD3DRendering", typeof(Boolean), typeof(Direct3DUserControl), new PropertyMetadata(false));

        public Boolean DesignModeD3DRendering
        {
            get
            {
                return (Boolean)GetValue(DesignModeD3DRenderingProperty);
            }
            set
            {
                SetValue(DesignModeD3DRenderingProperty, value);
            }
        }

        public Direct3DUserControl()
        {
            this.InitializeComponent();
            // Do D3D initialization when element is loaded, because DesignModeD3DRendering is yet not set in ctor
            this.Loaded += Direct3DUserControl_Loaded;            
        }

        void Direct3DUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            // Do not initialize D3D in design mode as default, since it may cause designer crashes
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled && !DesignModeD3DRendering)
                return;

            // Safely dispose any previous instance
            // Creates a new DeviceManager (Direct3D, Direct2D, DirectWrite, WIC)
            deviceManager = new DeviceManager();            

            // Use current control as the rendering target (Initialize SwapChain, RenderTargetView, DepthStencilView, BitmapTarget)
            target = new SwapChainPanelTarget(this);

            // Add Initializer to device manager
            deviceManager.OnInitialize += target.Initialize;

            // New CubeRenderer
            cubeRenderer = new CubeRenderer();
            cubeRenderer.ShowCube = true;
            deviceManager.OnInitialize += cubeRenderer.Initialize;
            target.OnRender += cubeRenderer.Render;

            // New ShapeRenderer
            shapeRenderer = new ShapeRenderer();
            shapeRenderer.Show = true;
            shapeRenderer.EnableClear = false;
            deviceManager.OnInitialize += shapeRenderer.Initialize;            
            target.OnRender += shapeRenderer.Render;

            // Initialize the device manager and all registered deviceManager.OnInitialize             
            try {
                deviceManager.Initialize(DisplayInformation.GetForCurrentView().LogicalDpi);
                DisplayInformation.GetForCurrentView().DpiChanged += DisplayInformation_LogicalDpiChanged;
            } catch (Exception ex) {
                //DisplayInformation.GetForCurrentView() will throw exception in designer
                deviceManager.Initialize(96.0f);
            }

            // Setup rendering callback
            CompositionTargetEx.Rendering += CompositionTarget_Rendering;

            this.LayoutUpdated += Direct3DUserControl_LayoutUpdated;
            this.CompositionScaleChanged += Direct3DUserControl_CompositionScaleChanged;
        }

        void Direct3DUserControl_CompositionScaleChanged(SwapChainPanel sender, object args)
        {
            //TODO: handle changed composition scale
        }

        void Direct3DUserControl_LayoutUpdated(object sender, object e)
        {
            //TODO: handle updated Layout
        }

        void DisplayInformation_LogicalDpiChanged(DisplayInformation displayInformation, object sender)
        {
            deviceManager.Dpi = displayInformation.LogicalDpi;
            //TODO: handle other value affected by DPI change
        }

        void CompositionTarget_Rendering(object sender, object e)
        {
            target.RenderAll();
            target.Present();
        }
    }
}
