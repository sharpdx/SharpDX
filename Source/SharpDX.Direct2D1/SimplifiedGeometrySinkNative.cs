// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
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

using SharpDX.Mathematics.Interop;

namespace SharpDX.Direct2D1
{
    /// <summary>
    /// Describes a geometric path that does not contain quadratic Bezier curves or arcs.
    /// </summary>
    /// <remarks>
    /// A geometry sink consists of one or more figures. Each figure is made up of one or more line or Bezier curve segments. To create a figure, call the <strong>BeginFigure</strong> method and specify the figure's start point, then use <strong>AddLines</strong> and <strong>AddBeziers</strong> to add line and Bezier segments. When you are finished adding segments, call the <strong>EndFigure</strong> method. You can repeat this sequence to create additional figures. When you are finished creating figures, call the <strong>Close</strong> method. To create geometry paths that can contain arcs and quadratic Bezier curves, use an <strong><see cref="SharpDX.Direct2D1.GeometrySink"/></strong>.
    /// </remarks>
    internal partial class SimplifiedGeometrySinkNative
    {
        /// <summary>
        /// Creates a sequence of cubic Bezier curves and adds them to the geometry sink.
        /// </summary>
        /// <param name="beziers">An array of Bezier segments that describes the Bezier curves to create. A curve is drawn from the geometry sink's current point (the end point of the last segment drawn or the location specified by <strong>BeginFigure</strong>) to the end point of the first Bezier segment in the array. If the array contains additional Bezier segments, each subsequent Bezier segment uses the end point of the preceding Bezier segment as its start point.</param>
        public void AddBeziers(BezierSegment[] beziers)
        {
            AddBeziers_(beziers, beziers.Length);
        }

        /// <summary>
        /// Creates a sequence of lines using the specified points and adds them to the geometry sink.
        /// </summary>
        /// <param name="points">An array of one or more points that describe the lines to draw. A line is drawn from the geometry sink's current point (the end point of the last segment drawn or the location specified by <strong>BeginFigure</strong>) to the first point in the array. If the array contains additional points, a line is drawn from the first point to the second point in the array, from the second point to the third point, and so on.</param>
        public void AddLines(RawVector2[] points)
        {
            AddLines_(points, points.Length);
        }

        /// <summary>
        /// Starts a new figure at the specified point.
        /// </summary>
        /// <param name="startPoint">The point at which to begin the new figure.</param>
        /// <param name="figureBegin">Whether the new figure should be hollow or filled.</param>
        /// <remarks>
        /// If this method is called while a figure is currently in progress, the interface is invalidated and all future methods will fail.
        /// </remarks>
        public void BeginFigure(RawVector2 startPoint, FigureBegin figureBegin)
        {
            BeginFigure_(startPoint, figureBegin);
        }

        /// <summary>
        /// Closes the geometry sink, indicates whether it is in an error state, and resets the sink's error state.
        /// </summary>
        /// <returns>If this method succeeds, it returns <strong><see cref="SharpDX.Result.Ok"/></strong>. Otherwise, it returns an <strong><see cref="SharpDX.Result"/></strong> error code.</returns>
        /// <remarks>
        /// Do not close the geometry sink while a figure is still in progress; doing so puts the geometry sink in an error state. For the close operation to be successful, there must be one <strong>EndFigure</strong> call for each call to <strong>BeginFigure</strong>.After calling this method, the geometry sink might not be usable. Direct2D implementations of this interface do not allow the geometry sink to be modified after it is closed, but other implementations might not impose this restriction.
        /// </remarks>
        public void Close()
        {
            Close_();
        }

        /// <summary>
        ///  Ends the current figure; optionally, closes it.
        /// </summary>
        /// <param name="figureEnd">A value that indicates whether the current figure is closed. If the figure is closed, a line is drawn between the current point and the start point specified by <strong>BeginFigure</strong>.</param>
        /// <remarks>
        /// Calling this method without a matching call to <strong>BeginFigure</strong> places the geometry sink in an error state; subsequent calls are ignored, and the overall failure will be returned when the <strong>Close</strong> method is called.
        /// </remarks>
        public void EndFigure(FigureEnd figureEnd)
        {
            EndFigure_(figureEnd);
        }

        /// <summary>
        /// Specifies the method used to determine which points are inside the geometry described by this geometry sink  and which points are outside.
        /// </summary>
        /// <param name="fillMode">The method used to determine whether a given point is part of the geometry.</param>
        /// <remarks>
        /// The fill mode defaults to <see cref="SharpDX.Direct2D1.FillMode.Alternate"/>. To set the fill mode, call <strong>SetFillMode</strong> before the first call to <strong>BeginFigure</strong>. Not doing will put the geometry sink in an error state.
        /// </remarks>
        public void SetFillMode(FillMode fillMode)
        {
            SetFillMode_(fillMode);
        }

        /// <summary>
        /// Specifies stroke and join options to be applied to new segments added to the geometry sink.
        /// </summary>
        /// <param name="vertexFlags">Stroke and join options to be applied to new segments added to the geometry sink.</param>
        /// <remarks>
        /// After this method is called, the specified segment flags are applied to each segment subsequently added to the sink. The segment flags are applied to every additional segment until this method is called again and a different set of segment flags is specified.
        /// </remarks>
        public void SetSegmentFlags(PathSegment vertexFlags)
        {
            SetSegmentFlags_(vertexFlags);
        }
    }
}