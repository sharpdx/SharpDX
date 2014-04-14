//*********************************************************
//
// Copyright (c) Microsoft. All rights reserved.
// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************

using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media;
using System;
using SDKTemplate;
using Scenario1Component;

namespace SurfaceImageSource
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Scenario1 : SDKTemplate.Common.LayoutAwarePage
    {
        // A pointer back to the main page.  This is needed if you want to call methods in MainPage such
        // as NotifyUser()
        MainPage rootPage = MainPage.Current;

        // An image source derived from SurfaceImageSource, used to draw DirectX content
        private Scenario1ImageSource Scenario1Drawing;

        public Scenario1()
        {
            this.InitializeComponent();

            Scenario1Drawing = new Scenario1ImageSource((int)Image1.Width, (int)Image1.Height, true);

            // Use Scenario1Drawing as a source for the Image control
            Image1.Source = Scenario1Drawing;
            // Use Scenario1Drawing as a source for the Ellipse shape's fill
            Ellipse1.Fill = new ImageBrush() { ImageSource = Scenario1Drawing };
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void Scenario1DrawRectangles_Click(object sender, RoutedEventArgs e)
        {
            // Begin updating the SurfaceImageSource
            Scenario1Drawing.BeginDraw();

            // Clear background
            Scenario1Drawing.Clear(Colors.Bisque);

            // Create a new pseudo-random number generator
            Random randomGenerator = new Random();            
            byte[] pixelValues = new byte[3]; // Represents the red, green, and blue channels of a color
            
            // Draw 50 random retangles
            for (int i = 0; i < 50; i++)
            {
                // Generate a new random color
                randomGenerator.NextBytes(pixelValues);
                Color color = new Color() { R = pixelValues[0], G = pixelValues[1], B = pixelValues[2], A = 255 };

                // Add a new randomly colored 50x50 rectangle that will fit somewhere within the bounds of the Image1 control
                Scenario1Drawing.FillSolidRect(
                    color,
                    new Rect(randomGenerator.Next((int)Image1.Width - 50), randomGenerator.Next((int)Image1.Height - 50), 50, 50)
                    );
            }

            // Stop updating the SurfaceImageSource and draw its contents
            Scenario1Drawing.EndDraw();
        }
    }
}
