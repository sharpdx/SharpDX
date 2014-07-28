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
    [ShadowAttribute(typeof(SimplifiedGeometrySinkShadow))]
    public partial interface SimplifiedGeometrySink
    {
        /// <summary>	
        /// Specifies the method used to determine which points are inside the geometry described by this geometry sink  and which points are outside.  	
        /// </summary>	
        /// <remarks>	
        /// The fill mode defaults to D2D1_FILL_MODE_ALTERNATE. To set the fill mode, call SetFillMode before the first call to {{BeginFigure}}. Not doing will put the geometry sink in an error state. 	
        /// </remarks>	
        /// <param name="fillMode">The method used to determine whether a given point is part of the geometry.</param>
        /// <unmanaged>void SetFillMode([None] D2D1_FILL_MODE fillMode)</unmanaged>
        void SetFillMode(SharpDX.Direct2D1.FillMode fillMode);


        /// <summary>	
        /// Specifies stroke and join options to be applied to new segments added to the geometry sink. 	
        /// </summary>	
        /// <remarks>	
        /// After this method is called, the specified segment flags are applied to each segment subsequently added to the sink. The segment flags are applied to every additional segment until this method is called again and a different set of segment flags is specified.     	
        /// </remarks>	
        /// <param name="vertexFlags">Stroke and join options to be applied to new segments added to the geometry sink.</param>
        /// <unmanaged>void SetSegmentFlags([None] D2D1_PATH_SEGMENT vertexFlags)</unmanaged>
        void SetSegmentFlags(SharpDX.Direct2D1.PathSegment vertexFlags);


        /// <summary>	
        /// Starts a new figure at the specified point. 	
        /// </summary>	
        /// <remarks>	
        /// If this method is called while a figure is currently in progress, the interface is invalidated and all future methods will fail.	
        /// </remarks>	
        /// <param name="startPoint">The point at which to begin the new figure.</param>
        /// <param name="figureBegin">Whether the new figure should be hollow or filled.</param>
        /// <unmanaged>void BeginFigure([None] D2D1_POINT_2F startPoint,[None] D2D1_FIGURE_BEGIN figureBegin)</unmanaged>
        void BeginFigure(RawVector2 startPoint, SharpDX.Direct2D1.FigureBegin figureBegin);


        /// <summary>	
        ///  Creates a sequence of lines using the specified points and adds them to the geometry sink.	
        /// </summary>	
        /// <param name="ointsRef">A pointer to an array of one or more points that describe the lines to draw. A line is drawn from the geometry sink's current point (the end point of the last segment drawn or the location specified by {{BeginFigure}}) to the first point in the array. if the array contains additional points, a line is drawn from the first point to the second point in the array, from the second point to the third point, and so on.   </param>
        /// <unmanaged>void AddLines([In, Buffer] const D2D1_POINT_2F* points,[None] UINT pointsCount)</unmanaged>
        void AddLines(RawVector2[] ointsRef);


        /// <summary>	
        /// Creates a sequence of cubic Bezier curves and adds them to the geometry sink. 	
        /// </summary>	
        /// <param name="beziers">A pointer to an array of Bezier segments that describes the Bezier curves to create. A curve is drawn from the geometry sink's current point (the end point of the last segment drawn or the location specified by {{BeginFigure}}) to the end point of the first Bezier segment in the array. if the array contains additional Bezier segments, each subsequent Bezier segment uses the end point of the preceding Bezier segment as its start point.</param>
        /// <unmanaged>void AddBeziers([In, Buffer] const D2D1_BEZIER_SEGMENT* beziers,[None] UINT beziersCount)</unmanaged>
        void AddBeziers(SharpDX.Direct2D1.BezierSegment[] beziers);


        /// <summary>	
        ///  Ends the current figure; optionally, closes it.	
        /// </summary>	
        /// <remarks>	
        /// Calling this method without a matching call to {{BeginFigure}} places the geometry sink in an error state; subsequent calls are ignored, and the overall failure will be returned when the {{Close}} method is called.	
        /// </remarks>	
        /// <param name="figureEnd">A value that indicates whether the current figure is closed. If the figure is closed, a line is drawn between the current point and the start point specified by {{BeginFigure}}.</param>
        /// <unmanaged>void EndFigure([None] D2D1_FIGURE_END figureEnd)</unmanaged>
        void EndFigure(SharpDX.Direct2D1.FigureEnd figureEnd);


        /// <summary>	
        /// Closes the geometry sink, indicates whether it is in an error state, and resets the sink's error state. 	
        /// </summary>	
        /// <remarks>	
        /// Do not close the geometry sink while a figure is still in progress; doing so puts the geometry sink in an error state. For the close operation to be successful, there must be one {{EndFigure}} call for each call to {{BeginFigure}}.After calling this method, the geometry sink might not be usable. Direct2D implementations of this interface do not allow the geometry sink to be modified after it is closed, but other implementations might not impose this restriction.	
        /// </remarks>	
        /// <unmanaged>HRESULT Close()</unmanaged>
        void Close();
    }
}
