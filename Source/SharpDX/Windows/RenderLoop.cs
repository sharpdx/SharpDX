// Copyright (c) 2010-2011 SharpDX - Alexandre Mutel
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
#if !WIN8
using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace SharpDX.Windows
{
    /// <summary>
    /// RenderLoop provides a rendering loop infrastructure.
    /// </summary>
    public class RenderLoop
    {
        private RenderLoop()
        {
        }

        /// <summary>
        /// Delegate for the rendering loop.
        /// </summary>
        public delegate void RenderCallback();

        /// <summary>
        /// Runs the specified main loop in the specified context.
        /// </summary>
        public static void Run(ApplicationContext context, RenderCallback renderCallback)
        {
            Run(context.MainForm, renderCallback);            
        }

        /// <summary>
        /// Runs the specified main loop for the specified windows form.
        /// </summary>
        /// <param name="form">The form.</param>
        /// <param name="renderCallback">The rendering callback.</param>
        public static void Run(Form form, RenderCallback renderCallback)
        {
            var proxyWindow = new ProxyNativeWindow(form);
            proxyWindow.Run(renderCallback);
        }

        /// <summary>
        /// Gets a value indicating whether this instance is application idle.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is application idle; otherwise, <c>false</c>.
        /// </value>
        public static bool IsIdle
        {
            get
            {
                Win32Native.NativeMessage msg;
                return (bool) (Win32Native.PeekMessage(out msg, IntPtr.Zero, 0, 0, 0) == 0);
            }
        }

        /// <summary>
        /// ProxyNativeWindow, used only to detect if the original window is destroyed
        /// </summary>
        private class ProxyNativeWindow
        {
            private readonly Form _form;
            private readonly IntPtr _windowHandle;
            private readonly IntPtr _ptrToDefaultWndProc;
            private Win32Native.WndProc _wndProc;
            private bool _isAlive;

            /// <summary>
            /// Initializes a new instance of the <see cref="ProxyNativeWindow"/> class.
            /// </summary>
            public ProxyNativeWindow(Form form)
            {
                _form = form;
                _windowHandle = form.Handle;
                _ptrToDefaultWndProc = Win32Native.GetWindowLong(new HandleRef(this, _windowHandle), Win32Native.WindowLongType.WndProc);
                _isAlive = true;
            }

            /// <summary>
            /// Private WindowProc in order to handle NCDestroy message
            /// </summary>
            private IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam)
            {
                var result = Win32Native.CallWindowProc(_ptrToDefaultWndProc, hWnd, msg, wParam, lParam);
                // WM_NCDESTROY = &H82
                if (msg == 130)
                    _isAlive = false;
                return result;
            }
            
            /// <summary>
            /// Private rendering loop
            /// </summary>
            public void Run(RenderCallback renderCallback)
            {
                _wndProc = new Win32Native.WndProc(WndProc);

                // Set our own private wndproc in order to catch NCDestroy message
                Win32Native.SetWindowLong(new HandleRef(this, _windowHandle), Win32Native.WindowLongType.WndProc, _wndProc);

                // Show the form
                _form.Show();

                // Main rendering loop);
                while (_isAlive)
                {
                    Win32Native.NativeMessage msg;
                    while (Win32Native.PeekMessage(out msg, _windowHandle, 0, 0, 0) != 0)
                    {
                        if (Win32Native.GetMessage(out msg, _windowHandle, 0, 0) == -1)
                        {
                            throw new InvalidOperationException(String.Format(
                                "An error happened in rendering loop while processing windows messages. Error: {0}",
                                Marshal.GetLastWin32Error()));
                        }

                        Win32Native.TranslateMessage(ref msg);
                        Win32Native.DispatchMessage(ref msg);
                    }
                    renderCallback();
                }
            }
        }
    }
}
#endif