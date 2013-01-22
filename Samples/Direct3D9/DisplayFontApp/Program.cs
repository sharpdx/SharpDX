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
using SharpDX;
using SharpDX.Direct3D9;
using SharpDX.Windows;

namespace DisplayFontApp
{
    /// <summary>
    ///   Direct3D9 Font Sample
    /// </summary>
    static class Program
    {
        [STAThread]
        static void Main()
        {
            var form = new RenderForm("SharpDX - Direct3D9 Font Sample");
            int width = form.ClientSize.Width;
            int height = form.ClientSize.Height;
            var device = new Device(new Direct3D(), 0, DeviceType.Hardware, form.Handle, CreateFlags.HardwareVertexProcessing, new PresentParameters(width, height) { PresentationInterval = PresentInterval.One });

            // Initialize the Font
            FontDescription fontDescription = new FontDescription()
            {
                Height = 72,
                Italic = false,
                CharacterSet = FontCharacterSet.Ansi,
                FaceName = "Arial",
                MipLevels = 0,
                OutputPrecision = FontPrecision.TrueType,
                PitchAndFamily = FontPitchAndFamily.Default,
                Quality = FontQuality.ClearType,
                Weight = FontWeight.Bold
            };



            var font = new Font(device, fontDescription);

            var displayText = "Direct3D9 Text!";

            // Measure the text to display
            var fontDimension = font.MeasureText(null, displayText, new Rectangle(0, 0, width, height), FontDrawFlags.Center | FontDrawFlags.VerticalCenter);

            int xDir = 1;
            int yDir = 1;

            RenderLoop.Run(form, () =>
            {
                device.Clear(ClearFlags.Target, Color.Black, 1.0f, 0);
                device.BeginScene();

                // Make the text boucing on the screen limits
                if ((fontDimension.Right + xDir) > width)
                    xDir = -1;
                else if ((fontDimension.Left + xDir) <= 0)
                    xDir = 1;

                if ((fontDimension.Bottom + yDir) > height)
                    yDir = -1;
                else if ((fontDimension.Top + yDir) <= 0)
                    yDir = 1;

                fontDimension.Left += (int)xDir;
                fontDimension.Top += (int)yDir;
                fontDimension.Bottom += (int)yDir;
                fontDimension.Right += (int)xDir;

                // Draw the text
                font.DrawText(null, displayText, fontDimension, FontDrawFlags.Center | FontDrawFlags.VerticalCenter, Color.White);

                device.EndScene();
                device.Present();
            });
        }
    }
}