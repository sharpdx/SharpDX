namespace MiniCube.SwitchContext.WPF.MVVM
{
    using System;
    using System.ComponentModel;
    using SharpDX.Toolkit;

    public sealed class GameViewModel : INotifyPropertyChanged
    {
        private string _name;
        private Game _game;

        public GameViewModel(string name, Game game)
        {
            if (game == null) throw new ArgumentNullException("game");

            Name = name;
            Game = game;
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (Equals(_name, value)) return;
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        public Game Game
        {
            get { return _game; }
            set
            {
                if (ReferenceEquals(_game, value)) return;
                _game = value;
                OnPropertyChanged("Game");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}