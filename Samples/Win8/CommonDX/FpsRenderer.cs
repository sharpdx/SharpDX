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
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonDX;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using Matrix = SharpDX.Matrix;
using TextAntialiasMode = SharpDX.Direct2D1.TextAntialiasMode;

namespace CommonDX
{
    /// <summary>
    /// Display an overlay text with FPS and ms/frame counters.
    /// </summary>
    public class FpsRenderer
    {
        TextFormat textFormat;
        Brush sceneColorBrush;
        Stopwatch clock;
        double totalTime;
        long frameCount;
        double measuredFPS;

        /// <summary>
        /// Initializes a new instance of <see cref="FpsRenderer"/> class.
        /// </summary>
        public FpsRenderer()
        {
            Show = true;
        }

        public bool Show { get; set; }

        public virtual void Initialize(DeviceManager deviceManager)
        {
            sceneColorBrush = new SolidColorBrush(deviceManager.ContextDirect2D, Color.White);
            textFormat = new TextFormat(deviceManager.FactoryDirectWrite, "Calibri", 20) { TextAlignment = TextAlignment.Leading, ParagraphAlignment = ParagraphAlignment.Center };
            clock = Stopwatch.StartNew();
            deviceManager.ContextDirect2D.TextAntialiasMode = TextAntialiasMode.Grayscale;
        }

        public virtual void Render(TargetBase target)
        {
            if (!Show)
                return;

            frameCount++;
            var timeElapsed = (double)clock.ElapsedTicks / Stopwatch.Frequency; ;
            totalTime += timeElapsed;
            if (totalTime >= 1.0f)
            {
                measuredFPS = (double)frameCount / totalTime;
                frameCount = 0;
                totalTime = 0.0;
            }

            var context2D = target.DeviceManager.ContextDirect2D;

            context2D.BeginDraw();
            context2D.Transform = Matrix.Identity;
            context2D.DrawText(string.Format("{0:F2} FPS ({1:F1} ms)", measuredFPS, timeElapsed * 1000.0), textFormat, new RectangleF(8, 8, 8 + 256, 8 + 16), sceneColorBrush);
            context2D.EndDraw();

            clock.Restart();
        }
    }
}
