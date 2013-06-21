// Copyright (c) 2010-2013 SharpDX - Alexandre Mutel
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

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// A parameter of an effect.
    /// </summary>
    /// <remarks>
    /// A parameter can be a value type that will be set to a constant buffer, or a resource type (SRV, UAV, SamplerState).
    /// </remarks>
    public sealed class EffectParameter : ComponentBase
    {
        internal readonly EffectData.Parameter ParameterDescription;
        internal readonly EffectConstantBuffer buffer;
        private readonly EffectResourceLinker resourceLinker;
        private readonly GetMatrixDelegate GetMatrixImpl;
        private readonly CopyMatrixDelegate CopyMatrix;
        private int offset;

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectParameter"/> class.
        /// </summary>
        internal EffectParameter(EffectData.ValueTypeParameter parameterDescription, EffectConstantBuffer buffer)
            : base(parameterDescription.Name)
        {
            this.ParameterDescription = parameterDescription;
            this.buffer = buffer;

            ResourceType = EffectResourceType.None;
            IsValueType = true;
            ParameterClass = parameterDescription.Class;
            ParameterType = parameterDescription.Type;
            RowCount = parameterDescription.RowCount;
            ColumnCount = parameterDescription.ColumnCount;
            ElementCount = parameterDescription.Count;
            Offset = parameterDescription.Offset;
            Size = parameterDescription.Size;

            // If the expecting Matrix is column_major or the expected size is != from Matrix, than we need to remap SharpDX.Matrix to it.
            if (ParameterClass == EffectParameterClass.MatrixRows || ParameterClass == EffectParameterClass.MatrixColumns)
            {
                var isMatrixToMap = parameterDescription.Size != Interop.SizeOf<Matrix>() || ParameterClass == EffectParameterClass.MatrixColumns;
                // Use the correct function for this parameter
                CopyMatrix = isMatrixToMap ? (ParameterClass == EffectParameterClass.MatrixRows) ? new CopyMatrixDelegate(CopyMatrixRowMajor) : CopyMatrixColumnMajor : CopyMatrixDirect;
                GetMatrixImpl = isMatrixToMap ? (ParameterClass == EffectParameterClass.MatrixRows) ? new GetMatrixDelegate(GetMatrixRowMajorFrom) : GetMatrixColumnMajorFrom : GetMatrixDirectFrom;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectParameter"/> class.
        /// </summary>
        internal EffectParameter(EffectData.ResourceParameter parameterDescription, EffectResourceType resourceType, int offset, EffectResourceLinker resourceLinker)
            : base(parameterDescription.Name)
        {
            this.ParameterDescription = parameterDescription;
            this.resourceLinker = resourceLinker;

            ResourceType = resourceType;
            IsValueType = false;
            ParameterClass = parameterDescription.Class;
            ParameterType = parameterDescription.Type;
            RowCount = ColumnCount = 0;
            ElementCount = parameterDescription.Count;
            Offset = offset;
        }

        /// <summary>
        /// A unique index of this parameter instance inside the <see cref="EffectParameterCollection"/> of an effect. See remarks.
        /// </summary>
        /// <remarks>
        /// This unique index can be used between different instance of the effect with different deferred <see cref="GraphicsDevice"/>.
        /// </remarks>
        public int Index { get; internal set; }

        /// <summary>
        /// Gets the parameter class.
        /// </summary>
        /// <value>The parameter class.</value>
        public readonly EffectParameterClass ParameterClass;

        /// <summary>
        /// Gets the resource type.
        /// </summary>
        public readonly EffectResourceType ResourceType;

        /// <summary>
        /// Gets the type of the parameter.
        /// </summary>
        /// <value>The type of the parameter.</value>
        public readonly EffectParameterType ParameterType;

        /// <summary>
        /// Gets a boolean indicating if this parameter is a value type (true) or a resource type (false).
        /// </summary>
        public readonly bool IsValueType;

        /// <summary>	
        /// Number of rows in a matrix. Otherwise a numeric type returns 1, any other type returns 0. 	
        /// </summary>	
        /// <unmanaged>int Rows</unmanaged>
        public readonly int RowCount;

        /// <summary>	
        /// Number of columns in a matrix. Otherwise a numeric type returns 1, any other type returns 0. 	
        /// </summary>	
        /// <unmanaged>int Columns</unmanaged>
        public readonly int ColumnCount;

        /// <summary>
        /// Gets the collection of effect parameters.
        /// </summary>
        public readonly int ElementCount;

        /// <summary>
        /// Size in bytes of the element, only valid for value types.
        /// </summary>
        public readonly int Size;

        /// <summary>
        /// Offset of this parameter.
        /// </summary>
        /// <remarks>
        /// For a value type, this offset is the offset in bytes inside the constant buffer.
        /// For a resource type, this offset is an index to the resource linker.
        /// </remarks>
        public int Offset
        {
            get
            {
                return offset;
            }

            internal set
            {
                offset = value;
            }
        }

        /// <summary>
        /// Gets a single value to the associated parameter in the constant buffer.
        /// </summary>
        /// <typeparam name="T">The type of the value to read from the buffer.</typeparam>
        /// <returns>The value of this parameter.</returns>
        public T GetValue<T>() where T : struct
        {
            return buffer.Get<T>(offset);
        }

        /// <summary>
        /// Gets an array of values to the associated parameter in the constant buffer.
        /// </summary>
        /// <typeparam name = "T">The type of the value to read from the buffer.</typeparam>
        /// <returns>The value of this parameter.</returns>
        public T[] GetValueArray<T>(int count) where T : struct
        {
            return buffer.GetRange<T>(offset, count);
        }

        /// <summary>
        /// Gets a single value to the associated parameter in the constant buffer.
        /// </summary>
        /// <returns>The value of this parameter.</returns>
        public Matrix GetMatrix()
        {
            return GetMatrixImpl(offset);
        }

        /// <summary>
        /// Gets a single value to the associated parameter in the constant buffer.
        /// </summary>
        /// <returns>The value of this parameter.</returns>
        public Matrix GetMatrix(int startIndex)
        {
            return GetMatrixImpl(offset + (startIndex << 6));
        }

        /// <summary>
        /// Gets an array of matrices to the associated parameter in the constant buffer.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <returns>Matrix[][].</returns>
        /// <returns>The value of this parameter.</returns>
        public Matrix[] GetMatrixArray(int count)
        {
            return GetMatrixArray(0, count);
        }

        /// <summary>
        /// Gets an array of matrices to the associated parameter in the constant buffer.
        /// </summary>
        /// <returns>The value of this parameter.</returns>
        public unsafe Matrix[] GetMatrixArray(int startIndex, int count)
        {
            var result = new Matrix[count];
            var localOffset = offset + (startIndex << 6);
            // Fix the whole buffer
            fixed (Matrix* pMatrix = result)
            {
                for (int i = 0; i < result.Length; i++, localOffset += Utilities.SizeOf<Matrix>())
                    pMatrix[i] = GetMatrixImpl(localOffset);
            }
            buffer.IsDirty = true;
            return result;
        }

        /// <summary>
        /// Sets a single value to the associated parameter in the constant buffer.
        /// </summary>
        /// <typeparam name = "T">The type of the value to be written to the buffer.</typeparam>
        /// <param name = "value">The value to write to the buffer.</param>
        public void SetValue<T>(ref T value) where T : struct
        {
            buffer.Set(offset, ref value);
            buffer.IsDirty = true;
        }

        /// <summary>
        /// Sets a single value to the associated parameter in the constant buffer.
        /// </summary>
        /// <typeparam name = "T">The type of the value to be written to the buffer.</typeparam>
        /// <param name = "value">The value to write to the buffer.</param>
        public void SetValue<T>(T value) where T : struct
        {
            buffer.Set(offset, value);
            buffer.IsDirty = true;
        }

        /// <summary>
        /// Sets a single matrix value to the associated parameter in the constant buffer.
        /// </summary>
        /// <param name = "value">The matrix to write to the buffer.</param>
        public void SetValue(ref Matrix value)
        {
            CopyMatrix(ref value, offset);
            buffer.IsDirty = true;
        }

        /// <summary>
        /// Sets a single matrix value to the associated parameter in the constant buffer.
        /// </summary>
        /// <param name = "value">The matrix to write to the buffer.</param>
        public void SetValue(Matrix value)
        {
            CopyMatrix(ref value, offset);
            buffer.IsDirty = true;
        }

        /// <summary>
        /// Sets an array of matrices to the associated parameter in the constant buffer.
        /// </summary>
        /// <param name = "values">An array of matrices to be written to the current buffer.</param>
        public unsafe void SetValue(Matrix[] values)
        {
            var localOffset = offset;
            // Fix the whole buffer
            fixed (Matrix* pMatrix = values)
            {
                for (int i = 0; i < values.Length; i++, localOffset += Utilities.SizeOf<Matrix>())
                    CopyMatrix(ref pMatrix[i], localOffset);
            }
            buffer.IsDirty = true;
        }

        /// <summary>
        /// Sets a single matrix at the specified index for the associated parameter in the constant buffer.
        /// </summary>
        /// <param name="index">Index of the matrix to write in element count.</param>
        /// <param name = "value">The matrix to write to the buffer.</param>
        public void SetValue(int index, Matrix value) 
        {
            CopyMatrix(ref value, offset + (index << 6));
            buffer.IsDirty = true;
        }

        /// <summary>
        /// Sets an array of matrices to at the specified index for the associated parameter in the constant buffer.
        /// </summary>
        /// <param name="index">Index of the matrix to write in element count.</param>
        /// <param name = "values">An array of matrices to be written to the current buffer.</param>
        public unsafe void SetValue(int index, Matrix[] values) 
        {
            var localOffset = this.offset + (index << 6);
            // Fix the whole buffer
            fixed (Matrix* pMatrix = values)
            {
                for (int i = 0; i < values.Length; i++, localOffset += Utilities.SizeOf<Matrix>())
                    CopyMatrix(ref pMatrix[i], localOffset);
            }
            buffer.IsDirty = true;
        }

        /// <summary>
        /// Sets an array of values to the associated parameter in the constant buffer.
        /// </summary>
        /// <typeparam name = "T">The type of the value to be written to the buffer.</typeparam>
        /// <param name = "values">An array of values to be written to the current buffer.</param>
        public void SetValue<T>(T[] values) where T : struct
        {
            buffer.Set(offset, values);
            buffer.IsDirty = true;
        }

        /// <summary>
        /// Sets a single value at the specified index for the associated parameter in the constant buffer.
        /// </summary>
        /// <typeparam name = "T">The type of the value to be written to the buffer.</typeparam>
        /// <param name="index">Index of the value to write in typeof(T) element count.</param>
        /// <param name = "value">The value to write to the buffer.</param>
        public void SetValue<T>(int index, ref T value) where T : struct
        {
            buffer.Set(offset + Interop.SizeOf<T>() * index, ref value);
            buffer.IsDirty = true;
        }

        /// <summary>
        /// Sets a single value at the specified index for the associated parameter in the constant buffer.
        /// </summary>
        /// <typeparam name = "T">The type of the value to be written to the buffer.</typeparam>
        /// <param name="index">Index of the value to write in typeof(T) element count.</param>
        /// <param name = "value">The value to write to the buffer.</param>
        public void SetValue<T>(int index, T value) where T : struct
        {
            buffer.Set(offset + Interop.SizeOf<T>() * index, value);
            buffer.IsDirty = true;
        }

        /// <summary>
        /// Sets an array of values to at the specified index for the associated parameter in the constant buffer.
        /// </summary>
        /// <typeparam name = "T">The type of the value to be written to the buffer.</typeparam>
        /// <param name="index">Index of the value to write in typeof(T) element count.</param>
        /// <param name = "values">An array of values to be written to the current buffer.</param>
        public void SetValue<T>(int index, T[] values) where T : struct
        {
            buffer.Set(offset + Interop.SizeOf<T>() * index, values);
            buffer.IsDirty = true;
        }

        /// <summary>
        /// Gets the resource view set for this parameter.
        /// </summary>
        /// <typeparam name = "T">The type of the resource view.</typeparam>
        /// <returns>The resource view.</returns>
        public T GetResource<T>() where T : class
        {
            return resourceLinker.GetResource<T>(offset);
        }

        /// <summary>
        /// Sets a shader resource for the associated parameter.
        /// </summary>
        /// <typeparam name = "T">The type of the resource view.</typeparam>
        /// <param name="resourceView">The resource view.</param>
        public void SetResource<T>(T resourceView) where T : class
        {
            resourceLinker.SetResource(offset, ResourceType, resourceView);
        }

        /// <summary>
        /// Sets a shader resource for the associated parameter.
        /// </summary>
        /// <param name="resourceView">The resource.</param>
        /// <param name="initialUAVCount">The initial count for the UAV (-1) to keep it</param>
        public void SetResource(Direct3D11.UnorderedAccessView resourceView, int initialUAVCount)
        {
            resourceLinker.SetResource(offset, ResourceType, resourceView, initialUAVCount);
        }

        /// <summary>
        /// Direct access to the resource pointer in order to 
        /// </summary>
        /// <param name="resourcePointer"></param>
        internal void SetResourcePointer(IntPtr resourcePointer) 
        {
            resourceLinker.SetResourcePointer(offset, ResourceType, resourcePointer);
        }

        /// <summary>
        /// Sets a an array of shader resource views for the associated parameter.
        /// </summary>
        /// <typeparam name = "T">The type of the resource view.</typeparam>
        /// <param name="resourceViewArray">The resource view array.</param>
        public void SetResource<T>(params T[] resourceViewArray) where T : class
        {
            resourceLinker.SetResource(offset, ResourceType, resourceViewArray);
        }

        /// <summary>
        /// Sets a an array of shader resource views for the associated parameter.
        /// </summary>
        /// <param name="resourceViewArray">The resource view array.</param>
        /// <param name="uavCounts">Sets the initial uavCount</param>
        public void SetResource(Direct3D11.UnorderedAccessView[] resourceViewArray, int[] uavCounts)
        {
            resourceLinker.SetResource(offset, ResourceType, resourceViewArray, uavCounts);
        }

        /// <summary>
        /// Sets a shader resource at the specified index for the associated parameter.
        /// </summary>
        /// <typeparam name = "T">The type of the resource view.</typeparam>
        /// <param name="index">Index to start to set the resource views</param>
        /// <param name="resourceView">The resource view.</param>
        public void SetResource<T>(int index, T resourceView) where T : class
        {
            resourceLinker.SetResource(offset + index, ResourceType, resourceView);
        }

        /// <summary>
        /// Sets a an array of shader resource views at the specified index for the associated parameter.
        /// </summary>
        /// <typeparam name = "T">The type of the resource view.</typeparam>
        /// <param name="index">Index to start to set the resource views</param>
        /// <param name="resourceViewArray">The resource view array.</param>
        public void SetResource<T>(int index, params T[] resourceViewArray) where T : class
        {
            resourceLinker.SetResource(offset + index, ResourceType, resourceViewArray);
        }

        /// <summary>
        /// Sets a an array of shader resource views at the specified index for the associated parameter.
        /// </summary>
        /// <param name="index">Index to start to set the resource views</param>
        /// <param name="resourceViewArray">The resource view array.</param>
        /// <param name="uavCount">Sets the initial uavCount</param>
        public void SetResource(int index, Direct3D11.UnorderedAccessView[] resourceViewArray, int[] uavCount)
        {
            resourceLinker.SetResource(offset + index, ResourceType, resourceViewArray, uavCount);
        }

        internal void SetDefaultValue()
        {
            if (IsValueType)
            {
                var defaultValue = ((EffectData.ValueTypeParameter) ParameterDescription).DefaultValue;
                if (defaultValue != null)
                {
                    SetValue(defaultValue);
                }
            }
        }

        public override string ToString()
        {
            return string.Format("[{0}] {1} Class: {2}, Resource: {3}, Type: {4}, IsValue: {5}, RowCount: {6}, ColumnCount: {7}, ElementCount: {8} Offset: {9}", Index, Name, ParameterClass, ResourceType, ParameterType, IsValueType, RowCount, ColumnCount, ElementCount, Offset);
        }

        /// <summary>
        /// CopyMatrix delegate used to reorder matrix when copying from <see cref="Matrix"/>.
        /// </summary>
        /// <param name="matrix">The source matrix.</param>
        /// <param name="offset">The offset in bytes to write to</param>
        private delegate void CopyMatrixDelegate(ref Matrix matrix, int offset);

        /// <summary>
        /// Copy matrix in row major order.
        /// </summary>
        /// <param name="matrix">The source matrix.</param>
        /// <param name="offset">The offset in bytes to write to</param>
        private unsafe void CopyMatrixRowMajor(ref Matrix matrix, int offset)
        {
            var pDest = (float*)((byte*)buffer.DataPointer + offset);
            fixed (void* pMatrix = &matrix)
            {
                var pSrc = (float*)pMatrix;
                // If Matrix is row_major but expecting less columns/rows
                // then copy only necessary columns/rows.
                for (int i = 0; i < RowCount; i++, pSrc +=4, pDest += 4)
                {
                    for (int j = 0; j < ColumnCount; j++)
                        pDest[j] = pSrc[j];
                }
            }
        }

        /// <summary>
        /// Copy matrix in column major order.
        /// </summary>
        /// <param name="matrix">The source matrix.</param>
        /// <param name="offset">The offset in bytes to write to</param>
        private unsafe void CopyMatrixColumnMajor(ref Matrix matrix, int offset)
        {
            var pDest = (float*)((byte*)buffer.DataPointer + offset);
            fixed (void* pMatrix = &matrix)
            {
                var pSrc = (float*)pMatrix;
                // If Matrix is column_major, then we need to transpose it
                for (int i = 0; i < ColumnCount; i++, pSrc++, pDest += 4)
                {
                    for (int j = 0; j < RowCount; j++)
                        pDest[j] = pSrc[j * 4];
                }
            }
        }

        /// <summary>
        /// Straight Matrix copy, no conversion.
        /// </summary>
        /// <param name="matrix">The source matrix.</param>
        /// <param name="offset">The offset in bytes to write to</param>
        private void CopyMatrixDirect(ref Matrix matrix, int offset)
        {
            buffer.Set(offset, matrix);
        }

        /// <summary>
        /// CopyMatrix delegate used to reorder matrix when copying from <see cref="Matrix"/>.
        /// </summary>
        /// <param name="offset">The offset in bytes to write to</param>
        private delegate Matrix GetMatrixDelegate(int offset);

        /// <summary>
        /// Copy matrix in row major order.
        /// </summary>
        /// <param name="offset">The offset in bytes to write to</param>
        private unsafe Matrix GetMatrixRowMajorFrom(int offset)
        {
            var result = default(Matrix);
            var pSrc = (float*)((byte*)buffer.DataPointer + offset);
            var pDest = (float*)&result;

            // If Matrix is row_major but expecting less columns/rows
            // then copy only necessary columns/rows.
            for (int i = 0; i < RowCount; i++, pSrc += 4, pDest += 4)
            {
                for (int j = 0; j < ColumnCount; j++)
                    pDest[j] = pSrc[j];
            }
            return result;
        }

        /// <summary>
        /// Copy matrix in column major order.
        /// </summary>
        /// <param name="offset">The offset in bytes to write to</param>
        private unsafe Matrix GetMatrixColumnMajorFrom(int offset)
        {
            var result = default(Matrix);
            var pSrc = (float*)((byte*)buffer.DataPointer + offset);
            var pDest = (float*)&result;

            // If Matrix is column_major, then we need to transpose it
            for (int i = 0; i < ColumnCount; i++, pSrc +=4, pDest++)
            {
                for (int j = 0; j < RowCount; j++)
                    pDest[j * 4] = pSrc[j];
            }
            return result;
        }

        /// <summary>
        /// Straight Matrix copy, no conversion.
        /// </summary>
        /// <param name="offset">The offset in bytes to write to</param>
        private Matrix GetMatrixDirectFrom(int offset)
        {
            return buffer.GetMatrix(offset);
        }
    }
}