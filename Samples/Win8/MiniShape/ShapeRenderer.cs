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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonDX;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;

namespace MiniShape
{
    public class ShapeRenderer
    {
        TextFormat textFormat;
        TextLayout textLayout;
        Brush sceneColorBrush;
        RoundedRectangleGeometry rectangle;

        public virtual void Initialize(DeviceManager deviceManager)
        {
            // Initialize a TextFormat
            textFormat = new TextFormat(deviceManager.FactoryDirectWrite, "Calibri", 128) { TextAlignment = TextAlignment.Center, ParagraphAlignment = ParagraphAlignment.Center };

            textLayout = new TextLayout(deviceManager.FactoryDirectWrite, "SharpDX D2D1 - DirectWrite", textFormat, 1024, 512);

            sceneColorBrush = new SolidColorBrush(deviceManager.ContextDirect2D, Colors.White);

            rectangle = new RoundedRectangleGeometry(deviceManager.FactoryDirect2D, new RoundedRect() { RadiusX = 32, RadiusY = 32, Rect = new RectangleF(0, 0, 128, 128) });
        }

        public virtual void Render(TargetBase target)
        {
            var context2D = target.DeviceManager.ContextDirect2D;

            context2D.BeginDraw();

            context2D.TextAntialiasMode = TextAntialiasMode.Grayscale;

            context2D.DrawTextLayout(new DrawingPointF(0, 0), textLayout, sceneColorBrush, DrawTextOptions.None);

            context2D.FillGeometry(rectangle, sceneColorBrush);

            context2D.EndDraw();
        }

    }
}
