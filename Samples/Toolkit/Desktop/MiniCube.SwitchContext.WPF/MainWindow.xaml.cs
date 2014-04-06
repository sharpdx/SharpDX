namespace MiniCube.SwitchContext.WPF
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using SharpDX.Toolkit;

    public partial class MainWindow
    {
        private readonly Game _game;

        public MainWindow()
        {
            InitializeComponent();

            _game = new MiniCubeGame();
            RunGameOn(button1, button2, contentContainer1);
        }

        private void Button1Click(object sender, RoutedEventArgs e)
        {
            RunGameOn(button1, button2, contentContainer1);
        }

        private void Button2Click(object sender, RoutedEventArgs e)
        {
            RunGameOn(button2, button1, contentContainer2);
        }

        private void RunGameOn(UIElement disableButton, UIElement enableButton, ContentControl contentContainer)
        {
            DisposeContent(contentContainer1);
            DisposeContent(contentContainer2);

            disableButton.IsEnabled = false;
            enableButton.IsEnabled = true;

            var element = new SharpDXElement
                          {
                              SendResizeToGame = true,
                              SendResizeDelay = TimeSpan.FromSeconds(2),
                              LowPriorityRendering = false,
                          };

            contentContainer.Content = element;

            if (_game.IsRunning)
                _game.Switch(element);
            else
                _game.Run(element);
        }

        private void DisposeContent(ContentControl contentContainer)
        {
            var element = contentContainer.Content as SharpDXElement;
            contentContainer.Content = null;
            if (element != null)
                element.Dispose();
        }
    }
}
