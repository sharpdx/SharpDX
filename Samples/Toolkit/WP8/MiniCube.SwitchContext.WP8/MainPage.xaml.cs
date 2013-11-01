namespace MiniCube.SwitchContext.WP8
{
    using System.Windows;
    using System.Windows.Controls;
    using SharpDX.Toolkit;

    public partial class MainPage
    {
        private readonly Game game;
        private int count;

        public MainPage()
        {
            InitializeComponent();

            game = new MiniCubeGame();

            SwitchGame();
        }

        private DrawingSurfaceBackgroundGrid BuildGrid()
        {
            var grid = new DrawingSurfaceBackgroundGrid();
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });

            var label = new TextBlock { Text = "Count: " + (count++) };
            var button = new Button { Content = "Switch" };

            button.Click += HandleButtonClick;

            grid.Children.Add(label);
            grid.Children.Add(button);

            button.SetValue(Grid.RowProperty, 1);

            Content = grid;

            return grid;
        }

        private void HandleButtonClick(object sender, RoutedEventArgs e)
        {
            SwitchGame();
        }

        private void SwitchGame()
        {
            var grid = BuildGrid();

            if (game.IsRunning)
                game.Switch(grid);
            else
                game.Run(grid);
        }
    }
}