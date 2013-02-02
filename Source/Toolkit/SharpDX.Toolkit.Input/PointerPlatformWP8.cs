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
#if WP8

using System;
using System.Windows.Controls;
using Windows.Phone.Input.Interop;

namespace SharpDX.Toolkit.Input
{
    /// <summary>
    /// WinPhone 8 platform-specific implmentation of <see cref="PointerPlatform"/>.
    /// </summary>
    /// <remarks>Implements <see cref="IDrawingSurfaceManipulationHandler"/>.</remarks>
    internal sealed class PointerPlatformWP8 : PointerPlatform, IDrawingSurfaceManipulationHandler
    {
        /// <summary>
        /// Initializes a new instace of <see cref="PointerPlatformWP8"/> class.
        /// </summary>
        /// <param name="nativeWindow">The platform-specific reference to window object</param>
        /// <param name="manager">The <see cref="PointerManager"/> whose events will be raised in response to platform-specific events</param>
        /// <exception cref="ArgumentNullException">Is thrown when either <paramref name="nativeWindow"/> or <paramref name="manager"/> is null.</exception>
        public PointerPlatformWP8(object nativeWindow, PointerManager manager)
            : base(nativeWindow, manager) { }

        /// <inheritdoc />
        protected override void BindWindow(object nativeWindow)
        {
            if (nativeWindow == null) throw new ArgumentNullException("nativeWindow");

            // only DrawingSurfaceBackgroundGrid is supported at this time
            var grid = nativeWindow as DrawingSurfaceBackgroundGrid;
            if (grid != null)
            {
                // avoid threading issues as this is much more restrictive on WP8
                if (grid.Dispatcher.CheckAccess())
                    BindManipulationEvents(grid);
                else
                    grid.Dispatcher.BeginInvoke(() => BindManipulationEvents(grid));

                return;
            }

            throw new ArgumentException("Should be an instance of DrawingSurfaceBackgroundGrid", "nativeWindow");
        }

        /// <summary>
        /// Binds the corresponding event handler to the provided <see cref="DrawingSurfaceBackgroundGrid"/>
        /// </summary>
        /// <param name="grid">An instance of <see cref="DrawingSurfaceBackgroundGrid"/> whose events needs to be bound to</param>
        /// <exception cref="ArgumentNullException">Is thrown if <paramref name="grid"/> is null</exception>
        private void BindManipulationEvents(DrawingSurfaceBackgroundGrid grid)
        {
            if (grid == null) throw new ArgumentNullException("grid");

            grid.SetBackgroundManipulationHandler(this);

            // TODO: review if we need to unbind the handlers to avoid memory leaks
            grid.Unloaded += (_, __) => grid.SetBackgroundManipulationHandler(null);
            Disposing += (_, __) => grid.SetBackgroundManipulationHandler(null);
        }

        /// <summary>
        /// Binds to the following events:
        ///  <see cref="DrawingSurfaceManipulationHost.PointerMoved"/>,
        ///  <see cref="DrawingSurfaceManipulationHost.PointerPressed"/>,
        ///  <see cref="DrawingSurfaceManipulationHost.PointerReleased"/>
        /// </summary>
        /// <param name="manipulationHost">An instance of <see cref="DrawingSurfaceManipulationHost"/>.</param>
        /// <exception cref="ArgumentNullException">Is thrown if <paramref name="manipulationHost"/> is null.</exception>
        void IDrawingSurfaceManipulationHandler.SetManipulationHost(DrawingSurfaceManipulationHost manipulationHost)
        {
            if (manipulationHost == null) throw new ArgumentNullException("manipulationHost");

            manipulationHost.PointerMoved += (_, e) => CreateAndAddPoint(PointerEventType.Moved, e.CurrentPoint);
            manipulationHost.PointerPressed += (_, e) => CreateAndAddPoint(PointerEventType.Pressed, e.CurrentPoint);
            manipulationHost.PointerReleased += (_, e) => CreateAndAddPoint(PointerEventType.Released, e.CurrentPoint);
        }

        /// <summary>
        /// Creates a platform-independent instance of <see cref="PointerPoint"/> class from WP8-specific objects.
        /// </summary>
        /// <param name="type">The pointer event type.</param>
        /// <param name="point">The WP8-specific instance of pointer point.</param>
        /// <returns>An instance of <see cref="PointerPoint"/> class.</returns>
        private void CreateAndAddPoint(PointerEventType type, global::Windows.UI.Input.PointerPoint point)
        {
            if (point == null) throw new ArgumentNullException("point");

            var position = point.Position;
            var properties = point.Properties;
            var contactRect = properties.ContactRect;

            var result = new PointerPoint
                         {
                             EventType = type,
                             DeviceType = PointerDeviceType.Touch,
                             KeyModifiers = KeyModifiers.None,
                             PointerId = point.PointerId,
                             Position = new DrawingPointF((float)position.X, (float)position.Y),
                             Timestamp = point.Timestamp,
                             ContactRect = new DrawingRectangleF((float)contactRect.X, (float)contactRect.Y, (float)contactRect.Width, (float)contactRect.Height),
                             IsBarrelButtonPresset = properties.IsBarrelButtonPressed,
                             IsCanceled = properties.IsCanceled,
                             IsEraser = properties.IsEraser,
                             IsHorizontalMouseWheel = properties.IsHorizontalMouseWheel,
                             IsInRange = properties.IsInRange,
                             IsInverted = properties.IsInverted,
                             IsLeftButtonPressed = properties.IsLeftButtonPressed,
                             IsMiddleButtonPressed = properties.IsMiddleButtonPressed,
                             IsPrimary = properties.IsPrimary,
                             IsRightButtonPressed = properties.IsRightButtonPressed,
                             IsXButton1Pressed = properties.IsXButton1Pressed,
                             IsXButton2Pressed = properties.IsXButton2Pressed,
                             MouseWheelDelta = properties.MouseWheelDelta,
                             Orientation = properties.Orientation,
                             TouchConfidence = properties.TouchConfidence,
                             Twist = properties.Twist,
                             XTilt = properties.XTilt,
                             YTilt = properties.YTilt,
                             PointerUpdateKind = PointerUpdateKind.Other
                         };

            manager.AddPointerEvent(ref result);
        }
    }
}

#endif