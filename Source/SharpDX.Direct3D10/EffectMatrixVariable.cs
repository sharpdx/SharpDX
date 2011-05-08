// Copyright (c) 2010-2011 SharpDX - Alexandre Mutel
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
using SharpDX;

namespace SharpDX.Direct3D10
{
    public partial class EffectMatrixVariable
    {
        /// <summary>	
        /// Set a floating-point matrix.	
        /// </summary>	
        /// <param name="dataRef"> A pointer to the first element in the matrix. </param>
        /// <returns>Returns one of the following {{Direct3D 10 Return Codes}}. </returns>
        /// <unmanaged>HRESULT ID3D10EffectMatrixVariable::SetMatrix([In] float* pData)</unmanaged>
        public SharpDX.Result SetMatrix(SharpDX.Matrix dataRef)
        {
            return SetMatrix(ref dataRef);
        }

        /// <summary>	
        /// Set an array of floating-point matrices.	
        /// </summary>	
        /// <param name="matrixArray"> A pointer to the first matrix. </param>
        /// <returns>Returns one of the following {{Direct3D 10 Return Codes}}. </returns>
        /// <unmanaged>HRESULT ID3D10EffectMatrixVariable::SetMatrixArray([In, Buffer] float* pData,[None] int Offset,[None] int Count)</unmanaged>
        public SharpDX.Result SetMatrix(SharpDX.Matrix[] matrixArray)
        {
            return SetMatrix(matrixArray, 0);
        }

        /// <summary>	
        /// Set an array of floating-point matrices.	
        /// </summary>	
        /// <param name="matrixArray"> A pointer to the first matrix. </param>
        /// <param name="offset"> The number of matrix elements to skip from the start of the array. </param>
        /// <returns>Returns one of the following {{Direct3D 10 Return Codes}}. </returns>
        /// <unmanaged>HRESULT ID3D10EffectMatrixVariable::SetMatrixArray([In, Buffer] float* pData,[None] int Offset,[None] int Count)</unmanaged>
        public SharpDX.Result SetMatrix(SharpDX.Matrix[] matrixArray, int offset)
        {
            return SetMatrixArray(matrixArray, offset, matrixArray.Length);
        }

        /// <summary>	
        /// Get an array of matrices.	
        /// </summary>	
        /// <param name="count"> The number of matrices in the returned array. </param>
        /// <returns>Returns an array of matrix. </returns>
        /// <unmanaged>HRESULT ID3D10EffectMatrixVariable::GetMatrixArray([Out, Buffer] float* pData,[None] int Offset,[None] int Count)</unmanaged>
        public SharpDX.Matrix[] GetMatrixArray(int count)
        {
            return GetMatrixArray(0, count);
        }

        /// <summary>	
        /// Get an array of matrices.	
        /// </summary>	
        /// <param name="offset"> The offset (in number of matrices) between the start of the array and the first matrix returned. </param>
        /// <param name="count"> The number of matrices in the returned array. </param>
        /// <returns>Returns an array of matrix. </returns>
        /// <unmanaged>HRESULT ID3D10EffectMatrixVariable::GetMatrixArray([Out, Buffer] float* pData,[None] int Offset,[None] int Count)</unmanaged>
        public SharpDX.Matrix[] GetMatrixArray(int offset, int count)
        {
            var temp = new SharpDX.Matrix[count];
            GetMatrixArray(temp, offset, count);
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
        /// <unmanaged>HRESULT ID3D10EffectMatrixVariable::SetMatrixTranspose([In] float* pData)</unmanaged>
        public SharpDX.Result SetMatrixTranspose(SharpDX.Matrix matrix)
        {
            return SetMatrixTranspose(ref matrix);
        }

        /// <summary>	
        /// Transpose and set an array of floating-point matrices.	
        /// </summary>	
        /// <remarks>	
        ///  Transposing a matrix will rearrange the data order from row-column order to column-row order (or vice versa). 	
        /// </remarks>	
        /// <param name="matrixArray"> A pointer to an array of matrices. </param>
        /// <returns>Returns one of the following {{Direct3D 10 Return Codes}}. </returns>
        /// <unmanaged>HRESULT ID3D10EffectMatrixVariable::SetMatrixTransposeArray([In] float* pData,[None] int Offset,[None] int Count)</unmanaged>
        public SharpDX.Result SetMatrixTranspose(SharpDX.Matrix[] matrixArray)
        {
            return SetMatrixTranspose(matrixArray, 0);
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
        /// <unmanaged>HRESULT ID3D10EffectMatrixVariable::SetMatrixTransposeArray([In] float* pData,[None] int Offset,[None] int Count)</unmanaged>
        public SharpDX.Result SetMatrixTranspose(SharpDX.Matrix[] matrixArray, int offset)
        {
            return SetMatrixTransposeArray(matrixArray, offset, matrixArray.Length);
        }

        /// <summary>	
        /// Transpose and get an array of floating-point matrices.	
        /// </summary>	
        /// <remarks>	
        ///  Transposing a matrix will rearrange the data order from row-column order to column-row order (or vice versa). 	
        /// </remarks>	
        /// <param name="count"> The number of matrices in the array to get. </param>
        /// <returns>Returns an array of transposed <see cref="SharpDX.Matrix"/>. </returns>
        /// <unmanaged>HRESULT ID3D10EffectMatrixVariable::GetMatrixTransposeArray([Out, Buffer] float* pData,[None] int Offset,[None] int Count)</unmanaged>
        public SharpDX.Matrix[] GetMatrixTransposeArray(int count)
        {
            return GetMatrixTransposeArray(0, count);
        }   

        /// <summary>	
        /// Transpose and get an array of floating-point matrices.	
        /// </summary>	
        /// <remarks>	
        ///  Transposing a matrix will rearrange the data order from row-column order to column-row order (or vice versa). 	
        /// </remarks>	
        /// <param name="offset"> The offset (in number of matrices) between the start of the array and the first matrix to get. </param>
        /// <param name="count"> The number of matrices in the array to get. </param>
        /// <returns>Returns an array of transposed <see cref="SharpDX.Matrix"/>. </returns>
        /// <unmanaged>HRESULT ID3D10EffectMatrixVariable::GetMatrixTransposeArray([Out, Buffer] float* pData,[None] int Offset,[None] int Count)</unmanaged>
        public SharpDX.Matrix[]  GetMatrixTransposeArray(int offset, int count)
        {
            SharpDX.Matrix[] temp = new Matrix[count];
            GetMatrixTransposeArray(temp, offset, count);
            return temp;
        }        
    }
}