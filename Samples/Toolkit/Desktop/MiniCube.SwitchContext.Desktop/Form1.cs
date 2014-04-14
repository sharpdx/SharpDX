namespace MiniCube.SwitchContext.Desktop
{
    using System;
    using System.Drawing;
    using SharpDX.Toolkit;
    using SharpDX.Windows;

    public partial class Form1 : System.Windows.Forms.Form
    {
        private readonly Game _game;
        private RenderControl _currentRenderControl;

        public Form1(Game game)
        {
            if (game == null) throw new ArgumentNullException("game");

            _game = game;

            InitializeComponent();
        }

        public void RunGame()
        {
            button1.Enabled = false;
            button2.Enabled = true;

            SwitchToNewControlAt(new Point(13,13));
        }

        private void Button1Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button2.Enabled = true;

            SwitchToNewControlAt(new Point(13, 13));
        }

        private void Button2Click(object sender, EventArgs e)
        {
            button1.Enabled = true;
            button2.Enabled = false;

            SwitchToNewControlAt(new Point(339, 13));
        }

        private void SwitchToNewControlAt(Point location)
        {
            if(_currentRenderControl != null)
            {
                Controls.Remove(_currentRenderControl);
                _currentRenderControl = null;
            }

            _currentRenderControl = new RenderControl();
            Controls.Add(_currentRenderControl);

            _currentRenderControl.Location = location;
            _currentRenderControl.MaximumSize = new Size(320, 240);
            _currentRenderControl.MinimumSize = new Size(320, 240);
            _currentRenderControl.Size = new Size(320, 240);

            if (_game.IsRunning)
                _game.Switch(_currentRenderControl);
            else
                _game.Run(_currentRenderControl);

        }
    }
}
