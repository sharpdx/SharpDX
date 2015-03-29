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
using System;

namespace SharpDX.Direct2D1
{
    /// <summary>	
    /// <p>Encapsulates a device- and transform-dependent representation of a filled or stroked geometry.  Callers should consider creating a geometry realization when they wish to accelerate repeated rendering of a given geometry. This interface exposes no methods.</p>	
    /// </summary>	
    /// <include file='.\Documentation\CodeComments.xml' path="/comments/comment[@id='ID2D1GeometryRealization']/*"/>	
    /// <msdn-id>dn280515</msdn-id>	
    /// <unmanaged>ID2D1GeometryRealization</unmanaged>	
    /// <unmanaged-short>ID2D1GeometryRealization</unmanaged-short>	
    public partial class GeometryRealization
    {
        /// <summary>	
        /// <p>Creates a device-dependent representation of the fill of the geometry that can be subsequently rendered.</p>	
        /// </summary>
        /// <param name="context">The device context where the created instance should be attached to.</param>
        /// <param name="geometry"><dd>  <p>The geometry to realize.</p> </dd></param>	
        /// <param name="flatteningTolerance"><dd>  <p>The flattening tolerance to use when converting Beziers to line segments. This parameter shares the same units as the coordinates of the geometry.</p> </dd></param>	
        /// <returns><p>The method returns an <strong><see cref="SharpDX.Result"/></strong>. Possible values include, but are not limited to, those in the following table.</p><table> <tr><th><see cref="SharpDX.Result"/></th><th>Description</th></tr> <tr><td><see cref="SharpDX.Result.Ok"/></td><td>No error occurred.</td></tr> <tr><td>E_OUTOFMEMORY</td><td>Direct2D could not allocate sufficient memory to complete the call.</td></tr> <tr><td>E_INVALIDARG</td><td>An invalid value was passed to the method.</td></tr> </table><p>?</p></returns>	
        /// <remarks>	
        /// <p>This method is used in conjunction with <strong><see cref="SharpDX.Direct2D1.DeviceContext1.DrawGeometryRealization"/></strong>. The <strong>D2D1::ComputeFlatteningTolerance</strong> helper API may be used to determine the proper flattening tolerance.</p><p>If the provided stroke style specifies a stroke transform type other than <strong><see cref="SharpDX.Direct2D1.StrokeTransformType.Normal"/></strong>, then the stroke will be realized assuming the identity transform and a DPI of 96.</p>	
        /// </remarks>	
        /// <include file='.\Documentation\CodeComments.xml' path="/comments/comment[@id='ID2D1DeviceContext1::CreateFilledGeometryRealization']/*"/>	
        /// <msdn-id>dn280462</msdn-id>	
        /// <unmanaged>HRESULT ID2D1DeviceContext1::CreateFilledGeometryRealization([In] ID2D1Geometry* geometry,[In] float flatteningTolerance,[Out, Fast] ID2D1GeometryRealization** geometryRealization)</unmanaged>	
        /// <unmanaged-short>ID2D1DeviceContext1::CreateFilledGeometryRealization</unmanaged-short>	
        public GeometryRealization(DeviceContext1 context, Geometry geometry, float flatteningTolerance)
            : this(IntPtr.Zero)
        {
            context.CreateFilledGeometryRealization(geometry, flatteningTolerance, this);
        }

        /// <summary>	
        /// <p>Creates a device-dependent representation of the stroke of a geometry that can be subsequently rendered.</p>	
        /// </summary>	
        /// <param name="context">The device context where the created instance should be attached to.</param>
        /// <param name="geometry"><dd>  <p>The geometry to realize.</p> </dd></param>	
        /// <param name="flatteningTolerance"><dd>  <p>The flattening tolerance to use when converting Beziers to line segments. This parameter shares the same units as the coordinates of the geometry.</p> </dd></param>	
        /// <param name="strokeWidth"><dd>  <p>The width of the stroke. This parameter shares the same units as the coordinates of the geometry.</p> </dd></param>	
        /// <param name="strokeStyle"><dd>  <p>The stroke style (optional).</p> </dd></param>	
        /// <returns><p>The method returns an <strong><see cref="SharpDX.Result"/></strong>. Possible values include, but are not limited to, those in the following table.</p><table> <tr><th><see cref="SharpDX.Result"/></th><th>Description</th></tr> <tr><td><see cref="SharpDX.Result.Ok"/></td><td>No error occurred.</td></tr> <tr><td>E_OUTOFMEMORY</td><td>Direct2D could not allocate sufficient memory to complete the call.</td></tr> <tr><td>E_INVALIDARG</td><td>An invalid value was passed to the method.</td></tr> </table><p>?</p></returns>	
        /// <remarks>	
        /// <p>This method is used in conjunction with <strong><see cref="SharpDX.Direct2D1.DeviceContext1.DrawGeometryRealization"/></strong>. The <strong>D2D1::ComputeFlatteningTolerance</strong> helper API may be used to determine the proper flattening tolerance.</p><p>If the provided stroke style specifies a stroke transform type other than <strong><see cref="SharpDX.Direct2D1.StrokeTransformType.Normal"/></strong>, then the stroke will be realized assuming the identity transform and a DPI of 96.</p>	
        /// </remarks>	
        /// <include file='.\Documentation\CodeComments.xml' path="/comments/comment[@id='ID2D1DeviceContext1::CreateStrokedGeometryRealization']/*"/>	
        /// <msdn-id>dn280463</msdn-id>	
        /// <unmanaged>HRESULT ID2D1DeviceContext1::CreateStrokedGeometryRealization([In] ID2D1Geometry* geometry,[In] float flatteningTolerance,[In] float strokeWidth,[In, Optional] ID2D1StrokeStyle* strokeStyle,[Out, Fast] ID2D1GeometryRealization** geometryRealization)</unmanaged>	
        /// <unmanaged-short>ID2D1DeviceContext1::CreateStrokedGeometryRealization</unmanaged-short>	
        public GeometryRealization(DeviceContext1 context, Geometry geometry, float flatteningTolerance, float strokeWidth, StrokeStyle strokeStyle)
            : this(IntPtr.Zero)
        {
            context.CreateStrokedGeometryRealization(geometry, flatteningTolerance, strokeWidth, strokeStyle, this);
        }
    }
}