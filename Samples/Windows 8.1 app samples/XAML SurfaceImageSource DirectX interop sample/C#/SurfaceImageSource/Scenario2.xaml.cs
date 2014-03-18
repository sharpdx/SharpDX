//*********************************************************
//
// Copyright (c) Microsoft. All rights reserved.
// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************

using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media;
using System;
using SDKTemplate;
using Scenario2Component;

namespace SurfaceImageSource
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Scenario2 : SDKTemplate.Common.LayoutAwarePage
    {
        // A pointer back to the main page.  This is needed if you want to call methods in MainPage such
        // as NotifyUser()
        MainPage rootPage = MainPage.Current;
        
        // An image source derived from SurfaceImageSource, used to draw DirectX content
        private Scenario2ImageSource Scenario2Drawing;

        // A flag indicating whether the ImageSource is currently being animated
        private bool Animating = false;

        public Scenario2()
        {
            this.InitializeComponent();
            
            Scenario2Drawing = new Scenario2ImageSource((int)Image2.Width, (int)Image2.Height, true);
            
            // Use Scenario2Drawing as a source for the Image control
            Image2.Source = Scenario2Drawing;
            // Use Scenario2Drawing as a source for the Ellipse shape's fill
            Ellipse2.Fill = new ImageBrush() { ImageSource = Scenario2Drawing };
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            if (Animating)
            {
                CompositionTarget.Rendering -= AdvanceAnimation;
            }
        }

        private void ToggleAnimation_Click(object sender, RoutedEventArgs e)
        {
            // Toggle animation by hooking or unhooking the XAML CompositionTarget.Rendering event
            if (Animating)
            {
                CompositionTarget.Rendering -= AdvanceAnimation;
            }
            else
            {
                CompositionTarget.Rendering += AdvanceAnimation;
            }

            Animating = !Animating;
        }

        void AdvanceAnimation(object sender, object e)
        {
            // Begin updating the SurfaceImageSource
            Scenario2Drawing.BeginDraw();

            // Clear background
            Scenario2Drawing.Clear(Colors.MidnightBlue);

            // Render next animation frame
            Scenario2Drawing.RenderNextAnimationFrame();

            // Stop updating the SurfaceImageSource and draw the new frame
            Scenario2Drawing.EndDraw();
        }
    }
}
