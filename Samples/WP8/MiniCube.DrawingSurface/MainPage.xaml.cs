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
using System.Security;
using Microsoft.Phone.Controls;
using System;
using System.Windows;

namespace MiniTriApp
{
    /// <summary>
    /// This is a port of Direct3D C++ WP8 sample. This port is not clean and complete. 
    /// The preferred way to access Direct3D on WP8 is by using SharpDX.Toolkit.
    /// </summary>
    public partial class MainPage : PhoneApplicationPage
    {
        private SharpDXInterop _sdInterop = null;

        // Constructor
        [SecuritySafeCritical]
        public MainPage()
        {
            InitializeComponent();


        }

        private void DrawingSurface_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (_sdInterop == null)
            {
                _sdInterop = new SharpDXInterop();

                // Set window bounds in dips
                _sdInterop.WindowBounds = new Windows.Foundation.Size(
                    (float)DrawingSurface.ActualWidth,
                    (float)DrawingSurface.ActualHeight
                    );

                // Set native resolution in pixels
                _sdInterop.NativeResolution = new Windows.Foundation.Size(
                    (float)Math.Floor(DrawingSurface.ActualWidth * Application.Current.Host.Content.ScaleFactor / 100.0f + 0.5f),
                    (float)Math.Floor(DrawingSurface.ActualHeight * Application.Current.Host.Content.ScaleFactor / 100.0f + 0.5f)
                    );

                // Set render resolution to the full native resolution
                _sdInterop.RenderResolution = _sdInterop.NativeResolution;

                // Hook-up native component to DrawingSurface
                DrawingSurface.SetContentProvider(_sdInterop.CreateContentProvider());
                DrawingSurface.SetManipulationHandler(_sdInterop);
            }

        }
    }
}