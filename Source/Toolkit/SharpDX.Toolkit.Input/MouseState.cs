namespace SharpDX.Toolkit.Input
{
    public struct MouseState
    {
        private readonly KeyState left;
        private readonly KeyState middle;
        private readonly KeyState right;
        private readonly KeyState xButton1;
        private readonly KeyState xButton2;
        private readonly int x;
        private readonly int y;
        private readonly int wheelDelta;

        public MouseState(KeyState left, KeyState middle, KeyState right, KeyState xButton1, KeyState xButton2, int x, int y, int wheelDelta)
        {
            this.left = left;
            this.middle = middle;
            this.right = right;
            this.xButton1 = xButton1;
            this.xButton2 = xButton2;
            this.x = x;
            this.y = y;
            this.wheelDelta = wheelDelta;
        }

        public KeyState Left { get { return left; } }
        public KeyState Middle { get { return middle; } }
        public KeyState Right { get { return right; } }
        public KeyState XButton1 { get { return xButton1; } }
        public KeyState XButton2 { get { return xButton2; } }
        public int X { get { return x; } }
        public int Y { get { return y; } }
        public int WheelDelta { get { return wheelDelta; } }
    }
}