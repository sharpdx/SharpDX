namespace MiniCube.WPF
{
    public partial class MainWindow
    {
        private readonly MiniCubeGame game;

        public MainWindow()
        {
            InitializeComponent();

            game = new MiniCubeGame();
            game.Run(GameSurface);
        }
    }
}
