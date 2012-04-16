using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace D2DCustomVertexShaderEffect
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class ChooseDemo : D2DCustomVertexShaderEffect.Common.LayoutAwarePage
    {
        public ChooseDemo()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

 

        private void butSwapChainBackgroundPanel_Click(object sender, RoutedEventArgs e)
        {

            // Place the frame in the current Window and ensure that it is active
            var swapchainPanel = new MPSCBP();
            Window.Current.Content = swapchainPanel;
            Window.Current.Activate();
        }

        private void butSurfaceImageSource_Click(object sender, RoutedEventArgs e)
        {
            // Create a Frame to act navigation context and navigate to the first page
            var rootFrame = new Frame();
            rootFrame.Navigate(typeof(MPSIS));

            // Place the frame in the current Window and ensure that it is active
            Window.Current.Content = rootFrame;
            Window.Current.Activate();


        }
    }
}
