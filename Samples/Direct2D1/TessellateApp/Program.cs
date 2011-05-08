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
using SharpDX.Samples;

namespace TessellateApp
{
    /// <summary>
    /// Direct2D1 Tessellate Demo.
    /// </summary>
    public class Program : Direct2D1DemoApp, TessellationSink
    {
        EllipseGeometry Ellipse { get; set; }
        PathGeometry TesselatedGeometry{ get; set; }
        GeometrySink GeometrySink { get; set; }

        protected override void Initialize(DemoConfiguration demoConfiguration)
        {
            base.Initialize(demoConfiguration);

            // Create an ellipse
            Ellipse = new EllipseGeometry(Factory2D,
                                          new Ellipse(new PointF(demoConfiguration.Width/2, demoConfiguration.Height/2), demoConfiguration.Width/2 - 100,
                                                      demoConfiguration.Height/2 - 100));

            // Populate a PathGeometry from Ellipse tessellation 
            TesselatedGeometry = new PathGeometry(Factory2D);
            GeometrySink = TesselatedGeometry.Open();
            // Force RoundLineJoin otherwise the tesselated looks buggy at line joins
            GeometrySink.SetSegmentFlags(PathSegment.ForceRoundLineJoin); 

            // Tesselate the ellipse to our TessellationSink
            Ellipse.Tessellate(1, this);

            // Close the GeometrySink
            GeometrySink.Close();
        }


        protected override void Draw(DemoTime time)
        {
            base.Draw(time);

            // Draw the TextLayout
            RenderTarget2D.DrawGeometry(TesselatedGeometry, SceneColorBrush, 1, null);
        }

        void TessellationSink.AddTriangles(Triangle[] triangles)
        {
            // Add Tessellated triangles to the opened GeometrySink
            foreach (var triangle in triangles)
            {
                GeometrySink.BeginFigure(triangle.Point1, FigureBegin.Filled);
                GeometrySink.AddLine(triangle.Point2);
                GeometrySink.AddLine(triangle.Point3);
                GeometrySink.EndFigure(FigureEnd.Closed);                
            }
        }

        void TessellationSink.Close()
        {            
        }

        [STAThread]
        static void Main(string[] args)
        {
            Program program = new Program();
            program.Run(new DemoConfiguration("SharpDX Direct2D1 Tessellate Demo"));
        }
    }
}
