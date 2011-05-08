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
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using SharpDX.Samples;

namespace SimpleHelloWorld
{

    /// <summary>
    /// Shows how to use DirectWrite to render simple text.
    /// Port of DirectWrite sample SimpleHelloWorld from Windows 7 SDK samples
    /// http://msdn.microsoft.com/en-us/library/dd742738%28v=VS.85%29.aspx
    /// </summary>
    public class Program :  Direct2D1WinFormDemoApp
    {
        public TextFormat TextFormat { get; private set; }
        public RectangleF ClientRectangle { get; private set; }
        protected override void Initialize(DemoConfiguration demoConfiguration)
        {
            base.Initialize(demoConfiguration);

            // Initialize a TextFormat
            TextFormat = new TextFormat(FactoryDWrite, "Gabriola", 96) { TextAlignment = TextAlignment.Center, ParagraphAlignment = ParagraphAlignment.Center };

            RenderTarget2D.TextAntialiasMode = TextAntialiasMode.Cleartype;

            ClientRectangle = new RectangleF(0, 0, demoConfiguration.Width, demoConfiguration.Height);

            SceneColorBrush.Color = new Color4(1, 0, 0, 0);               
        }

        protected override void Draw(DemoTime time)
        {
            base.Draw(time);

            RenderTarget2D.Clear(new Color4(1,1,1,1));

            RenderTarget2D.DrawText("Hello World using DirectWrite!", TextFormat, ClientRectangle, SceneColorBrush);
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Program program = new Program();
            program.Run(new DemoConfiguration("SharpDX DirectWrite Simple HelloWorld Demo"));
        }
    }
}
