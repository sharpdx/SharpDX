namespace SharpDX.Toolkit.Input
{
    using System;

    public class MouseManager : Component, IGameSystem, IMouseService
    {
        private readonly Game game;

        private KeyState left;
        private KeyState middle;
        private KeyState right;
        private KeyState xButton1;
        private KeyState xButton2;
        private int wheelDelta;

        private WindowBinder binder;

        public MouseManager(Game game)
        {
            if (game == null) throw new ArgumentNullException("game");

            this.game = game;
            game.Services.AddService(typeof(IMouseService), this);
            game.GameSystems.Add(this);
        }

        public void Initialize()
        {
            var w = game.Window.NativeWindow;
            binder = WindowBinder.Create(w);

            binder.MouseDown += HandleMouseDown;
            binder.MouseUp += HandleMouseUp;
            binder.MouseWheelDelta += HandleWheelDelta;
        }

        public MouseState GetState()
        {
            var position = binder.GetLocation();
            return new MouseState(left, middle, right, xButton1, xButton2, position.X, position.Y, wheelDelta);
        }

        private void HandleMouseDown(MouseButton button)
        {
            SetButtonStateTo(button, KeyState.Down);
        }

        private void HandleMouseUp(MouseButton button)
        {
            SetButtonStateTo(button, KeyState.Up);
        }

        private void HandleWheelDelta(int wheelDelta)
        {
            this.wheelDelta = wheelDelta;
        }

        private void SetButtonStateTo(MouseButton button, KeyState state)
        {
            switch (button)
            {
                case MouseButton.None:
                    break;
                case MouseButton.Left:
                    left = state;
                    break;
                case MouseButton.Middle:
                    middle = state;
                    break;
                case MouseButton.Right:
                    right = state;
                    break;
                case MouseButton.XButton1:
                    xButton1 = state;
                    break;
                case MouseButton.XButton2:
                    xButton2 = state;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("button");
            }
        }
    }
}