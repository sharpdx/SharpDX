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

        public Direct3DUserControl()
        {
            this.InitializeComponent();

            // Safely dispose any previous instance
            // Creates a new DeviceManager (Direct3D, Direct2D, DirectWrite, WIC)
            deviceManager = new DeviceManager();            

            // Use current control as the rendering target (Initialize SwapChain, RenderTargetView, DepthStencilView, BitmapTarget)
            target = new SwapChainPanelTarget(this);

            // Add Initializer to device manager
            deviceManager.OnInitialize += target.Initialize;
            try
            {
                //// New CubeRenderer
                //cubeRenderer = new CubeRenderer();
                //cubeRenderer.ShowCube = true;
                //deviceManager.OnInitialize += cubeRenderer.Initialize;
                //target.OnRender += cubeRenderer.Render;
            }
            catch (Exception ex) { 
                //TODO: handle file not found exception in designer
            }


            shapeRenderer = new ShapeRenderer();
            shapeRenderer.Show = true;
            shapeRenderer.EnableClear = true;
            deviceManager.OnInitialize += shapeRenderer.Initialize;            
            target.OnRender += shapeRenderer.Render;

            // Initialize the device manager and all registered deviceManager.OnInitialize             
            try {
                deviceManager.Initialize(DisplayInformation.GetForCurrentView().LogicalDpi);
                DisplayInformation.GetForCurrentView().DpiChanged += DisplayProperties_LogicalDpiChanged;
            } catch (Exception ex) {
                //DisplayInformation.GetForCurrentView() will throw exception in designer
                deviceManager.Initialize(96.0f);
            }

            // Setup rendering callback
            CompositionTargetEx.Rendering += CompositionTarget_Rendering;

            this.SizeChanged += Direct3DUserControl_SizeChanged;
            this.LayoutUpdated += Direct3DUserControl_LayoutUpdated;
        }

        void Direct3DUserControl_LayoutUpdated(object sender, object e)
        {
            //TODO: handle updated Layout
            //throw new NotImplementedException();
        }

        void Direct3DUserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            target.UpdateForSizeChange();
        }

        void DisplayProperties_LogicalDpiChanged(DisplayInformation displayInformation, object sender)
        {
            deviceManager.Dpi = displayInformation.LogicalDpi;
        }

        void CompositionTarget_Rendering(object sender, object e)
        {
            target.RenderAll();
            target.Present();
        }
    }
}
