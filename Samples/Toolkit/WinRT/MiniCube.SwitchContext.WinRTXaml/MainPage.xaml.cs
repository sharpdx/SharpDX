namespace MiniCube.SwitchContext.WinRTXaml
{
    using System;
    using SharpDX.Toolkit;

    public sealed partial class MainPage
    {
        private readonly Game _game;

        public MainPage(Game game)
        {
            if(game == null) throw new ArgumentNullException("game");

            _game = game;

            InitializeComponent();

            if (_game.IsRunning)
                _game.Switch(this);
            else
                _game.Run(this);
        }
    }
}
