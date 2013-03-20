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
    public struct PointerPoint
    {
        /// <summary>
        /// The type of event that represents current pointer point
        /// </summary>
        public PointerEventType EventType { get; internal set; }

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
        public Vector2 Position { get; internal set; }

        /// <summary>
        /// The timestamp when the event occured.
        /// </summary>
        public ulong Timestamp { get; internal set; }

        /// <summary>
        /// The pressed key modifiers when the event occured.
        /// </summary>
        public KeyModifiers KeyModifiers { get; internal set; }

        /// <summary>
        /// The bounding rectangle of the contact area (typically for touch).
        /// </summary>
        public RectangleF ContactRect { get; internal set; }

        /// <summary>
        /// Indicates whether the barrel button of the pen/stylus device is pressed.
        /// </summary>
        public bool IsBarrelButtonPresset { get; internal set; }

        /// <summary>
        /// Indicates whether the input was canceled by pointer device.
        /// </summary>
        public bool IsCanceled { get; internal set; }

        /// <summary>
        /// Indicates whether the input is from the digitizer eraser.
        /// </summary>
        public bool IsEraser { get; internal set; }

        /// <summary>
        /// Indicates whether the input is from the mouse tilt wheel.
        /// </summary>
        public bool IsHorizontalMouseWheel { get; internal set; }

        /// <summary>
        /// Indicates whether the finger or pen is in range of the digitizer.
        /// </summary>
        public bool IsInRange { get; internal set; }

        /// <summary>
        /// Indicates whether the digitizer is inverted.
        /// </summary>
        public bool IsInverted { get; internal set; }

        /// <summary>
        /// Indicates whether the input is from the left button of the mouse or other input device.
        /// </summary>
        public bool IsLeftButtonPressed { get; internal set; }

        /// <summary>
        /// Indicates whether the input is from the middle button of the mouse or other input device.
        /// </summary>
        public bool IsMiddleButtonPressed { get; internal set; }

        /// <summary>
        /// Indicates whether the input is from the right button of the mouse or other input device.
        /// </summary>
        public bool IsRightButtonPressed { get; internal set; }

        /// <summary>
        /// Indicates whether the input is from the X button 1 of the mouse or other input device.
        /// </summary>
        public bool IsXButton1Pressed { get; internal set; }

        /// <summary>
        /// Indicates whether the input is from the X button 2 of the mouse or other input device.
        /// </summary>
        public bool IsXButton2Pressed { get; internal set; }

        /// <summary>
        /// Indicates whether the input is from the primary pointer when multiple pointers are registered.
        /// </summary>
        public bool IsPrimary { get; internal set; }

        /// <summary>
        /// Indicates the raw device value of the change in wheel button input from the last event.
        /// </summary>
        public int MouseWheelDelta { get; internal set; }

        /// <summary>
        /// Indicates the counter-clockwise angle of the pointer device around the z-axis (perpendicular to digitizer).
        /// </summary>
        public float Orientation { get; internal set; }

        /// <summary>
        /// Indicates whether the pointer device rejected the touch input.
        /// </summary>
        public bool TouchConfidence { get; internal set; }

        /// <summary>
        /// Indicates the clock-wise rotation of the pointer device around its own major axis.
        /// </summary>
        public float Twist { get; internal set; }

        /// <summary>
        /// Indicates the plane angle between the Y-Z plane and the plane that contain the Y axis and the axis of the input device (typically pen or stylus).
        /// </summary>
        public float XTilt { get; internal set; }

        /// <summary>
        /// Indicates the plane angle between the X-Z plane and the plane that contain the X axis and the axis of the input device (typically pen or stylus).
        /// </summary>
        public float YTilt { get; internal set; }

        /// <summary>
        /// Indicates the kind of pointer state change.
        /// </summary>
        public PointerUpdateKind PointerUpdateKind { get; internal set; }
    }
}