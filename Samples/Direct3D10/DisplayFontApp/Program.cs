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
using System;
using SharpDX;
using SharpDX.Direct3D10;
using SharpDX.Samples;


namespace DisplayFontApp
{
    /// <summary>
    /// SharpDX Demo using D3D10 Font rendering.
    /// Animates and draws a text, boucing on the screen limits.
    /// </summary>
    public class Program : Direct3D10DemoApp
    {
        private Font font;
        private Rectangle fontDimension;
        private float xDir, yDir;
        private const string DisplayText = "SharpDX D3D10 Font";

        protected override void Initialize(DemoConfiguration demoConfiguration)
        {
            base.Initialize(demoConfiguration);

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


            font = new Font(Device, fontDescription);

            // Measure the text to display
            fontDimension = font.Measure(null, DisplayText, new Rectangle(0, 0, 800, 600), FontDrawFlags.Center | FontDrawFlags.VerticalCenter);

            xDir = 1;
            yDir = 1;
        }

        protected override void Draw(DemoTime time)
        {
            base.Draw(time);

            Device.ClearRenderTargetView(BackBufferView, new Color4(1,0.1f,0.1f,0.1f));

            // Make the text boucing on the screen limits
            if ((fontDimension.Right + xDir) > Config.Width)
                xDir = -1;
            else if ((fontDimension.Left + xDir) <= 0)
                xDir = 1;

            if ((fontDimension.Bottom + yDir) > Config.Height)
                yDir = -1;
            else if ((fontDimension.Top + yDir) <= 0)
                yDir = 1;

            fontDimension.Left += (int)xDir;
            fontDimension.Top += (int)yDir;
            fontDimension.Bottom += (int)yDir;
            fontDimension.Right += (int)xDir;

            // Draw the text
            font.DrawText(null, DisplayText, fontDimension, FontDrawFlags.Center | FontDrawFlags.VerticalCenter, new Color4(1, 1, 1, 1));
        }

        [STAThread]
        static void Main(string[] args)
        {
            Program program = new Program();
            program.Run(new DemoConfiguration("SharpDX D3D10 Font Rendering Demo") { WaitVerticalBlanking = true });
        }
    }
}