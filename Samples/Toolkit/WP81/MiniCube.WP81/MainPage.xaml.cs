using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using SharpDX.Toolkit;

namespace MiniCube.WP81
{
    public sealed partial class MainPage : Page
    {
        private Game _game;

        public MainPage()
        {
            InitializeComponent();

            NavigationCacheMode = NavigationCacheMode.Required;

            _game = new MiniCubeGame();
            _game.Run(DrawingSurface);
        }
    }
}
