namespace SharpDX.Toolkit.Input
{
    public sealed class PointerPointProperties
    {
        public DrawingRectangleF ContactRect { get; internal set; }
        public bool IsBarrelButtonPresset { get; internal set; }
        public bool IsCanceled { get; internal set; }
        public bool IsEraser { get; internal set; }
        public bool IsHorizontalMouseWheel { get; internal set; }
        public bool IsInRange { get; internal set; }
        public bool IsInverted { get; internal set; }
        public bool IsLeftButtonPressed { get; internal set; }
        public bool IsMiddleButtonPressed { get; internal set; }
        public bool IsRightButtonPressed { get; internal set; }
        public bool IsXButton1Pressed { get; internal set; }
        public bool IsXButton2Pressed { get; internal set; }
        public bool IsPrimary { get; internal set; }
        public int MouseWheelDelta { get; internal set; }
        public float Orientation { get; internal set; }
        public bool TouchConfidence { get; internal set; }
        public float Twist { get; internal set; }
        public float XTilt { get; internal set; }
        public float YTilt { get; internal set; }
        public PointerUpdateKind PointerUpdateKind { get; internal set; }
    }
}