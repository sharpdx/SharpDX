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
    public partial class MainPage : PhoneApplicationPage
    {
        private SharpDXInterop _sdInterop = new SharpDXInterop();

        // Constructor
        [SecuritySafeCritical]
        public MainPage()
        {
            InitializeComponent();

            //DrawingSurface.SetContentProvider(_sdInterop.CreateContentProvider());
            //DrawingSurface.SetManipulationHandler(_sdInterop);
            DrawingSurface.SetBackgroundContentProvider( _sdInterop.CreateContentProvider() );
            DrawingSurface.SetBackgroundManipulationHandler(_sdInterop);
        }
    }
}