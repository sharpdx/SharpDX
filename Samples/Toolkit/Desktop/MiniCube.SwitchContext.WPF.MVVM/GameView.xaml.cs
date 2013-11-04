namespace MiniCube.SwitchContext.WPF.MVVM
{
    using System;
    using System.Windows;
    using SharpDX.Toolkit;

    public partial class GameView
    {
        public static readonly DependencyProperty GameProperty = DependencyProperty
            .Register("Game", typeof(Game), typeof(GameView), new FrameworkPropertyMetadata(HandleGameChanged));

        private static void HandleGameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var view = d as GameView;
            if (view == null) return;

            var newGame = e.NewValue as Game;

            view.Content = null; // old element will be unloaded and will disable the game

            if(newGame != null)
            {
                // new element will reactivate the game on loading
                var element = new SharpDXElement
                              {
                                  SendResizeToGame = true,
                                  SendResizeDelay = TimeSpan.FromSeconds(1),
                                  LowPriorityRendering = false
                              };
                view.Content = element;

                if(newGame.IsRunning)
                    newGame.Switch(element);
                else
                    newGame.Run(element);
            }
        }

        public GameView()
        {
            InitializeComponent();
        }

        public Game Game
        {
            get { return (Game)GetValue(GameProperty); }
            set { SetValue(GameProperty, value); }
        }
    }
}
