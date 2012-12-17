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
	/// Represents a 3x3 matrix. Assimp docs say their matrices are always row-major,
	/// and it looks like they're only describing the memory layout. Matrices are treated
	/// as column vectors however (X base in the first column, Y base the second, and Z base the third)
	/// </summary>
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	internal struct Matrix3x3 : IEquatable<Matrix3x3> {
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

		private static Matrix3x3 _identity = new Matrix3x3(1.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f, 1.0f);

		/// <summary>
		/// Gets the identity matrix.
		/// </summary>
		public static Matrix3x3 Identity {
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
				B1 <= epsilon && B1 >= -epsilon &&
				B3 <= epsilon && B3 >= -epsilon &&
				C1 <= epsilon && C1 >= -epsilon &&
				C2 <= epsilon && C2 >= -epsilon &&
				A1 <= 1.0f + epsilon && A1 >= 1.0f - epsilon && 
				B2 <= 1.0f + epsilon && B2 >= 1.0f - epsilon && 
				C3 <= 1.0f + epsilon && C3 >= 1.0f - epsilon);
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
						}
						break;
				}
			}
		}

		/// <summary>
		/// Constructs a new Matrix3x3.
		/// </summary>
		/// <param name="a1">Element at row 1, column 1</param>
		/// <param name="a2">Element at row 1, column 2</param>
		/// <param name="a3">Element at row 1, column 3</param>
		/// <param name="b1">Element at row 2, column 1</param>
		/// <param name="b2">Element at row 2, column 2</param>
		/// <param name="b3">Element at row 2, column 3</param>
		/// <param name="c1">Element at row 3, column 1</param>
		/// <param name="c2">Element at row 3, column 2</param>
		/// <param name="c3">Element at row 3, column 3</param>
		public Matrix3x3(float a1, float a2, float a3, float b1, float b2, float b3, float c1, float c2, float c3) {
				this.A1 = a1;
				this.A2 = a2;
				this.A3 = a3;
				this.B1 = b1;
				this.B2 = b2;
				this.B3 = b3;
				this.C1 = c1;
				this.C2 = c2;
				this.C3 = c3;
		}

		/// <summary>
		/// Constructs a new Matrix3x3.
		/// </summary>
		/// <param name="rotMatrix">A 4x4 matrix to construct from, only taking the rotation/scaling part.</param>
		public Matrix3x3(Matrix4x4 rotMatrix) {
			this.A1 = rotMatrix.A1;
			this.A2 = rotMatrix.A2;
			this.A3 = rotMatrix.A3;

			this.B1 = rotMatrix.B1;
			this.B2 = rotMatrix.B2;
			this.B3 = rotMatrix.B3;

			this.C1 = rotMatrix.C1;
			this.C2 = rotMatrix.C2;
			this.C3 = rotMatrix.C3;
		}

		/// <summary>
		/// Transposes this matrix (rows become columns, vice versa).
		/// </summary>
		public void Transpose() {
			Matrix3x3 m = new Matrix3x3(this);

			A2 = m.B1;
			A3 = m.C1;

			B1 = m.A2;
			B3 = m.C2;

			C1 = m.A3;
			C2 = m.B3;
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

				B1 = float.NaN;
				B2 = float.NaN;
				B3 = float.NaN;

				C1 = float.NaN;
				C2 = float.NaN;
				C3 = float.NaN;
			}

			float invDet = 1.0f / det;

			float a1 = invDet  * (B2 * C3 - B3 * C2);
			float a2 = -invDet * (A2 * C3 - A3 * C2);
			float a3 = invDet  * (A2 * B3 - A3 * B2);

			float b1 = -invDet * (B1 * C3 - B3 * C1);
			float b2 = invDet  * (A1 * C3 - A3 * C1);
			float b3 = -invDet * (A1 * B3 - A3 * B1);

			float c1 = invDet  * (B1 * C2 - B2 * C1);
			float c2 = -invDet * (A1 * C2 - A2 * C1);
			float c3 = invDet  * (A1 * B2 - A2 * B1);

			A1 = a1;
			A2 = a2;
			A3 = a3;

			B1 = b1;
			B2 = b2;
			B3 = b3;

			C1 = c1;
			C2 = c2;
			C3 = c3;
		}

		/// <summary>
		/// Compute the determinant of this matrix.
		/// </summary>
		/// <returns>The determinant</returns>
		public float Determinant() {
			return A1*B2*C3 - A1*B3*C2 + A2*B3*C1 - A2*B1*C3 + A3*B1*C2 - A3*B2*C1;
		}

		/// <summary>
		/// Creates a rotation matrix from a set of euler angles.
		/// </summary>
		/// <param name="x">Rotation angle about the x-axis, in radians.</param>
		/// <param name="y">Rotation angle about the y-axis, in radians.</param>
		/// <param name="z">Rotation angle about the z-axis, in radians.</param>
		/// <returns>The rotation matrix</returns>
		public static Matrix3x3 FromEulerAnglesXYZ(float x, float y, float z) {
			float cr = (float) Math.Cos(x);
			float sr = (float) Math.Sin(x);
			float cp = (float) Math.Cos(y);
			float sp = (float) Math.Sin(y);
			float cy = (float) Math.Cos(z);
			float sy = (float) Math.Sin(z);

			float srsp = sr * sp;
			float crsp = cr * sp;

			Matrix3x3 m;
			m.A1 = cp * cy;
			m.A2 = cp * sy;
			m.A3 = -sp;

			m.B1 = srsp * cy - cr * sy;
			m.B2 = srsp * sy + cr * cy;
			m.B3 = sr * cp;

			m.C1 = crsp * cy + sr * sy;
			m.C2 = crsp * sy - sr * cy;
			m.C3 = cr * cp;

			return m;
		}

		/// <summary>
		/// Creates a rotation matrix from a set of euler angles.
		/// </summary>
		/// <param name="angles">Vector containing the rotation angles about the x, y, z axes, in radians.</param>
		/// <returns>The rotation matrix</returns>
		public static Matrix3x3 FromEulerAnglesXYZ(Vector3D angles) {
			return Matrix3x3.FromEulerAnglesXYZ(angles.X, angles.Y, angles.Z);
		}

		/// <summary>
		/// Creates a rotation matrix for a rotation about the x-axis.
		/// </summary>
		/// <param name="radians">Rotation angle in radians.</param>
		/// <returns>The rotation matrix</returns>
		public static Matrix3x3 FromRotationX(float radians) {
			/*
				 |  1  0       0      |
			 M = |  0  cos(A) -sin(A) |
				 |  0  sin(A)  cos(A) |	
			*/
			Matrix3x3 m = Identity;
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
		public static Matrix3x3 FromRotationY(float radians) {
			/*
				 |  cos(A)  0   sin(A) |
			 M = |  0       1   0      |
				 | -sin(A)  0   cos(A) |
			*/
			Matrix3x3 m = Identity;
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
		public static Matrix3x3 FromRotationZ(float radians) {
			/*
				 |  cos(A)  -sin(A)   0 |
			 M = |  sin(A)   cos(A)   0 |
				 |  0        0        1 |
			 */
			Matrix3x3 m = Identity;
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
		public static Matrix3x3 FromAngleAxis(float radians, Vector3D axis) {
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

			Matrix3x3 m;
			m.A1 = xx + (cos * (1.0f - xx));
			m.B1 = (xy - (cos * xy)) + (sin * z);
			m.C1 = (xz - (cos * xz)) - (sin * y);

			m.A2 = (xy - (cos * xy)) - (sin * z);
			m.B2 = yy + (cos * (1.0f - yy));
			m.C2 = (yz - (cos * yz)) + (sin * x);

			m.A3 = (xz - (cos * xz)) + (sin * y);
			m.B3 = (yz - (cos * yz)) - (sin * x);
			m.C3 = zz + (cos * (1.0f - zz));

			return m;
		}

		/// <summary>
		/// Creates a scaling matrix.
		/// </summary>
		/// <param name="scaling">Scaling vector</param>
		/// <returns>The scaling vector</returns>
		public static Matrix3x3 FromScaling(Vector3D scaling) {
			Matrix3x3 m = Identity;
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
		public static Matrix3x3 FromToMatrix(Vector3D from, Vector3D to) {
			float e = Vector3D.Dot(from, to);
			float f = (e < 0) ? -e : e;

			Matrix3x3 m = Identity;

			//"from" and "to" vectors almost parallel
			if(f > 1.0f -  0.00001f) {
				Vector3D u, v; //Temp variables
				Vector3D x; //Vector almost orthogonal to "from"

				x.X = (from.X > 0.0f) ? from.X : -from.X;
				x.Y = (from.Y > 0.0f) ? from.Y : -from.Y;
				x.Z = (from.Z > 0.0f) ? from.Z : -from.Z;

				if(x.X < x.Y) {
					if(x.X < x.Z) {
						x.X = 1.0f;
						x.Y = 0.0f;
						x.Z = 0.0f;
					} else {
						x.X = 0.0f;
						x.Y = 0.0f;
						x.Z = 1.0f;
					}
				} else {
					if(x.Y < x.Z) {
						x.X = 0.0f;
						x.Y = 1.0f;
						x.Z = 0.0f;
					} else {
						x.X = 0.0f;
						x.Y = 0.0f;
						x.Z = 1.0f;
					}
				}

				u.X = x.X - from.X;
				u.Y = x.Y - from.Y;
				u.Z = x.Z - from.Z;

				v.X = x.X - to.X;
				v.Y = x.Y - to.Y;
				v.Z = x.Z - to.Z;

				float c1 = 2.0f / Vector3D.Dot(u, u);
				float c2 = 2.0f / Vector3D.Dot(v, v);
				float c3 = c1 * c2 * Vector3D.Dot(u, v);

				for(int i = 1; i < 4; i++) {
					for(int j = 1; j < 4; j++) {
						//This is somewhat unreadable, but the indices for u, v vectors are "zero-based" while
						//matrix indices are "one-based" always subtract by one to index those
						m[i, j] = -c1 * u[i-1] * u[j-1] - c2 * v[i-1] * v[j-1] + c3 * v[i-1] * u[j-1];
					}
					m[i, i] += 1.0f;
				}

			} else {
				//Most common case, unless "from" = "to" or "from" =- "to"
				Vector3D v = Vector3D.Cross(from, to);

				//Hand optimized version (9 mults less) by Gottfried Chen
				float h = 1.0f / (1.0f + e);
				float hvx = h * v.X;
				float hvz = h * v.Z;
				float hvxy = hvx * v.Y;
				float hvxz = hvx * v.Z;
				float hvyz = hvz * v.Y;

				m.A1 = e + hvx * v.X;
				m.A2 = hvxy - v.Z;
				m.A3 = hvxz + v.Y;

				m.B1 = hvxy + v.Z;
				m.B2 = e + h * v.Y * v.Y;
				m.B3 = hvyz - v.X;

				m.C1 = hvxz - v.Y;
				m.C2 = hvyz + v.X;
				m.C3 = e + hvz * v.Z;
			}

			return m;
		}

		/// <summary>
		/// Tests equality between two matrices.
		/// </summary>
		/// <param name="a">First matrix</param>
		/// <param name="b">Second matrix</param>
		/// <returns>True if the matrices are equal, false otherwise</returns>
		public static bool operator==(Matrix3x3 a, Matrix3x3 b) {
			return (((a.A1 == b.A1) && (a.A2 == b.A2) && (a.A3 == b.A3))
				&& ((a.B1 == b.B1) && (a.B2 == b.B2) && (a.B3 == b.B3))
				&& ((a.C1 == b.C1) && (a.C2 == b.C2) && (a.C3 == b.C3)));
		}

		/// <summary>
		/// Tests inequality between two matrices.
		/// </summary>
		/// <param name="a">First matrix</param>
		/// <param name="b">Second matrix</param>
		/// <returns>True if the matrices are not equal, false otherwise</returns>
		public static bool operator!=(Matrix3x3 a, Matrix3x3 b) {
			return (((a.A1 != b.A1) || (a.A2 != b.A2) || (a.A3 != b.A3))
				|| ((a.B1 != b.B1) || (a.B2 != b.B2) || (a.B3 != b.B3))
				|| ((a.C1 != b.C1) || (a.C2 != b.C2) || (a.C3 != b.C3)));
		}


		/// <summary>
		/// Performs matrix multiplication.Multiplication order is B x A. That way, SRT concatenations
		/// are left to right.
		/// </summary>
		/// <param name="a">First matrix</param>
		/// <param name="b">Second matrix</param>
		/// <returns>Multiplied matrix</returns>
		public static Matrix3x3 operator*(Matrix3x3 a, Matrix3x3 b) {
			return new Matrix3x3(a.A1 * b.A1 + a.B1 * b.A2 + a.C1 * b.A3,
								 a.A2 * b.A1 + a.B2 * b.A2 + a.C2 * b.A3,
								 a.A3 * b.A1 + a.B3 * b.A2 + a.C3 * b.A3,
								 a.A1 * b.B1 + a.B1 * b.B2 + a.C1 * b.B3,
								 a.A2 * b.B1 + a.B2 * b.B2 + a.C2 * b.B3,
								 a.A3 * b.B1 + a.B3 * b.B2 + a.C3 * b.B3,
								 a.A1 * b.C1 + a.B1 * b.C2 + a.C1 * b.C3,
								 a.A2 * b.C1 + a.B2 * b.C2 + a.C2 * b.C3,
								 a.A3 * b.C1 + a.B3 * b.C2 + a.C3 * b.C3);
		}

		/// <summary>
		/// Implicit conversion from a 4x4 matrix to a 3x3 matrix.
		/// </summary>
		/// <param name="mat">4x4 matrix</param>
		/// <returns>3x3 matrix</returns>
		public static implicit operator Matrix3x3(Matrix4x4 mat) {
			Matrix3x3 m;
			m.A1 = mat.A1;
			m.A2 = mat.A2;
			m.A3 = mat.A3;

			m.B1 = mat.B1;
			m.B2 = mat.B2;
			m.B3 = mat.B3;

			m.C1 = mat.C1;
			m.C2 = mat.C2;
			m.C3 = mat.C3;
			return m;
		}

		/// <summary>
		/// Tests equality between this matrix and another.
		/// </summary>
		/// <param name="other">Other matrix to test</param>
		/// <returns>True if the matrices are equal, false otherwise</returns>
		public bool Equals(Matrix3x3 other) {
			return (((A1 == other.A1) && (A2 == other.A2) && (A3 == other.A3))
				&& ((B1 == other.B1) && (B2 == other.B2) && (B3 == other.B3))
				&& ((C1 == other.C1) && (C2 == other.C2) && (C3 == other.C3)));
		}

		/// <summary>
		/// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
		/// </summary>
		/// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
		/// <returns>
		///   <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
		/// </returns>
		public override bool Equals(Object obj) {
			if(obj is Matrix3x3) {
				return Equals((Matrix3x3) obj);
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
			return A1.GetHashCode() + A2.GetHashCode() + A3.GetHashCode() + B1.GetHashCode() + B2.GetHashCode() + B3.GetHashCode() +
				C1.GetHashCode() + C2.GetHashCode() + C3.GetHashCode();
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public override String ToString() {
			CultureInfo info = CultureInfo.CurrentCulture;
			Object[] args = new object[] { A1.ToString(info), A2.ToString(info), A3.ToString(info),
				B1.ToString(info), B2.ToString(info), B3.ToString(info),
				C1.ToString(info), C2.ToString(info), C3.ToString(info)};
			return String.Format(info, "{{[A1:{0} A2:{1} A3:{2}] [B1:{3} B2:{4} B3:{5}] [C1:{6} C2:{7} C3:{8}]}}", args);
		}
	}
}
