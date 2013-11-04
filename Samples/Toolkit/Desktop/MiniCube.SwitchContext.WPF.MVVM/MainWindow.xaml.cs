namespace MiniCube.SwitchContext.WPF.MVVM
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new[]
                          {
                              new GameViewModel("Game 1", new MiniCubeGame()),
                              new GameViewModel("Game 2", new MiniCubeGame())
                          };
        }
    }
}
