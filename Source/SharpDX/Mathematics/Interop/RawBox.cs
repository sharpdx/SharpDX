using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SharpDX.Mathematics.Interop
{
    /// <summary>
    /// Interop type for a Rectangle (4 ints).
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    [DebuggerDisplay("X: {X}, Y: {Y}, Width: {Width}, Height: {Height}")]
    public struct RawBox
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RawBox"/> struct.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public RawBox(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        /// <summary>
        /// The left position.
        /// </summary>
        public int X;

        /// <summary>
        /// The top position.
        /// </summary>
        public int Y;

        /// <summary>
        /// The right position
        /// </summary>
        public int Width;

        /// <summary>
        /// The bottom position.
        /// </summary>
        public int Height;
    }
}