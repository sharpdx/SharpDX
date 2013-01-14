namespace SharpDX.Toolkit.Input
{
    public sealed class PointerPoint
    {
        public PointerDeviceType DeviceType { get; internal set; }
        public uint PointerId { get; internal set; }
        public DrawingPointF Position { get; internal set; }
        public ulong Timestamp { get; internal set; }

        public KeyModifiers KeyModifiers { get; internal set; }

        public PointerPointProperties Properties { get; internal set; }
    }
}