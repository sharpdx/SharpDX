// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
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

#if !W8CORE

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SharpDX.Toolkit.Input
{
    // functionality that implements touch handling
    internal sealed partial class PointerPlatformDesktop
    {
        // the controls registered for touch.
        private static readonly List<Control> _registeredControls = new List<Control>();
        // pointer to injected WindowProc - to avoid its garbage collection
        private User32.WindowProcDelegate windowProcDelegate;
        // pointer to original window proc
        private IntPtr originalWindowProcId;

        // used to adjust touch coordinates
        private float dpiX;
        private float dpiY;

        /// <summary>
        /// Tries to register a handler for touch Win32 events.
        /// </summary>
        /// <remarks>If Windows OS version is below Windows 7 - does nothing.</remarks>
        private void TryRegisterTouch()
        {
            // touch is not supported for OS below Windows 7
            var osVersion = Environment.OSVersion.Version;

            // Fixing Windows10 recognition
            if (osVersion.Major < 6 || 
                (osVersion.Major == 6 && osVersion.Minor < 1)) return;

            // inform OS that we can handle correctly DPI
            // TODO: check if we need this here
            User32.SetProcessDPIAware();
            RegisterTouchWndProc();
        }

        /// <summary>
        /// Registers a windows procedure that intercepts and handles the Win32 touch messages
        /// </summary>
        private void RegisterTouchWndProc()
        {
            lock (_registeredControls)
            {
                // each control can have only one handler
                if (_registeredControls.Contains(control))
                    throw new InvalidOperationException("Provided control is already registered for touch events.");

                control.HandleDestroyed += (s, e) => _registeredControls.Remove((Control)s);
                _registeredControls.Add(control);
            }

            if (control.IsHandleCreated)
                InitializeTouchProcedures();
            else
                control.HandleCreated += (s, e) => InitializeTouchProcedures();
        }

        /// <summary>
        /// Initializes window proc hook and reads current DPI info
        /// </summary>
        private void InitializeTouchProcedures()
        {
            if (!User32.RegisterTouchWindow(control.Handle, 0))
                throw new ArgumentException("Cannot register touch window.");

            // save a pointer to window proc delegate, otherwise it will get garbage collected
            windowProcDelegate = WindowProcSubClass;

            // According to the SDK doc SetWindowLongPtr should be exported by both 32/64 bit O/S
            // But it does not.
            originalWindowProcId = IntPtr.Size == 4 ?
                User32.SubclassWindow(control.Handle, User32.GWLP_WNDPROC, windowProcDelegate) :
                User32.SubclassWindow64(control.Handle, User32.GWLP_WNDPROC, windowProcDelegate);

            using (var graphics = System.Drawing.Graphics.FromHwnd(control.Handle))
            {
                dpiX = graphics.DpiX;
                dpiY = graphics.DpiY;
            }
        }

        /// <summary>
        /// Window procedure that allows interception of needed events
        /// </summary>
        /// <param name="hWnd">The Windows Handle</param>
        /// <param name="msg">Windows Message</param>
        /// <param name="wparam">wParam</param>
        /// <param name="lparam">lParam</param>
        /// <returns>0 if message wasn't handled, any other value - the message was handled</returns>
        private uint WindowProcSubClass(IntPtr hWnd, int msg, IntPtr wparam, IntPtr lparam)
        {
            var result = WindowProc(hWnd, msg, wparam, lparam);

            if (result != 0)
                return result;

            return User32.CallWindowProc(originalWindowProcId, hWnd, msg, wparam, lparam);
        }

        /// <summary>
        /// Intercept and raise touch events
        /// </summary>
        /// <remarks>Will intercept and drop mouse events generated from touch.</remarks>
        /// <param name="hWnd">The Windows Handle</param>
        /// <param name="msg">Windows Message</param>
        /// <param name="wparam">wParam</param>
        /// <param name="lparam">lParam</param>
        /// <returns>0 if message wasn't handled, any other value - the message was handled</returns>
        private uint WindowProc(IntPtr hWnd, int msg, IntPtr wparam, IntPtr lparam)
        {
            switch (msg)
            {
                case User32.WM_TOUCH:
                    DecodeAndDispatchTouchMessages(wparam, lparam);
                    return 1;
                // drop all additional events originated from touch:
                // http://the-witness.net/news/2012/10/wm_touch-is-totally-bananas/
                // http://msdn.microsoft.com/en-us/library/ms703320%28v=vs.85%29.aspx
                case User32.WM_LBUTTONDOWN:
                case User32.WM_LBUTTONUP:
                case User32.WM_RBUTTONDOWN:
                case User32.WM_RBUTTONUP:
                case User32.WM_MOUSEMOTION:
                    // Fixing x64 message processing
                    if ((User32.GetMessageExtraInfo().ToInt64() & User32.MOUSEEVENTF_FROMTOUCH) == User32.MOUSEEVENTF_FROMTOUCH)
                        return 1;
                    break;
            }

            return 0;
        }

        /// <summary>
        /// Decodes touch messages and calls corresponding methods on <see cref="PointerManager"/> class
        /// </summary>
        /// <param name="wParam">wParam</param>
        /// <param name="lParam">lParam</param>
        private void DecodeAndDispatchTouchMessages(IntPtr wParam, IntPtr lParam)
        {
            var inputsCount = User32.LoWord(wParam.ToInt32());
            var inputs = new TOUCHINPUT[inputsCount];

            try
            {
                if (!User32.GetTouchInputInfo(lParam, inputsCount, inputs, Marshal.SizeOf(typeof(TOUCHINPUT))))
                    throw new InvalidOperationException("Error calling GetTouchInfo API");

                for (var i = 0; i < inputsCount; i++)
                    DecodeAndDispatchTouchPoint(inputs[i]);
            }
            finally
            {
                User32.CloseTouchInputHandle(lParam);
            }
        }

        /// <summary>
        /// Decodes a single touch point and creates from it the <see cref="PointerPoint"/> class.
        /// </summary>
        /// <param name="input">The touch point input structure.</param>
        private void DecodeAndDispatchTouchPoint(TOUCHINPUT input)
        {
            var p = control.PointToClient(new System.Drawing.Point(AdjustX(input.x), AdjustY(input.y)));

            var mask = input.dwMask;
            var flags = input.dwFlags;

            var isPrimary = (flags & User32.TOUCHEVENTF_PRIMARY) != 0;

            PointerUpdateKind pointerUpdateKind;
            PointerEventType eventType;
            if ((flags & User32.TOUCHEVENTF_DOWN) != 0)
            {
                pointerUpdateKind = PointerUpdateKind.LeftButtonPressed;
                eventType = PointerEventType.Pressed;
            }
            else if ((flags & User32.TOUCHEVENTF_DOWN) != 0)
            {
                pointerUpdateKind = PointerUpdateKind.LeftButtonReleased;
                eventType = PointerEventType.Released;
            }
            else
            {
                pointerUpdateKind = PointerUpdateKind.Other;
                eventType = PointerEventType.Moved;
            }

            var clientSize = control.ClientSize;
            var position = new Vector2((float)p.X / clientSize.Width, (float)p.Y / clientSize.Height);
            position.Saturate();

            var point = new PointerPoint
                        {
                            EventType = eventType,
                            DeviceType = ((flags & User32.TOUCHEVENTF_PEN) != 0) ? PointerDeviceType.Pen : PointerDeviceType.Touch,
                            PointerId = (uint)input.dwID,
                            Timestamp = (ulong)input.dwTime,
                            Position = position,
                            KeyModifiers = GetCurrentKeyModifiers(),
                            IsPrimary = isPrimary,
                            IsInRange = (flags & User32.TOUCHEVENTF_INRANGE) != 0,
                            TouchConfidence = (flags & User32.TOUCHEVENTF_PALM) != 0,
                            PointerUpdateKind = pointerUpdateKind,
                        };

            if ((mask & User32.TOUCHINPUTMASKF_CONTACTAREA) != 0)
                point.ContactRect = new RectangleF(position.X, position.Y, (float)AdjustX(input.cxContact) / clientSize.Width, (float)AdjustY(input.cyContact) / clientSize.Height);

            manager.AddPointerEvent(ref point);
        }

        /// <summary>
        /// Adjusts the value by current DPI on X axis
        /// </summary>
        /// <param name="x">Value to adjust</param>
        /// <returns>Adjusted value</returns>
        private int AdjustX(int x)
        {
            return (int)(x * 0.96f / dpiX);
        }

        /// <summary>
        /// Adjusts the value by current DPI on Y axis
        /// </summary>
        /// <param name="y">Value to adjust</param>
        /// <returns>Adjusted value</returns>
        private int AdjustY(int y)
        {
            return (int)(y * 0.96f / dpiY);
        }

        #region Win32 API stuff

        private enum DigitizerIndex
        {
            SM_DIGITIZER = 94,
            SM_MAXIMUMTOUCHES = 95
        }

        private enum TouchWindowFlag : uint
        {
            FineTouch = 0x1,
            WantPalm = 0x2
        }

        private static class User32
        {
            public const int GWLP_WNDPROC = -4;

            public const int WM_MOUSEMOTION = 0x0200;
            public const int WM_LBUTTONDOWN = 0x0201;
            public const int WM_LBUTTONUP = 0x0202;
            public const int WM_RBUTTONDOWN = 0x0204;
            public const int WM_RBUTTONUP = 0x0205;
            public const int WM_TOUCH = 0x0240;

            public const uint MOUSEEVENTF_FROMTOUCH = 0xFF515700;

            public const int TOUCHEVENTF_MOVE = 0x0001;
            public const int TOUCHEVENTF_DOWN = 0x0002;
            public const int TOUCHEVENTF_UP = 0x0004;
            public const int TOUCHEVENTF_INRANGE = 0x0008;
            public const int TOUCHEVENTF_PRIMARY = 0x0010;
            public const int TOUCHEVENTF_NOCOALESCE = 0x0020;
            public const int TOUCHEVENTF_PEN = 0x0040;
            public const int TOUCHEVENTF_PALM = 0x0080;

            public const int TOUCHINPUTMASKF_TIMEFROMSYSTEM = 0x0001; // the dwTime field contains a system generated value
            public const int TOUCHINPUTMASKF_EXTRAINFO = 0x0002; // the dwExtraInfo field is valid
            public const int TOUCHINPUTMASKF_CONTACTAREA = 0x0004; // the cxContact and cyContact fields are valid

            public delegate uint WindowProcDelegate(IntPtr hWnd, int msg, IntPtr wparam, IntPtr lparam);

            [DllImport("user32")]
            public static extern bool SetProcessDPIAware();

            [DllImport("user32", EntryPoint = "SetWindowLongPtr")]
            public static extern IntPtr SubclassWindow64(IntPtr hWnd, int nIndex, WindowProcDelegate dwNewLong);

            [DllImport("user32", EntryPoint = "SetWindowLong")]
            public static extern IntPtr SubclassWindow(IntPtr hWnd, int nIndex, WindowProcDelegate dwNewLong);

            [DllImport("user32")]
            public static extern uint CallWindowProc(IntPtr prevWndFunc, IntPtr hWnd, int msg, IntPtr wparam, IntPtr lparam);

            [DllImport("user32", EntryPoint = "GetSystemMetrics")]
            public static extern int GetDigitizerCapabilities(DigitizerIndex index);

            [DllImport("user32.dll", SetLastError = false)]
            public static extern IntPtr GetMessageExtraInfo();

            [DllImport("user32")]
            public static extern bool RegisterTouchWindow(IntPtr hWnd, TouchWindowFlag flags);

            [DllImport("user32")]
            public static extern bool UnregisterTouchWindow(IntPtr hWnd);

            [DllImport("user32")]
            public static extern bool IsTouchWindow(IntPtr hWnd, out uint ulFlags);

            [DllImport("user32")]
            public static extern bool GetTouchInputInfo(IntPtr hTouchInput, int cInputs, [In, Out] TOUCHINPUT[] pInputs, int cbSize);

            [DllImport("user32")]
            public static extern void CloseTouchInputHandle(IntPtr lParam);

            public static ushort LoWord(uint number)
            {
                return (ushort)(number & 0xffff);
            }

            public static ushort HiWord(uint number)
            {
                return (ushort)((number >> 16) & 0xffff);
            }

            public static uint LoDWord(ulong number)
            {
                return (uint)(number & 0xffffffff);
            }

            public static uint HiDWord(ulong number)
            {
                return (uint)((number >> 32) & 0xffffffff);
            }

            public static short LoWord(int number)
            {
                return (short)number;
            }

            public static short HiWord(int number)
            {
                return (short)(number >> 16);
            }

            public static int LoDWord(long number)
            {
                return (int)(number);
            }

            public static int HiDWord(long number)
            {
                return (int)((number >> 32));
            }
        }

        /// <summary>
        /// Touch API defined structures [winuser.h]
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        private struct TOUCHINPUT
        {
            public int x;
            public int y;
            public System.IntPtr hSource;
            public int dwID;
            public int dwFlags;
            public int dwMask;
            public int dwTime;
            public System.IntPtr dwExtraInfo;
            public int cxContact;
            public int cyContact;
        }

        /// <summary>
        /// All available digitizer capabilities
        /// </summary>
        [Flags]
        private enum DigitizerStatus : byte
        {
            IntegratedTouch = 0x01,
            ExternalTouch = 0x02,
            IntegratedPan = 0x04,
            ExternalPan = 0x08,
            MultiInput = 0x40,
            StackReady = 0x80
        }

        /// <summary>
        /// Report digitizer capabilities
        /// </summary>
        private static class DigitizerCapabilities
        {
            /// <summary>
            /// Get the current Digitizer Status
            /// </summary>
            public static DigitizerStatus Status { get { return (DigitizerStatus)User32.GetDigitizerCapabilities(DigitizerIndex.SM_DIGITIZER); } }

            /// <summary>
            /// Get the maximum touches capability
            /// </summary>
            public static int MaxumumTouches { get { return User32.GetDigitizerCapabilities(DigitizerIndex.SM_MAXIMUMTOUCHES); } }

            /// <summary>
            /// Check for integrated touch support
            /// </summary>
            public static bool IsIntegratedTouch { get { return (Status & DigitizerStatus.IntegratedTouch) != 0; } }

            /// <summary>
            /// Check for external touch support
            /// </summary>
            public static bool IsExternalTouch { get { return (Status & DigitizerStatus.ExternalTouch) != 0; } }

            /// <summary>
            /// Check for Pen support
            /// </summary>
            public static bool IsIntegratedPan { get { return (Status & DigitizerStatus.IntegratedPan) != 0; } }

            /// <summary>
            /// Check for external Pan support
            /// </summary>
            public static bool IsExternalPan { get { return (Status & DigitizerStatus.ExternalPan) != 0; } }

            /// <summary>
            /// Check for multi-input
            /// </summary>
            public static bool IsMultiInput { get { return (Status & DigitizerStatus.MultiInput) != 0; } }

            /// <summary>
            /// Check if touch device is ready
            /// </summary>
            public static bool IsStackReady { get { return (Status & DigitizerStatus.StackReady) != 0; } }

            /// <summary>
            /// Check if Multi-touch support device is ready
            /// </summary>
            public static bool IsMultiTouchReady { get { return (Status & (DigitizerStatus.StackReady | DigitizerStatus.MultiInput)) != 0; } }
        }

        #endregion
    }
}

#endif