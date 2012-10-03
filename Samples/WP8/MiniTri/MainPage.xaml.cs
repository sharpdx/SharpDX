using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX;
using SharpDX.IO;
using SharpDX.MediaFoundation;
using Color = SharpDX.Color;

namespace MiniTriApp
{
    public class MyObject : DrawingSurfaceBackgroundContentProviderNativeBase
    {
        protected DrawingSurfaceRuntimeHost Host;
        protected Device CurrentDevice;
        private Stopwatch clock;

        public override void Connect(DrawingSurfaceRuntimeHost host, Device device)
        {
            this.Host = host;
            this.CurrentDevice = device;
            clock = Stopwatch.StartNew();
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

            var color = Color.CornflowerBlue;
            color = Color.AdjustContrast(color, .5f + .5f*(float) Math.Sin(clock.ElapsedMilliseconds/500.0f));

            context.ClearRenderTargetView(renderTargetView, color);

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

            var device = new Device(DriverType.Hardware);
            device.Dispose();

            var audio2 = new SharpDX.XAudio2.XAudio2();
            var data = audio2.PerformanceData;

            var mfactory = new MediaEngineClassFactory();
            var mengine = new MediaEngine(mfactory);

            var path = Windows.ApplicationModel.Package.Current.InstalledLocation.Path;

            var test = NativeFile.ReadAllBytes(path + @"\SplashScreenImage.jpg");

            


            DrawingSurfaceBackground.SetBackgroundContentProvider(background);
            //DrawingSurfaceBackground.SetBackgroundManipulationHandler(m_d3dBackground);
        }
    }
}