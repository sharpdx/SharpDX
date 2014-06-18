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
    [ShadowAttribute(typeof(GeometrySinkShadow))]
    public partial interface GeometrySink
    {
        /// <summary>	
        /// Creates a line segment between the current point and the specified end point and adds it to the geometry sink. 	
        /// </summary>	
        /// <param name="point">The end point of the line to draw.</param>
        /// <unmanaged>void AddLine([None] D2D1_POINT_2F point)</unmanaged>
        void AddLine(RawVector2 point);


        /// <summary>	
        ///  Creates  a cubic Bezier curve between the current point and the specified endpoint.	
        /// </summary>	
        /// <param name="bezier">A structure that describes the control points and endpoint of the Bezier curve to add. </param>
        /// <unmanaged>void AddBezier([In] const D2D1_BEZIER_SEGMENT* bezier)</unmanaged>
        void AddBezier(SharpDX.Direct2D1.BezierSegment bezier);


        /// <summary>	
        /// Creates  a quadratic Bezier curve between the current point and the specified endpoint.	
        /// </summary>	
        /// <param name="bezier">A structure that describes the control point and the endpoint of the quadratic Bezier curve to add.</param>
        /// <unmanaged>void AddQuadraticBezier([In] const D2D1_QUADRATIC_BEZIER_SEGMENT* bezier)</unmanaged>
        void AddQuadraticBezier(SharpDX.Direct2D1.QuadraticBezierSegment bezier);


        /// <summary>	
        /// Adds a sequence of quadratic Bezier segments as an array in a single call.	
        /// </summary>	
        /// <param name="beziers">An array of a sequence of quadratic Bezier segments.</param>
        /// <unmanaged>void AddQuadraticBeziers([In, Buffer] const D2D1_QUADRATIC_BEZIER_SEGMENT* beziers,[None] UINT beziersCount)</unmanaged>
        void AddQuadraticBeziers(SharpDX.Direct2D1.QuadraticBezierSegment[] beziers);

        /// <summary>	
        /// Adds a single arc to the path geometry.	
        /// </summary>	
        /// <param name="arc">The arc segment to add to the figure.</param>
        /// <unmanaged>void AddArc([In] const D2D1_ARC_SEGMENT* arc)</unmanaged>
        void AddArc(SharpDX.Direct2D1.ArcSegment arc);
    }
}
