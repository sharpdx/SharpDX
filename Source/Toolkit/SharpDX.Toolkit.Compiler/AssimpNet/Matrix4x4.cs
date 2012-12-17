/*
* Copyright (c) 2012 Nicholas Woodfield
* 
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
* 
* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
* 
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
* THE SOFTWARE.
*/

using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Assimp {
	/// <summary>
	/// Represents a 4x4 column-vector matrix (X base is the first column, Y base is the second, Z base the third, and translation the fourth). 
    /// Memory layout is row major. Right handed conventions are used by default.
	/// </summary>
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	internal struct Matrix4x4 : IEquatable<Matrix4x4> {
		/// <summary>
		/// Value at row 1, column 1 of the matrix
		/// </summary>
		public float A1;

		/// <summary>
		/// Value at row 1, column 2 of the matrix
		/// </summary>
		public float A2;

		/// <summary>
		/// Value at row 1, column 3 of the matrix
		/// </summary>
		public float A3;

		/// <summary>
		/// Value at row 1, column 4 of the matrix
		/// </summary>
		public float A4;

		/// <summary>
		/// Value at row 2, column 1 of the matrix
		/// </summary>
		public float B1;

		/// <summary>
		/// Value at row 2, column 2 of the matrix
		/// </summary>
		public float B2;

		/// <summary>
		/// Value at row 2, column 3 of the matrix
		/// </summary>
		public float B3;

		/// <summary>
		/// Value at row 2, column 4 of the matrix
		/// </summary>
		public float B4;

		/// <summary>
		/// Value at row 3, column 1 of the matrix
		/// </summary>
		public float C1;

		/// <summary>
		/// Value at row 3, column 2 of the matrix
		/// </summary>
		public float C2;

		/// <summary>
		/// Value at row 3, column 3 of the matrix
		/// </summary>
		public float C3;

		/// <summary>
		/// Value at row 3, column 4 of the matrix
		/// </summary>
		public float C4;

		/// <summary>
		/// Value at row 4, column 1 of the matrix
		/// </summary>
		public float D1;

		/// <summary>
		/// Value at row 4, column 2 of the matrix
		/// </summary>
		public float D2;

		/// <summary>
		/// Value at row 4, column 3 of the matrix
		/// </summary>
		public float D3;

		/// <summary>
		/// Value at row 4, column 4 of the matrix
		/// </summary>
		public float D4;

		private static Matrix4x4 _identity = new Matrix4x4(1.0f, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f);

		/// <summary>
		/// Gets the identity matrix.
		/// </summary>
		public static Matrix4x4 Identity {
			get {
				return _identity;
			}
		}

		/// <summary>
		/// Gets if this matrix is an identity matrix.
		/// </summary>
		public bool IsIdentity {
			get {
				float epsilon = 10e-3f;

				return (A2 <= epsilon && A2 >= -epsilon &&
				A3 <= epsilon && A3 >= -epsilon &&
				A4 <= epsilon && A4 >= -epsilon &&
				B1 <= epsilon && B1 >= -epsilon &&
				B3 <= epsilon && B3 >= -epsilon &&
				B4 <= epsilon && B4 >= -epsilon &&
				C1 <= epsilon && C1 >= -epsilon &&
				C2 <= epsilon && C2 >= -epsilon &&
				C4 <= epsilon && C4 >= -epsilon &&
				D1 <= epsilon && D1 >= -epsilon &&
				D2 <= epsilon && D2 >= -epsilon &&
				D3 <= epsilon && D3 >= -epsilon &&
				A1 <= 1.0f + epsilon && A1 >= 1.0f - epsilon && 
				B2 <= 1.0f + epsilon && B2 >= 1.0f - epsilon && 
				C3 <= 1.0f + epsilon && C3 >= 1.0f - epsilon && 
				D4 <= 1.0f + epsilon && D4 >= 1.0f - epsilon);
			}
		}

		/// <summary>
		/// Gets or sets the value at the specific one-based row, column
		/// index. E.g. i = 1, j = 2 gets the value in row 1, column 2 (MA2). Indices
		/// out of range return a value of zero.
		/// 
		/// </summary>
		/// <param name="i">One-based Row index</param>
		/// <param name="j">One-based Column index</param>
		/// <returns>Matrix value</returns>
		public float this[int i, int j] {
			get {
				switch(i) {
					case 1:
						switch(j) {
							case 1:
								return A1;
							case 2:
								return A2;
							case 3:
								return A3;
							case 4:
								return A4;
							default:
								return 0;
						}
					case 2:
						switch(j) {
							case 1:
								return B1;
							case 2:
								return B2;
							case 3:
								return B3;
							case 4:
								return B4;
							default:
								return 0;
						}
					case 3:
						switch(j) {
							case 1:
								return C1;
							case 2:
								return C2;
							case 3:
								return C3;
							case 4:
								return C4;
							default:
								return 0;
						}
					case 4:
						switch(j) {
							case 1:
								return D1;
							case 2:
								return D2;
							case 3:
								return D3;
							case 4:
								return D4;
							default:
								return 0;
						}
					default:
						return 0;
				}
			}
			set {
				switch(i) {
					case 1:
						switch(j) {
							case 1:
								A1 = value;
								break;
							case 2:
								A2 = value;
								break;
							case 3:
								A3 = value;
								break;
							case 4:
								A4 = value;
								break;
						}
						break;
					case 2:
						switch(j) {
							case 1:
								B1 = value;
								break;
							case 2:
								B2 = value;
								break;
							case 3:
								B3 = value;
								break;
							case 4:
								B4 = value;
								break;
						}
						break;
					case 3:
						switch(j) {
							case 1:
								C1 = value;
								break;
							case 2:
								C2 = value;
								break;
							case 3:
								C3 = value;
								break;
							case 4:
								C4 = value;
								break;
						}
						break;
					case 4:
						switch(j) {
							case 1:
								D1 = value;
								break;
							case 2:
								D2 = value;
								break;
							case 3:
								D3 = value;
								break;
							case 4:
								D4 = value;
								break;
						}
						break;
				}
			}
		}

		/// <summary>
		/// Constructs a new Matrix4x4.
		/// </summary>
		/// <param name="a1">Element at row 1, column 1</param>
		/// <param name="a2">Element at row 1, column 2</param>
		/// <param name="a3">Element at row 1, column 3</param>
		/// <param name="a4">Element at row 1, column 4</param>
		/// <param name="b1">Element at row 2, column 1</param>
		/// <param name="b2">Element at row 2, column 2</param>
		/// <param name="b3">Element at row 2, column 3</param>
		/// <param name="b4">Element at row 2, column 4</param>
		/// <param name="c1">Element at row 3, column 1</param>
		/// <param name="c2">Element at row 3, column 2</param>
		/// <param name="c3">Element at row 3, column 3</param>
		/// <param name="c4">Element at row 3, column 4</param>
		/// <param name="d1">Element at row 4, column 1</param>
		/// <param name="d2">Element at row 4, column 2</param>
		/// <param name="d3">Element at row 4, column 3</param>
		/// <param name="d4">Element at row 4, column 4</param>
		public Matrix4x4(float a1, float a2, float a3, float a4, float b1, float b2, float b3, float b4,
			float c1, float c2, float c3, float c4, float d1, float d2, float d3, float d4) {
				this.A1 = a1;
				this.A2 = a2;
				this.A3 = a3;
				this.A4 = a4;

				this.B1 = b1;
				this.B2 = b2;
				this.B3 = b3;
				this.B4 = b4;

				this.C1 = c1;
				this.C2 = c2;
				this.C3 = c3;
				this.C4 = c4;

				this.D1 = d1;
				this.D2 = d2;
				this.D3 = d3;
				this.D4 = d4;
		}

		/// <summary>
		/// Constructs a new Matrix4x4.
		/// </summary>
		/// <param name="rotMatrix">Rotation matrix to copy values from.</param>
		public Matrix4x4(Matrix3x3 rotMatrix) {
			this.A1 = rotMatrix.A1;
			this.A2 = rotMatrix.A2;
			this.A3 = rotMatrix.A3;
			this.A4 = 0;

			this.B1 = rotMatrix.B1;
			this.B2 = rotMatrix.B2;
			this.B3 = rotMatrix.B3;
			this.B4 = 0;

			this.C1 = rotMatrix.C1;
			this.C2 = rotMatrix.C2;
			this.C3 = rotMatrix.C3;
			this.C4 = 0;

			this.D1 = 0;
			this.D2 = 0;
			this.D3 = 0;
			this.D4 = 1;
		}

		/// <summary>
		/// Transposes this matrix (rows become columns, vice versa).
		/// </summary>
		public void Transpose() {
			Matrix4x4 m = new Matrix4x4(this);
			m.A4 = A4;
			m.B4 = B4;
			m.C4 = C4;
			m.D1 = D1;
			m.D2 = D2;
			m.D3 = D3;

			A2 = m.B1;
			A3 = m.C1;
			A4 = m.D1;

			B1 = m.A2;
			B3 = m.C2;
			B4 = m.D2;

			C1 = m.A3;
			C2 = m.B3;
			C4 = m.D3;

			D1 = m.A4;
			D2 = m.B4;
			D3 = m.C4;
		}

		/// <summary>
		/// Inverts the matrix. If the matrix is *not* invertible all elements are set to <see cref="float.NaN"/>.
		/// </summary>
		public void Inverse() {
			float det = Determinant();
			if(det == 0.0f) {
				// Matrix not invertible. Setting all elements to NaN is not really
				// correct in a mathematical sense but it is easy to debug for the
				// programmer.
				A1 = float.NaN;
				A2 = float.NaN;
				A3 = float.NaN;
				A4 = float.NaN;

				B1 = float.NaN;
				B2 = float.NaN;
				B3 = float.NaN;
				B4 = float.NaN;

				C1 = float.NaN;
				C2 = float.NaN;
				C3 = float.NaN;
				C4 = float.NaN;

				D1 = float.NaN;
				D2 = float.NaN;
				D3 = float.NaN;
				D4 = float.NaN;
			}

			float invDet = 1.0f / det;

			float a1 = invDet  * (B2 * (C3 * D4 - C4 * D3) + B3 * (C4 * D2 - C2 * D4) + B4 * (C2 * D3 - C3 * D2));
			float a2 = -invDet * (A2 * (C3 * D4 - C4 * D3) + A3 * (C4 * D2 - C2 * D4) + A4 * (C2 * D3 - C3 * D2));
			float a3 = invDet  * (A2 * (B3 * D4 - B4 * D3) + A3 * (B4 * D2 - B2 * D4) + A4 * (B2 * D3 - B3 * D2));
			float a4 = -invDet * (A2 * (B3 * C4 - B4 * C3) + A3 * (B4 * C2 - B2 * C4) + A4 * (B2 * C3 - B3 * C2));

			float b1 = -invDet * (B1 * (C3 * D4 - C4 * D3) + B3 * (C4 * D1 - C1 * D4) + B4 * (C1 * D3 - C3 * D1));
			float b2 = invDet  * (A1 * (C3 * D4 - C4 * D3) + A3 * (C4 * D1 - C1 * D4) + A4 * (C1 * D3 - C3 * D1));
			float b3 = -invDet * (A1 * (B3 * D4 - B4 * D3) + A3 * (B4 * D1 - B1 * D4) + A4 * (B1 * D3 - B3 * D1));
			float b4 = invDet  * (A1 * (B3 * C4 - B4 * C3) + A3 * (B4 * C1 - B1 * C4) + A4 * (B1 * C3 - B3 * C1));

			float c1 = invDet  * (B1 * (C2 * D4 - C4 * D2) + B2 * (C4 * D1 - C1 * D4) + B4 * (C1 * D2 - C2 * D1));
			float c2 = -invDet * (A1 * (C2 * D4 - C4 * D2) + A2 * (C4 * D1 - C1 * D4) + A4 * (C1 * D2 - C2 * D1));
			float c3 = invDet  * (A1 * (B2 * D4 - B4 * D2) + A2 * (B4 * D1 - B1 * D4) + A4 * (B1 * D2 - B2 * D1));
			float c4 = -invDet * (A1 * (B2 * C4 - B4 * C2) + A2 * (B4 * C1 - B1 * C4) + A4 * (B1 * C2 - B2 * C1));

			float d1 = -invDet * (B1 * (C2 * D3 - C3 * D2) + B2 * (C3 * D1 - C1 * D3) + B3 * (C1 * D2 - C2 * D1));
			float d2 = invDet  * (A1 * (C2 * D3 - C3 * D2) + A2 * (C3 * D1 - C1 * D3) + A3 * (C1 * D2 - C2 * D1));
			float d3 = -invDet * (A1 * (B2 * D3 - B3 * D2) + A2 * (B3 * D1 - B1 * D3) + A3 * (B1 * D2 - B2 * D1));
			float d4 = invDet  * (A1 * (B2 * C3 - B3 * C2) + A2 * (B3 * C1 - B1 * C3) + A3 * (B1 * C2 - B2 * C1));

			A1 = a1;
			A2 = a2;
			A3 = a3;
			A4 = a4;

			B1 = b1;
			B2 = b2;
			B3 = b3;
			B4 = b4;

			C1 = c1;
			C2 = c2;
			C3 = c3;
			C4 = c4;

			D1 = d1;
			D2 = d2;
			D3 = d3;
			D4 = d4;
		}

		/// <summary>
		/// Compute the determinant of this matrix.
		/// </summary>
		/// <returns>The determinant</returns>
		public float Determinant() {
			return A1*B2*C3*D4 - A1*B2*C4*D3 + A1*B3*C4*D2 - A1*B3*C2*D4 
				+ A1*B4*C2*D3 - A1*B4*C3*D2 - A2*B3*C4*D1 + A2*B3*C1*D4 
				- A2*B4*C1*D3 + A2*B4*C3*D1 - A2*B1*C3*D4 + A2*B1*C4*D3 
				+ A3*B4*C1*D2 - A3*B4*C2*D1 + A3*B1*C2*D4 - A3*B1*C4*D2 
				+ A3*B2*C4*D1 - A3*B2*C1*D4 - A4*B1*C2*D3 + A4*B1*C3*D2
				- A4*B2*C3*D1 + A4*B2*C1*D3 - A4*B3*C1*D2 + A4*B3*C2*D1;
		}


		/// <summary>
		/// Decomposes a transformation matrix into its original scale, rotation, and translation components. The
		/// scaling vector receives the scaling for the x, y, z axes. The rotation is returned as a hamilton quaternion. And
		/// the translation is the output position for the x, y, z axes.
		/// </summary>
		/// <param name="scaling">Vector to hold the scaling component</param>
		/// <param name="rotation">Quaternion to hold the rotation component</param>
		/// <param name="translation">Vector to hold the translation component</param>
		public void Decompose(out Vector3D scaling, out Quaternion rotation, out Vector3D translation) {
            //Extract the translation
            translation.X = A4;
            translation.Y = B4;
            translation.Z = C4;

            //Extract row vectors of the matrix
            Vector3D row1 = new Vector3D(A1, A2, A3);
            Vector3D row2 = new Vector3D(B1, B2, B3);
            Vector3D row3 = new Vector3D(C1, C2, C3);

            //Extract the scaling factors
			scaling.X = row1.Length();
			scaling.Y = row2.Length();
			scaling.Z = row3.Length();

			 //Handle negative scaling
			if (Determinant() < 0) {
				scaling.X = -scaling.X;
				scaling.Y = -scaling.Y;
				scaling.Z = -scaling.Z;
			}

			//Remove scaling from the matrix
			if(scaling.X != 0) {
				row1 /= scaling.X;
			}

			if(scaling.Y != 0) {
				row2 /= scaling.Y;
			}

			if(scaling.Z != 0) {
				row3 /= scaling.Z;
			}

            
			//Build 3x3 rot matrix, convert it to quaternion
            Matrix3x3 rotMat = new Matrix3x3(row1.X, row1.Y, row1.Z,
                                             row2.X, row2.Y, row2.Z,
                                             row3.X, row3.Y, row3.Z);

			rotation = new Quaternion(rotMat);
		}

		/// <summary>
		/// Decomposes a transformation matrix with no scaling. The rotation is returned as a hamilton
		/// quaternion. The translation receives the output position for the x, y, z axes.
		/// </summary>
		/// <param name="rotation">Quaternion to hold the rotation component</param>
		/// <param name="translation">Vector to hold the translation component</param>
		public void DecomposeNoScaling(out Quaternion rotation, out Vector3D translation) {

            //Extract translation
            translation.X = A4;
            translation.Y = B4;
            translation.Z = C4;

			rotation = new Quaternion(new Matrix3x3(this));
		}

		/// <summary>
		/// Creates a rotation matrix from a set of euler angles.
		/// </summary>
		/// <param name="x">Rotation angle about the x-axis, in radians.</param>
		/// <param name="y">Rotation angle about the y-axis, in radians.</param>
		/// <param name="z">Rotation angle about the z-axis, in radians.</param>
		/// <returns>The rotation matrix</returns>
		public static Matrix4x4 FromEulerAnglesXYZ(float x, float y, float z) {
			float cr = (float) Math.Cos(x);
			float sr = (float) Math.Sin(x);
			float cp = (float) Math.Cos(y);
			float sp = (float) Math.Sin(y);
			float cy = (float) Math.Cos(z);
			float sy = (float) Math.Sin(z);

			float srsp = sr * sp;
			float crsp = cr * sp;

			Matrix4x4 m;
			m.A1 = cp * cy;
			m.A2 = cp * sy;
			m.A3 = -sp;
			m.A4 = 0.0f;

			m.B1 = srsp * cy - cr * sy;
			m.B2 = srsp * sy + cr * cy;
			m.B3 = sr * cp;
			m.B4 = 0.0f;

			m.C1 = crsp * cy + sr * sy;
			m.C2 = crsp * sy - sr * cy;
			m.C3 = cr * cp;
			m.C4 = 0.0f;

			m.D1 = 0.0f;
			m.D2 = 0.0f;
			m.D3 = 0.0f;
			m.D4 = 1.0f;

			return m;
		}

		/// <summary>
		/// Creates a rotation matrix from a set of euler angles.
		/// </summary>
		/// <param name="angles">Vector containing the rotation angles about the x, y, z axes, in radians.</param>
		/// <returns>The rotation matrix</returns>
		public static Matrix4x4 FromEulerAnglesXYZ(Vector3D angles) {
			return Matrix4x4.FromEulerAnglesXYZ(angles.X, angles.Y, angles.Z);
		}

		/// <summary>
		/// Creates a rotation matrix for a rotation about the x-axis.
		/// </summary>
		/// <param name="radians">Rotation angle in radians.</param>
		/// <returns>The rotation matrix</returns>
		public static Matrix4x4 FromRotationX(float radians) {
			/*
				 |  1  0       0       0 |
			 M = |  0  cos(A) -sin(A)  0 |
				 |  0  sin(A)  cos(A)  0 |
				 |  0  0       0       1 |	
			*/
			Matrix4x4 m = Identity;
			m.B2 = m.C3 = (float) Math.Cos(radians);
			m.C2 = (float) Math.Sin(radians);
			m.B3 = -m.C2;
			return m;
		}

		/// <summary>
		/// Creates a rotation matrix for a rotation about the y-axis.
		/// </summary>
		/// <param name="radians">Rotation angle in radians.</param>
		/// <returns>The rotation matrix</returns>
		public static Matrix4x4 FromRotationY(float radians) {
			/*
				 |  cos(A)  0   sin(A)  0 |
			 M = |  0       1   0       0 |
				 | -sin(A)  0   cos(A)  0 |
				 |  0       0   0       1 |
			*/
			Matrix4x4 m = Identity;
			m.A1 = m.C3 = (float) Math.Cos(radians);
			m.A3 = (float) Math.Sin(radians);
			m.C1 = -m.A3;
			return m;
		}

		/// <summary>
		/// Creates a rotation matrix for a rotation about the z-axis.
		/// </summary>
		/// <param name="radians">Rotation angle in radians.</param>
		/// <returns>The rotation matrix</returns>
		public static Matrix4x4 FromRotationZ(float radians) {
			/*
				 |  cos(A)  -sin(A)   0   0 |
			 M = |  sin(A)   cos(A)   0   0 |
				 |  0        0        1   0 |
				 |  0        0        0   1 |	
			 */
			Matrix4x4 m = Identity;
			m.A1 = m.B2 = (float) Math.Cos(radians);
			m.B1 = (float) Math.Sin(radians);
			m.A2 = -m.B1;
			return m;
		}

		/// <summary>
		/// Creates a rotation matrix for a rotation about an arbitrary axis.
		/// </summary>
		/// <param name="radians">Rotation angle, in radians</param>
		/// <param name="axis">Rotation axis, which should be a normalized vector.</param>
		/// <returns>The rotation matrix</returns>
		public static Matrix4x4 FromAngleAxis(float radians, Vector3D axis) {
            float x = axis.X;
            float y = axis.Y;
            float z = axis.Z;

            float sin = (float) System.Math.Sin((double) radians);
            float cos = (float) System.Math.Cos((double) radians);

            float xx = x * x;
            float yy = y * y;
            float zz = z * z;
            float xy = x * y;
            float xz = x * z;
            float yz = y * z;

            Matrix4x4 m;
            m.A1 = xx + (cos * (1.0f - xx));
            m.B1 = (xy - (cos * xy)) + (sin * z);
            m.C1 = (xz - (cos * xz)) - (sin * y);
            m.D1 = 0.0f;

            m.A2 = (xy - (cos * xy)) - (sin * z);
            m.B2 = yy + (cos * (1.0f - yy));
            m.C2 = (yz - (cos * yz)) + (sin * x);
            m.D2 = 0.0f;

            m.A3 = (xz - (cos * xz)) + (sin * y);
            m.B3 = (yz - (cos * yz)) - (sin * x);
            m.C3 = zz + (cos * (1.0f - zz));
            m.D3 = 0.0f;

            m.A4 = 0.0f;
            m.B4 = 0.0f;
            m.C4 = 0.0f;
            m.D4 = 1.0f;

			return m;
		}

		/// <summary>
		/// Creates a translation matrix.
		/// </summary>
		/// <param name="translation">Translation vector</param>
		/// <returns>The translation matrix</returns>
		public static Matrix4x4 FromTranslation(Vector3D translation) {
			Matrix4x4 m = Identity;
			m.A4 = translation.X;
			m.B4 = translation.Y;
			m.C4 = translation.Z;
			return m;
		}

		/// <summary>
		/// Creates a scaling matrix.
		/// </summary>
		/// <param name="scaling">Scaling vector</param>
		/// <returns>The scaling vector</returns>
		public static Matrix4x4 FromScaling(Vector3D scaling) {
			Matrix4x4 m = Identity;
			m.A1 = scaling.X;
			m.B2 = scaling.Y;
			m.C3 = scaling.Z;
			return m;
		}

		/// <summary>
		/// Creates a rotation matrix that rotates a vector called "from" into another
		/// vector called "to". Based on an algorithm by Tomas Moller and John Hudges:
		/// <para>
		/// "Efficiently Building a Matrix to Rotate One Vector to Another"         
		/// Journal of Graphics Tools, 4(4):1-4, 1999
		/// </para>
		/// </summary>
		/// <param name="from">Starting vector</param>
		/// <param name="to">Ending vector</param>
		/// <returns>Rotation matrix to rotate from the start to end.</returns>
		public static Matrix4x4 FromToMatrix(Vector3D from, Vector3D to) {
			Matrix3x3 m3 = Matrix3x3.FromToMatrix(from, to);

			return new Matrix4x4(m3);
		}

		/// <summary>
		/// Tests equality between two matrices.
		/// </summary>
		/// <param name="a">First matrix</param>
		/// <param name="b">Second matrix</param>
		/// <returns>True if the matrices are equal, false otherwise</returns>
		public static bool operator==(Matrix4x4 a, Matrix4x4 b) {
			return (((a.A1 == b.A1) && (a.A2 == b.A2) && (a.A3 == b.A3) && (a.A4 == b.A4))
				&& ((a.B1 == b.B1) && (a.B2 == b.B2) && (a.B3 == b.B3) && (a.B4 == b.B4))
				&& ((a.C1 == b.C1) && (a.C2 == b.C2) && (a.C3 == b.C3) && (a.C4 == b.C4))
				&& ((a.D1 == b.D1) && (a.D2 == b.D2) && (a.D3 == b.D3) && (a.D4 == b.D4)));
		}

		/// <summary>
		/// Tests inequality between two matrices.
		/// </summary>
		/// <param name="a">First matrix</param>
		/// <param name="b">Second matrix</param>
		/// <returns>True if the matrices are not equal, false otherwise</returns>
		public static bool operator!=(Matrix4x4 a, Matrix4x4 b) {
			return (((a.A1 != b.A1) || (a.A2 != b.A2) || (a.A3 != b.A3) || (a.A4 != b.A4))
				|| ((a.B1 != b.B1) || (a.B2 != b.B2) || (a.B3 != b.B3) || (a.B4 != b.B4))
				|| ((a.C1 != b.C1) || (a.C2 != b.C2) || (a.C3 != b.C3) || (a.C4 != b.C4))
				|| ((a.D1 != b.D1) || (a.D2 != b.D2) || (a.D3 != b.D3) || (a.D4 != b.D4)));
		}


		/// <summary>
		/// Performs matrix multiplication. Multiplication order is B x A. That way, SRT concatenations
        /// are left to right.
		/// </summary>
		/// <param name="a">First matrix</param>
		/// <param name="b">Second matrix</param>
		/// <returns>Multiplied matrix</returns>
		public static Matrix4x4 operator*(Matrix4x4 a, Matrix4x4 b) {
			return new Matrix4x4(a.A1 * b.A1 + a.B1 * b.A2 + a.C1 * b.A3 + a.D1 * b.A4,
								 a.A2 * b.A1 + a.B2 * b.A2 + a.C2 * b.A3 + a.D2 * b.A4,
								 a.A3 * b.A1 + a.B3 * b.A2 + a.C3 * b.A3 + a.D3 * b.A4,
								 a.A4 * b.A1 + a.B4 * b.A2 + a.C4 * b.A3 + a.D4 * b.A4,
								 a.A1 * b.B1 + a.B1 * b.B2 + a.C1 * b.B3 + a.D1 * b.B4,
								 a.A2 * b.B1 + a.B2 * b.B2 + a.C2 * b.B3 + a.D2 * b.B4,
								 a.A3 * b.B1 + a.B3 * b.B2 + a.C3 * b.B3 + a.D3 * b.B4,
								 a.A4 * b.B1 + a.B4 * b.B2 + a.C4 * b.B3 + a.D4 * b.B4,
								 a.A1 * b.C1 + a.B1 * b.C2 + a.C1 * b.C3 + a.D1 * b.C4,
								 a.A2 * b.C1 + a.B2 * b.C2 + a.C2 * b.C3 + a.D2 * b.C4,
								 a.A3 * b.C1 + a.B3 * b.C2 + a.C3 * b.C3 + a.D3 * b.C4,
								 a.A4 * b.C1 + a.B4 * b.C2 + a.C4 * b.C3 + a.D4 * b.C4,
								 a.A1 * b.D1 + a.B1 * b.D2 + a.C1 * b.D3 + a.D1 * b.D4,
								 a.A2 * b.D1 + a.B2 * b.D2 + a.C2 * b.D3 + a.D2 * b.D4,
								 a.A3 * b.D1 + a.B3 * b.D2 + a.C3 * b.D3 + a.D3 * b.D4,
								 a.A4 * b.D1 + a.B4 * b.D2 + a.C4 * b.D3 + a.D4 * b.D4);
		}

		/// <summary>
		/// Implicit conversion from a 3x3 matrix to a 4x4 matrix.
		/// </summary>
		/// <param name="mat">3x3 matrix</param>
		/// <returns>4x4 matrix</returns>
		public static implicit operator Matrix4x4(Matrix3x3 mat) {
			Matrix4x4 m;
			m.A1 = mat.A1;
			m.A2 = mat.A2;
			m.A3 = mat.A3;
			m.A4 = 0.0f;

			m.B1 = mat.B1;
			m.B2 = mat.B2;
			m.B3 = mat.B3;
			m.B4 = 0.0f;

			m.C1 = mat.C1;
			m.C2 = mat.C2;
			m.C3 = mat.C3;
			m.C4 = 0.0f;

			m.D1 = 0.0f;
			m.D2 = 0.0f;
			m.D3 = 0.0f;
			m.D4 = 1.0f;

			return m;
		}

		/// <summary>
		/// Tests equality between this matrix and another.
		/// </summary>
		/// <param name="other">Other matrix to test</param>
		/// <returns>True if the matrices are equal, false otherwise</returns>
		public bool Equals(Matrix4x4 other) {
			return (((A1 == other.A1) && (A2 == other.A2) && (A3 == other.A3) && (A4 == other.A4))
				&& ((B1 == other.B1) && (B2 == other.B2) && (B3 == other.B3) && (B4 == other.B4))
				&& ((C1 == other.C1) && (C2 == other.C2) && (C3 == other.C3) && (C4 == other.C4))
				&& ((D1 == other.D1) && (D2 == other.D2) && (D3 == other.D3) && (D4 == other.D4)));
		}

		/// <summary>
		/// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
		/// </summary>
		/// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
		/// <returns>
		///   <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
		/// </returns>
		public override bool Equals(Object obj) {
			if(obj is Matrix4x4) {
				return Equals((Matrix4x4) obj);
			}
			return false;
		}

		/// <summary>
		/// Returns a hash code for this instance.
		/// </summary>
		/// <returns>
		/// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
		/// </returns>
		public override int GetHashCode() {
			return A1.GetHashCode() + A2.GetHashCode() + A3.GetHashCode() + A4.GetHashCode() + B1.GetHashCode() + B2.GetHashCode() + B3.GetHashCode() + B4.GetHashCode() +
				C1.GetHashCode() + C2.GetHashCode() + C3.GetHashCode() + C4.GetHashCode() + D1.GetHashCode() + D2.GetHashCode() + D3.GetHashCode() + D4.GetHashCode();
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public override String ToString() {
			CultureInfo info = CultureInfo.CurrentCulture;
			Object[] args = new object[] { A1.ToString(info), A2.ToString(info), A3.ToString(info), A4.ToString(info), 
				B1.ToString(info), B2.ToString(info), B3.ToString(info), B4.ToString(info),
				C1.ToString(info), C2.ToString(info), C3.ToString(info), C4.ToString(info), 
				D1.ToString(info), D2.ToString(info), D3.ToString(info), D4.ToString(info) };
			return String.Format(info, "{{[A1:{0} A2:{1} A3:{2} A4:{3}] [B1:{4} B2:{5} B3:{6} B4:{7}] [C1:{8} C2:{9} C3:{10} C4:{11}] [D1:{12} D2:{13} D3:{14} D4:{15}]}}", args);
		}
	}
}
