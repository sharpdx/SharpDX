namespace SharpDX.Toolkit.Input
{
    using System;

    public interface IPointerService
    {
        event Action<PointerPoint> PointerCaptureLost;
        event Action<PointerPoint> PointerEntered;
        event Action<PointerPoint> PointerExited;
        event Action<PointerPoint> PointerMoved;
        event Action<PointerPoint> PointerPressed;
        event Action<PointerPoint> PointerReleased;
        event Action<PointerPoint> PointerWheelChanged;
    }
}