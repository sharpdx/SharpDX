namespace MiniCube.WP8.DrawingSurface
{
    public partial class MainPage
    {
        private readonly MiniCubeGame helloWorldGame;

        public MainPage()
        {
            InitializeComponent();

            helloWorldGame = new MiniCubeGame();
            helloWorldGame.Run(DrawingSurface);
        }
    }
}