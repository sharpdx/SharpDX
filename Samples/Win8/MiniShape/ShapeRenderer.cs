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

namespace MiniShape
{
    public class ShapeRenderer
    {
        TextFormat textFormat;
        Brush sceneColorBrush;
        PathGeometry1 pathGeometry1;
        Stopwatch clock;

        public ShapeRenderer()
        {
            EnableClear = true;
            Show = true;
        }

        public bool EnableClear { get; set; }

        public bool Show { get; set; }

        public virtual void Initialize(DeviceManager deviceManager)
        {

            sceneColorBrush = new SolidColorBrush(deviceManager.ContextDirect2D, Color.White);

            clock = Stopwatch.StartNew();
        }

        public virtual void Render(TargetBase target)
        {
            if (!Show)
                return;

            var context2D = target.DeviceManager.ContextDirect2D;

            context2D.BeginDraw();

            if (EnableClear)
                context2D.Clear(Color.Black);

            var sizeX = (float)target.RenderTargetBounds.Width;
            var sizeY = (float)target.RenderTargetBounds.Height;
            var globalScaling = Matrix.Scaling(Math.Min(sizeX, sizeY));

            var centerX = (float)(target.RenderTargetBounds.X + sizeX / 2.0f);
            var centerY = (float)(target.RenderTargetBounds.Y + sizeY / 2.0f);

            if (textFormat == null)
            {
                // Initialize a TextFormat
                textFormat = new TextFormat(target.DeviceManager.FactoryDirectWrite, "Calibri", 96 * sizeX / 1920) { TextAlignment = TextAlignment.Center, ParagraphAlignment = ParagraphAlignment.Center };
            }

            if (pathGeometry1 == null)
            {
                var sizeShape = sizeX / 4.0f;

                // Creates a random geometry inside a circle
                pathGeometry1 = new PathGeometry1(target.DeviceManager.FactoryDirect2D);
                var pathSink = pathGeometry1.Open();
                var startingPoint = new Vector2(sizeShape * 0.5f, 0.0f);
                pathSink.BeginFigure(startingPoint, FigureBegin.Hollow);
                for (int i = 0; i < 128; i++)
                {
                    float angle = (float)i / 128.0f * (float)Math.PI * 2.0f;
                    float R = (float)(Math.Cos(angle) * 0.1f + 0.4f);
                    R *= sizeShape;
                    Vector2 point1 = new Vector2(R * (float)Math.Cos(angle), R * (float)Math.Sin(angle));

                    if ((i & 1) > 0)
                    {
                        R = (float)(Math.Sin(angle * 6.0f) * 0.1f + 0.9f);
                        R *= sizeShape;
                        point1 = new Vector2(R * (float)Math.Cos(angle + Math.PI / 12), R * (float)Math.Sin(angle + Math.PI / 12));
                    }
                    pathSink.AddLine(point1);
                }
                pathSink.EndFigure(FigureEnd.Open);
                pathSink.Close();
            }

            context2D.TextAntialiasMode = TextAntialiasMode.Grayscale;
            float t = clock.ElapsedMilliseconds / 1000.0f;

            context2D.Transform = Matrix.RotationZ((float)(Math.Cos(t * 2.0f * Math.PI * 0.5f))) * Matrix.Translation(centerX, centerY, 0);

            context2D.DrawText("SharpDX\nDirect2D1\nDirectWrite", textFormat, new RectangleF(-sizeX / 2.0f, -sizeY / 2.0f, +sizeX/2.0f, sizeY/2.0f), sceneColorBrush);

            float scaling = (float)(Math.Cos(t * 2.0 * Math.PI * 0.25) * 0.5f + 0.5f) * 0.5f + 0.5f;
            context2D.Transform = Matrix.Scaling(scaling) * Matrix.RotationZ(t * 1.5f) * Matrix.Translation(centerX, centerY, 0);

            context2D.DrawGeometry(pathGeometry1, sceneColorBrush, 2.0f);

            context2D.EndDraw();
        }

    }
}
