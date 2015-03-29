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
using System.Diagnostics;

namespace SharpDX.Direct3D11
{
    public partial class EffectMatrixVariable
    {
        private const string MatrixInvalidSize = "Invalid Matrix size: Must be 64 bytes, 16 floats";

        /// <summary>	
        /// Set a floating-point matrix.	
        /// </summary>	
        /// <param name="matrix"> A pointer to the first element in the matrix. </param>
        /// <returns>Returns one of the following {{Direct3D 10 Return Codes}}. </returns>
        /// <unmanaged>HRESULT ID3D11EffectMatrixVariable::SetMatrix([In] float* pData)</unmanaged>
        public void SetMatrix<T>(T matrix) where T : struct
        {
            SetMatrix(ref matrix);
        }

        /// <summary>	
        /// Get a matrix.	
        /// </summary>	
        /// <returns><para>A reference to the first element in a matrix.</para></returns>	
        /// <remarks>	
        /// Note??The DirectX SDK does not supply any compiled binaries for effects. You must use Effects 11 source to build  your effects-type application. For more information about using Effects 11 source, see Differences Between Effects 10 and Effects 11.	
        /// </remarks>	
        /// <unmanaged>HRESULT ID3DX11EffectMatrixVariable::GetMatrix([Out] SHARPDX_MATRIX* pData)</unmanaged>	
        public unsafe T GetMatrix<T>() where T : struct
        {
            T value;
            GetMatrix(out *(RawMatrix*)Interop.CastOut(out value));
            return value;
        }

        /// <summary>	
        /// Get a matrix.	
        /// </summary>	
        /// <returns><para>A reference to the first element in a matrix.</para></returns>	
        /// <remarks>	
        /// Note??The DirectX SDK does not supply any compiled binaries for effects. You must use Effects 11 source to build  your effects-type application. For more information about using Effects 11 source, see Differences Between Effects 10 and Effects 11.	
        /// </remarks>	
        /// <unmanaged>HRESULT ID3DX11EffectMatrixVariable::GetMatrix([Out] SHARPDX_MATRIX* pData)</unmanaged>	
        public RawMatrix GetMatrix()
        {
            RawMatrix value;
            GetMatrix(out value);
            return value;
        }

        /// <summary>	
        /// Set a floating-point matrix.	
        /// </summary>	
        /// <param name="matrix"> A pointer to the first element in the matrix. </param>
        /// <returns>Returns one of the following {{Direct3D 10 Return Codes}}. </returns>
        /// <unmanaged>HRESULT ID3D11EffectMatrixVariable::SetMatrix([In] float* pData)</unmanaged>
        public unsafe void SetMatrix<T>(ref T matrix) where T : struct
        {
            System.Diagnostics.Debug.Assert(Utilities.SizeOf<T>() <= 64, MatrixInvalidSize);
            SetMatrix(ref *(RawMatrix*)Interop.Fixed(ref matrix));
        }

        /// <summary>	
        /// Set an array of floating-point matrices.	
        /// </summary>	
        /// <param name="matrixArray"> A pointer to the first matrix. </param>
        /// <returns>Returns one of the following {{Direct3D 10 Return Codes}}. </returns>
        /// <unmanaged>HRESULT ID3D11EffectMatrixVariable::SetMatrixArray([In, Buffer] float* pData,[None] int Offset,[None] int Count)</unmanaged>
        public void SetMatrix<T>(T[] matrixArray) where T : struct
        {
            SetMatrix(matrixArray, 0);
        }

        /// <summary>	
        /// Set an array of floating-point matrices.	
        /// </summary>	
        /// <param name="matrixArray"> A pointer to the first matrix. </param>
        /// <param name="offset"> The number of matrix elements to skip from the start of the array. </param>
        /// <returns>Returns one of the following {{Direct3D 10 Return Codes}}. </returns>
        /// <unmanaged>HRESULT ID3D11EffectMatrixVariable::SetMatrixArray([In, Buffer] float* pData,[None] int Offset,[None] int Count)</unmanaged>
        public void SetMatrix<T>(T[] matrixArray, int offset) where T : struct
        {
            System.Diagnostics.Debug.Assert(Utilities.SizeOf<T>() == 64, MatrixInvalidSize);
            SetMatrixArray(Interop.CastArray<RawMatrix, T>(matrixArray), offset, matrixArray.Length);
        }

        /// <summary>	
        /// Get an array of matrices.	
        /// </summary>	
        /// <param name="count"> The number of matrices in the returned array. </param>
        /// <returns>Returns an array of matrix. </returns>
        /// <unmanaged>HRESULT ID3D11EffectMatrixVariable::GetMatrixArray([Out, Buffer] float* pData,[None] int Offset,[None] int Count)</unmanaged>
        public T[] GetMatrixArray<T>(int count) where T : struct
        {
            return GetMatrixArray<T>(0, count);
        }

        /// <summary>	
        /// Get an array of matrices.	
        /// </summary>	
        /// <param name="offset"> The offset (in number of matrices) between the start of the array and the first matrix returned. </param>
        /// <param name="count"> The number of matrices in the returned array. </param>
        /// <returns>Returns an array of matrix. </returns>
        /// <unmanaged>HRESULT ID3D11EffectMatrixVariable::GetMatrixArray([Out, Buffer] float* pData,[None] int Offset,[None] int Count)</unmanaged>
        public T[] GetMatrixArray<T>(int offset, int count) where T : struct
        {
            var temp = new T[count];
            GetMatrixArray(Interop.CastArray<RawMatrix, T>(temp), offset, count);
            return temp;
        }

        /// <summary>	
        /// Transpose and set a floating-point matrix.	
        /// </summary>	
        /// <remarks>	
        ///  Transposing a matrix will rearrange the data order from row-column order to column-row order (or vice versa). 	
        /// </remarks>	
        /// <param name="matrix"> A pointer to the first element of a matrix. </param>
        /// <returns>Returns one of the following {{Direct3D 10 Return Codes}}. </returns>
        /// <unmanaged>HRESULT ID3D11EffectMatrixVariable::SetMatrixTranspose([In] float* pData)</unmanaged>
        public void SetMatrixTranspose<T>(T matrix) where T : struct
        {
            SetMatrixTranspose(ref matrix);
        }

        /// <summary>	
        /// Transpose and set a floating-point matrix.	
        /// </summary>	
        /// <remarks>	
        ///  Transposing a matrix will rearrange the data order from row-column order to column-row order (or vice versa). 	
        /// </remarks>	
        /// <param name="matrix"> A pointer to the first element of a matrix. </param>
        /// <returns>Returns one of the following {{Direct3D 10 Return Codes}}. </returns>
        /// <unmanaged>HRESULT ID3D11EffectMatrixVariable::SetMatrixTranspose([In] float* pData)</unmanaged>
        public unsafe void SetMatrixTranspose<T>(ref T matrix) where T : struct
        {
            System.Diagnostics.Debug.Assert(Utilities.SizeOf<T>() <= 64, MatrixInvalidSize);
            SetMatrixTranspose(ref *(RawMatrix*)Interop.Cast(ref matrix));
        }

        /// <summary>	
        /// Transpose and set an array of floating-point matrices.	
        /// </summary>	
        /// <remarks>	
        ///  Transposing a matrix will rearrange the data order from row-column order to column-row order (or vice versa). 	
        /// </remarks>	
        /// <param name="matrixArray"> A pointer to an array of matrices. </param>
        /// <returns>Returns one of the following {{Direct3D 10 Return Codes}}. </returns>
        /// <unmanaged>HRESULT ID3D11EffectMatrixVariable::SetMatrixTransposeArray([In] float* pData,[None] int Offset,[None] int Count)</unmanaged>
        public void SetMatrixTranspose<T>(T[] matrixArray) where T : struct
        {
            SetMatrixTranspose(matrixArray, 0);
        }

        /// <summary>	
        /// Transpose and set an array of floating-point matrices.	
        /// </summary>	
        /// <remarks>	
        ///  Transposing a matrix will rearrange the data order from row-column order to column-row order (or vice versa). 	
        /// </remarks>	
        /// <param name="matrixArray"> A pointer to an array of matrices. </param>
        /// <param name="offset"> The offset (in number of matrices) between the start of the array and the first matrix to set. </param>
        /// <returns>Returns one of the following {{Direct3D 10 Return Codes}}. </returns>
        /// <unmanaged>HRESULT ID3D11EffectMatrixVariable::SetMatrixTransposeArray([In] float* pData,[None] int Offset,[None] int Count)</unmanaged>
        public void SetMatrixTranspose<T>(T[] matrixArray, int offset) where T : struct
        {
            SetMatrixTransposeArray(Interop.CastArray<RawMatrix, T>(matrixArray), offset, matrixArray.Length);
        }

        /// <summary>	
        /// Transpose and get a floating-point matrix.	
        /// </summary>	
        /// <returns>The transposed matrix.</returns>	
        /// <remarks>	
        /// Transposing a matrix will rearrange the data order from row-column order to column-row order (or vice versa).Note??The DirectX SDK does not supply any compiled binaries for effects. You must use Effects 11 source to build  your effects-type application. For more information about using Effects 11 source, see Differences Between Effects 10 and Effects 11.	
        /// </remarks>	
        /// <unmanaged>HRESULT ID3DX11EffectMatrixVariable::GetMatrixTranspose([Out] SHARPDX_MATRIX* pData)</unmanaged>	
        public unsafe T GetMatrixTranspose<T>() where T : struct
        {
            T value;
            GetMatrixTranspose(out *(RawMatrix*)Interop.CastOut(out value));
            return value;
        }

        /// <summary>	
        /// Transpose and get a floating-point matrix.	
        /// </summary>	
        /// <returns>The transposed matrix.</returns>	
        /// <remarks>	
        /// Transposing a matrix will rearrange the data order from row-column order to column-row order (or vice versa).Note??The DirectX SDK does not supply any compiled binaries for effects. You must use Effects 11 source to build  your effects-type application. For more information about using Effects 11 source, see Differences Between Effects 10 and Effects 11.	
        /// </remarks>	
        /// <unmanaged>HRESULT ID3DX11EffectMatrixVariable::GetMatrixTranspose([Out] SHARPDX_MATRIX* pData)</unmanaged>	
        public RawMatrix GetMatrixTranspose()
        {
            RawMatrix value;
            GetMatrixTranspose(out value);
            return value;
        }

        /// <summary>	
        /// Transpose and get an array of floating-point matrices.	
        /// </summary>	
        /// <remarks>	
        ///  Transposing a matrix will rearrange the data order from row-column order to column-row order (or vice versa). 	
        /// </remarks>	
        /// <param name="count"> The number of matrices in the array to get. </param>
        /// <returns>Returns an array of transposed <see cref="RawMatrix"/>. </returns>
        /// <unmanaged>HRESULT ID3D11EffectMatrixVariable::GetMatrixTransposeArray([Out, Buffer] float* pData,[None] int Offset,[None] int Count)</unmanaged>
        public T[] GetMatrixTransposeArray<T>(int count) where T : struct
        {
            return GetMatrixTransposeArray<T>(0, count);
        }

        /// <summary>	
        /// Transpose and get an array of floating-point matrices.	
        /// </summary>	
        /// <remarks>	
        ///  Transposing a matrix will rearrange the data order from row-column order to column-row order (or vice versa). 	
        /// </remarks>	
        /// <param name="offset"> The offset (in number of matrices) between the start of the array and the first matrix to get. </param>
        /// <param name="count"> The number of matrices in the array to get. </param>
        /// <returns>Returns an array of transposed <see cref="RawMatrix"/>. </returns>
        /// <unmanaged>HRESULT ID3D11EffectMatrixVariable::GetMatrixTransposeArray([Out, Buffer] float* pData,[None] int Offset,[None] int Count)</unmanaged>
        public T[] GetMatrixTransposeArray<T>(int offset, int count) where T : struct
        {
            var temp = new T[count];
            GetMatrixTransposeArray(Interop.CastArray<RawMatrix, T>(temp), offset, count);
            return temp;
        }
    }
}