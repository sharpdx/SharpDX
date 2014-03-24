namespace Audio.WP8
{
    public partial class MainPage
    {
        private readonly AudioGame game;

        public MainPage()
        {
            InitializeComponent();

            game = new AudioGame();
            game.Run(DrawingSurface);
        }
    }
}