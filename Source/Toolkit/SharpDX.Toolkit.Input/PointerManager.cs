namespace SharpDX.Toolkit.Input
{
    using System;

    public class PointerManager : Component, IGameSystem, IPointerService
    {
        private readonly Game game;
        private PointerPlatform platform;

        public PointerManager(Game game)
        {
            if (game == null) throw new ArgumentNullException("game");

            this.game = game;

            this.game.Services.AddService(typeof(IPointerService), this);
            this.game.GameSystems.Add(this);
        }

        public event Action<PointerPoint> PointerCaptureLost;
        public event Action<PointerPoint> PointerEntered;
        public event Action<PointerPoint> PointerExited;
        public event Action<PointerPoint> PointerMoved;
        public event Action<PointerPoint> PointerPressed;
        public event Action<PointerPoint> PointerReleased;
        public event Action<PointerPoint> PointerWheelChanged;

        public void Initialize()
        {
            // TODO: assume Initialize is called only once. Implement cleanup in case if it is called several times.
            platform = PointerPlatform.Create(game.Window.NativeWindow, this);
        }

        internal void RaiseCaptureLost(PointerPoint point)
        {
            RaisePointerEvent(PointerCaptureLost, point);
        }

        internal void RaiseEntered(PointerPoint point)
        {
            RaisePointerEvent(PointerEntered, point);
        }

        internal void RaiseExited(PointerPoint point)
        {
            RaisePointerEvent(PointerExited, point);
        }

        internal void RaiseMoved(PointerPoint point)
        {
            RaisePointerEvent(PointerMoved, point);
        }

        internal void RaisePressed(PointerPoint point)
        {
            RaisePointerEvent(PointerPressed, point);
        }

        internal void RaiseReleased(PointerPoint point)
        {
            RaisePointerEvent(PointerReleased, point);
        }

        internal void RaiseWheelChanged(PointerPoint point)
        {
            RaisePointerEvent(PointerWheelChanged, point);
        }

        private static void RaisePointerEvent(Action<PointerPoint> handler, PointerPoint point)
        {
            if (point == null) throw new ArgumentNullException("point");

            if (handler != null)
                handler(point);
        }
    }
}