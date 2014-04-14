using System.Windows;

namespace MiniCube.SwitchContext.WP8.DrawingSurface
{
    using System.Windows.Controls;
    using SharpDX.Toolkit;

    public partial class MainPage
    {
        private readonly Game game;
        private int index;
        private DrawingSurface currentSurface;

        public MainPage()
        {
            InitializeComponent();

            game = new MiniCubeGame();
            RunOrSwitchContext();
        }

        private void HandleButtonClick(object sender, RoutedEventArgs e)
        {
            RunOrSwitchContext();
        }

        private void RunOrSwitchContext()
        {
            if (currentSurface != null)
                LayoutRoot.Children.Remove(currentSurface);

            currentSurface = new DrawingSurface();
            LayoutRoot.Children.Add(currentSurface);

            if (game.IsRunning)
                game.Switch(currentSurface);
            else
                game.Run(currentSurface);

            label.Text = "Context nr. " + (index++);
        }
    }
}