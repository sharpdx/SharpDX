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
    public partial class GeometryGroup
    {
        /// <summary>	
        /// Creates an <see cref="SharpDX.Direct2D1.GeometryGroup"/>, which is an object that holds other geometries.	
        /// </summary>	
        /// <remarks>	
        /// Geometry groups are a convenient way to group several geometries simultaneously so all figures of several distinct geometries are concatenated into one. To create a  <see cref="SharpDX.Direct2D1.GeometryGroup"/> object, call  the CreateGeometryGroup method on the <see cref="SharpDX.Direct2D1.Factory"/> object, passing in the fillMode with possible values of   <see cref="SharpDX.Direct2D1.FillMode.Alternate"/> (alternate) and D2D1_FILL_MODE_WINDING, an array of geometry objects to add to the geometry group, and the number of elements in this array. 	
        /// </remarks>	
        /// <param name="factory">an instance of <see cref = "SharpDX.Direct2D1.Factory" /></param>
        /// <param name="fillMode">A value that specifies the rule that a composite shape uses to determine whether a given point is part of the geometry. </param>
        /// <param name="geometries">An array containing the geometry objects to add to the geometry group. The number of elements in this array is indicated by the geometriesCount parameter.</param>
        public GeometryGroup(Factory factory, FillMode fillMode, Geometry[] geometries)
            : base(IntPtr.Zero)
        {
            factory.CreateGeometryGroup(fillMode, geometries, geometries.Length, this);
        }

        /// <summary>	
        /// Retrieves the geometries in the geometry group. 	
        /// </summary>	
        /// <remarks>	
        /// The returned geometries are referenced and  counted, and the caller must release them. 	
        /// </remarks>	
        /// <returns>an array of geometries to be filled by this method. The length of the array is specified by the geometryCount parameter.</returns>
        /// <unmanaged>void ID2D1GeometryGroup::GetSourceGeometries([Out, Buffer] ID2D1Geometry** geometries,[None] int geometriesCount)</unmanaged>
        public SharpDX.Direct2D1.Geometry[] GetSourceGeometry()
        {
            return GetSourceGeometry(SourceGeometryCount);
        }

        /// <summary>	
        /// Retrieves the geometries in the geometry group. 	
        /// </summary>	
        /// <remarks>	
        /// The returned geometries are referenced and  counted, and the caller must release them. 	
        /// </remarks>	
        /// <param name="geometriesCount">A value indicating the number of geometries to return in the geometries array. If this value is less than the number of geometries in the geometry group, the remaining geometries are omitted. If this value is larger than the number of geometries in the geometry group, the extra geometries are set to NULL. To obtain the number of geometries currently in the geometry group, use the {{GetSourceGeometryCount}} method. </param>
        /// <returns>an array of geometries to be filled by this method. The length of the array is specified by the geometryCount parameter.</returns>
        /// <unmanaged>void ID2D1GeometryGroup::GetSourceGeometries([Out, Buffer] ID2D1Geometry** geometries,[None] int geometriesCount)</unmanaged>
        public SharpDX.Direct2D1.Geometry[] GetSourceGeometry(int geometriesCount)
        {
            Geometry[] geometries = new Geometry[geometriesCount];
            GetSourceGeometries(geometries, geometriesCount);
            return geometries;
        }
    }
}
