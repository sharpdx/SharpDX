using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using SharpDX.Direct3D11;
using SharpDX;

namespace PhoneDirect3DXamlAppInterop
{
    public class MyObject : DrawingSurfaceBackgroundContentProviderNativeBase
    {
        protected DrawingSurfaceRuntimeHost Host;
        protected Device CurrentDevice;

        public override void Connect(DrawingSurfaceRuntimeHost host, Device device)
        {
            this.Host = host;
            this.CurrentDevice = device;

        }

        public override void Disconnect()
        {

        }

        public override void PrepareResources(DateTime presentTargetTime, DrawingSizeF desiredRenderTargetSize)
        {
        }

        public override void Draw(Device device, DeviceContext context, RenderTargetView renderTargetView)
        {
            if (CurrentDevice != device)
                CurrentDevice = device;

            context.ClearRenderTargetView(renderTargetView, new Color4(DateTime.Now.Second / 60.0f));

            Host.RequestAdditionalFrame();
        }
    }

    public partial class MainPage : PhoneApplicationPage
    {
        private MyObject background = new MyObject();


        // Constructor
        [SecuritySafeCritical]
        public MainPage()
        {
            InitializeComponent();

            DrawingSurfaceBackground.SetBackgroundContentProvider(background);
            //DrawingSurfaceBackground.SetBackgroundManipulationHandler(m_d3dBackground);
        }
    }
}