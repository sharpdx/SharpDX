// Copyright (c) 2010-2012 SharpDX - Alexandre Mutel
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

namespace SharpDX.Toolkit.Input
{
    /// <summary>
    /// Represents a platform-independent information about a pointer event.
    /// </summary>
    public sealed class PointerPoint
    {
        /// <summary>
        /// The device type that raised the event.
        /// </summary>
        public PointerDeviceType DeviceType { get; internal set; }

        /// <summary>
        /// An unique identifier of this pointer input point.
        /// </summary>
        public uint PointerId { get; internal set; }

        /// <summary>
        /// The location of pointer input point in client coordinates.
        /// </summary>
        public DrawingPointF Position { get; internal set; }

        /// <summary>
        /// The timestamp when the event occured.
        /// </summary>
        public ulong Timestamp { get; internal set; }

        /// <summary>
        /// The pressed key modifiers when the event occured.
        /// </summary>
        public KeyModifiers KeyModifiers { get; internal set; }

        /// <summary>
        /// The extended information about the pointer input point.
        /// </summary>
        public PointerPointProperties Properties { get; internal set; }
    }
}