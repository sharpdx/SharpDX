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
        /// Offset of this parameter.
        /// </summary>
        /// <remarks>
        /// For a value type, this offset is the offset in bytes inside the constant buffer.
        /// For a resource type, this offset is an index to the resource linker.
        /// </remarks>
        internal int Offset;

        /// <summary>
        /// Gets a single value to the associated parameter in the constant buffer.
        /// </summary>
        /// <typeparam name = "T">The type of the value to read from the buffer.</typeparam>
        public T GetValue<T>() where T : struct
        {
            return buffer.Get<T>(Offset);
        }

        /// <summary>
        /// Gets a single value to the associated parameter in the constant buffer.
        /// </summary>
        /// <typeparam name = "T">The type of the value to read from the buffer.</typeparam>
        public T[] GetValueArray<T>(int count) where T : struct
        {
            return buffer.GetRange<T>(Offset, count);
        }

        /// <summary>
        /// Sets a single value to the associated parameter in the constant buffer.
        /// </summary>
        /// <typeparam name = "T">The type of the value to be written to the buffer.</typeparam>
        /// <param name = "value">The value to write to the buffer.</param>
        public void SetValue<T>(T value) where T : struct
        {
            buffer.Set(Offset, value);
            buffer.IsDirty = true;
        }

        ///// <summary>
        ///// Sets a single matrix transposed value.
        ///// </summary>
        ///// <param name="matrix"></param>
        //public unsafe void SetValueTranspose(Matrix matrix)
        //{
        //    Matrix.TransposeByRef(ref matrix, ref *(Matrix*)((byte*)buffer.DataPointer + Offset));
        //}

        /// <summary>
        /// Sets an array of values to the associated parameter in the constant buffer.
        /// </summary>
        /// <typeparam name = "T">The type of the value to be written to the buffer.</typeparam>
        /// <param name = "values">An array of values to be written to the current buffer.</param>
        public void SetValue<T>(T[] values) where T : struct
        {
            buffer.Set(Offset, values);
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
            buffer.Set(Offset + Utilities.SizeOf<T>() * index, value);
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
            buffer.Set(Offset + Utilities.SizeOf<T>() * index, values);
            buffer.IsDirty = true;
        }

        public T GetResource<T>() where T : class
        {
            return resourceLinker.GetResource<T>(Offset);
        }

        public void SetResource<T>(T value) where T : class
        {
            resourceLinker.SetResource(Offset, ResourceType, value);
        }

        /// <summary>
        /// Direct access to the resource pointer in order to 
        /// </summary>
        /// <param name="resourcePointer"></param>
        internal void SetResourcePointer(IntPtr resourcePointer) 
        {
            resourceLinker.SetResourcePointer(Offset, ResourceType, resourcePointer);
        }

        public void SetResource<T>(params T[] valueArray) where T : class
        {
            resourceLinker.SetResource(Offset, ResourceType, valueArray);
        }

        public void SetResource<T>(int index, T value) where T : class
        {
            resourceLinker.SetResource(Offset + index, ResourceType, value);
        }

        public void SetResource<T>(int index, params T[] valueArray) where T : class
        {
            resourceLinker.SetResource(Offset + index, ResourceType, valueArray);
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
            return string.Format("[{0}] {1} Class: {2}, Resource: {3}, Type: {4}, IsValue: {5}, RowCount: {6}, ColumnCount: {7}, ElementCount: {8}", Index, Name, ParameterClass, ResourceType, ParameterType, IsValueType, RowCount, ColumnCount, ElementCount);
        }
    }
}