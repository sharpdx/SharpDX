namespace MiniCube.WinRT.SwapChainPanel
{
    using SharpDX.Toolkit;

    public sealed partial class MainPage
    {
        private readonly Game _game;

        public MainPage()
        {
            InitializeComponent();

            _game = new MiniCubeGame();
            _game.Run(panel);
        }
    }
}
