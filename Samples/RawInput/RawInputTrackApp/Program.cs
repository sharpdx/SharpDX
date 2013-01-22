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

using System;
using System.Windows.Forms;

using SharpDX.Multimedia;
using SharpDX.RawInput;

namespace MouseTrackApp
{
    /// <summary>
    /// Show how to use 
    /// </summary>
    static class Program
    {
        private static TextBox textBox;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            var form = new Form { Width = 800, Height = 600};
            textBox = new TextBox() { Dock = DockStyle.Fill, Multiline = true, Text = "Interact with the mouse or the keyboard...\r\n", ReadOnly = true};
            form.Controls.Add(textBox);
            form.Visible = true;

            // setup the device
            Device.RegisterDevice(UsagePage.Generic, UsageId.GenericMouse, DeviceFlags.None);
            Device.MouseInput += (sender, args) => textBox.Invoke(new UpdateTextCallback(UpdateMouseText), args);

            Device.RegisterDevice(UsagePage.Generic, UsageId.GenericKeyboard, DeviceFlags.None);
            Device.KeyboardInput += (sender, args) => textBox.Invoke(new UpdateTextCallback(UpdateKeyboardText), args);

            Application.Run(form);
        }

        /// <summary>
        /// Updates the mouse text.
        /// </summary>
        /// <param name="rawArgs">The <see cref="SharpDX.RawInput.RawInputEventArgs"/> instance containing the event data.</param>
        static void UpdateMouseText(RawInputEventArgs rawArgs)
        {
            var args = (MouseInputEventArgs)rawArgs;

            textBox.AppendText(string.Format("(x,y):({0},{1}) Buttons: {2} State: {3} Wheel: {4}\r\n", args.X, args.Y, args.ButtonFlags, args.Mode, args.WheelDelta));            
        }

        /// <summary>
        /// Updates the keyboard text.
        /// </summary>
        /// <param name="rawArgs">The <see cref="SharpDX.RawInput.RawInputEventArgs"/> instance containing the event data.</param>
        static void UpdateKeyboardText(RawInputEventArgs rawArgs)
        {
            var args = (KeyboardInputEventArgs)rawArgs;
            textBox.AppendText(string.Format("Key: {0} State: {1} ScanCodeFlags: {2}\r\n", args.Key, args.State, args.ScanCodeFlags));
        }

        /// <summary>
        /// Delegate use for printing events
        /// </summary>
        /// <param name="args">The <see cref="SharpDX.RawInput.RawInputEventArgs"/> instance containing the event data.</param>
        public delegate void UpdateTextCallback(RawInputEventArgs args);
    }
}
