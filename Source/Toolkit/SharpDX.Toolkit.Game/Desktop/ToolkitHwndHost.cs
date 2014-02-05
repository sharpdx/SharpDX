// Copyright (c) 2010-2013 SharpDX - Alexandre Mutel
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
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace SharpDX.Toolkit
{
    internal class ToolkitHwndHost : HwndHost
    {
        private readonly HandleRef childHandle;

        public ToolkitHwndHost(IntPtr childHandle)
        {
            this.childHandle = new HandleRef(this, childHandle);
        }

        protected override HandleRef BuildWindowCore(HandleRef hwndParent)
        {
            int style = (Win32Native.GetWindowLong(childHandle, Win32Native.WindowLongType.Style)).ToInt32();
            // Removes Caption bar and the sizing border
            style = style & ~WS_CAPTION & ~WS_THICKFRAME;
            // Must be a child window to be hosted
            style |= WS_CHILD;

            Win32Native.SetWindowLong(childHandle, Win32Native.WindowLongType.Style, new IntPtr(style));

            //MoveWindow(childHandle, 0, 0, (int)ActualWidth, (int)ActualHeight, true);
            Win32Native.SetParent(childHandle, hwndParent.Handle);

            Win32Native.ShowWindow(childHandle, false);

            return childHandle;
        }

        protected override void DestroyWindowCore(HandleRef hwnd)
        {
            Win32Native.SetParent(childHandle, IntPtr.Zero);
        }

        protected override void OnRenderSizeChanged(System.Windows.SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
        }

        // ReSharper disable InconsistentNaming
        private const int WS_CAPTION = unchecked(0x00C00000);
        private const int WS_THICKFRAME = unchecked(0x00040000);
        private const int WS_CHILD = unchecked(0x40000000);
        // ReSharper restore InconsistentNaming
    }
}
#endif