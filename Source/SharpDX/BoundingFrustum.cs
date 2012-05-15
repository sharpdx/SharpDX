// Copyright (c) 2010-2012 SharpDX - Alexandre Mutel
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
using System.Runtime.InteropServices;

namespace SharpDX
{
    /// <summary>
    /// Defines a frustum which can be used in frustum culling, zoom to Extents (zoom to fit) operations, 
    /// (matrix, frustum, camera) interchange, and many kind of intersection testing.
    /// </summary>
#if !WIN8METRO
    [Serializable]
#endif
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct BoundingFrustum : IEquatable<BoundingFrustum>
    {
        private Matrix pMatrix;
        private Plane pNear;
        private Plane pFar;
        private Plane pLeft;
        private Plane pRight;
        private Plane pTop;
        private Plane pBottom;

        /// <summary>
        /// Gets or sets the Matrix that describes this bounding frustum.
        /// </summary>
        public Matrix Matrix
        {
            get
            {
                return pMatrix;
            }
            set
            {
                pMatrix = Matrix;
                GetPlanesFromMatrix(ref pMatrix, out pNear, out pFar, out pLeft, out pRight, out pTop, out pBottom);
            }
        }
        /// <summary>
        /// Gets the near plane of the BoundingFrustum.
        /// </summary>
        public Plane Near
        {
            get
            {
                return pNear;
            }
        }
        /// <summary>
        /// Gets the far plane of the BoundingFrustum.
        /// </summary>
        public Plane Far
        {
            get
            {
                return pFar;
            }
        }
        /// <summary>
        /// Gets the left plane of the BoundingFrustum.
        /// </summary>
        public Plane Left
        {
            get
            {
                return pLeft;
            }
        }
        /// <summary>
        /// Gets the right plane of the BoundingFrustum.
        /// </summary>
        public Plane Right
        {
            get
            {
                return pRight;
            }
        }
        /// <summary>
        /// Gets the top plane of the BoundingFrustum.
        /// </summary>
        public Plane Top
        {
            get
            {
                return pTop;
            }
        }
        /// <summary>
        /// Gets the bottom plane of the BoundingFrustum.
        /// </summary>
        public Plane Bottom
        {
            get
            {
                return pBottom;
            }
        }

        /// <summary>
        /// Creates a new instance of BoundingFrustum.
        /// </summary>
        /// <param name="matrix">Combined matrix that usually takes view × projection matrix.</param>
        public BoundingFrustum(Matrix matrix)
        {
            pMatrix = matrix;
            GetPlanesFromMatrix(ref pMatrix, out pNear, out pFar, out pLeft, out pRight, out pTop, out pBottom);
        }

        public override int GetHashCode()
        {
            return pMatrix.GetHashCode();
        }

        /// <summary>
        /// Determines whether the specified <see cref="BoundingFrustum"/> is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="BoundingFrustum"/> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="BoundingFrustum"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(BoundingFrustum other)
        {
            return this.pMatrix == other.pMatrix;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj != null && obj is BoundingFrustum)
                return Equals((BoundingFrustum)obj);
            return false;
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(BoundingFrustum left, BoundingFrustum right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(BoundingFrustum left, BoundingFrustum right)
        {
            return !left.Equals(right);
        }

        private static void GetPlanesFromMatrix(ref Matrix matrix, out Plane near, out Plane far, out Plane left, out Plane right, out Plane top, out Plane bottom)
        {
            //http://www.chadvernon.com/blog/resources/directx9/frustum-culling/

            // Left plane
            left.Normal.X = matrix.M14 + matrix.M11;
            left.Normal.Y = matrix.M24 + matrix.M21;
            left.Normal.Z = matrix.M34 + matrix.M31;
            left.D = matrix.M44 + matrix.M41;
            left.Normalize();

            // Right plane
            right.Normal.X = matrix.M14 - matrix.M11;
            right.Normal.Y = matrix.M24 - matrix.M21;
            right.Normal.Z = matrix.M34 - matrix.M31;
            right.D = matrix.M44 - matrix.M41;
            right.Normalize();

            // Top plane
            top.Normal.X = matrix.M14 - matrix.M12;
            top.Normal.Y = matrix.M24 - matrix.M22;
            top.Normal.Z = matrix.M34 - matrix.M32;
            top.D = matrix.M44 - matrix.M42;
            top.Normalize();

            // Bottom plane
            bottom.Normal.X = matrix.M14 + matrix.M12;
            bottom.Normal.Y = matrix.M24 + matrix.M22;
            bottom.Normal.Z = matrix.M34 + matrix.M32;
            bottom.D = matrix.M44 + matrix.M42;
            bottom.Normalize();

            // Near plane
            near.Normal.X = matrix.M13;
            near.Normal.Y = matrix.M23;
            near.Normal.Z = matrix.M33;
            near.D = matrix.M43;
            near.Normalize();

            // Far plane
            far.Normal.X = matrix.M14 - matrix.M13;
            far.Normal.Y = matrix.M24 - matrix.M23;
            far.Normal.Z = matrix.M34 - matrix.M33;
            far.D = matrix.M44 - matrix.M43;
            far.Normalize();
        }

        private static Vector3 Get3PlanesInterPoint(ref Plane p1, ref Plane p2, ref Plane p3)
        {
            //P = -d1 * N2xN3 / N1.N2xN3 - d2 * N3xN1 / N2.N3xN1 - d3 * N1xN2 / N3.N1xN2 
            Vector3 v =
                -p1.D * Vector3.Cross(p2.Normal, p3.Normal) / Vector3.Dot(p1.Normal, Vector3.Cross(p2.Normal, p3.Normal))
                - p2.D * Vector3.Cross(p3.Normal, p1.Normal) / Vector3.Dot(p2.Normal, Vector3.Cross(p3.Normal, p1.Normal))
                - p3.D * Vector3.Cross(p1.Normal, p2.Normal) / Vector3.Dot(p3.Normal, Vector3.Cross(p1.Normal, p2.Normal));

            return v;
        }

        /// <summary>
        /// Creates a new frustum relaying on perspective camera parameters
        /// </summary>
        /// <param name="cameraPos">The camera pos.</param>
        /// <param name="lookDir">The look dir.</param>
        /// <param name="upDir">Up dir.</param>
        /// <param name="fov">The fov.</param>
        /// <param name="znear">The znear.</param>
        /// <param name="zfar">The zfar.</param>
        /// <param name="aspect">The aspect.</param>
        /// <returns>The bouding frustum calculated from perspective camera</returns>
        public static BoundingFrustum FromCamera(Vector3 cameraPos, Vector3 lookDir, Vector3 upDir, float fov, float znear, float zfar, float aspect)
        {
            //http://knol.google.com/k/view-frustum

            lookDir = Vector3.Normalize(lookDir);
            upDir = Vector3.Normalize(upDir);

            Vector3 nearCenter = cameraPos + lookDir * znear;
            Vector3 farCenter = cameraPos + lookDir * zfar;
            float nearHalfHeight = (float)(znear * Math.Tan(fov / 2f));
            float farHalfHeight = (float)(zfar * Math.Tan(fov / 2f));
            float nearHalfWidth = nearHalfHeight * aspect;
            float farHalfWidth = farHalfHeight * aspect;

            Vector3 rightDir = Vector3.Normalize(Vector3.Cross(upDir, lookDir));
            Vector3 Near1 = nearCenter - nearHalfHeight * upDir + nearHalfWidth * rightDir;
            Vector3 Near2 = nearCenter + nearHalfHeight * upDir + nearHalfWidth * rightDir;
            Vector3 Near3 = nearCenter + nearHalfHeight * upDir - nearHalfWidth * rightDir;
            Vector3 Near4 = nearCenter - nearHalfHeight * upDir - nearHalfWidth * rightDir;
            Vector3 Far1 = farCenter - farHalfHeight * upDir + farHalfWidth * rightDir;
            Vector3 Far2 = farCenter + farHalfHeight * upDir + farHalfWidth * rightDir;
            Vector3 Far3 = farCenter + farHalfHeight * upDir - farHalfWidth * rightDir;
            Vector3 Far4 = farCenter - farHalfHeight * upDir - farHalfWidth * rightDir;

            var result = new BoundingFrustum();
            result.pNear = new Plane(Near1, Near2, Near3);
            result.pFar = new Plane(Far3, Far2, Far1);
            result.pLeft = new Plane(Near4, Near3, Far3);
            result.pRight = new Plane(Far1, Far2, Near2);
            result.pTop = new Plane(Near2, Far2, Far3);
            result.pBottom = new Plane(Far4, Far1, Near1);

            result.pNear.Normalize();
            result.pFar.Normalize();
            result.pLeft.Normalize();
            result.pRight.Normalize();
            result.pTop.Normalize();
            result.pBottom.Normalize();

            result.pMatrix = Matrix.LookAtLH(cameraPos, cameraPos + lookDir * 10, upDir) * Matrix.PerspectiveFovLH(fov, aspect, znear, zfar);

            return result;
        }
        /// <summary>
        /// Creates a new frustum relaying on perspective camera parameters
        /// </summary>
        /// <param name="cameraParams">The camera params.</param>
        /// <returns>The bouding frustum from camera params</returns>
        public static BoundingFrustum FromCamera(FrustumCameraParams cameraParams)
        {
            return FromCamera(cameraParams.Position, cameraParams.LookAtDir, cameraParams.UpDir, cameraParams.FOV, cameraParams.ZNear, cameraParams.ZFar, cameraParams.AspectRatio);
        }

        /// <summary>
        /// Returns the 8 corners of the frustum, element0 is Near1 (near right down corner)
        /// , element1 is Near2 (near right top corner)
        /// , element2 is Near3 (near Left top corner)
        /// , element3 is Near4 (near Left down corner)
        /// , element4 is Far1 (far right down corner)
        /// , element5 is Far2 (far right top corner)
        /// , element6 is Far3 (far left top corner)
        /// , element7 is Far4 (far left down corner)
        /// </summary>
        /// <returns>The 8 corners of the frustum</returns>
        public Vector3[] GetCorners()
        {
            var corners = new Vector3[8];
            corners[0] = Get3PlanesInterPoint(ref pNear, ref  pBottom, ref  pRight);    //Near1
            corners[1] = Get3PlanesInterPoint(ref pNear, ref  pTop, ref  pRight);       //Near2
            corners[2] = Get3PlanesInterPoint(ref pNear, ref  pTop, ref  pLeft);        //Near3
            corners[3] = Get3PlanesInterPoint(ref pNear, ref  pBottom, ref  pLeft);     //Near3
            corners[4] = Get3PlanesInterPoint(ref pFar, ref  pBottom, ref  pRight);    //Far1
            corners[5] = Get3PlanesInterPoint(ref pFar, ref  pTop, ref  pRight);       //Far2
            corners[6] = Get3PlanesInterPoint(ref pFar, ref  pTop, ref  pLeft);        //Far3
            corners[7] = Get3PlanesInterPoint(ref pFar, ref  pBottom, ref  pLeft);     //Far3
            return corners;
        }
        /// <summary>
        /// Extracts perspective camera parameters from the frustum, dosn't work with orthographic frustums.
        /// </summary>
        /// <returns>Perspective camera parameters from the frustum</returns>
        public FrustumCameraParams GetCameraParams()
        {
            var corners = GetCorners();
            var cameraParam = new FrustumCameraParams();
            cameraParam.Position = Get3PlanesInterPoint(ref pRight, ref pTop, ref pLeft);
            cameraParam.LookAtDir = pNear.Normal;
            cameraParam.UpDir = Vector3.Normalize(Vector3.Cross(pRight.Normal, pNear.Normal));
            cameraParam.FOV = (float)((Math.PI / 2.0 - Math.Acos(Vector3.Dot(pNear.Normal, pTop.Normal))) * 2);
            cameraParam.AspectRatio = (corners[6] - corners[5]).Length() / (corners[4] - corners[5]).Length();
            cameraParam.ZNear = (cameraParam.Position + (pNear.Normal * pNear.D)).Length();
            cameraParam.ZFar = (cameraParam.Position + (pFar.Normal * pFar.D)).Length();
            return cameraParam;
        }

        /// <summary>
        /// Checks whether a point lay inside, intersects or lay outside the frustum.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <returns>Type of the containment</returns>
        public ContainmentType Contains(ref Vector3 point)
        {
            var result = PlaneIntersectionType.Front;
            var planeResult = PlaneIntersectionType.Front;
            for (int i = 0; i < 6; i++)
            {
                switch (i)
                {
                    case 0: planeResult = pNear.Intersects(ref point); break;
                    case 1: planeResult = pFar.Intersects(ref point); break;
                    case 2: planeResult = pLeft.Intersects(ref point); break;
                    case 3: planeResult = pRight.Intersects(ref point); break;
                    case 4: planeResult = pTop.Intersects(ref point); break;
                    case 5: planeResult = pBottom.Intersects(ref point); break;
                }
                switch (planeResult)
                {
                    case PlaneIntersectionType.Back:
                        return ContainmentType.Disjoint;
                    case PlaneIntersectionType.Intersecting:
                        result = PlaneIntersectionType.Intersecting;
                        break;
                }
            }
            switch (result)
            {
                case PlaneIntersectionType.Intersecting: return ContainmentType.Intersects;
                default: return ContainmentType.Contains;
            }
        }

        /// <summary>
        /// Checks whether a point lay inside, intersects or lay outside the frustum.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <returns>Type of the containment</returns>
        public ContainmentType Contains(Vector3 point)
        {
            return Contains(point);
        }

        /// <summary>
        /// Checks whether a group of points lay totally inside the frustum (Contains), or lay partially inside the frustum (Intersects), or lay outside the frustum (Disjoint).
        /// </summary>
        /// <param name="points">The points.</param>
        /// <returns>Type of the containment</returns>
        public ContainmentType Contains(Vector3[] points)
        {
            var containsAny = false;
            var containsAll = true;
            for (int i = 0; i < points.Length; i++)
            {
                switch (Contains(ref points[i]))
                {
                    case ContainmentType.Contains:
                    case ContainmentType.Intersects:
                        containsAny = true;
                        break;
                    case ContainmentType.Disjoint:
                        containsAll = false;
                        break;
                }
            }
            if (containsAny)
            {
                if (containsAll)
                    return ContainmentType.Contains;
                else
                    return ContainmentType.Intersects;
            }
            else
                return ContainmentType.Disjoint;
        }
        /// <summary>
        /// Checks whether a group of points lay totally inside the frsutrum (Contains), or lay partially inside the frustum (Intersects), or lay outside the frustum (Disjoint).
        /// </summary>
        /// <param name="points">The points.</param>
        /// <param name="result">Type of the containment.</param>
        public void Contains(Vector3[] points, out ContainmentType result)
        {
            result = Contains(points);
        }
        /// <summary>
        /// Determines the intersection relationship between the frustum and a bounding box.
        /// </summary>
        /// <param name="box">The box.</param>
        /// <returns>Type of the containment</returns>
        public ContainmentType Contains(ref BoundingBox box)
        {
            return Contains(box.GetCorners());
        }
        /// <summary>
        /// Determines the intersection relationship between the frustum and a bounding box.
        /// </summary>
        /// <param name="box">The box.</param>
        /// <param name="result">Type of the containment.</param>
        public void Contains(ref BoundingBox box, out ContainmentType result)
        {
            result = Contains(box.GetCorners());
        }
        /// <summary>
        /// Determines the intersection relationship between the frustum and a bounding sphere.
        /// </summary>
        /// <param name="sphere">The sphere.</param>
        /// <returns>Type of the containment</returns>
        public ContainmentType Contains(ref BoundingSphere sphere)
        {
            var result = PlaneIntersectionType.Front;
            var planeResult = PlaneIntersectionType.Front;
            for (int i = 0; i < 6; i++)
            {
                switch (i)
                {
                    case 0: planeResult = pNear.Intersects(ref sphere); break;
                    case 1: planeResult = pFar.Intersects(ref sphere); break;
                    case 2: planeResult = pLeft.Intersects(ref sphere); break;
                    case 3: planeResult = pRight.Intersects(ref sphere); break;
                    case 4: planeResult = pTop.Intersects(ref sphere); break;
                    case 5: planeResult = pBottom.Intersects(ref sphere); break;
                }
                switch (planeResult)
                {
                    case PlaneIntersectionType.Back:
                        return ContainmentType.Disjoint;
                    case PlaneIntersectionType.Intersecting:
                        result = PlaneIntersectionType.Intersecting;
                        break;
                }
            }
            switch (result)
            {
                case PlaneIntersectionType.Intersecting: return ContainmentType.Intersects;
                default: return ContainmentType.Contains;
            }
        }
        /// <summary>
        /// Determines the intersection relationship between the frustum and a bounding sphere.
        /// </summary>
        /// <param name="sphere">The sphere.</param>
        /// <param name="result">Type of the containment.</param>
        public void Contains(ref BoundingSphere sphere, out ContainmentType result)
        {
            result = Contains(ref sphere);
        }
        /// <summary>
        /// Determines the intersection relationship between the frustum and another bounding frustum.
        /// </summary>
        /// <param name="frustum">The frustum.</param>
        /// <returns>Type of the containment</returns>
        public ContainmentType Contains(ref BoundingFrustum frustum)
        {
            return Contains(frustum.GetCorners());
        }
        /// <summary>
        /// Determines the intersection relationship between the frustum and another bounding frustum.
        /// </summary>
        /// <param name="frustum">The frustum.</param>
        /// <param name="result">Type of the containment.</param>
        public void Contains(ref BoundingFrustum frustum, out ContainmentType result)
        {
            result = Contains(frustum.GetCorners());
        }

        /// <summary>
        /// Checks whether the current BoundingFrustum intersects a BoundingSphere.
        /// </summary>
        /// <param name="sphere">The sphere.</param>
        /// <returns>Type of the containment</returns>
        public bool Intersects(ref BoundingSphere sphere)
        {
            return Contains(ref sphere) != ContainmentType.Disjoint;
        }
        /// <summary>
        /// Checks whether the current BoundingFrustum intersects a BoundingSphere.
        /// </summary>
        /// <param name="sphere">The sphere.</param>
        /// <param name="result">Set to <c>true</c> if the current BoundingFrustum intersects a BoundingSphere.</param>
        public void Intersects(ref BoundingSphere sphere, out bool result)
        {
            result = Contains(ref sphere) != ContainmentType.Disjoint;
        }
        /// <summary>
        /// Checks whether the current BoundingFrustum intersects a BoundingBox.
        /// </summary>
        /// <param name="box">The box.</param>
        /// <returns><c>true</c> if the current BoundingFrustum intersects a BoundingSphere.</returns>
        public bool Intersects(ref BoundingBox box)
        {
            return Contains(ref box) != ContainmentType.Disjoint;
        }
        /// <summary>
        /// Checks whether the current BoundingFrustum intersects a BoundingBox.
        /// </summary>
        /// <param name="box">The box.</param>
        /// <param name="result"><c>true</c> if the current BoundingFrustum intersects a BoundingSphere.</param>
        public void Intersects(ref BoundingBox box, out bool result)
        {
            result = Contains(ref box) != ContainmentType.Disjoint;
        }

        private PlaneIntersectionType PlaneIntersectsPoints(ref Plane plane, Vector3[] points)
        {
            var result = Collision.PlaneIntersectsPoint(ref plane, ref points[0]);
            for (int i = 1; i < points.Length; i++)
                if (Collision.PlaneIntersectsPoint(ref plane, ref points[i]) != result)
                    return PlaneIntersectionType.Intersecting;
            return result;
        }

        /// <summary>
        /// Checks whether the current BoundingFrustum intersects the specified Plane.
        /// </summary>
        /// <param name="plane">The plane.</param>
        /// <returns>Plane intersection type.</returns>
        public PlaneIntersectionType Intersects(ref Plane plane)
        {
            return PlaneIntersectsPoints(ref plane, GetCorners());
        }
        /// <summary>
        /// Checks whether the current BoundingFrustum intersects the specified Plane.
        /// </summary>
        /// <param name="plane">The plane.</param>
        /// <param name="result">Plane intersection type.</param>
        public void Intersects(ref Plane plane, out PlaneIntersectionType result)
        {
            result = PlaneIntersectsPoints(ref plane, GetCorners());
        }
        /// <summary>
        /// Checks whether the current BoundingFrustum intersects with another BoundingFrustum.
        /// </summary>
        /// <param name="frustum">The frustum.</param>
        /// <returns><c>true</c> if the current BoundingFrustum intersects with another BoundingFrustum.</returns>
        public bool Intersects(ref BoundingFrustum frustum)
        {
            return Contains(frustum.GetCorners()) != ContainmentType.Disjoint;
        }
        /// <summary>
        /// Checks whether the current BoundingFrustum intersects with another BoundingFrustum.
        /// </summary>
        /// <param name="frustum">The frustum.</param>
        /// <param name="result"><c>true</c> if the current BoundingFrustum intersects with another BoundingFrustum.</param>
        public void Intersects(ref BoundingFrustum frustum, out bool result)
        {
            result = Contains(frustum.GetCorners()) != ContainmentType.Disjoint;
        }

        /// <summary>
        /// Get the width of the frustum at specified depth.
        /// </summary>
        /// <param name="depth">the depth at which to calculate frustum width.</param>
        /// <returns>With of the frustum at the specified depth</returns>
        public float GetWidthAtDepth(float depth)
        {
            float hAngle = (float)((Math.PI / 2.0 - Math.Acos(Vector3.Dot(pNear.Normal, pLeft.Normal))));
            return (float)(Math.Tan(hAngle) * depth * 2);
        }

        /// <summary>
        /// Get the height of the frustum at specified depth.
        /// </summary>
        /// <param name="depth">the depth at which to calculate frustum height.</param>
        /// <returns>Height of the frustum at the specified depth</returns>
        public float GetHeightAtDepth(float depth)
        {
            float vAngle = (float)((Math.PI / 2.0 - Math.Acos(Vector3.Dot(pNear.Normal, pTop.Normal))));
            return (float)(Math.Tan(vAngle) * depth * 2);
        }

        private BoundingFrustum GetInsideOutClone()
        {
            var frustum = this;
            frustum.pNear.Normal = -frustum.pNear.Normal;
            frustum.pFar.Normal = -frustum.pFar.Normal;
            frustum.pLeft.Normal = -frustum.pLeft.Normal;
            frustum.pRight.Normal = -frustum.pRight.Normal;
            frustum.pTop.Normal = -frustum.pTop.Normal;
            frustum.pBottom.Normal = -frustum.pBottom.Normal;
            return frustum;
        }

        /// <summary>
        /// Checks whether the current BoundingFrustum intersects the specified Ray.
        /// </summary>
        /// <param name="ray">The ray.</param>
        /// <returns><c>true</c> if the current BoundingFrustum intersects the specified Ray.</returns>
        public bool Intersects(ref Ray ray)
        {
            var planeIntersects = false;
            for (int i = 0; i < 6; i++)
            {
                switch (i)
                {
                    case 0: planeIntersects = pNear.Intersects(ref ray); break;
                    case 1: planeIntersects = pFar.Intersects(ref ray); break;
                    case 2: planeIntersects = pLeft.Intersects(ref ray); break;
                    case 3: planeIntersects = pRight.Intersects(ref ray); break;
                    case 4: planeIntersects = pTop.Intersects(ref ray); break;
                    case 5: planeIntersects = pBottom.Intersects(ref ray); break;
                }
                if (planeIntersects)
                    return true;
            }
            return false;
        }
        /// <summary>
        /// Checks whether the current BoundingFrustum intersects the specified Ray.
        /// </summary>
        /// <param name="ray">The Ray to check for intersection with.</param>
        /// <param name="inDistance">The distance at which the ray enters the frustum if there is an intersection and the ray starts outside the frustum.</param>
        /// <param name="outDistance">The distance at which the ray exits the frustum if there is an intersection.</param>
        /// <returns><c>true</c> if the current BoundingFrustum intersects the specified Ray.</returns>
        public bool Intersects(ref Ray ray, out float? inDistance, out float? outDistance)
        {
            var result = false;
            inDistance = null;
            outDistance = null;

            var ioFrustrum = GetInsideOutClone();

            for (int i = 0; i < 6; i++)
            {
                var planeIntersects = false;
                float interDist = 0;
                switch (i)
                {
                    case 0: planeIntersects = ioFrustrum.pNear.Intersects(ref ray, out interDist); break;
                    case 1: planeIntersects = ioFrustrum.pFar.Intersects(ref ray, out interDist); break;
                    case 2: planeIntersects = ioFrustrum.pLeft.Intersects(ref ray, out interDist); break;
                    case 3: planeIntersects = ioFrustrum.pRight.Intersects(ref ray, out interDist); break;
                    case 4: planeIntersects = ioFrustrum.pTop.Intersects(ref ray, out interDist); break;
                    case 5: planeIntersects = ioFrustrum.pBottom.Intersects(ref ray, out interDist); break;
                }
                if (planeIntersects)
                {
                    result = true;
                    if (!inDistance.HasValue)
                    {
                        inDistance = interDist;
                    }
                    else
                    {
                        if (!outDistance.HasValue)
                        {
                            if (interDist < inDistance.Value)
                            {
                                outDistance = inDistance;
                                inDistance = interDist;
                            }
                            else
                            {
                                outDistance = interDist;
                            }
                        }
                    }
                }
            }

            //if the intersection happed at one point, then the ray starts from inside the frustum
            //and intersects it while going out.
            if (result && inDistance.HasValue && !outDistance.HasValue)
            {
                outDistance = inDistance;
                inDistance = null;
            }
            return result;
        }

        /// <summary>
        /// Get the distance which when added to camera position along the lookat direction will do the effect of zoom to extents (zoom to fit) operation,
        /// so all the passed points will fit in the current view.
        /// if the returned value is poistive, the camera will move toward the lookat direction (ZoomIn).
        /// if the returned value is negative, the camera will move in the revers direction of the lookat direction (ZoomOut).
        /// </summary>
        /// <param name="points">The points.</param>
        /// <returns>The zoom to fit distance</returns>
        public float GetZoomToExtentsShiftDistance(Vector3[] points)
        {
            float vAngle = (float)((Math.PI / 2.0 - Math.Acos(Vector3.Dot(pNear.Normal, pTop.Normal))));
            float vSin = (float)Math.Sin(vAngle);
            float hAngle = (float)((Math.PI / 2.0 - Math.Acos(Vector3.Dot(pNear.Normal, pLeft.Normal))));
            float hSin = (float)Math.Sin(hAngle);
            float horizontalToVerticalMapping = vSin / hSin;

            var ioFrustrum = GetInsideOutClone();

            float maxPointDist = float.MinValue;
            for (int i = 0; i < points.Length; i++)
            {
                float pointDist = Collision.DistancePlanePoint(ref ioFrustrum.pTop, ref points[i]);
                pointDist = Math.Max(pointDist, Collision.DistancePlanePoint(ref ioFrustrum.pBottom, ref points[i]));
                pointDist = Math.Max(pointDist, Collision.DistancePlanePoint(ref ioFrustrum.pLeft, ref points[i]) * horizontalToVerticalMapping);
                pointDist = Math.Max(pointDist, Collision.DistancePlanePoint(ref ioFrustrum.pRight, ref points[i]) * horizontalToVerticalMapping);

                maxPointDist = Math.Max(maxPointDist, pointDist);
            }
            return -maxPointDist / vSin;
        }

        /// <summary>
        /// Get the distance which when added to camera position along the lookat direction will do the effect of zoom to extents (zoom to fit) operation,
        /// so all the passed points will fit in the current view.
        /// if the returned value is poistive, the camera will move toward the lookat direction (ZoomIn).
        /// if the returned value is negative, the camera will move in the revers direction of the lookat direction (ZoomOut).
        /// </summary>
        /// <param name="boundingBox">The bounding box.</param>
        /// <returns>The zoom to fit distance</returns>
        public float GetZoomToExtentsShiftDistance(ref BoundingBox boundingBox)
        {
            return GetZoomToExtentsShiftDistance(boundingBox.GetCorners());
        }

        /// <summary>
        /// Get the vector shift which when added to camera position will do the effect of zoom to extents (zoom to fit) operation,
        /// so all the passed points will fit in the current view.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <returns>The zoom to fit vector</returns>
        public Vector3 GetZoomToExtentsShiftVector(Vector3[] points)
        {
            return GetZoomToExtentsShiftDistance(points) * pNear.Normal;
        }
        /// <summary>
        /// Get the vector shift which when added to camera position will do the effect of zoom to extents (zoom to fit) operation,
        /// so all the passed points will fit in the current view.
        /// </summary>
        /// <param name="boundingBox">The bounding box.</param>
        /// <returns>The zoom to fit vector</returns>
        public Vector3 GetZoomToExtentsShiftVector(ref BoundingBox boundingBox)
        {
            return GetZoomToExtentsShiftDistance(boundingBox.GetCorners()) * pNear.Normal;
        }

        /// <summary>
        /// Indicate whether the current BoundingFrustrum is Orthographic.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if the current BoundingFrustrum is Orthographic; otherwise, <c>false</c>.
        /// </value>
        public bool IsOrthographic
        {
            get
            {
                return (pLeft.Normal == -pRight.Normal) && (pTop.Normal == -pBottom.Normal);
            }
        }
    }
}
