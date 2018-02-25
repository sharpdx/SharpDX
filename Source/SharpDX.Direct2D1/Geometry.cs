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
    public partial class Geometry
    {
        private float _flatteningTolerance = DefaultFlatteningTolerance;

        /// <summary>
        /// Default flattening tolerance used for all methods that are not explicitly using it. Default is set to 0.25f.
        /// </summary>
        public const float DefaultFlatteningTolerance = 0.25f;

        /// <summary>
        /// Get or set the default flattening tolerance used for all methods that are not explicitly using it. Default is set to 0.25f.
        /// </summary>
        public float FlatteningTolerance
        {
            get { return _flatteningTolerance; }
            set { _flatteningTolerance = value; }
        }

        /// <summary>	
        /// Combines this geometry with the specified geometry and stores the result in an <see cref="SharpDX.Direct2D1.SimplifiedGeometrySink"/>.  	
        /// </summary>	
        /// <param name="inputGeometry">The geometry to combine with this instance.</param>
        /// <param name="combineMode">The type of combine operation to perform.</param>
        /// <param name="geometrySink">The result of the combine operation.</param>
        /// <returns>If the method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        /// <unmanaged>HRESULT CombineWithGeometry([In] ID2D1Geometry* inputGeometry,[None] D2D1_COMBINE_MODE combineMode,[In, Optional] const D2D1_MATRIX_3X2_F* inputGeometryTransform,[None] FLOAT flatteningTolerance,[In] ID2D1SimplifiedGeometrySink* geometrySink)</unmanaged>
        public void Combine(SharpDX.Direct2D1.Geometry inputGeometry, SharpDX.Direct2D1.CombineMode combineMode, GeometrySink geometrySink)
        {
            this.Combine(inputGeometry, combineMode, null, FlatteningTolerance, geometrySink);
        }

        /// <summary>	
        /// Combines this geometry with the specified geometry and stores the result in an <see cref="SharpDX.Direct2D1.SimplifiedGeometrySink"/>.  	
        /// </summary>	
        /// <param name="inputGeometry">The geometry to combine with this instance.</param>
        /// <param name="combineMode">The type of combine operation to perform.</param>
        /// <param name="flatteningTolerance">The maximum bounds on the distance between points in the polygonal approximation of the geometries. Smaller values produce more accurate results but cause slower execution. </param>
        /// <param name="geometrySink">The result of the combine operation.</param>
        /// <returns>If the method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        /// <unmanaged>HRESULT CombineWithGeometry([In] ID2D1Geometry* inputGeometry,[None] D2D1_COMBINE_MODE combineMode,[In, Optional] const D2D1_MATRIX_3X2_F* inputGeometryTransform,[None] FLOAT flatteningTolerance,[In] ID2D1SimplifiedGeometrySink* geometrySink)</unmanaged>
        public void Combine(SharpDX.Direct2D1.Geometry inputGeometry, SharpDX.Direct2D1.CombineMode combineMode, float flatteningTolerance, GeometrySink geometrySink)
        {
            this.Combine(inputGeometry, combineMode, null, flatteningTolerance, geometrySink);
        }

        /// <summary>	
        /// Describes the intersection between this geometry and the specified geometry. The comparison is performed by using the specified flattening tolerance.	
        /// </summary>	
        /// <remarks>	
        /// When interpreting the returned relation value, it is important to remember that the member <see cref="F:SharpDX.Direct2D1.GeometryRelation.IsContained" /> of the  D2D1_GEOMETRY_RELATION enumeration type means that this geometry is contained  inside inputGeometry, not that this geometry contains inputGeometry.  For  more information about how to interpret other possible return values, see <see cref="T:SharpDX.Direct2D1.GeometryRelation" />. 	
        /// </remarks>	
        /// <param name="inputGeometry">The geometry to test.  </param>
        /// <returns>When this method returns, contains a reference to a value that describes how this geometry is related to inputGeometry. You must allocate storage for this parameter.   </returns>
        /// <unmanaged>HRESULT ID2D1Geometry::CompareWithGeometry([In] ID2D1Geometry* inputGeometry,[In, Optional] const D2D1_MATRIX_3X2_F* inputGeometryTransform,[None] float flatteningTolerance,[Out] D2D1_GEOMETRY_RELATION* relation)</unmanaged>
        public GeometryRelation Compare(Geometry inputGeometry)
        {
            return Compare(inputGeometry, null, FlatteningTolerance);
        }

        /// <summary>	
        /// Describes the intersection between this geometry and the specified geometry. The comparison is performed by using the specified flattening tolerance.	
        /// </summary>	
        /// <remarks>	
        /// When interpreting the returned relation value, it is important to remember that the member <see cref="F:SharpDX.Direct2D1.GeometryRelation.IsContained" /> of the  D2D1_GEOMETRY_RELATION enumeration type means that this geometry is contained  inside inputGeometry, not that this geometry contains inputGeometry.  For  more information about how to interpret other possible return values, see <see cref="T:SharpDX.Direct2D1.GeometryRelation" />. 	
        /// </remarks>	
        /// <param name="inputGeometry">The geometry to test.  </param>
        /// <param name="flatteningTolerance">The maximum bounds on the distance between points in the polygonal approximation of the geometries. Smaller values produce more accurate results but cause slower execution.  </param>
        /// <returns>When this method returns, contains a reference to a value that describes how this geometry is related to inputGeometry. You must allocate storage for this parameter.   </returns>
        /// <unmanaged>HRESULT ID2D1Geometry::CompareWithGeometry([In] ID2D1Geometry* inputGeometry,[In, Optional] const D2D1_MATRIX_3X2_F* inputGeometryTransform,[None] float flatteningTolerance,[Out] D2D1_GEOMETRY_RELATION* relation)</unmanaged>
        public GeometryRelation Compare(Geometry inputGeometry, float flatteningTolerance)
        {
            return Compare(inputGeometry, null, flatteningTolerance);
        }

        /// <summary>	
        /// Computes the area of the geometry after it has been transformed by the specified matrix and flattened using the specified tolerance.	
        /// </summary>	
        /// <returns>When this this method returns, contains a reference to the area of the transformed, flattened version of this geometry. You must allocate storage for this parameter. </returns>
        /// <unmanaged>HRESULT ID2D1Geometry::ComputeArea([In, Optional] const D2D1_MATRIX_3X2_F* worldTransform,[None] float flatteningTolerance,[Out] float* area)</unmanaged>
        public float ComputeArea()
        {
            return ComputeArea(null, FlatteningTolerance);
        }

        /// <summary>	
        /// Computes the area of the geometry after it has been transformed by the specified matrix and flattened using the specified tolerance.	
        /// </summary>	
        /// <param name="flatteningTolerance">The maximum bounds on the distance between points in the polygonal approximation of the geometry. Smaller values produce more accurate results but cause slower execution.  </param>
        /// <returns>When this this method returns, contains a reference to the area of the transformed, flattened version of this geometry. You must allocate storage for this parameter. </returns>
        /// <unmanaged>HRESULT ID2D1Geometry::ComputeArea([In, Optional] const D2D1_MATRIX_3X2_F* worldTransform,[None] float flatteningTolerance,[Out] float* area)</unmanaged>
        public float ComputeArea(float flatteningTolerance)
        {
            return ComputeArea(null, flatteningTolerance);
        }

        /// <summary>	
        /// Calculates the length of the geometry as though each segment were unrolled into a line. 	
        /// </summary>	
        /// <returns>When this method returns, contains a reference to the length of the geometry. For closed geometries, the length includes an implicit closing segment. You must allocate storage for this parameter. </returns>
        /// <unmanaged>HRESULT ID2D1Geometry::ComputeLength([In, Optional] const D2D1_MATRIX_3X2_F* worldTransform,[None] float flatteningTolerance,[Out] float* length)</unmanaged>
        public float ComputeLength()
        {
            return ComputeLength(null, FlatteningTolerance);
        }

        /// <summary>	
        /// Calculates the length of the geometry as though each segment were unrolled into a line. 	
        /// </summary>	
        /// <param name="flatteningTolerance">The maximum bounds on the distance between points in the polygonal approximation of the geometry. Smaller values produce more accurate results but cause slower execution.  </param>
        /// <returns>When this method returns, contains a reference to the length of the geometry. For closed geometries, the length includes an implicit closing segment. You must allocate storage for this parameter. </returns>
        /// <unmanaged>HRESULT ID2D1Geometry::ComputeLength([In, Optional] const D2D1_MATRIX_3X2_F* worldTransform,[None] float flatteningTolerance,[Out] float* length)</unmanaged>
        public  float ComputeLength(float flatteningTolerance)
        {
            return ComputeLength(null, flatteningTolerance);
        }

        /// <summary>	
        /// Calculates the point and tangent vector at the specified distance along the geometry after it has been transformed by the specified matrix and flattened using the specified tolerance.	
        /// </summary>	
        /// <param name="length">The distance along the geometry of the point and tangent to find. If this distance is less then 0, this method calculates the first point in the geometry. If this distance is greater than the length of the geometry, this method calculates the last point in the geometry. </param>
        /// <param name="unitTangentVector">When this method returns, contains a reference to the tangent vector at the specified distance along the geometry. If the geometry is empty,  this vector contains NaN as its x and y values. You must allocate storage for this parameter. </param>
        /// <returns>The location at the specified distance along the geometry. If the geometry is empty,  this point contains NaN as its x and y values. </returns>
        /// <unmanaged>HRESULT ID2D1Geometry::ComputePointAtLength([None] float length,[In, Optional] const D2D1_MATRIX_3X2_F* worldTransform,[None] float flatteningTolerance,[Out, Optional] D2D1_POINT_2F* point,[Out, Optional] D2D1_POINT_2F* unitTangentVector)</unmanaged>
        public RawVector2 ComputePointAtLength(float length, out RawVector2 unitTangentVector)
        {
            return ComputePointAtLength(length, null, FlatteningTolerance, out unitTangentVector);
        }

        /// <summary>	
        /// Calculates the point and tangent vector at the specified distance along the geometry after it has been transformed by the specified matrix and flattened using the specified tolerance.	
        /// </summary>	
        /// <param name="length">The distance along the geometry of the point and tangent to find. If this distance is less then 0, this method calculates the first point in the geometry. If this distance is greater than the length of the geometry, this method calculates the last point in the geometry. </param>
        /// <param name="flatteningTolerance">The maximum bounds on the distance between points in the polygonal approximation of the geometry. Smaller values produce more accurate results but cause slower execution. </param>
        /// <param name="unitTangentVector">When this method returns, contains a reference to the tangent vector at the specified distance along the geometry. If the geometry is empty,  this vector contains NaN as its x and y values. You must allocate storage for this parameter. </param>
        /// <returns>The location at the specified distance along the geometry. If the geometry is empty,  this point contains NaN as its x and y values. </returns>
        /// <unmanaged>HRESULT ID2D1Geometry::ComputePointAtLength([None] float length,[In, Optional] const D2D1_MATRIX_3X2_F* worldTransform,[None] float flatteningTolerance,[Out, Optional] D2D1_POINT_2F* point,[Out, Optional] D2D1_POINT_2F* unitTangentVector)</unmanaged>
        public RawVector2 ComputePointAtLength(float length, float flatteningTolerance, out RawVector2 unitTangentVector)
        {
            return ComputePointAtLength(length, null, flatteningTolerance, out unitTangentVector);
        }

        /// <summary>	
        /// Indicates whether the area filled by the geometry would contain the specified point given the specified flattening tolerance. 	
        /// </summary>	
        /// <param name="point">The point to test. </param>
        /// <returns>When this method returns, contains a bool value that is true if the area filled by the geometry contains point; otherwise, false.You must allocate storage for this parameter. </returns>
        /// <unmanaged>HRESULT ID2D1Geometry::FillContainsPoint([None] D2D1_POINT_2F point,[In, Optional] const D2D1_MATRIX_3X2_F* worldTransform,[None] float flatteningTolerance,[Out] BOOL* contains)</unmanaged>
        public bool FillContainsPoint(RawPoint point)
        {
            return FillContainsPoint(new RawVector2() { X = point.X, Y = point.Y }, null, FlatteningTolerance);
        }

        /// <summary>	
        /// Indicates whether the area filled by the geometry would contain the specified point given the specified flattening tolerance. 	
        /// </summary>	
        /// <param name="point">The point to test. </param>
        /// <returns>When this method returns, contains a bool value that is true if the area filled by the geometry contains point; otherwise, false.You must allocate storage for this parameter. </returns>
        /// <unmanaged>HRESULT ID2D1Geometry::FillContainsPoint([None] D2D1_POINT_2F point,[In, Optional] const D2D1_MATRIX_3X2_F* worldTransform,[None] float flatteningTolerance,[Out] BOOL* contains)</unmanaged>
        public bool FillContainsPoint(RawVector2 point)
        {
            return FillContainsPoint(point, null, FlatteningTolerance);
        }

        /// <summary>	
        /// Indicates whether the area filled by the geometry would contain the specified point given the specified flattening tolerance. 	
        /// </summary>	
        /// <param name="point">The point to test. </param>
        /// <param name="flatteningTolerance">The numeric accuracy with which the precise geometric path and path intersection is calculated. Points missing the fill by less than the tolerance are still considered inside.  Smaller values produce more accurate results but cause slower execution.  </param>
        /// <returns>When this method returns, contains a bool value that is true if the area filled by the geometry contains point; otherwise, false.You must allocate storage for this parameter. </returns>
        /// <unmanaged>HRESULT ID2D1Geometry::FillContainsPoint([None] D2D1_POINT_2F point,[In, Optional] const D2D1_MATRIX_3X2_F* worldTransform,[None] float flatteningTolerance,[Out] BOOL* contains)</unmanaged>
        public bool FillContainsPoint(RawPoint point, float flatteningTolerance)
        {
            return FillContainsPoint(new RawVector2 { X = point.X, Y = point.Y}, null, flatteningTolerance);
        }

        /// <summary>	
        /// Indicates whether the area filled by the geometry would contain the specified point given the specified flattening tolerance. 	
        /// </summary>	
        /// <param name="point">The point to test. </param>
        /// <param name="flatteningTolerance">The numeric accuracy with which the precise geometric path and path intersection is calculated. Points missing the fill by less than the tolerance are still considered inside.  Smaller values produce more accurate results but cause slower execution.  </param>
        /// <returns>When this method returns, contains a bool value that is true if the area filled by the geometry contains point; otherwise, false.You must allocate storage for this parameter. </returns>
        /// <unmanaged>HRESULT ID2D1Geometry::FillContainsPoint([None] D2D1_POINT_2F point,[In, Optional] const D2D1_MATRIX_3X2_F* worldTransform,[None] float flatteningTolerance,[Out] BOOL* contains)</unmanaged>
        public bool FillContainsPoint(RawVector2 point, float flatteningTolerance)
        {
            return FillContainsPoint(point, null, flatteningTolerance);
        }

        /// <summary>	
        /// Indicates whether the area filled by the geometry would contain the specified point given the specified flattening tolerance. 	
        /// </summary>	
        /// <param name="point">The point to test. </param>
        /// <param name="worldTransform">The transform to apply to the geometry prior to testing for containment, or NULL. </param>
        /// <param name="flatteningTolerance">The numeric accuracy with which the precise geometric path and path intersection is calculated. Points missing the fill by less than the tolerance are still considered inside.  Smaller values produce more accurate results but cause slower execution.  </param>
        /// <returns>When this method returns, contains a bool value that is true if the area filled by the geometry contains point; otherwise, false.You must allocate storage for this parameter. </returns>
        /// <unmanaged>HRESULT ID2D1Geometry::FillContainsPoint([None] D2D1_POINT_2F point,[In, Optional] const D2D1_MATRIX_3X2_F* worldTransform,[None] float flatteningTolerance,[Out] BOOL* contains)</unmanaged>
        public bool FillContainsPoint(RawPoint point, RawMatrix3x2 worldTransform, float flatteningTolerance)
        {
            return FillContainsPoint(new RawVector2 {X = point.X, Y = point.Y}, worldTransform, flatteningTolerance);
        }


        /// <summary>	
        /// Retrieves the bounds of the geometry.	
        /// </summary>	
        /// <returns>When this method returns, contains the bounds of this geometry. If the bounds are empty, this will be a rect where bounds.left &gt; bounds.right. You must allocate storage for this parameter. </returns>
        /// <unmanaged>HRESULT ID2D1Geometry::GetBounds([In, Optional] const D2D1_MATRIX_3X2_F* worldTransform,[Out] D2D1_RECT_F* bounds)</unmanaged>
        public RawRectangleF GetBounds()
        {
            return GetBounds(null);
        }

        /// <summary>	
        /// Gets the bounds of the geometry after it has been widened by the specified stroke width and style and transformed by the specified matrix.	
        /// </summary>	
        /// <param name="strokeWidth">The amount by which to widen the geometry by stroking its outline. </param>
        /// <returns>When this method returns, contains the bounds of the widened geometry. You must allocate storage for this parameter. </returns>
        /// <unmanaged>HRESULT ID2D1Geometry::GetWidenedBounds([None] float strokeWidth,[In, Optional] ID2D1StrokeStyle* strokeStyle,[In, Optional] const D2D1_MATRIX_3X2_F* worldTransform,[None] float flatteningTolerance,[Out] D2D1_RECT_F* bounds)</unmanaged>
        public RawRectangleF GetWidenedBounds(float strokeWidth)
        {
            return GetWidenedBounds(strokeWidth, null, null, FlatteningTolerance);
        }

        /// <summary>	
        /// Gets the bounds of the geometry after it has been widened by the specified stroke width and style and transformed by the specified matrix.	
        /// </summary>	
        /// <param name="strokeWidth">The amount by which to widen the geometry by stroking its outline. </param>
        /// <param name="flatteningTolerance">The maximum bounds on the distance between points in the polygonal approximation of the geometry. Smaller values produce more accurate results but cause slower execution.  </param>
        /// <returns>When this method returns, contains the bounds of the widened geometry. You must allocate storage for this parameter. </returns>
        /// <unmanaged>HRESULT ID2D1Geometry::GetWidenedBounds([None] float strokeWidth,[In, Optional] ID2D1StrokeStyle* strokeStyle,[In, Optional] const D2D1_MATRIX_3X2_F* worldTransform,[None] float flatteningTolerance,[Out] D2D1_RECT_F* bounds)</unmanaged>
        public RawRectangleF GetWidenedBounds(float strokeWidth, float flatteningTolerance)
        {
            return GetWidenedBounds(strokeWidth, null, null, flatteningTolerance);
        }

        /// <summary>	
        /// Gets the bounds of the geometry after it has been widened by the specified stroke width and style and transformed by the specified matrix.	
        /// </summary>	
        /// <param name="strokeWidth">The amount by which to widen the geometry by stroking its outline. </param>
        /// <param name="strokeStyle">The style of the stroke that widens the geometry. </param>
        /// <param name="flatteningTolerance">The maximum bounds on the distance between points in the polygonal approximation of the geometry. Smaller values produce more accurate results but cause slower execution.  </param>
        /// <returns>When this method returns, contains the bounds of the widened geometry. You must allocate storage for this parameter. </returns>
        /// <unmanaged>HRESULT ID2D1Geometry::GetWidenedBounds([None] float strokeWidth,[In, Optional] ID2D1StrokeStyle* strokeStyle,[In, Optional] const D2D1_MATRIX_3X2_F* worldTransform,[None] float flatteningTolerance,[Out] D2D1_RECT_F* bounds)</unmanaged>
        public RawRectangleF GetWidenedBounds(float strokeWidth, StrokeStyle strokeStyle, float flatteningTolerance)
        {
            return GetWidenedBounds(strokeWidth, strokeStyle, null, flatteningTolerance);
        }


        /// <summary>	
        /// Computes the outline of the geometry and writes the result to an <see cref="SharpDX.Direct2D1.SimplifiedGeometrySink"/>.	
        /// </summary>	
        /// <remarks>	
        /// The {{Outline}} method allows the caller to produce a geometry with an equivalent fill to the input geometry, with the following additional properties: The output geometry contains no transverse intersections; that is, segments may touch, but they never cross.The outermost figures in the output geometry are all oriented counterclockwise. The output geometry is fill-mode invariant; that is, the fill of the geometry does not depend on the choice of the fill mode. For more information about the fill mode, see <see cref="SharpDX.Direct2D1.FillMode"/>.Additionally, the  {{Outline}} method can be useful in removing redundant portions of said geometries to simplify complex geometries. It can also be useful in combination with <see cref="SharpDX.Direct2D1.GeometryGroup"/> to create unions among several geometries simultaneously.	
        /// </remarks>	
        /// <param name="geometrySink">The <see cref="SharpDX.Direct2D1.SimplifiedGeometrySink"/> to which the geometry's transformed outline is appended. </param>
        /// <returns>If the method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        /// <unmanaged>HRESULT Outline([In, Optional] const D2D1_MATRIX_3X2_F* worldTransform,[None] FLOAT flatteningTolerance,[In] ID2D1SimplifiedGeometrySink* geometrySink)</unmanaged>
        public void Outline(GeometrySink geometrySink)
        {
            this.Outline(null, FlatteningTolerance, geometrySink);
        }

        /// <summary>	
        /// Computes the outline of the geometry and writes the result to an <see cref="SharpDX.Direct2D1.SimplifiedGeometrySink"/>.	
        /// </summary>	
        /// <remarks>	
        /// The {{Outline}} method allows the caller to produce a geometry with an equivalent fill to the input geometry, with the following additional properties: The output geometry contains no transverse intersections; that is, segments may touch, but they never cross.The outermost figures in the output geometry are all oriented counterclockwise. The output geometry is fill-mode invariant; that is, the fill of the geometry does not depend on the choice of the fill mode. For more information about the fill mode, see <see cref="SharpDX.Direct2D1.FillMode"/>.Additionally, the  {{Outline}} method can be useful in removing redundant portions of said geometries to simplify complex geometries. It can also be useful in combination with <see cref="SharpDX.Direct2D1.GeometryGroup"/> to create unions among several geometries simultaneously.	
        /// </remarks>	
        /// <param name="flatteningTolerance">The maximum bounds on the distance between points in the polygonal approximation of the geometry. Smaller values produce more accurate results but cause slower execution. </param>
        /// <param name="geometrySink">The <see cref="SharpDX.Direct2D1.SimplifiedGeometrySink"/> to which the geometry's transformed outline is appended. </param>
        /// <returns>If the method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        /// <unmanaged>HRESULT Outline([In, Optional] const D2D1_MATRIX_3X2_F* worldTransform,[None] FLOAT flatteningTolerance,[In] ID2D1SimplifiedGeometrySink* geometrySink)</unmanaged>
        public void Outline(float flatteningTolerance, GeometrySink geometrySink)
        {
            this.Outline(null, flatteningTolerance, geometrySink);
        }

        /// <summary>	
        /// Creates a simplified version of the geometry that contains only lines and (optionally) cubic Bezier curves and writes the result to an <see cref="SharpDX.Direct2D1.SimplifiedGeometrySink"/>.	
        /// </summary>	
        /// <param name="simplificationOption">A value that specifies whether the simplified geometry should contain curves.</param>
        /// <param name="geometrySink"> The <see cref="SharpDX.Direct2D1.SimplifiedGeometrySink"/> to which the simplified geometry is appended. </param>
        /// <returns>If the method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        /// <unmanaged>HRESULT Simplify([None] D2D1_GEOMETRY_SIMPLIFICATION_OPTION simplificationOption,[In, Optional] const D2D1_MATRIX_3X2_F* worldTransform,[None] FLOAT flatteningTolerance,[In] ID2D1SimplifiedGeometrySink* geometrySink)</unmanaged>
        public void Simplify(SharpDX.Direct2D1.GeometrySimplificationOption simplificationOption, SimplifiedGeometrySink geometrySink)
        {
            Simplify(simplificationOption, null, FlatteningTolerance, geometrySink);
        }

        /// <summary>	
        /// Creates a simplified version of the geometry that contains only lines and (optionally) cubic Bezier curves and writes the result to an <see cref="SharpDX.Direct2D1.SimplifiedGeometrySink"/>.	
        /// </summary>	
        /// <param name="simplificationOption">A value that specifies whether the simplified geometry should contain curves.</param>
        /// <param name="flatteningTolerance">The maximum bounds on the distance between points in the polygonal approximation of the geometry. Smaller values produce more accurate results but cause slower execution. </param>
        /// <param name="geometrySink"> The <see cref="SharpDX.Direct2D1.SimplifiedGeometrySink"/> to which the simplified geometry is appended. </param>
        /// <returns>If the method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        /// <unmanaged>HRESULT Simplify([None] D2D1_GEOMETRY_SIMPLIFICATION_OPTION simplificationOption,[In, Optional] const D2D1_MATRIX_3X2_F* worldTransform,[None] FLOAT flatteningTolerance,[In] ID2D1SimplifiedGeometrySink* geometrySink)</unmanaged>
        public void Simplify(SharpDX.Direct2D1.GeometrySimplificationOption simplificationOption, float flatteningTolerance, SimplifiedGeometrySink geometrySink)
        {
            Simplify(simplificationOption, null, flatteningTolerance, geometrySink);
        }

        /// <summary>	
        /// Determines whether the geometry's stroke contains the specified point given the specified stroke thickness, style, and transform. 	
        /// </summary>	
        /// <param name="point">The point to test for containment. </param>
        /// <param name="strokeWidth">The thickness of the stroke to apply. </param>
        /// <returns>When this method returns, contains a boolean value set to true if the geometry's stroke contains the specified point; otherwise, false. You must allocate storage for this parameter. </returns>
        /// <unmanaged>HRESULT ID2D1Geometry::StrokeContainsPoint([None] D2D1_POINT_2F point,[None] float strokeWidth,[In, Optional] ID2D1StrokeStyle* strokeStyle,[In, Optional] const D2D1_MATRIX_3X2_F* worldTransform,[None] float flatteningTolerance,[Out] BOOL* contains)</unmanaged>
        public bool StrokeContainsPoint(RawPoint point, float strokeWidth)
        {
            return StrokeContainsPoint(point, strokeWidth, null);
        }

        /// <summary>	
        /// Determines whether the geometry's stroke contains the specified point given the specified stroke thickness, style, and transform. 	
        /// </summary>	
        /// <param name="point">The point to test for containment. </param>
        /// <param name="strokeWidth">The thickness of the stroke to apply. </param>
        /// <returns>When this method returns, contains a boolean value set to true if the geometry's stroke contains the specified point; otherwise, false. You must allocate storage for this parameter. </returns>
        /// <unmanaged>HRESULT ID2D1Geometry::StrokeContainsPoint([None] D2D1_POINT_2F point,[None] float strokeWidth,[In, Optional] ID2D1StrokeStyle* strokeStyle,[In, Optional] const D2D1_MATRIX_3X2_F* worldTransform,[None] float flatteningTolerance,[Out] BOOL* contains)</unmanaged>
        public bool StrokeContainsPoint(RawVector2 point, float strokeWidth)
        {
            return StrokeContainsPoint(point, strokeWidth, null);
        }

        /// <summary>	
        /// Determines whether the geometry's stroke contains the specified point given the specified stroke thickness, style, and transform. 	
        /// </summary>	
        /// <param name="point">The point to test for containment. </param>
        /// <param name="strokeWidth">The thickness of the stroke to apply. </param>
        /// <param name="strokeStyle">The style of stroke to apply. </param>
        /// <returns>When this method returns, contains a boolean value set to true if the geometry's stroke contains the specified point; otherwise, false. You must allocate storage for this parameter. </returns>
        /// <unmanaged>HRESULT ID2D1Geometry::StrokeContainsPoint([None] D2D1_POINT_2F point,[None] float strokeWidth,[In, Optional] ID2D1StrokeStyle* strokeStyle,[In, Optional] const D2D1_MATRIX_3X2_F* worldTransform,[None] float flatteningTolerance,[Out] BOOL* contains)</unmanaged>
        public bool StrokeContainsPoint(RawPoint point, float strokeWidth, StrokeStyle strokeStyle)
        {
            return StrokeContainsPoint(new RawVector2 { X = point.X, Y = point.Y }, strokeWidth, strokeStyle);            
        }

        /// <summary>	
        /// Determines whether the geometry's stroke contains the specified point given the specified stroke thickness, style, and transform. 	
        /// </summary>	
        /// <param name="point">The point to test for containment. </param>
        /// <param name="strokeWidth">The thickness of the stroke to apply. </param>
        /// <param name="strokeStyle">The style of stroke to apply. </param>
        /// <returns>When this method returns, contains a boolean value set to true if the geometry's stroke contains the specified point; otherwise, false. You must allocate storage for this parameter. </returns>
        /// <unmanaged>HRESULT ID2D1Geometry::StrokeContainsPoint([None] D2D1_POINT_2F point,[None] float strokeWidth,[In, Optional] ID2D1StrokeStyle* strokeStyle,[In, Optional] const D2D1_MATRIX_3X2_F* worldTransform,[None] float flatteningTolerance,[Out] BOOL* contains)</unmanaged>
        public bool StrokeContainsPoint(RawVector2 point, float strokeWidth, StrokeStyle strokeStyle)
        {
            return StrokeContainsPoint(point, strokeWidth, strokeStyle, null, FlatteningTolerance);
        }

        /// <summary>	
        /// Determines whether the geometry's stroke contains the specified point given the specified stroke thickness, style, and transform. 	
        /// </summary>	
        /// <param name="point">The point to test for containment. </param>
        /// <param name="strokeWidth">The thickness of the stroke to apply. </param>
        /// <param name="strokeStyle">The style of stroke to apply. </param>
        /// <param name="transform">The transform to apply to the stroked geometry.  </param>
        /// <returns>When this method returns, contains a boolean value set to true if the geometry's stroke contains the specified point; otherwise, false. You must allocate storage for this parameter. </returns>
        /// <unmanaged>HRESULT ID2D1Geometry::StrokeContainsPoint([None] D2D1_POINT_2F point,[None] float strokeWidth,[In, Optional] ID2D1StrokeStyle* strokeStyle,[In, Optional] const D2D1_MATRIX_3X2_F* worldTransform,[None] float flatteningTolerance,[Out] BOOL* contains)</unmanaged>
        public bool StrokeContainsPoint(RawPoint point, float strokeWidth, StrokeStyle strokeStyle, RawMatrix3x2 transform)
        {
            return StrokeContainsPoint(point, strokeWidth, strokeStyle, transform, FlatteningTolerance);
        }

        /// <summary>	
        /// Determines whether the geometry's stroke contains the specified point given the specified stroke thickness, style, and transform. 	
        /// </summary>	
        /// <param name="point">The point to test for containment. </param>
        /// <param name="strokeWidth">The thickness of the stroke to apply. </param>
        /// <param name="strokeStyle">The style of stroke to apply. </param>
        /// <param name="transform">The transform to apply to the stroked geometry.  </param>
        /// <returns>When this method returns, contains a boolean value set to true if the geometry's stroke contains the specified point; otherwise, false. You must allocate storage for this parameter. </returns>
        /// <unmanaged>HRESULT ID2D1Geometry::StrokeContainsPoint([None] D2D1_POINT_2F point,[None] float strokeWidth,[In, Optional] ID2D1StrokeStyle* strokeStyle,[In, Optional] const D2D1_MATRIX_3X2_F* worldTransform,[None] float flatteningTolerance,[Out] BOOL* contains)</unmanaged>
        public bool StrokeContainsPoint(RawVector2 point, float strokeWidth, StrokeStyle strokeStyle, RawMatrix3x2 transform)
        {
            return StrokeContainsPoint(point, strokeWidth, strokeStyle, transform, FlatteningTolerance);            
        }

        /// <summary>	
        /// Determines whether the geometry's stroke contains the specified point given the specified stroke thickness, style, and transform. 	
        /// </summary>	
        /// <param name="point">The point to test for containment. </param>
        /// <param name="strokeWidth">The thickness of the stroke to apply. </param>
        /// <param name="strokeStyle">The style of stroke to apply. </param>
        /// <param name="transform">The transform to apply to the stroked geometry.  </param>
        /// <param name="flatteningTolerance">The numeric accuracy with which the precise geometric path and path intersection is calculated. Points missing the stroke by less than the tolerance are still considered inside.  Smaller values produce more accurate results but cause slower execution. </param>
        /// <returns>When this method returns, contains a boolean value set to true if the geometry's stroke contains the specified point; otherwise, false. You must allocate storage for this parameter. </returns>
        /// <unmanaged>HRESULT ID2D1Geometry::StrokeContainsPoint([None] D2D1_POINT_2F point,[None] float strokeWidth,[In, Optional] ID2D1StrokeStyle* strokeStyle,[In, Optional] const D2D1_MATRIX_3X2_F* worldTransform,[None] float flatteningTolerance,[Out] BOOL* contains)</unmanaged>
        public bool StrokeContainsPoint(RawPoint point, float strokeWidth, StrokeStyle strokeStyle, RawMatrix3x2 transform, float flatteningTolerance)
        {
            return StrokeContainsPoint(new RawVector2 { X = point.X, Y = point.Y}, strokeWidth, strokeStyle, transform, flatteningTolerance);
        }

        /// <summary>	
        /// Creates a set of clockwise-wound triangles that cover the geometry after it has been transformed using the specified matrix and flattened using the specified tolerance	
        /// </summary>	
        /// <param name="tessellationSink">The <see cref="T:SharpDX.Direct2D1.TessellationSink" /> to which the tessellated is appended.</param>
        /// <returns>If the method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        /// <unmanaged>HRESULT Tessellate([In, Optional] const D2D1_MATRIX_3X2_F* worldTransform,[None] FLOAT flatteningTolerance,[In] ID2D1TessellationSink* tessellationSink)</unmanaged>
        public void Tessellate(TessellationSink tessellationSink)
        {
            this.Tessellate(null, FlatteningTolerance, tessellationSink);
        }

        /// <summary>	
        /// Creates a set of clockwise-wound triangles that cover the geometry after it has been transformed using the specified matrix and flattened using the specified tolerance	
        /// </summary>	
        /// <param name="flatteningTolerance">The maximum bounds on the distance between points in the polygonal approximation of the geometry. Smaller values produce more accurate results but cause slower execution. </param>
        /// <param name="tessellationSink">The <see cref="SharpDX.Direct2D1.TessellationSink"/> to which the tessellated is appended.</param>
        /// <returns>If the method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        /// <unmanaged>HRESULT Tessellate([In, Optional] const D2D1_MATRIX_3X2_F* worldTransform,[None] FLOAT flatteningTolerance,[In] ID2D1TessellationSink* tessellationSink)</unmanaged>
        public void Tessellate(float flatteningTolerance, TessellationSink tessellationSink)
        {
            Tessellate(null, flatteningTolerance, tessellationSink);
        }

        /// <summary>	
        /// Widens the geometry by the specified stroke and writes the result to an <see cref="SharpDX.Direct2D1.SimplifiedGeometrySink"/> after it has been transformed by the specified matrix and flattened using the specified tolerance.	
        /// </summary>	
        /// <param name="strokeWidth">The amount by which to widen the geometry.</param>
        /// <param name="geometrySink">The <see cref="SharpDX.Direct2D1.SimplifiedGeometrySink"/> to which the widened geometry is appended.</param>
        /// <returns>If the method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        /// <unmanaged>HRESULT Widen([None] FLOAT strokeWidth,[In, Optional] ID2D1StrokeStyle* strokeStyle,[In, Optional] const D2D1_MATRIX_3X2_F* worldTransform,[None] FLOAT flatteningTolerance,[In] ID2D1SimplifiedGeometrySink* geometrySink)</unmanaged>
        public void Widen(float strokeWidth, GeometrySink geometrySink)
        {
            this.Widen(strokeWidth, null, null, FlatteningTolerance, geometrySink);
        }

        /// <summary>	
        /// Widens the geometry by the specified stroke and writes the result to an <see cref="SharpDX.Direct2D1.SimplifiedGeometrySink"/> after it has been transformed by the specified matrix and flattened using the specified tolerance.	
        /// </summary>	
        /// <param name="strokeWidth">The amount by which to widen the geometry.</param>
        /// <param name="flatteningTolerance">The maximum bounds on the distance between points in the polygonal approximation of the geometry. Smaller values produce more accurate results but cause slower execution.</param>
        /// <param name="geometrySink">The <see cref="SharpDX.Direct2D1.SimplifiedGeometrySink"/> to which the widened geometry is appended.</param>
        /// <returns>If the method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        /// <unmanaged>HRESULT Widen([None] FLOAT strokeWidth,[In, Optional] ID2D1StrokeStyle* strokeStyle,[In, Optional] const D2D1_MATRIX_3X2_F* worldTransform,[None] FLOAT flatteningTolerance,[In] ID2D1SimplifiedGeometrySink* geometrySink)</unmanaged>
        public void Widen(float strokeWidth, float flatteningTolerance, GeometrySink geometrySink)
        {
            this.Widen(strokeWidth, null, null, flatteningTolerance, geometrySink);
        }

        /// <summary>	
        /// Widens the geometry by the specified stroke and writes the result to an <see cref="SharpDX.Direct2D1.SimplifiedGeometrySink"/> after it has been transformed by the specified matrix and flattened using the specified tolerance.	
        /// </summary>	
        /// <param name="strokeWidth">The amount by which to widen the geometry.</param>
        /// <param name="strokeStyle">The style of stroke to apply to the geometry, or NULL.</param>
        /// <param name="flatteningTolerance">The maximum bounds on the distance between points in the polygonal approximation of the geometry. Smaller values produce more accurate results but cause slower execution.</param>
        /// <param name="geometrySink">The <see cref="SharpDX.Direct2D1.SimplifiedGeometrySink"/> to which the widened geometry is appended.</param>
        /// <returns>If the method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        /// <unmanaged>HRESULT Widen([None] FLOAT strokeWidth,[In, Optional] ID2D1StrokeStyle* strokeStyle,[In, Optional] const D2D1_MATRIX_3X2_F* worldTransform,[None] FLOAT flatteningTolerance,[In] ID2D1SimplifiedGeometrySink* geometrySink)</unmanaged>
        public void Widen(float strokeWidth, SharpDX.Direct2D1.StrokeStyle strokeStyle, float flatteningTolerance, GeometrySink geometrySink)
        {
            this.Widen(strokeWidth, strokeStyle, null, flatteningTolerance, geometrySink);
        }
    }
}
