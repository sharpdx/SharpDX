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
using System.Drawing;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using SharpDX.Samples;

namespace TextRenderingApp
{
    public class Program : Direct2D1DemoApp
    {        
        public TextFormat TextFormat { get; private set; }
        public TextLayout TextLayout { get; private set; }

        protected override void Initialize(DemoConfiguration demoConfiguration)
        {
            base.Initialize(demoConfiguration);

            // Initialize a TextFormat
            TextFormat = new TextFormat(FactoryDWrite, "Calibri", 128) {TextAlignment = TextAlignment.Center, ParagraphAlignment = ParagraphAlignment.Center};

            RenderTarget2D.TextAntialiasMode = TextAntialiasMode.Cleartype;

            // Initialize a TextLayout
            TextLayout = new TextLayout(FactoryDWrite, "SharpDX D2D1 - DWrite", TextFormat, demoConfiguration.Width, demoConfiguration.Height);
        }


        protected override void Draw(DemoTime time)
        {
            base.Draw(time);

            // Draw the TextLayout
            RenderTarget2D.DrawTextLayout(new PointF(0,0), TextLayout, SceneColorBrush, DrawTextOptions.None );
        }


        [STAThread]
        static void Main(string[] args)
        {
            Program program = new Program();
            program.Run(new DemoConfiguration("SharpDX DirectWrite Text Rendering Demo"));
        }
    }
}
